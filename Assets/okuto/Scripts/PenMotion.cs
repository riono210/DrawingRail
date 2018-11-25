using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenMotion : MonoBehaviour {
	public GameObject pen;
	public bool isDisplay;
	public float speed;
	public float radius;
	public float centerX;
	public float centerY;
	// Use this for initialization
	void Start () {
		pen.SetActive(true);
		isDisplay = true;
		speed = 1.5f;
		radius = (float)(Screen.height/5);
		centerX = (float)(Screen.width/2);
		centerY = (float)( (Screen.height/5)*3 );
		Debug.Log(centerY);
	}
	
	// Update is called once per frame
	void Update () {
		if(isDisplay){
			float x = centerX - Mathf.Cos(Time.time * speed) * radius;
			float y = centerY + Mathf.Sin(Time.time * speed) * radius;
			transform.position = new Vector3(x, y, 0f);
		}

		if(Input.GetMouseButton(0)){
			isDisplay = false;
			pen.SetActive(false);
		}

	}
}
