using UnityEngine;
using System.Collections;

namespace UnityTools.Effects {
	public class ClampedLinedEffect : LinedEffect {
		
		private float startClamping;
		private float endClamping;
		
		/*public static ClampedLinedEffect GetEffect(ShmangersEffectsController.EffectTypes type, float startClamping, float endClamping) {		
			// TODO: Replace ShowEffect with GetEffect and play.
			ClampedLinedEffect effect = ShmangersEffectsController.GetEffect(type) as ClampedLinedEffect;
			DebugTool.Assert(effect!=null, "Pool effect is not ClampedLinedEffect, but it should be");		
			effect.startClamping = startClamping;
			effect.endClamping = endClamping;
			return effect;
		}*/
		
		public override Vector3 Pos {
			get {
				Vector3 toEnd = (base.EndPos - base.Pos).normalized;			
				return base.Pos + toEnd*startClamping;
				
			}
		}
		
		public override Vector3 EndPos {
			get {
				Vector3 toStart = (base.Pos - base.EndPos).normalized;			
				return base.EndPos + toStart*endClamping;
			}
		}
	}
}
