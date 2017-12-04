using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float dashDistance;
	public float Health;
	public Text countText;
	public Text winText;

	//SHAKE
	public float flashTime;
	Color origionalColor;
	public MeshRenderer renderer;
	public Transform camTransform;

	Vector3 startPosition;
	float initialDuration;
	public float power;
	public float duration = 1f;
	public Transform camera;
	public float slowDownAmount = 1f;
	public bool shouldShake = false;

	//Animator anim;
	Vector3 movement; 
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100f;

	public GameObject blood;
	public GameObject bullet;
	public GameObject muzzle;
	public Transform shotSpawn1;

	//Timers
	public float fireRate = 0.3f;
	float nextFire;

	public float dashCdr;
	float nextDash;

	Animator anim;

	bool isRunning = false;

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

	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");

		// Set up references.
		//anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	void Update()
	{
		/*if (playerRigidbody.velocity != Vector3.zero) {
			anim.Play("Armature|Run");
		}*/
		transform.position += transform.forward * speed * Time.deltaTime;

		if (Input.GetMouseButton (0) && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			anim.Play ("Armature|Fire");
			//SHAKING
			shouldShake = true;
			power = 0.1f;
			duration = 0.2f;

			GameObject Bullet = Instantiate (bullet, shotSpawn1.position, shotSpawn1.rotation);
			GameObject Muzzle = Instantiate (muzzle, shotSpawn1.position, shotSpawn1.rotation);
			Bullet.GetComponent<Rigidbody>().velocity = Bullet.transform.forward * 38;
			Destroy (Bullet, 3f);
			Destroy (Muzzle, .5f);
		}

		if (Input.GetMouseButton (1) && Time.time > nextDash) {
			isRunning = true;
			anim.Play ("Armature|Blink");
			nextDash = Time.time + dashCdr;
			playerRigidbody.transform.Translate (0f, 0f, dashDistance);
			Invoke("isRunningA", 0.3f);
		}

		if (Input.GetMouseButton (2) && isRunning==false) {
			anim.Play ("Armature|Run");
			transform.position += transform.forward * speed * Time.deltaTime;
		}

		if (Input.GetMouseButtonUp (2) && isRunning==false) {
			anim.Play ("Armature|Idle");
			transform.position += transform.forward * speed * Time.deltaTime;
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
			gameObject.transform.position = new Vector3 (0, -5, 0);
			Invoke ("Death", 2f);
		}

	}

	void isRunningA()
	{
		isRunning = false;
	}

	void FixedUpdate ()
	{
		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		// Move the player around the scene.
		Move (h, v);

		if (isRunning == false) {
			Turning ();
		}

		// Animate the player.
		//Animating (h, v);
	}

	void Move (float h, float v)
	{
		//anim.Play ("Armature|Run");

		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning ()
	{
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;

			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Death()
	{
		Destroy (gameObject);
		Application.LoadLevel (Application.loadedLevel);
	}

	void FlashRed()
	{
		renderer.material.color = Color.red;
		transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
		Invoke("ResetColor", flashTime);
		shouldShake = true;
		Health -= 1;

	}

	void ResetColor()
	{
		renderer.material.color = origionalColor;
		transform.localScale -= new Vector3(0.3f, 0.3f, 0.3f);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Bullet"))
		{
			GameObject Blood = Instantiate (blood, shotSpawn1.position, shotSpawn1.rotation);
			Destroy (Blood, 0.5f);
			FlashRed ();
			SetCountText ();
		}

		if(other.gameObject.CompareTag("EBullet"))
		{
			GameObject Blood = Instantiate (blood, shotSpawn1.position, shotSpawn1.rotation);
			Destroy (Blood, 0.5f);
			FlashRed ();
			SetCountText ();
		}
	}

	void SetCountText()
	{
		countText.text = "You: " + Health.ToString ();
		if (Health < 1)
		{
			winText.text = "You have been defeated.";
		}
	}

	/*void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);
	}*/
}
