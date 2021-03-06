﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

//[RequireComponent (typeof (NavMeshAgent))]
public class TrainDerailmentCheck : MonoBehaviour {

	private GameObject train; // 電車インスタンス
	private Rigidbody trainRb; // 電車のrigidbody
	private bool getTrain; // 電車インスタンスを取得したか
	private GameObject trainChiled; //電車のスプライトObj

	private NavMeshAgent trainAgent; // 電車のNavMesh

	public GameObject dangerImg; // 速度危険のUI
	private IEnumerator flash; // 点滅させるコルーチン

	private float derailSpeed; // 脱線する速度
	private bool isDerail; // 脱線フラグ
	public GetObjectController speedBtnCtl; // 速度調節ボタンのイベント設定クラス
	public CreateTrain createTrain; // 電車生成クラス

	public GameObject explosionParticle; // 爆発パーティクル

	public BGMController bgmController; // bgmのBPM調整

	public GameObject speedMeterContent; // スピードメーター
	public AudioClip explosionSE; // 爆発SE

	// Use this for initialization
	void Start () {
		getTrain = false;
		derailSpeed = 0.9f;
		isDerail = false;

		bgmController = GameObject.Find ("BGM").GetComponent<BGMController> ();

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
			float speed = trainAgent.speed;
			//Debug.Log ("spped:" + speed);

			// 脱線する速度限界
			if (speed >= derailSpeed) {
				isDerail = true;
				dangerImg.SetActive (true);
				Derail ();

			} else if (speed < derailSpeed) {
				int speedRate = (int) ((speed / derailSpeed) * 10);
				//Debug.Log ("sp: " + speedRate);

				for (int i = 0; i < 5; i++) {
					Transform speedChiled = speedMeterContent.transform.GetChild (i);
					if (speedRate == 0) {
						speedChiled.gameObject.SetActive (false);
					} else if (i <= speedRate / 2) {
						speedChiled.gameObject.SetActive (true);
					} else {
						speedChiled.gameObject.SetActive (false);
					}
				}

				if (speedRate >= 8) { // 点滅
					if (flash == null) {
						flash = SpeedDanger ();
						StartCoroutine (flash);
					}
				} else {
					if (flash != null) { // 点滅停止
						StopCoroutine (flash);
						flash = null;
					}

					if (!isDerail) { // 脱線していない時
						// 点灯してコルーチンが終わった場合
						dangerImg.SetActive (false);
					}
				}
			}
		}
	}
	// 危険マークを点滅させる
	private IEnumerator SpeedDanger () {
		//Debug.Log ("call");
		while (true) {
			dangerImg.SetActive (true);

			yield return new WaitForSeconds (0.5f);

			dangerImg.SetActive (false);

			yield return new WaitForSeconds (0.5f);
		}
		yield break;
	}

	// 脱線
	private void Derail () {
		train.GetComponent<AudioSource> ().PlayOneShot (explosionSE);

		// 子供を外す
		trainChiled = train.transform.GetChild (0).gameObject;
		trainChiled.transform.parent = null;
		// 速度を0にしてこの関数が必要以上に呼ばれないように
		trainAgent.speed = 0;

		// 外したものにrigidbodyつける
		Rigidbody childRb = trainChiled.AddComponent<Rigidbody> ();
		//childRb.constraints = RigidbodyConstraints.FreezePositionY;
		// 空気抵抗など
		childRb.drag = 5;
		childRb.angularDrag = 5;

		trainChiled.GetComponent<BoxCollider> ().enabled = true;
		// addforce30
		Vector3 derailForceX = -trainChiled.transform.right;
		Vector3 derailForceY = trainChiled.transform.up;
		Vector3 derailForceZ = trainChiled.transform.forward;
		childRb.AddForce (derailForceX * 500 + derailForceY * 10 + derailForceZ * 50);
		// 爆発
		if (explosionParticle != null) {
			GameObject explosionObj = Instantiate (explosionParticle, trainChiled.transform.position, Quaternion.identity);
			Destroy (explosionObj, 2.5f);
		}
		StartCoroutine (ReloadScene ());
	}

	// n秒後にシーンのリロードをする
	private IEnumerator ReloadScene () {
		yield return new WaitForSeconds (3);

		Destroy (train);
		Destroy (trainChiled);
		bgmController.bgmReset ();
		dangerImg.SetActive (false);
		RailCreateManager.Instance.rootExistence = true; // 電車生成に関するフラグ
		RailCreateManager.Instance.isDerail = true; // いらないかも??
		getTrain = false;
		createTrain.trainDeparture = true;
		speedBtnCtl.CheckFrag = true;
		// #if UNITY_EDITOR_OSX 
		// 		SceneManager.LoadScene ("CreateRailScene");
		// #elif UNITY_IOS 
		// 		SceneManager.LoadScene ("ARTestScene");
		// #endif
	}

}