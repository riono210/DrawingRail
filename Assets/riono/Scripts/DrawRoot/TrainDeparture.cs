using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDeparture : MonoBehaviour {

	public bool departure;

	// Use this for initialization
	void Start () {
		departure = false;
	}

	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter (Collision other) {

		// if (departure) {
		// 	StartCoroutine (StartDeparture ());
		// 	Debug.Log ("touch");
		// 	departure = false;
		// }
		departure = true;
	}
}