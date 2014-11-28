using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityTools.Other {
	public class Motion : MonoBehaviour {
	
		public enum MotionType {
			Position,
			Rotation,
			Abstract,
			Function,
		}
	
		public enum BehaviourType {
			FromTo,
			To,
			By
		}
	
		public enum FunctionType {
			Linear,
			Square,
			SquareRoot,
			CubicSign,
			Smooth
		}
	
		private delegate Vector3 MotionFunction (Vector3 v1, Vector3 v2, float tc);
		private MotionFunction Linear;
		private MotionFunction Square;
		private MotionFunction SquareRoot;
		private MotionFunction CubicSign;
		private MotionFunction Smooth;
	
		private MotionFunction Func;
	
		public MotionType MotionType_ { get; private set; }
		public BehaviourType BehaviourType_ { get; private set; }
		public FunctionType FunctionType_ { get; private set; }
		public bool Local_ { get; private set; }
		public float Time_ { get; private set; }
		public float Delay_ { get; private set; }
	
		public Action<Vector3> Function { get; set; } 
	
		public bool ChangeX { get; private set; }
		public bool ChangeY { get; private set; }
		public bool ChangeZ { get; private set; }
	
		public bool GlobalTime { get; private set; }
	
		private Vector3 v1;
		private Vector3 v2;
	
		public Vector3 FromVector {
			get { return v1; }
		}
	
		public Vector3 ToVector {
			get { return v2; }
		}
	
		public Vector3 ByVector {
			get { return v1; }
		}
	
		public Vector3 AbstractVector { get; private set; }
	
		public delegate void MotionEventHandler ();
		public event MotionEventHandler OnEnd;
	
		public static Motion AddMotion (GameObject gObj, MotionType motionType, BehaviourType behaviourType,
				FunctionType functionType, float time, float delay, Vector3 v1, Vector3 v2, bool local, bool globalTime) {
	
			return AddMotion(gObj, motionType, behaviourType, functionType, time, delay, v1, v2, true, true, true, local, globalTime);
		}
	
		public static Motion AddMotion (GameObject gObj, MotionType motionType, BehaviourType behaviourType,
				FunctionType functionType, float time, float delay, Vector3 v1, Vector3 v2, bool local) {
	
			return AddMotion(gObj, motionType, behaviourType, functionType, time, delay, v1, v2, local, false);
		}
	
		public static Motion AddMotion (GameObject gObj, MotionType motionType, BehaviourType behaviourType,
				FunctionType functionType, float time, float delay, Vector3 v1, Vector3 v2, bool x, bool y, bool z, bool local, bool globalTime) {
	
			foreach (Motion motion in gObj.GetComponents<Motion>()) {
				if (motion.MotionType_ == motionType) {
					Destroy(motion);
				}
			}
	
			Motion newMotion = gObj.AddComponent<Motion>();
	
			newMotion.MotionType_ = motionType;
			newMotion.BehaviourType_ = behaviourType;
			newMotion.FunctionType_ = functionType;
			newMotion.Time_ = time;
			newMotion.Delay_ = delay;
			newMotion.Local_ = local;
			newMotion.v1 = v1;
			newMotion.v2 = v2;
	
			newMotion.ChangeX = x;
			newMotion.ChangeY = y;
			newMotion.ChangeZ = z;
	
			newMotion.GlobalTime = globalTime;
	
			newMotion.Initialize();
	
			return newMotion;
		}
	
		public static void RemoveMotions (GameObject gObj) {
			Motion[] motions = gObj.GetComponents<Motion>();
	
			foreach (Motion motion in motions) {
				Destroy(motion);
			}
		}
	
		private float startTime;
		private DateTime globalStartTime;
	
		// Use this for initialization
		void Start () {
			stopped = false;
		}
	
		private void Initialize () {
			Vector3 startValue;
			Vector3 endValue;
	
			if (BehaviourType_ == BehaviourType.By) {
				startValue = GetValue();
				endValue = startValue + v1;
			}
			else if (BehaviourType_ == BehaviourType.To) {
				startValue = GetValue();
				endValue = v1;
			}
			else {
				startValue = v1;
				endValue = v2;
			}
	
			v1 = startValue;
			v2 = endValue;
	
			SetValue(startValue);
	
			Linear = Vector3.Lerp;
	
			Square = (vec1, vec2, tc) => {
				tc = Mathf.Pow(tc, 2);
				return Vector3.Lerp(vec1, vec2, tc);
			};
	
			SquareRoot = (vec1, vec2, tc) => {
				tc = Mathf.Pow(tc, 0.5f);
				return Vector3.Lerp(vec1, vec2, tc);
			};
	
			CubicSign = (vec1, vec2, tc) => {
				tc = (Mathf.Pow(2 * tc - 1, 3.0f) + 1) / 2;
				return Vector3.Lerp(vec1, vec2, tc);
			};
	
			Smooth = (vec1, vec2, tc) => {
				tc = (Mathf.Sin(Mathf.PI * tc - Mathf.PI / 2.0f) + 1) / 2;
				return Vector3.Lerp(vec1, vec2, tc);
			};
	
			switch (FunctionType_) {
				case FunctionType.Linear:
					Func = Linear;
					break;
				case FunctionType.Square:
					Func = Square;
					break;
				case FunctionType.SquareRoot:
					Func = SquareRoot;
					break;
				case FunctionType.CubicSign:
					Func = CubicSign;
					break;
				case FunctionType.Smooth:
					Func = Smooth;
					break;
			}
	
			startTime = Time.time + Delay_;
			globalStartTime = DateTime.Now.AddSeconds(Delay_);
		}
	
		private void SetValue (Vector3 v) {
			switch (MotionType_) {
				case MotionType.Position:
					if (Local_) {
						Vector3 pos = transform.localPosition;
						transform.localPosition = new Vector3(ChangeX ? v.x : pos.x, ChangeY ? v.y : pos.y, ChangeZ ? v.z : pos.z);
					}
					else {
						Vector3 pos = transform.position;
						transform.position = new Vector3(ChangeX ? v.x : pos.x, ChangeY ? v.y : pos.y, ChangeZ ? v.z : pos.z);
					}
					break;
				case MotionType.Rotation:
					if (Local_) {
						Vector3 rot = transform.localRotation.eulerAngles;
						transform.localRotation = Quaternion.Euler(ChangeX ? v.x : rot.x, ChangeY ? v.y : rot.y, ChangeZ ? v.z : rot.z);
					}
					else {
						Vector3 rot = transform.rotation.eulerAngles;
						transform.rotation = Quaternion.Euler(ChangeX ? v.x : rot.x, ChangeY ? v.y : rot.y, ChangeZ ? v.z : rot.z);
					}
					break;
				case MotionType.Abstract:
					AbstractVector = v;
					break;
				case MotionType.Function:
					if (Function != null)
						Function(v);
					break;
			}
		}
	
		private Vector3 GetValue () {
			switch (MotionType_) {
				case MotionType.Position:
					return (Local_ ? transform.localPosition : transform.position);
				case MotionType.Rotation:
					return (Local_ ? transform.localRotation.eulerAngles : transform.rotation.eulerAngles);
				case MotionType.Abstract:
					return AbstractVector;
			}
	
			return Vector3.zero;
		}
	
		private bool stopped;
	
		// Update is called once per frame
		void Update () {
			if (stopped)
				return;
	
			if (Time_ < 0.01f)
				Time_ = 0.01f;
	
			float timeCoefficient = (!GlobalTime ? (Time.time - startTime) : (float)(DateTime.Now - globalStartTime).TotalMilliseconds / 1000) / Time_;
	
			HandleMotion(timeCoefficient);
		}
	
		private void HandleMotion (float tc) {
			if (tc < 0)
				return;
	
			if (tc < 1) {
				SetValue(Func(v1, v2, tc));
			}
			else {
				SetValue(v2);
	
				End();
			}
		}
	
		public void End (bool force = false) {
			stopped = true;
	
			if (!force && OnEnd != null) {
				OnEnd();
			}
			
			OnEnd = null;
	
			Destroy(this);
		}
	
		public static IEnumerator RotateToForward (GameObject gameObject, Vector3 target, float angularVelocity) {
			float rotAngle = Tools.Angle(gameObject.transform.forward, target, gameObject.transform.up);
	
			yield return new WaitForMotion(AddMotion(gameObject,
			                                         MotionType.Rotation,
			                                         BehaviourType.By,
			                                         FunctionType.Smooth,
			                                         Mathf.Abs(rotAngle) / angularVelocity,
			                                         0,
			                                         new Vector3(0, rotAngle, 0),
			                                         Vector3.zero,
			                                         false,
			                                         true,
			                                         false,
			                                         true,
			                                         false));
		}
	
	}
	
	public class MotionsCollection {
		public delegate void MotionsCollectionEventHandler ();
		public event MotionsCollectionEventHandler OnMotionsEnd;
	
		private List<Motion> Motions { get; set; }
		private List<bool> motionsEnded;
	
		public Motion this[int i] {
			get { return Motions[i]; }
		}
	
		public MotionsCollection () {
			Motions = new List<Motion>();
			motionsEnded = new List<bool>();
		}
	
		public void AddMotion (Motion motion) {
			Motions.Add(motion);
			motionsEnded.Add(false);
	
			int index = Motions.Count - 1;
	
			motion.OnEnd += () => {
				motionsEnded[index] = true;
				CheckEnded();
			};
		}
	
		private void CheckEnded () {
			if (motionsEnded.Any(motionEnded => !motionEnded))
				return;
	
			End();
		}
	
		private void End () {
			if (OnMotionsEnd != null) {
				OnMotionsEnd();
			}
		}
	}
	
	public class WaitForMotion : CoroutineReturn {
	
		public IEnumerator WaitForMotionEnumerator () {
			yield return this;
		}
	
		private readonly Motion motion;
		private bool isFinished;
	
		public WaitForMotion (Motion motion) {
			this.motion = motion;
	
			isFinished = false;
	
			motion.OnEnd += () => isFinished = true;
		}
	
		public override bool IsFinished {
			get { return isFinished || motion == null; }
		}
	
	}
	
	public class WaitForMotionsCollection : CoroutineReturn {
	
		private readonly MotionsCollection collection;
		private bool isFinished;
	
		public WaitForMotionsCollection (MotionsCollection collection) {
			this.collection = collection;
	
			isFinished = false;

		    collection.OnMotionsEnd += OnMotionEndHandler;

		    OnFinished += () => collection.OnMotionsEnd -= OnMotionEndHandler;

            //Debug.Log("WaitForMotionsCollection started");
		}
	
		public override bool IsFinished {
			get { return isFinished || collection == null; }
		}

	    private void OnMotionEndHandler()
	    {
	        isFinished = true;
            //Debug.Log("WaitForMotionsCollection ended. collection = " + collection);
	    }
	
	}
}