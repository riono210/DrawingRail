using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButtonClicked : MonoBehaviour {
	public AudioSource audioSource;
	public AudioSource bgm;
	public void ButtonClicked(){
		audioSource.Play();
		Destroy(bgm);
	}

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
