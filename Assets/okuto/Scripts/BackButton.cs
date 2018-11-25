using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {
	public AudioSource audioSource;
	public void ButtonClicked(){
		audioSource.Play();
		float SoundTime = 0.1f;
	    //DelayMethodを3.5秒後に呼び出す
    	Invoke("MoveScene", SoundTime);
	}


	void MoveScene(){
		string SceneName =	SceneManager.GetActiveScene().name;
		// CreateRailScene -> WriteLineScene -> Picker -> TitleScene
		switch(SceneName){
			case "CreateRailScene":
				SceneManager.LoadScene("WriteLineScene");
				break;
			case "WriteLineScene": 
				SceneManager.LoadScene("Picker");
				break;	
			case "Picker":
				SceneManager.LoadScene("TitleScene");
				break;
			default:
				Debug.Log(SceneManager.GetActiveScene().name + "がコードに書かれてません。");
				break;
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}