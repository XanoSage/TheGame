using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class ScaleByDistance : MonoBehaviour {
	
		public float defaultDistance;
		public Transform target;
	
		private Vector3 startingScale;
	
		// Use this for initialization
		void Start () {
			startingScale = transform.localScale;
		}
		
		// Update is called once per frame
		void Update () {
			float distance = Vector3.Distance(transform.position, target.position);
	
			transform.localScale = startingScale * (distance / defaultDistance);
		}
	}
}