using UnityEngine;
using System.Collections;

public class DrawLineMesh : MonoBehaviour {

	public int N;
	public float radius;
	public float delta0;
	public float delta1;
	public float delta2;
	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = new Mesh();	

		Vector3[] v = new Vector3[N];
		int[] indv = new int[N];
		Vector2[] uv = new Vector2[N];

		v[0] = radius*Random.onUnitSphere;
		indv[0] = 0;
		uv[0] = new Vector2(0f,0f);
		Vector3 vel = Vector3.zero;
		for(int i=1;i<N;i++){
			vel+= delta0*Random.insideUnitSphere;
			v[i] = v[i-1] + delta1*vel + delta2*Random.insideUnitSphere;
			vel.Normalize();
			v[i].Normalize();
			v[i] *= radius;
			indv[i] = i;
			uv[i] = new Vector2(i/(N-1f),0f);
		}

		GetComponent<MeshFilter>().mesh = mesh;
		mesh.vertices = v;
		mesh.uv = uv;
		mesh.SetIndices(indv,MeshTopology.LineStrip,0);



	}

}
