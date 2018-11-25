using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateTrain : MonoBehaviour {

	public GameObject trainObj; // 電車オブジェクト
	private GameObject trainInst; // 生成した電車オブジェクト
	public GameObject viewFiled; // レールオブジェクトの親
	private GameObject railObj; // レールオブジェクト

	private int railNum; // レールの数   

	private void Start () {
#if UNITY_EDITOR_OSX
		// レールobjとレールの数を取得
		railObj = viewFiled.transform.Find ("RailParent").gameObject;
		Debug.Log (railObj.name);
		railNum = RailCreateManager.Instance.railNum;
#endif
	}

	private void Update () {
#if UNITY_EDITOR_OSX  // editor
		MakeTrain ();

		if (!RailCreateManager.Instance.trainExistence) {
			try {
				trainInst.GetComponent<TrainDeparture> ();
				if (trainInst.GetComponent<TrainDeparture> ().departure) {
					StartCoroutine (StartDeparture ());
				}
			} catch (System.NullReferenceException) {
				//Debug.Log ("error");
			}
		}
		// if (trainInst.GetComponent<TrainDeparture> () != null && trainInst.GetComponent<TrainDeparture> ().departure) {
		// 	StartCoroutine (StartDeparture ());
		// } else {
		// 	Debug.Log ("error");
		// }

#elif UNITY_IOS    // 実機
		Debug.Log ("ios");
		// 実機では動的にViewFiledが生成されるため、生成後に取得
		if (RailCreateManager.Instance.ARFiledExist) {
			if (viewFiled == null) {
				viewFiled = GameObject.Find ("ViewFiled");
				// Debug.Log ("ダメです");
			}
			railObj = viewFiled.transform.Find ("RailParent").gameObject;
			Debug.Log (railObj.name);
			railNum = RailCreateManager.Instance.railNum;

			MakeTrain ();

			if (trainInst.GetComponent<TrainDeparture> () != null && trainInst.GetComponent<TrainDeparture> ().departure) {
				StartCoroutine (StartDeparture ());
			}

		}
#endif
	}

	// 電車の生成
	private void MakeTrain () {
		// NavMeshが存在していないとエラーが起こるため
		if (RailCreateManager.Instance.rootExistence) {
			// レールの0番目を取得
			Vector3 startRailPos = railObj.transform.GetChild (0).position + (Vector3.up * 0.1f);
			Debug.Log (startRailPos);

			//レールのインスタンス生成
			trainInst = Instantiate (trainObj,
				startRailPos,
				Quaternion.identity);

			SetRoot ();

			RailCreateManager.Instance.rootExistence = false;
		}
	}

	// 電車にrootの設定
	private void SetRoot () {
		// 目的地点の配列を初期化
		ObjectController trainCtr = trainInst.GetComponent<ObjectController> ();
		trainCtr.m_target = new Transform[railNum];

		int index = 0;
		// 子供を全取得
		var railChiled = railObj.transform.GetComponentsInChildren<Transform> ();
		foreach (var value in railChiled) {
			// 0番目にrailObj自体が入ってしまうのでコライダーの有無で判断
			if (value.GetComponent<BoxCollider> ()) {
				trainCtr.m_target[index] = value;
				Debug.Log (value);
				index++;
			}
		}
	}

	// そのままだとNavMeshが効かないため、一度消してactiveにする
	private IEnumerator StartDeparture () {
		trainInst.SetActive (false);

		yield return new WaitForSeconds (0.5f);

		trainInst.GetComponent<ObjectController> ().enabled = true;
		trainInst.SetActive (true);
		// 電車が生成されたと通知
		RailCreateManager.Instance.trainExistence = true;
		// 誤反応防止のため削除
		Destroy (trainInst.GetComponent<TrainDeparture> ());
	}

	// 生成したtrainを返す
	public GameObject GetTrainInst () {
		GameObject train;
		if (trainInst != null) {
			train = trainInst;
		} else {
			train = null;
		}
		return train;
	}
}