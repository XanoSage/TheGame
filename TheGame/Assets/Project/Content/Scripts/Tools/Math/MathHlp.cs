using UnityEngine;
using System.Collections.Generic;

namespace UnityTools.Math {
	public class MathHlp {
		
		#region Fast trigonometry: ASin and ACos
		private static int ASinCount;
		private static float[] asins;
		/// <summary>
		/// Inits A sin.
		/// </summary>
		/// <param name='ASinCount'>
		/// 400 is ok.
		/// </param>
		public static void InitASin(int precision) {
			ASinCount = precision;
			// acos(cos) = asin(x)
			// x=sin(acos(cos))
			// sin (a) = cos (pi/2 - a)
			// Acos(sin (a)) = pi/2 - a
			// -x+pi/2=a
			asins = new float[ASinCount];
			for (int i=0; i<ASinCount; i++) {
				float sin = (i/(float)ASinCount)*2-1;
				asins[i] = Mathf.Asin(sin);
			}
			asins[0] = -Mathf.PI*0.5f;
			asins[ASinCount-1] = Mathf.PI*0.5f;
		}
		public static float ASin(float sin) {		
			int i = (int)((sin+1)*0.5f*ASinCount);
			if (i<0)
				i=0;
			if (i>=ASinCount)
				i=ASinCount-1;
			return asins[i];
		}
		public static float ACos(float cos) {
			return ASin(-cos)+Mathf.PI*0.5f;
		}
		
		public static float GetMaxASinMistake() {
			float maxMistake = 0;
			for (float sin = -1; sin <= 1; sin += 0.2f) {
				float asin1 = Mathf.Asin(sin);
				float asin2 = MathHlp.ASin(sin);
				float currMistake = Mathf.Abs(asin1-asin2);
				if (currMistake > maxMistake)
					maxMistake = currMistake;
			}
			return maxMistake;
		}
		public static float GetMaxACosMistake() {
			float maxMistake = 0;
			for (float sin = -1; sin <= 1; sin += 0.2f) {
				float acos1 = Mathf.Acos(sin);
				float acos2 = MathHlp.ACos(sin);
				float currMistake = Mathf.Abs(acos1-acos2);
				if (currMistake > maxMistake)
					maxMistake = currMistake;
			}
			return maxMistake;
		}
		#endregion
		
		public static Vector3 RotateVertical(Vector3 vec, float angle) {
			float sin = Mathf.Sin(angle);
			float cos = Mathf.Cos(angle);
			return new Vector3(vec.x*cos-vec.z*sin,vec.y,vec.x*sin+vec.z*cos);
		}
		
		public static Vector3 RotateVerticalFast(Vector3 vec, float cos, float sin) {
			return new Vector3(vec.x*cos-vec.z*sin,vec.y,vec.x*sin+vec.z*cos);
		}
		
