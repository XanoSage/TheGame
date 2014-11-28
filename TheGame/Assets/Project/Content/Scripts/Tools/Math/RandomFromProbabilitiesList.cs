using UnityEngine;
using System.Collections.Generic;

namespace UnityTools.Math {
	public class RandomFromProbabilitiesList {
		
		public static int GetValue(List<float> probabilities) {
			float sum = 0;
			foreach (float probability in probabilities)
				sum += probability;
			if (sum == 0)
				return -1; // No probabilities at all.
			float realizaion = Random.value * sum;
			int res = 0;
			
			if (probabilities.Count == 0)
				return res;
			
			do {
				realizaion -= probabilities[res];
				if (realizaion<0)
					break;
				res++;
			} while (res<probabilities.Count);
			return res;
		}
	}
}