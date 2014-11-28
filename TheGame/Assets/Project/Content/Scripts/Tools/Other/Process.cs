using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class Process {
		
		private bool looped;
		private float duration;	
		private float elapsed;
		private bool paused;
		public bool Paused {
			get {
				return this.paused;
			}
			set {			
				paused = value;
			}
		}	
		public float Progress {
			get { return elapsed/duration; }
			set { 
				elapsed = value*duration;
				if (float.IsNaN(elapsed))
					Debug.Log("log");
			}
		}
		public float SmoothProgress {
			get { 
				float progress = Progress;
				
				progress = Mathf.Clamp01(progress);
				
				// ax4+bx3+cx2+dx+e=0
				
				// e=0
				// a+b+c+d=1
				// 0.0625a+0.125b+0.25c+0.5d = 0.5
				// d=0
				// 4a+3b+2c=0
				
				// a+b+c=1
				// 0.125a+0.25b+0.5c= 1
				// 2a+b+2=0
				
				// b=-2-2a
				// a-2-2a+c=1
				// c=3+a
				// 0.125a+0.25(-2-2a)+0.5(3+a) = 1
				// a+4(-1-a)+4(3+a) = 8
				// a = 8+4-12 = 0
				
				// a = 0
				// b = -2
				// c = 3
				// d = 0
				// e = 0
				
				return -2*progress*progress*progress+3*progress*progress;
			}
		}
		
		public float LoopProgress {
			get { return Progress - LoopInd; }
		}
		
		public float RemainingTime {
			get { return duration-elapsed; }
		}
			
		public Process (float duration, bool looped) {
			this.duration = duration;
			this.elapsed = 0;
			this.looped = looped;
		}
		
		public Process (float duration) {
			this.duration = duration;
			this.elapsed = 0;
			this.looped = false;
		}
		
		public bool IsFinished {
			get {
				return Progress>=1&&!looped;
			}
		}
		
		public int LoopInd {
			get {
				return (int)Progress;
			}
		}
		
		public void Update() {
			if (!paused)
				elapsed += Time.deltaTime;
		}
		
		public void UpdateWithDT(float dt) {
			//if (!paused)
				elapsed += dt;
		}
		
		public float Duration { get { return duration; } }
		public void Reset() {
			elapsed = 0;
		}
	}
}