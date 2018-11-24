using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDeparture : MonoBehaviour {

	// 発車に関わる変数
	// CreateTrainに影響
	public bool departure;

	// Use this for initialization
	void Start () {
		departure = false;
	}

	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter (Collision other) {
		departure = true;
	}
}