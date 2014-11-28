using System;
using UnityEngine;

namespace UnityTools.Other {
	[Serializable]
	public class SphericalCoordinates {
	
		public float radius;			// rho
		public float azimuthAngle;		// phi
		public float polarAngle;		// theta
	
		public SphericalCoordinates () { }
	
		public SphericalCoordinates (float radius, float azimuthAngle, float polarAngle) {
			this.radius = radius;
			this.azimuthAngle = azimuthAngle;
			this.polarAngle = polarAngle;
		}
	
		public SphericalCoordinates (SphericalCoordinates coordinates) {
			radius = coordinates.radius;
			azimuthAngle = coordinates.azimuthAngle;
			polarAngle = coordinates.polarAngle;
		}
	
		public void Limit (SphericalCoordinates minimum, SphericalCoordinates maximum) {
			if (radius > maximum.radius)
				radius = maximum.radius;
	
			if (radius < minimum.radius)
				radius = minimum.radius;
	
			if (azimuthAngle > maximum.azimuthAngle)
				azimuthAngle = maximum.azimuthAngle;
	
			if (azimuthAngle < minimum.azimuthAngle)
				azimuthAngle = minimum.azimuthAngle;
	
			if (polarAngle > maximum.polarAngle)
				polarAngle = maximum.polarAngle;
	
			if (polarAngle < minimum.polarAngle)
				polarAngle = minimum.polarAngle;
		}
	
		public Vector3 AsVector3 { get { return new Vector3(radius, azimuthAngle, polarAngle); } }
	
		public void FromVector3 (Vector3 vec) {
			radius = vec.x;
			azimuthAngle = vec.y;
			polarAngle = vec.z;
		}
	
		public static SphericalCoordinates GetFromVector3 (Vector3 vec) {
			SphericalCoordinates result = new SphericalCoordinates();
			result.FromVector3(vec);
			return result;
		}
	
		private static void NormalizeAngle (ref float angle) {
			angle %= Mathf.PI * 2f;
	
			if (Mathf.Abs(angle) > Mathf.PI)
				angle -= Mathf.PI * 2f * Tools.Sign(angle);
		}
	
		public static SphericalCoordinates Delta (SphericalCoordinates initial, SphericalCoordinates target) {
			SphericalCoordinates result = target - initial;
	
			NormalizeAngle(ref result.azimuthAngle);
			NormalizeAngle(ref result.polarAngle);
	
			return result;
		}
	
		public static SphericalCoordinates operator + (SphericalCoordinates sc1, SphericalCoordinates sc2) {
			return new SphericalCoordinates(sc1.radius + sc2.radius,
			                                sc1.azimuthAngle + sc2.azimuthAngle,
			                                sc1.polarAngle + sc2.polarAngle);
		}
	
		public static SphericalCoordinates operator - (SphericalCoordinates sc1, SphericalCoordinates sc2) {
			return new SphericalCoordinates(sc1.radius - sc2.radius,
			                                sc1.azimuthAngle - sc2.azimuthAngle,
			                                sc1.polarAngle - sc2.polarAngle);
		}
	
	}
}