using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.pitch = (Random.Range(0.30f, 0.60f));
		audio.Play();
	}
}
