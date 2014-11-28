using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

namespace UnityTools.Other {
	public abstract class CoroutineReturn {
	
		public delegate void CoroutineReturnEventsHandler ();
	
		public event CoroutineReturnEventsHandler OnFinished;
	
		public virtual bool IsFinished { get; protected set; }
		protected virtual bool IsCancel { get; set; }
	
		private bool canceled;
		private bool finishCalled;
	
		public virtual int Layer { get { return 0; } }
	
		public bool IsCanceled { get { return IsCancel || canceled; } }
	
		public void Cancel () {
			canceled = true;
			Finish();
		}
	
		public void Finish () {
			if (finishCalled)
				return;
			
			finishCalled = true;
	
			if (OnFinished != null) {
				OnFinished();
			}
		}
	
		protected CoroutineReturn () {
			canceled = false;
			finishCalled = false;
		}
	
	}
	
	public class WaitForAnimation : CoroutineReturn {
	
		private GameObject _go;
		private string _name;
		private float _time;
		private float _weight;
		private int startFrame;
	
		public string name {
			get { return _name; }
		}
	
		public WaitForAnimation (GameObject go, string name, float time = 1f, float weight = -1) {
			startFrame = Time.frameCount;
			_go = go;
			_name = name;
			_time = Mathf.Clamp01(time);
			_weight = weight;
		}
	
		public override bool IsFinished {
			get {
				if (Time.frameCount <= startFrame + 1) {
					return false;
				}
				
				if (_weight == -1) {
					return !_go.animation[_name].enabled || _go.animation[_name].normalizedTime >= _time ||
					       _go.animation[_name].weight == 0 || _go.animation[_name].speed == 0;
				}
	
				if (_weight < 0.5) {
					return _go.animation[_name].weight <= Mathf.Clamp01(_weight);
				}
	
				return _go.animation[_name].weight >= Mathf.Clamp01(_weight);
			}
			protected set { base.IsFinished = value; }
		}
	
	}
	
	public static class TaskHelpers {
	
		public static WaitForAnimation WaitForAnimation (this GameObject go, string name, float time = 1f) {
			return new WaitForAnimation(go, name, time, -1);
		}
	
		public static WaitForAnimation WaitForAnimationWeight (this GameObject go, string name, float weight = 0f) {
			return new WaitForAnimation(go, name, 0, weight);
		}
	
	}
	
	public class RadicalRoutine {
	
		private bool cancel;
		public IEnumerator enumerator;
		public event Action Cancelled;
		public event Action<bool> Finished;
	
		private int layer;
	
		private static readonly List<RadicalRoutine> Routines = new List<RadicalRoutine>(); 
		private static readonly List<CoroutineReturn> CoroutineReturns = new List<CoroutineReturn>(); 
	
		private static void AddRoutine (RadicalRoutine routine) {
			if (Routines.Exists(r => routine == r))
				return;
	
			Routines.Add(routine);
			routine.Finished += (b) => RemoveRoutine(routine);
		}
	
		private static void RemoveRoutine (RadicalRoutine routine) {
			if (Routines.Exists(r => routine == r))
				Routines.Remove(routine);
		}
	
		private static void AddCoroutineReturn (CoroutineReturn coroutineReturn) {
			if (CoroutineReturns.Exists(@return => coroutineReturn == @return))
				return;
	
			CoroutineReturns.Add(coroutineReturn);
			coroutineReturn.OnFinished += () => CoroutineReturns.Remove(coroutineReturn);
		}
	
		private static void RemoveCoroutineReturn (CoroutineReturn coroutineReturn) {
			if (CoroutineReturns.Exists(@return => coroutineReturn == @return))
				CoroutineReturns.Remove(coroutineReturn);
		}
	
		public static void CancelAll () {
			foreach (RadicalRoutine routine in Routines) {
				routine.Cancel();
			}
	
			Routines.Clear();
	
			for (int i = 0; i != CoroutineReturns.Count; i++) {
				int lastCount = CoroutineReturns.Count;
				
				CoroutineReturns[i].Cancel();
	
				i -= lastCount - CoroutineReturns.Count;
			}
	
			CoroutineReturns.Clear();
		}
	
		public static void CancelLayer (int layer) {
			List<RadicalRoutine> routines = Routines.Where(routine => routine.layer == layer).ToList();
	
			foreach (RadicalRoutine routine in routines) {
				routine.Cancel();
				Routines.Remove(routine);
			}
	
			List<CoroutineReturn> returns = CoroutineReturns.Where(@return => @return.Layer == layer).ToList();
	
			foreach (CoroutineReturn @return in returns) {
				@return.Cancel();
				CoroutineReturns.Remove(@return);
			}
		}
	
		public void Cancel () {
			cancel = true;
		}
	
		public static IEnumerator Run (IEnumerator extendedCoRoutine) {
			RadicalRoutine routine = Create(extendedCoRoutine, 0);
			return routine.Execute(extendedCoRoutine);
		}
	
		public static RadicalRoutine Create (IEnumerator extendedCoRoutine, int layer) {
			RadicalRoutine rr = new RadicalRoutine { layer = layer };
			rr.enumerator = rr.Execute(extendedCoRoutine);
			AddRoutine(rr);
			return rr;
		}
	
		private IEnumerator Execute (IEnumerator extendedCoRoutine) {
			while (!cancel && extendedCoRoutine != null && extendedCoRoutine.MoveNext()) {
				object v = extendedCoRoutine.Current;
				CoroutineReturn cr = v as CoroutineReturn;
	
				if (cr != null) {
					AddCoroutineReturn(cr);
	
					if (cr.IsCanceled) {
						cancel = true;
						break;
					}
	
					while (!cr.IsFinished) {
						if (cr.IsCanceled) {
							cancel = true;
							break;
						}
	
						yield return new WaitForEndOfFrame();
					}
	
					cr.Finish();
					RemoveCoroutineReturn(cr);
				}
				else {
					yield return v;
				}
			}
	
			if (cancel && Cancelled != null) {
				Cancelled();
			}
			if (Finished != null) {
				Finished(cancel);
			}
		}
	
	}
}