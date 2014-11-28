using UnityEngine;
using System.Collections.Generic;
using UnityTools.Pooling;
using UnityTools.Other;

namespace UnityTools.Effects {
	public class Effect : PoolItem {
		
		#region Public interface
		public virtual void Play(Vector3 pos) {
			this.pos = new RelativePosition(pos);
			Play(this.pos);
		}
		
		public virtual void Play(RelativePosition pos) {
			transform.position = pos.Position;
			foreach (ParticleSystem particles in particleSystems) {
				particles.Play(false);
			}
			playback = new Process(Duration);
		}
		
		public virtual void Stop() {
			playback = null;
			Pool.Push(this);
			if (onStopped!=null)
				onStopped(this);
		}
		
		public virtual void StopEmitting() {
			foreach (ParticleSystem particles in particleSystems) {
				particles.Stop();
			}
			if (onEmittingStopped!=null)
				onEmittingStopped(this);
		}
		
		public event System.Action<Effect> onStopped;
		public event System.Action<Effect> onEmittingStopped;
			
		public bool IsFinished { get { return playback==null || playback.IsFinished; } }
		public bool IsPlaying { get { return playback!=null && !playback.IsFinished; } }
		public bool IsEmitting { get { return particleSystems[0].isPlaying; } }
		public float Duration { get; private set; }
		public float Progress { get { return playback.Progress; } }
		public virtual Vector3 Pos { get { return pos.Position; } }
		public void SetColor(Color color) {
			foreach (ParticleSystem particles in particleSystems)
				particles.startColor = color;
		}
		#endregion
		
		#region Fields
		private RelativePosition pos;
		private List<ParticleSystem> particleSystems;
		private Process playback;
		#endregion
		
		#region Private
		#region Awake
		protected virtual void Awake() {
			FindParticleSystems();
			CalcDuration();
			InitScale();
		}
		private void FindParticleSystems() {
			particleSystems = new List<ParticleSystem>();
			FindChildParticleSystems(transform);
		}
		private void FindChildParticleSystems(Transform transform) {
			ParticleSystem paricles = transform.gameObject.GetComponent<ParticleSystem>();
			
			if (paricles!=null)
				particleSystems.Add(paricles);
			for (int i=0;i<transform.GetChildCount();i++)
				FindChildParticleSystems(transform.GetChild(i));
		}
		
		private void CalcDuration() {
			float maxDuration = 0;
			foreach (ParticleSystem particles in particleSystems) {
				if (particles.duration>maxDuration)
					maxDuration = particles.duration;
			}
			Duration = maxDuration;
		}
		#endregion
		
		#region Update
		protected virtual void Update() {
			if (IsPlaying) {
				ApplyPositions();
				playback.Update();
				if (playback.IsFinished) {
					Stop();
				}
			}
		}
		
		protected virtual void ApplyPositions() {
			transform.position = Pos;
		}
		#endregion
		
		#region Scaling
		protected float scale;
		private List<float> startScales;
		private List<float> startSpeeds;
		private void InitScale() {
			startScales = new List<float>();
			startSpeeds = new List<float>();
			foreach (ParticleSystem particles in particleSystems) {
				startScales.Add(particles.startSize);
				startSpeeds.Add(particles.startSpeed);
			}
			scale = 1;
		}
		public void SetScale(float scale) {	
			int i=0;
			foreach (ParticleSystem particles in particleSystems) {
				particles.startSize = startScales[i]*scale;
				particles.startSpeed = startSpeeds[i] * scale;
				i++;
			}
			this.scale = scale;
		}	
		#endregion
		#endregion
		
		#region PoolItem members
		// Activating happens on removing object from pool.
		public override void Activate() {
			base.Activate();
			transform.parent = null;
			gameObject.SetActive(true);
		}
		
		// Deactivating happens on pushing object to pool.
		public override void Deactivate() {
			base.Deactivate();
			gameObject.SetActive(false);
		}
		
		// Items can have their specific ways to recognize equal ones.
		public override bool EqualsTo(PoolItem item) {
			Effect effect = item as Effect;
			return effect != null && (effect.name==this.name || effect.name+"(Clone)"==this.name || effect.name==this.name+"(Clone)");
		}
		#endregion
	}
}
