using UnityEngine;

namespace UnityTools.Other {
	public class ConstantMotion : MonoBehaviour {
	
		public Vector3 rotation;
		public Vector3 position;
	
		public bool local;
	
		// Use this for initialization
		void Start () {
	
		}
	
		// Update is called once per frame
		void Update () {
			if (local) {
				transform.Rotate(rotation * Time.deltaTime);
				transform.Translate(position * Time.deltaTime);
			}
			else {
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation * Time.deltaTime);
				transform.position += position * Time.deltaTime;
			}
		}
	
	}
}