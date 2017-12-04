using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodgeBullet : MonoBehaviour {

	public float dashCdr;
	float nextDash;
	public GameObject user;
	public Animator anim;

	void Start()
	{
		anim = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Bullet") && Time.time > nextDash) {
			nextDash = Time.time + dashCdr;
			anim.Play ("Armature|Blink");
			Vector3 position = new Vector3 (Random.Range (-5.0f, 5.0f), 2, Random.Range (-5.0f, 5.0f));
			user.transform.position = position;
		}
	}
}
