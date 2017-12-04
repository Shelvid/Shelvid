using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public Vector3 telLocation;

	void OnTriggerEnter(Collider other)
	{
		other.gameObject.transform.position = telLocation;
	}
}
