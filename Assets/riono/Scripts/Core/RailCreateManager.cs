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

	// 初期化
	protected override void Awake () {
		Debug.Log ("init!");
		base.Awake ();

		linePoints = new List<Vector3> ();
		createRender = null;
		positionDiff = Vector3.zero;
	}

	// Use this for initialization
	void Start () {

	}
}