using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class RoutinesManager : MonoBehaviour {
	
		public static RoutinesManager Instance { get; private set; }
	
		private const int LayersCount = 5;
	
		private RoutinesLayer[] layers;
	
		void Awake () {
			Instance = this;
		}
	
		// Use this for initialization
		void Start () {
			layers = new RoutinesLayer[LayersCount];
				
			GameObject gObj = new GameObject(string.Format("RoutinesLayers"));
			gObj.transform.parent = transform;
			gObj.transform.position = Vector3.zero;
			gObj.transform.rotation = Quaternion.identity;
	
			for (int i = 0; i != LayersCount; i++) {
				RoutinesLayer layer = gObj.AddComponent<RoutinesLayer>();
				layers[i] = layer;
				layers[i].layer = i;
			}
		}
		
		public Coroutine StartRoutine (IEnumerator enumerator, int layer) {
			return layers[layer].StartRoutine(enumerator);
		}
	
		public void StopRoutines (int layer) {
			layers[layer].StopRoutines();
		}
		
	}
	
	public class RoutinesLayer : MonoBehaviour {
	
		public int layer;
	
		void Awake () {
		}
	
		public Coroutine StartRoutine (IEnumerator enumerator) {
			Coroutine coroutine = StartCoroutine(enumerator);
			return coroutine;
		}
	
		public void StopRoutines () {
			StopAllCoroutines();
		}
	
	}
}
