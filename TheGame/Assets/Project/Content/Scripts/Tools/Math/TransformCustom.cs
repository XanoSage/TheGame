using UnityEngine;
using System.Collections;

namespace UnityTools.Math {
	public class TransformCustom {
	
		private Matrix4x4 matrix;
		private Vector3 scale;
	
		public TransformCustom(Vector3 pos, Vector3 forward, Vector3 up) {
			scale = Vector3.one;
			matrix.SetTRS(pos, Quaternion.LookRotation(forward, up), scale);
		}
		
		public TransformCustom(Vector3 pos, Quaternion rotation, Vector3 scale) {
			this.scale = scale;
			matrix.SetTRS(pos, rotation, scale);
		}
		
		public TransformCustom(Matrix4x4 matrix) {
			scale = Vector3.one;
			this.matrix = matrix;
		}
		
		public TransformCustom GetInverse() {
			return new TransformCustom(matrix.inverse);
		}
		
		public static TransformCustom operator * (TransformCustom tr1, TransformCustom tr2) {		
			Matrix4x4 res = new Matrix4x4();
			
			Matrix4x4 m1 = tr1.matrix;
			Matrix4x4 m2 = tr2.matrix;
			
			res.m00 = m1.m00*m2.m00 + m1.m01*m2.m10 + m1.m02*m2.m20 + m1.m03*m2.m30;
			res.m01 = m1.m00*m2.m01 + m1.m01*m2.m11 + m1.m02*m2.m21 + m1.m03*m2.m31;
			res.m02 = m1.m00*m2.m02 + m1.m01*m2.m12 + m1.m02*m2.m22 + m1.m03*m2.m32;
			res.m03 = m1.m00*m2.m03 + m1.m01*m2.m13 + m1.m02*m2.m23 + m1.m03*m2.m33;
			
			res.m10 = m1.m10*m2.m00 + m1.m11*m2.m10 + m1.m12*m2.m20 + m1.m13*m2.m30;
			res.m11 = m1.m10*m2.m01 + m1.m11*m2.m11 + m1.m12*m2.m21 + m1.m13*m2.m31;
			res.m12 = m1.m10*m2.m02 + m1.m11*m2.m12 + m1.m12*m2.m22 + m1.m13*m2.m32;
			res.m13 = m1.m10*m2.m03 + m1.m11*m2.m13 + m1.m12*m2.m23 + m1.m13*m2.m33;
			
			res.m20 = m1.m20*m2.m00 + m1.m21*m2.m10 + m1.m22*m2.m20 + m1.m23*m2.m30;
			res.m21 = m1.m20*m2.m01 + m1.m21*m2.m11 + m1.m22*m2.m21 + m1.m23*m2.m31;
			res.m22 = m1.m20*m2.m02 + m1.m21*m2.m12 + m1.m22*m2.m22 + m1.m23*m2.m32;
			res.m23 = m1.m20*m2.m03 + m1.m21*m2.m13 + m1.m22*m2.m23 + m1.m23*m2.m33;
			
			res.m30 = m1.m30*m2.m00 + m1.m31*m2.m10 + m1.m32*m2.m20 + m1.m33*m2.m30;
			res.m31 = m1.m30*m2.m01 + m1.m31*m2.m11 + m1.m32*m2.m21 + m1.m33*m2.m31;
			res.m32 = m1.m30*m2.m02 + m1.m31*m2.m12 + m1.m32*m2.m22 + m1.m33*m2.m32;
			res.m33 = m1.m30*m2.m03 + m1.m31*m2.m13 + m1.m32*m2.m23 + m1.m33*m2.m33;
			return new TransformCustom(res);
		}
	
		public Vector3 position {
			get {
				Vector3 pos;
				pos.x = matrix.m03;
				pos.y = matrix.m13;
				pos.z = matrix.m23;
				return pos;
			}
			set {
				matrix.m03 = value.x;
				matrix.m13 = value.y;
				matrix.m23 = value.z;
			}
		}
	
		public Vector3 right {
			get {
				Vector3 pos;
				pos.x = matrix.m00;
				pos.y = matrix.m10;
				pos.z = matrix.m20;
				return pos.normalized;
			}
		}
		public Vector3 up {
			get {
				Vector3 pos;
				pos.x = matrix.m01;
				pos.y = matrix.m11;
				pos.z = matrix.m21;
				return pos.normalized;
			}
		}
		public Vector3 forward {
			get {
				Vector3 pos;
				pos.x = matrix.m02;
				pos.y = matrix.m12;
				pos.z = matrix.m22;
				return pos.normalized;
			}
		}
		public Vector3 Scale {
			get { return scale; }
			set {
				Vector3 scaleChange;
				scaleChange.x = value.x/scale.x;
				scaleChange.y = value.y/scale.y;
				scaleChange.z = value.z/scale.z;
				scale = value;
				matrix.m00 *= scaleChange.x;
				matrix.m10 *= scaleChange.x;
				matrix.m20 *= scaleChange.x;
				matrix.m01 *= scaleChange.y;
				matrix.m11 *= scaleChange.y;
				matrix.m21 *= scaleChange.y;
				matrix.m02 *= scaleChange.z;
				matrix.m12 *= scaleChange.z;
				matrix.m22 *= scaleChange.z;
			}
		}
		public Quaternion rotation {
			get { return Quaternion.LookRotation(forward, up); }
			set { matrix.SetTRS(position, value, scale); }
		}
		public Vector3 eulerAngles {
			get { return rotation.eulerAngles; }
			set { rotation = Quaternion.Euler(value); }
		}
		public void LookAt(Vector3 target, Vector3 up) {
			rotation = Quaternion.LookRotation(target-position, up);
		}
		public void LookAt(Vector3 target) {
			LookAt(target, Vector3.up);
		}
		public Vector3 TransformDirection(Vector3 vec) {
			return matrix.MultiplyVector(vec);
		}
		public Vector3 TransformPoint(Vector3 vec) {
			return matrix.MultiplyPoint3x4(vec);
		}
		public Vector3 InverseTransformDirection(Vector3 vec) {
			return matrix.inverse.MultiplyVector(vec);
		}
		public Vector3 InverseTransformPoint(Vector3 vec) {
			return matrix.inverse.MultiplyPoint3x4(vec);
		}
		public void RotateAround(Vector3 axis, float angle) {
			matrix *= Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angle * Mathf.Rad2Deg, axis), Vector3.one);
		}
	
		public static void DebugDrawTransform(TransformCustom tr, float axisLength = 5) {
			Vector3 pos = tr.position;
			Vector3 forwPos = pos + tr.forward*axisLength;
			Vector3 upPos = pos + tr.up*axisLength;
			Vector3 rightPos = pos + tr.right*axisLength;
			Debug.DrawLine(pos, forwPos, Color.blue);
			Debug.DrawLine(pos, upPos, Color.green);
			Debug.DrawLine(pos, rightPos, Color.red);
		}
		
		public void CopyToUnityTransform(Transform transform) {
			transform.position = position;
			transform.localScale = Scale;
			transform.rotation = rotation;
		}
	}
}
