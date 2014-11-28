using UnityEngine;
using System.Collections;
using UnityTools.Math;

namespace UnityTools.Effects {
	public class ParticleSystemTools {
	
		
		public static void MakeParticlesMoveAtHorizontalRing(ParticleSystem particleSystem) {
			ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.particleCount];
			int count = particleSystem.GetParticles(particles);
			Debug.Log("Particle count=" + count);
			for (int i=0;i<count;i++) {
				float angle = Random.value*Mathf.PI*2;
				Vector3 rotated = MathHlp.RotateVertical(Vector3.right, angle);
				particles[i].position = rotated;
				particles[i].velocity = rotated*particleSystem.startSpeed;
			}
			particleSystem.SetParticles(particles, count);
		}
	}
}