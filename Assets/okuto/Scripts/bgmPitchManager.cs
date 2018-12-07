using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmPitchManager : MonoBehaviour {
	public AudioSource bgm;
	public float minPitch;
	private float maxPitch = 2.0f;
	private float pitchInterval = 0.1f;
	// Use this for initialization
	void Start () {
		bgm = gameObject.GetComponent<AudioSource>();
		bgm.pitch = minPitch;
		DontDestroyOnLoad(bgm);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//BGMのピッチをあげる
	public void pitchUp(){
		if(bgm.pitch < maxPitch){
			bgm.pitch += pitchInterval;
		}
	}
	public void pitchDown(){
		if(bgm.pitch > minPitch){
			bgm.pitch -= pitchInterval;
		}
	}
}

