using UnityEngine;
using System.Collections;

public class DrawerPoint : MonoBehaviour {

	public SparkSystem sparkSystem;
	public AnimationCurve animCurve;
	public float maxTime;
	public DrawLineMesh drawLineMesh;
	public float timeScale;
	int index;
	float time;


	void Start () {
		time = 0f;
		sparkSystem.StartParticles();
	}
	
	void Update () {

		float dt = timeScale*Time.deltaTime;
		time += dt;

		float f = animCurve.Evaluate(time/maxTime);
		index = GetIndex(f);
		transform.position = drawLineMesh.transform.TransformPoint(drawLineMesh.GetComponent<MeshFilter>().mesh.vertices[index]);
		drawLineMesh.transform.GetComponent<MeshRenderer>().material.SetFloat("_appear",f);

		sparkSystem.posInSphere = transform.position;
		sparkSystem.UpdateParticleSystem(dt);
	}

	int GetIndex(float f){
		return Mathf.FloorToInt((drawLineMesh.N-1f)*f);
	}
}
