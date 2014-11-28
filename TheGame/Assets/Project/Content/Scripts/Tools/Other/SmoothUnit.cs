using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace UnityTools.Other {
	public class SmoothUnit {
		public enum SmoothUnitType {
			Float,
			Vec2,
			Vec3,
		}
	
		private int smoothRate;
		private SmoothUnitType unitType;
	
		public SmoothUnit (SmoothUnitType unitType, int smoothRate) {
			this.unitType = unitType;
			this.smoothRate = smoothRate;
	
			units = new List<object>();
		}
	
		private readonly List<object> units;
	
		public void Add (object unit) {
			if (unit is float && unitType != SmoothUnitType.Float ||
				unit is Vector2 && unitType != SmoothUnitType.Vec2 ||
				unit is Vector3 && unitType != SmoothUnitType.Vec3) {
	
				Debug.LogWarning("Unit type mismatch!");
			}
	
			units.Add(unit);
	
			if (units.Count > smoothRate)
				units.RemoveAt(0);
		}
	
		public float GetSmoothFloat () {
			return units.Cast<float>().Average(f => f);
		}
	
		public Vector2 GetSmoothVector2 () {
			return units.Cast<Vector2>().Aggregate(Vector2.zero, (arg1, arg2) => arg1 + arg2) / units.Count;
		}
	
		public Vector3 GetSmoothVector3 () {
			return units.Cast<Vector3>().Aggregate(Vector3.zero, (arg1, arg2) => arg1 + arg2) / units.Count;
		}
	
		public Vector3 GetLastVector3 () {
			if (units.Count == 0)
				return Vector3.zero;
	
			return (Vector3)units[0];
		}
	}
}