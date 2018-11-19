using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCreateManager : SingletonMonoBehaviour<RailCreateManager> {

	// 頂点のリスト(レールの元)
	[HideInInspector] public List<Vector3> linePoints;
	// 描いたレールのLineRenderer
	[HideInInspector] public LineRenderer createRender;
	// posの差異
	[HideInInspector] public Vector3 positionDiff;
	// NavMeshを生成できるか
	[HideInInspector] public bool railExistence;
	// 電車の生成ができるか
	[HideInInspector] public bool rootExistence;
	// レールの数
	[HideInInspector] public int railNum;
	// 実機上でViewFiledが生成されたか
	[HideInInspector] public bool ARFiledExist;
	// 電車が生成されたか
	[HideInInspector] public bool trainExistence;
	// manager staticなため取得可能
	[HideInInspector] public GameObject manager;

	// 初期化
	protected override void Awake () {
		Debug.Log ("init!");
		base.Awake ();

		linePoints = new List<Vector3> ();
		createRender = null;
		positionDiff = Vector3.zero;
		railExistence = false;
		rootExistence = false;
		railNum = 0;
		ARFiledExist = false;
		trainExistence = false;
		manager = this.gameObject;
	}

	// Use this for initialization
	void Start () {

	}
}