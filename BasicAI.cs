using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

	Rigidbody rb;
	public Transform target;
	public float speed;


	void Update () {
		transform.LookAt (target);
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
