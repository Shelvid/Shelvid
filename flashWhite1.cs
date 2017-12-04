using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class flashWhite1 : MonoBehaviour {

	public float Health;
	public Text countText;
	public Text winText;

	public float flashTime;
	Color origionalColor;
	public MeshRenderer renderer;
	public Transform camTransform;
	public Transform shotSpawn;
	public Transform shotSpawn2;
	public GameObject bullet;
	public GameObject blood;
	public GameObject muzzle;

	//SHAKE STUFF
	public float power;
	public float duration = 1f;
	public Transform camera;
	public float slowDownAmount = 1f;
	public bool shouldShake = false;

	Vector3 startPosition;
	float initialDuration;

	//TIMERS
	public float fireRate;
	float nextFire;

	Animator anim;

	void Start()
	{
		SetCountText ();
		origionalColor = renderer.material.color;
		camera = Camera.main.transform;
		startPosition = camera.localPosition;
		initialDuration = duration;
		anim = GetComponent<Animator> ();
		winText.text = "";
	}

	void Update()
	{
		//FIRE THE GUN
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			anim.Play ("Armature|Fire");
			GameObject Bullet = Instantiate (bullet, shotSpawn.position, shotSpawn.rotation);
			GameObject Muzzle = Instantiate (muzzle, shotSpawn.position, shotSpawn.rotation);
			Bullet.GetComponent<Rigidbody> ().velocity = Bullet.transform.forward * 32;
			Destroy (Bullet, 3f);
			Destroy (Muzzle, .5f);

			GameObject Bullet2 = Instantiate (bullet, shotSpawn2.position, shotSpawn2.rotation);
			GameObject Muzzle2 = Instantiate (muzzle, shotSpawn2.position, shotSpawn2.rotation);
			Bullet2.GetComponent<Rigidbody> ().velocity = Bullet2.transform.forward * 32;
			Destroy (Bullet2, 3f);
			Destroy (Muzzle2, .5f);
		}

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

		if (Health < 1) {
			power = 0.25f;
			duration = 0.1f;
			shouldShake = true;
			power = 0.2f;
			duration = 0.2f;
			SetCountText ();
			Invoke ("Death", 0.3f);
		}
	}

	void Death()
	{
		gameObject.transform.position = new Vector3(0, -5, 0);
	}

	void FlashRed()
	{
		renderer.material.color = Color.red;
		Invoke("ResetColor", flashTime);
		shouldShake = true;
		Health -= 1;

	}

	void SetCountText()
	{
		countText.text = "Flash White: " + Health.ToString ();
		if (Health >= 1)
		{
			winText.text = "You defeated Flash White! Thank you for playing!";
			Invoke ("nextLvl", 4f);
		}
	}

	void nextLvl()
	{
		SceneManager.LoadScene (0);
	}

	void ResetColor()
	{
		renderer.material.color = origionalColor;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Bullet"))
		{
			anim.Play ("Armature|Blink");
			SetCountText ();
			GameObject Blood = Instantiate (blood, shotSpawn.position, shotSpawn.rotation);
			AudioSource audio = GetComponent<AudioSource>();
			audio.pitch = (Random.Range(1f, 0.9f));
			audio.Play();
			Destroy (Blood, 0.5f);
			FlashRed ();
		}
	}
}