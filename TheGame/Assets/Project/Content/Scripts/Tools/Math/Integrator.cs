using UnityEngine;
using System.Collections.Generic;

namespace UnityTools.Math {
	public class Integrator {
	
		private int valuesCount = 50;
		private List<Vector3> values;
		private Vector3 sum;
		public Integrator(int valuesCount) {
			this.valuesCount = valuesCount;
			values = new List<Vector3>();
		}
		public Vector3 Integrate(Vector3 currValue) {
			values.Add(currValue);
			sum = sum + currValue;
			if (values.Count>=valuesCount) {
				sum -= values[0];
				values.RemoveAt(0);
			}
			return sum / values.Count;
		}
	}
}