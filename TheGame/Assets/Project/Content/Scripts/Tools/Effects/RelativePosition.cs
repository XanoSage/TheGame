using UnityEngine;
using System.Collections.Generic;
using UnityTools.Math;

namespace UnityTools.Effects {
	public class RelativePosition {
	
		private enum PositionType {
			fromDelegate,
			fromTransform,
			fromVector3
		}
		private PositionType type;
		private TransformCustom transform;
		private Vector3 pos;
		public delegate Vector3 GetPositionDelegate();
		private GetPositionDelegate getPosition;
		public RelativePosition(Vector3 pos) {
			this.pos = pos;
			type = PositionType.fromVector3;
		}
		public RelativePosition(TransformCustom transform) {
			this.transform = transform;
			type = PositionType.fromTransform;
			pos = Position;
		}
		// delegateHolder is something encomposes getPosition delegate owner
		public RelativePosition(GetPositionDelegate getPosition, TransformCustom delegateHolder) {
			this.getPosition = getPosition;
			this.transform = delegateHolder;
			type = PositionType.fromDelegate;
			pos = Position;
		}
		public Vector3 Position {
			get {
				// Change mode to fromVector3 if transform is destryed.
				if (type != PositionType.fromVector3 && transform == null)
					type = PositionType.fromVector3;
	
				if (type == PositionType.fromDelegate)
					pos = getPosition();
				else if (type == PositionType.fromTransform)
					pos = transform.position;
	
				return pos;
			}
		}
	}
}
