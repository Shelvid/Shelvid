using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashOnHit : MonoBehaviour {

	public float Health;

	public float flashTime;
	Color origionalColor;
	public MeshRenderer renderer;
	public Transform camTransform;
	Animator anim;

	//SHAKE STUFF
	public float power;
	public float duration = 1f;
	public Transform camera;
	public float slowDownAmount = 1f;
	public bool shouldShake = false;

	Vector3 startPosition;
	float initialDuration;

	void Start()
	{
		anim = GetComponent<Animator> ();
		origionalColor = renderer.material.color;
		camera = Camera.main.transform;
		startPosition = camera.localPosition;
		initialDuration = duration;
	}

	void Update()
	{
		if (shouldShake) {
			if (duration > 0) {
				camera.localPosition = startPosition + Random.insideUnitSphere * power;
				duration -= Time.deltaTime * slowDownAmount;
			} else {
				shouldShake = false;
				duration = initialDuration;
				camera.localPosition = startPosition;
			}
		}

		if (Health < 2) {
			anim.Play ("ShakeAnim");
		}

		if (Health < 1) {
			power = 0.25f;
			duration = 0.1f;
			shouldShake = true;
			power = 0.2f;
			duration = 0.2f;
			Invoke ("Death", 0.3f);
		}
	}

	void Death()
	{
		Destroy (gameObject);
	}

	void FlashRed()
	{
		renderer.material.color = Color.red;
		Invoke("ResetColor", flashTime);
		shouldShake = true;
		Health -= 1;

	}

	void ResetColor()
	{
		renderer.material.color = origionalColor;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Bullet"))
		{
			FlashRed ();
		}
	}
}