using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent (typeof (NavMeshAgent))]
public class TrainDerailmentCheck : MonoBehaviour {

	private GameObject train; // 電車インスタンス
	private Rigidbody trainRb; // 電車のrigidbody
	private bool getTrain; // 電車インスタンスを取得したか

	private NavMeshAgent trainAgent;

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
		if (RailCreateManager.Instance.trainExistence && !getTrain) {
			train = gameObject.GetComponent<CreateTrain> ().GetTrainInst ();

			if (train != null) {
				trainRb = train.GetComponent<Rigidbody> ();
				trainAgent = train.GetComponent<NavMeshAgent> ();
				getTrain = true;
			}

			//RailCreateManager.Instance.trainExistence = false;
		}
	}

	// 電車の速度を見る
	private void CheckTrainVector () {
		if (getTrain) {
			float speed = trainRb.velocity.magnitude;
			float speed2 = trainAgent.speed;
			Debug.Log ("spped2:" + speed2);

			if (speed2 >= 0.5f) {
				// 子供を外す
				GameObject trainChiled = train.transform.GetChild (0).gameObject;
				trainChiled.transform.parent = null;
				Destroy (train);

				// 外したものにrigidbodyつけるy軸固定
				Rigidbody childRb = trainChiled.AddComponent<Rigidbody> ();
				//childRb.constraints = RigidbodyConstraints.FreezePositionY;
				childRb.drag = 5;
				childRb.angularDrag = 5;

				trainChiled.GetComponent<BoxCollider> ().enabled = true;
				// addforce30
				Vector3 derailForceX = -trainChiled.transform.right;
				Vector3 derailForceY = trainChiled.transform.up;
				Vector3 derailForceZ = trainChiled.transform.forward;
				childRb.AddForce (derailForceX * 500 + derailForceY * 10 + derailForceZ * 50);
			}
		}
	}
}