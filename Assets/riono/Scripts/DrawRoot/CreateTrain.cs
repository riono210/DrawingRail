using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateTrain : MonoBehaviour {

	public GameObject trainObj; // 電車オブジェクト
	private GameObject trainInst; // 生成した電車オブジェクト
	public GameObject viewFiled; // レールオブジェクトの親
	private GameObject railObj; // レールオブジェクト

	public NavMeshSurface NMSurface;
	private int railNum;

	private void Start () {
		railObj = viewFiled.transform.Find ("RailParent").gameObject;
		Debug.Log (railObj.name);
		railNum = RailCreateManager.Instance.railNum;

	}

	private void Update () {
		MakeTrain ();

		if (trainInst.GetComponent<TrainDeparture> () != null && trainInst.GetComponent<TrainDeparture> ().departure) {
			StartCoroutine (StartDeparture ());
		}
	}

	// 電車の生成
	private void MakeTrain () {

		if (RailCreateManager.Instance.rootExistence) {
			Vector3 startRailPos = railObj.transform.GetChild (0).position + (Vector3.up * 0.1f);
			Debug.Log (startRailPos);

			trainInst = Instantiate (trainObj,
				startRailPos,
				Quaternion.identity);

			SetRoot ();

			RailCreateManager.Instance.rootExistence = false;
		}
	}

	// 電車にrootの設定
	private void SetRoot () {
		ObjectController trainCtr = trainInst.GetComponent<ObjectController> ();

		trainCtr.m_target = new Transform[railNum];

		int index = 0;
		var railChiled = railObj.transform.GetComponentsInChildren<Transform> ();
		foreach (var value in railChiled) {
			if (value.GetComponent<BoxCollider> ()) {
				trainCtr.m_target[index] = value;
				Debug.Log (value);
				index++;
			}
		}
	}

	private IEnumerator StartDeparture () {
		trainInst.SetActive (false);

		yield return new WaitForSeconds (0.5f);

		trainInst.GetComponent<ObjectController> ().enabled = true;
		trainInst.SetActive (true);
		// 誤反応防止のため削除
		Destroy (trainInst.GetComponent<TrainDeparture> ());

	}
}