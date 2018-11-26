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
	// レールの数
	[HideInInspector] public int railNum;
	// manager staticなため取得可能
	[HideInInspector] public GameObject manager;
    //Sprite型の選択した電車の画像
    [HideInInspector] public Sprite SelectTrain;

    // 実機上でViewFiledが生成されたか  0
    [HideInInspector] public bool ARFiledExist;
	// NavMeshを生成できるか  1
	[HideInInspector] public bool railExistence;
	// レールが調整されたか　　2
	[HideInInspector] public bool shapeRail;
	// 電車の生成ができるか  3
	[HideInInspector] public bool rootExistence;
	// 電車が生成されたか  4
	[HideInInspector] public bool trainExistence;
	// 電車が脱線したか　  5
	[HideInInspector] public bool isDerail;

	// 初期化
	protected override void Awake () {
		Debug.Log ("init!");
		base.Awake ();

		linePoints = new List<Vector3> ();
		createRender = null;
		positionDiff = Vector3.zero;
		railNum = 0;
		manager = this.gameObject;
        SelectTrain = null;

		ARFiledExist = false;
		railExistence = false;
		shapeRail = false;
		rootExistence = false;
		trainExistence = false;
		isDerail = false;
	}

	// Use this for initialization
	void Start () {

	}
}