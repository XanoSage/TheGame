using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class LookAt : MonoBehaviour {
	
		public enum ModeType {
			BackToTarget,
			CommonForward,
		}
	
		public Transform target;
		public ModeType mode;
		
		// Update is called once per frame
		void Update () {
			switch (mode) {
				case ModeType.BackToTarget :
					transform.LookAt(2 * transform.position - target.position, target.up);
					break;
				case ModeType.CommonForward :
					transform.LookAt(transform.position + target.forward, Vector3.up);
					break;
			}
		}
	}
}
