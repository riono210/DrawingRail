using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCreateManager : SingletonMonoBehaviour<RailCreateManager> {

	// 頂点のリスト(レールの元)
	[HideInInspector] public List<Vector3> linePoints;
	// 描いたレールのLineRenderer
	[HideInInspector] public LineRenderer createRender;

	// 初期化
	protected override void Awake () {
		Debug.Log ("init!");
		base.Awake ();

		linePoints = new List<Vector3> ();
		createRender = null;
	}

	// Use this for initialization
	void Start () {

	}
}