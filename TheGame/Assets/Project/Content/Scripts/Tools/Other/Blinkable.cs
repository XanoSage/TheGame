using System;
using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class Blinkable : MonoBehaviour {
	
		public float fadeInTime;
		public float fadeOutTime;
	
		public float maxVisibilityTime;
		public float minVisibilityTime;
	
		public float maxVisibility;
		public float minVisibility;
	
		private float fadingPointTime;
	
		public float Visibility {
			get { return renderer.material.color.a; }
	
			set {
				renderer.enabled = value > 0.01f;
	
				renderer.material.color = new Color(renderer.material.color.r,
				                                    renderer.material.color.g,
				                                    renderer.material.color.b,
				                                    value);
			}
		}
	
		// Use this for initialization
		void Start () {
		}
	
		private Motion motion;
	
		public void StartBlinking () {
			StopMotion();
			FadeIn(true);
		}
	
		public void StopBlinking (Action onStop = null) {
			StopMotion();
			FadeOut(true, true, onStop);
		}
	
		private void StopMotion () {
			if (motion != null)
				motion.End(true);
		}
	
		private void FadeIn (bool now = false, bool stop = false) {
			motion = Motion.AddMotion(gameObject,
			                          Motion.MotionType.Function,
			                          Motion.BehaviourType.FromTo,
			                          Motion.FunctionType.Linear,
			                          fadeInTime,
			                          now ? 0 : minVisibilityTime,
			                          new Vector3(Visibility, 0, 0),
			                          new Vector3(maxVisibility, 0, 0),
			                          true);
	
			motion.Function = vec => Visibility = vec.x;
	
			if (!stop)
				motion.OnEnd += () => FadeOut();
		}
	
		private void FadeOut (bool now = false, bool stop = false, Action onStop = null) {
			motion = Motion.AddMotion(gameObject,
			                          Motion.MotionType.Function,
			                          Motion.BehaviourType.FromTo,
			                          Motion.FunctionType.Linear,
			                          fadeOutTime,
			                          now ? 0 : maxVisibilityTime,
			                          new Vector3(Visibility, 0, 0),
			                          new Vector3(!stop ? minVisibility : 0, 0, 0), 
			                          true);
	
			motion.Function = vec => Visibility = vec.x;
			
			if (!stop)
				motion.OnEnd += () => FadeIn();
	
			if (onStop != null)
				motion.OnEnd += () => onStop();
		}
	
	}
}
