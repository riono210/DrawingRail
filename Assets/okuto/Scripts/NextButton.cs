using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour {

	public void ButtonClicked(){
		string SceneName =	SceneManager.GetActiveScene().name;

		switch(SceneName){
			case "TitleScene":
				SceneManager.LoadScene("WriteLineScene");
				break;

			case "WriteLineScene":
				SceneManager.LoadScene("LoadPictureScene");
				break;
	
			case "LoadPictureScene":
				SceneManager.LoadScene("ARTestScene");
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