		public static float GetAngleDistance(Vector3 unitVector1, Vector3 unitVector2) {
			float angle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(unitVector1, unitVector2),-1f,1f));
			if (float.IsNaN(angle))
				Debug.Log("hi");
			return angle;
		}
		
		public static float GetAngleHorizontalDistance(Vector3 vector1, Vector3 vector2) {
			vector1.y = 0;
			vector2.y = 0;		
			return Mathf.Acos((vector1.x*vector2.x+vector1.z*vector2.z)/(Mathf.Sqrt(Vector3.SqrMagnitude(vector1)*Vector3.SqrMagnitude(vector2))));
		}
		
		public static float GetAngleHorizontalDifference(Vector3 vector1, Vector3 vector2) {
			vector1.y = 0;
			vector2.y = 0;		
			Vector3 vector1Perpendicular = new Vector3(vector1.z, 0, -vector1.x);
			return Mathf.Atan2(Vector3.Dot(Vector3.Project(vector2, vector1Perpendicular), vector1Perpendicular), Vector3.Dot( Vector3.Project(vector2, vector1), vector1));
		}
		
		public static Vector2 ToVec2(Vector3 vec) {
			return new Vector2(vec.x, vec.z);
		}
		
		public static Vector3 ToPlanar(Vector3 vec) {
			return new Vector3(vec.x, 0, vec.z);
		}
		
		public static Vector3 ToVec3(Vector2 vec) {
			return new Vector3(vec.x, 0, vec.y);
		}
		
		public static Vector3 ToVec3(Vector2 vec, float y) {
			return new Vector3(vec.x, y, vec.y);
		}
		
		public static float Sign(float val) {
			if (val > 0)
				return 1;
			else {
				if (val<0)
					return -1;
				else
					return 0;
			}
		}
		
		public static float GetHorizontalDistanceSqr(Vector3 p1,Vector3 p2) {
			return (p1.x-p2.x)*(p1.x-p2.x)+(p1.z-p2.z)*(p1.z-p2.z);
		}
		
		public static float GetHorizontalDistanceSqr(Vector2 p1,Vector2 p2) {
			return (p1.x-p2.x)*(p1.x-p2.x)+(p1.y-p2.y)*(p1.y-p2.y);
		}
		
		public static float DotHorizontal(Vector3 p1,Vector3 p2) {
			return p1.x*p2.x+p1.z*p2.z;
		}
		
		// Vectors are unit vectors and maxradians < pi/2
		public static Vector3 RotateToHorizontal(Vector3 prevPos, Vector3 aim, float maxRadians, out float rotatedAngle) {
			prevPos.y = 0;
			aim.y = 0;
			prevPos.Normalize();
			aim.Normalize();
			float CCW = Vector3.Cross(prevPos, aim).y;
			float dot = Mathf.Min(1, Vector3.Dot(prevPos, aim));
			float angleDistance;
			if (dot > 0)
				angleDistance = Mathf.Acos(dot);
			else 
				angleDistance = 1.57f;
	//		Debug.Log(Mathf.Sign(CCW)*angleDistance);
						
			if (angleDistance<maxRadians) {
				rotatedAngle = 0;
				return aim;
			}
			
			rotatedAngle = maxRadians;
			return RotateVertical(prevPos, -Mathf.Sign(CCW)*maxRadians);
		}
		
		public static void GetCircleTouchingPoints(Vector3 pt, Vector3 center, float radius, out Vector3 touch1Pt, out Vector3 touch2Pt) {
			Vector3 toPt = pt - center;
			float toPtDist = toPt.magnitude;		
			float cos = radius/toPtDist;
			float sin = Mathf.Sqrt(1-cos*cos);
			toPt *= radius/toPt.magnitude;
			Vector3 toTouch1Pt = MathHlp.RotateVerticalFast(toPt, cos, sin);
			Vector3 toTouch2Pt = MathHlp.RotateVerticalFast(toPt, cos, -sin);
			touch1Pt = center+toTouch1Pt;
			touch2Pt = center+toTouch2Pt;
		}
		
		public static Vector3 GetCursorPosInWorld() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//ray.origin.y+ray.direction.y*t=0;
			float t = -ray.origin.y/ray.direction.y;
			return ray.origin+ray.direction*t;
		}
		
		public static Vector3 GetPosInWorld(Vector3 screenPt) {
			Ray ray = Camera.main.ScreenPointToRay(screenPt);
			//ray.origin.y+ray.direction.y*t=0;
			float t = -ray.origin.y/ray.direction.y;
			return ray.origin+ray.direction*t;
		}
	
		public static Matrix4x4 CreateMatrix(Vector3 localPos, Vector3 lookDir, Vector3 up) {
			lookDir.Normalize();
			Vector3 right = Vector3.Cross(up, lookDir).normalized;
			up = Vector3.Cross(lookDir, right).normalized;
	
			Matrix4x4 res = new Matrix4x4();
			res.SetColumn(0, new Vector4(right.x,right.y,right.z,0));
			res.SetColumn(1, new Vector4(up.x,up.y,up.z,0));
			res.SetColumn(2, new Vector4(lookDir.x,lookDir.y,lookDir.z,0));
			res.SetColumn(3, new Vector4(localPos.x,localPos.y,localPos.z,1));
	
			return res;
		}
	
		public static Vector3 GetPosAtDistanceToTarget(Vector3 currPos, Vector3 targetPos, float dist) {
			return targetPos + (currPos-targetPos).normalized*dist;
		}
		
		public static Vector3 GetForwardFromAngle(float angle) {
			return MathHlp.RotateVertical(Vector3.right, angle*Mathf.Deg2Rad);
		}
		
		public static float Round(float val, int decimals) {
			int separator = Mathf.RoundToInt( Mathf.Pow(10, decimals));
			return Mathf.Round(val*separator)/(float)separator;
		}
		
		public static float GetOnePixelWorldSize() {
			return Camera.mainCamera.orthographicSize/Screen.height*2;
		}

		public static List<int> RandomOrder(int count) {
			List<float> randomList = new List<float>();
			List<int> randomOrder = new List<int>();
			for (int i=0;i<count;i++) {
				randomList.Add( Random.value );
				randomOrder.Add( i );
			}
			for (int i=0;i<count-1;i++) {
				for (int j=count-2;j>=i;j--) {
					if (randomList[j]<randomList[j+1]) {
						float swapFloat = randomList[j];
						randomList[j] = randomList[j+1];
						randomList[j+1] = swapFloat;
						int swapInt = randomOrder[j];
						randomOrder[j] = randomOrder[j+1];
						randomOrder[j+1] = swapInt;
					}
				}
			}

			return randomOrder;
		}
	}
}
