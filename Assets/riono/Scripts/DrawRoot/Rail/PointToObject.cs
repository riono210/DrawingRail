using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToObject : MonoBehaviour {

	// 頂点インデックス
	public int lineIndex;
	private LineRenderer railRender;

	// Use this for initialization
	void Start () {
		// シングルトン取得
		railRender = RailCreateManager.Instance.createRender;
		// 位置調整
		//this.transform.position = railRender.GetPosition (lineIndex);
		//Debug.Log (lineIndex + " " + railRender.GetPosition (lineIndex));

	}

	// Update is called once per frame
	void Update () {
		// 本番ではいらない??
		MovePoint ();
	}

	// GameObjectと位置と頂点の位置を同期させる
	public void MovePoint () {
		railRender.SetPosition (lineIndex, this.transform.position);
	}
}