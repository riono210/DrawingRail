using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDerailmentCheck : MonoBehaviour {

	private GameObject train; // 電車インスタンス
	private Rigidbody trainRb; // 電車のrigidbody
	private bool getTrain; // 電車インスタンスを取得したか

	// Use this for initialization
	void Start () {
		getTrain = false;
	}

	// Update is called once per frame
	void Update () {
		SetTrainInst ();

		CheckTrainVector ();
	}

	// 電車インスタンスとrigidbodyを取得
	private void SetTrainInst () {
		if (RailCreateManager.Instance.trainExistence) {
			train = gameObject.GetComponent<CreateTrain> ().GetTrainInst ();

			if (train != null) {
				trainRb = train.GetComponent<Rigidbody> ();
			}

			//RailCreateManager.Instance.trainExistence = false;
			getTrain = true;
		}
	}

	private void CheckTrainVector () {
		if (getTrain) {
			float speed = trainRb.velocity.magnitude;
			//Debug.Log ("spped" + speed);
		}
	}
}