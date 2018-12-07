using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButton : MonoBehaviour {
	public AudioSource audioSource;

	public void ButtonClicked () {
		audioSource.Play ();
		float SoundTime = 0.15f;
		//DelayMethodを3.5秒後に呼び出す
		Invoke ("MoveScene", SoundTime);
	}

	void MoveScene () {
		// TitleScene -> Picker -> WriteLineScene -> CreateRailScene
		string SceneName = SceneManager.GetActiveScene ().name;
		switch (SceneName) {
			case "TitleScene":
				//SceneManager.LoadScene("Picker");
				Fanctions.Instance.LoadScene ("Picker");
				break;
			case "Picker":
				//SceneManager.LoadScene ("WriteLineScene");
				Fanctions.Instance.LoadScene ("WriteLineScene");
				break;
			case "WriteLineScene":
				//SceneManager.LoadScene ("CreateRailScene");
				Fanctions.Instance.LoadScene ("CreateRailScene");
				break;
			default:
				Debug.Log (SceneManager.GetActiveScene ().name + "というシーンは存在しません");
				break;
		}
	}

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}
}