using UnityEngine;
using System.Collections.Generic;

namespace UnityTools.Effects {
	public class LinedEffect : Effect {
		
		public void Play(RelativePosition pos, RelativePosition end) {
			SetEnd(end);
			Play(pos);
		}
		
		public virtual Vector3 EndPos {
			get { return endPos.Position; }
		}
		
		protected override void Awake()
		{
			base.Awake();
			FindStartEndEffects();
		}
		
		private RelativePosition endPos;
		private List<ParticleSystem> startEffects;
		private List<ParticleSystem> endEffects;
		private Transform endEffectsParent;
		
		private void FindStartEndEffects() {
			Transform startEffectsParent = transform.FindChild("StartEffects");
			if (startEffectsParent==null)
				Debug.LogError(string.Format("Effect {0} doesn't have StartEffects child transform", gameObject.name));
			endEffectsParent = transform.FindChild("EndEffects");
			if (endEffectsParent==null)
				Debug.LogError(string.Format("Effect {0} doesn't have EndEffects child transform", gameObject.name));
			startEffects = new List<ParticleSystem>();
			endEffects = new List<ParticleSystem>();
			AddChildEffectsToList(startEffects, startEffectsParent);
			AddChildEffectsToList(endEffects, endEffectsParent);
		}
		
		private void AddChildEffectsToList(List<ParticleSystem> list, Transform transform) {
			if (transform.particleSystem!=null)
				list.Add(transform.particleSystem);
			for (int i=0; i<transform.GetChildCount(); i++) {
				AddChildEffectsToList(list, transform.GetChild(i));
			}
		}
		
		protected override void ApplyPositions()
		{
			Vector3 start = Pos;
			Vector3 end = EndPos;
			transform.position = start;
			endEffectsParent.position = end;
			foreach (ParticleSystem particles in startEffects)
				particles.transform.LookAt(end);
			foreach (ParticleSystem particles in endEffects)
				particles.transform.LookAt(start);
		}
		
		private void SetEnd(RelativePosition endPos) {
			this.endPos = endPos;
		}
	}
}