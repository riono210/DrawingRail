using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour {

	public void ButtonClicked(){
		string SceneName =	SceneManager.GetActiveScene().name;

		switch(SceneName){
			case "WriteLineScene":
				SceneManager.LoadScene("TitleScene");
				break;
	
			case "LoadPictureScene":
				SceneManager.LoadScene("WriteLineScene");
				break;

			case "CreateRailScene":
				SceneManager.LoadScene("LoadPictureScene");
				break;

			default:
				Debug.Log(SceneManager.GetActiveScene().name + "がコードに書かれてません。");
				break;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}