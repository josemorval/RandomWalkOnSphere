using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {

	public float height;
	public float radius;
	public float vel;
	public Transform whereToLook;


	void Update () {
		transform.position = new Vector3(radius*Mathf.Cos(vel*Time.time),height,radius*Mathf.Sin(vel*Time.time));
		transform.LookAt(whereToLook);
	}
}
