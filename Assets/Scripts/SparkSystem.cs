using UnityEngine;
using System.Collections;

public class SparkSystem : MonoBehaviour {

	ParticleEmitter emitter;
	Particle[] particles;
	public float[] particlesTime;
	public float particleLifeTime;
	public float initialParticleTime;
	public int numberParticles;

	public AnimationCurve animSizeLifeTime;
	public float maxSize;

	public Vector3 posInSphere;
	public Vector3 gravityVector;
	public float randDir;
	public float vel;
	public float baseHeight;

	// Use this for initialization
	public void StartParticles () {

		emitter = GetComponent<ParticleEmitter>();
		emitter.maxEmission = numberParticles;
		emitter.minEmission = numberParticles;
		emitter.Emit();

		particlesTime = new float[numberParticles];
		InitializeSystem();
	
	}

	void InitializeSystem(){

		particles = emitter.particles;

		for (int i = 0; i < numberParticles; i++)
		{
			float f = Random.value;
			particles[i].position = Vector3.zero;
			particles[i].velocity = Vector3.zero; 
			particles[i].color = Color.white;
			particles[i].size = 0.0f;
			particlesTime[i] = -initialParticleTime + Random.value * initialParticleTime;
		}

		emitter.particles = particles;

	}

	void UpdateCollisions(int i, float dt){
		if(particles[i].position.y<baseHeight){
			Vector3 v = particles[i].position;
			v.y = baseHeight;
			particles[i].position = v;

			v = particles[i].velocity;
			v.y = -v.y;
			particles[i].velocity = v;
		}
	}

	void UpdateVel(int i,float dt){
		particles[i].velocity -= dt*gravityVector;
		particles[i].velocity -= dt*particles[i].velocity;
	}

	void UpdatePos(int i,float dt){
		particles[i].position -= dt*particles[i].velocity;
	}

	void UpdateSize(int i,float dt){
		particles[i].size = maxSize*animSizeLifeTime.Evaluate(particlesTime[i]/particleLifeTime);
	}

	void ResetParticle(int i){
		particles[i].position = posInSphere;
		particles[i].velocity = posInSphere+randDir*Random.insideUnitSphere;
		particles[i].velocity = vel*particles[i].velocity.normalized;
	}

	void UpdateColor(int i, float dt){
		particles[i].color = Color.white;
	}

	public void UpdateParticleSystem(float dt){

		particles = emitter.particles;

		for(int i=0;i<numberParticles;i++){

			if(particlesTime[i]>=0f && particlesTime[i]<=particleLifeTime){

				UpdateVel(i,dt);
				UpdatePos(i,dt);
				UpdateSize(i,dt);
				UpdateColor(i,dt);
				UpdateCollisions(i,dt);

			}else if(particlesTime[i]<0f){
				ResetParticle(i);
			}else{
				ResetParticle(i);
				particlesTime[i] = -initialParticleTime + Random.value * initialParticleTime;
			}
				
			particlesTime[i] += dt;

		}

		emitter.particles = particles;

	}
}
