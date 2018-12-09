using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bgmPitchManager : MonoBehaviour {
	public static AudioSource bgm;
	public AudioClip titleBGM;
	public AudioClip trainBGM;

	private string beforeScene = "TitleScene";

	private static float minPitch = 0.5f;
	private static float maxPitch = 2.0f;
	private static float pitchInterval = 0.05f;

	// Use this for initialization
	void Start () {
		bgm = gameObject.GetComponent<AudioSource>();
		bgm.clip = titleBGM;
		bgm.Play();
        //シーンが切り替わった時に呼ばれるメソッドを登録
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//シーンが切り替わった時に呼ばれるメソッド
    void OnActiveSceneChanged ( Scene prevScene, Scene nextScene ) {
        //電車が走るシーンへ移動
        if (beforeScene == "WriteLineScene" && nextScene.name == "ARTestScene" | beforeScene == "WriteLineScene" && nextScene.name == "CreateRailScene") {
            bgm.Stop ();
            bgm.clip = trainBGM;    //流すクリップを切り替える
            bgm.Play ();
			bgm.pitch = 0.5f;
        }

        //電車が走るシーンから戻った時
        if (beforeScene == "ARTestScene" | beforeScene == "CreateRailScene" ){
            bgm.Stop ();
            bgm.clip = titleBGM;    //流すクリップを切り替える
            bgm.Play ();
			bgm.pitch = 1.0f;
        }
		beforeScene = nextScene.name; //prevSceneはNullが入るので、こちらで代用
    }

	//BGMのピッチをあげる
	public static void pitchUp(){
		Debug.Log("currentPitch = " + bgm.pitch + "maxPitch = " + maxPitch);
		if(bgm.pitch < maxPitch){
			bgm.pitch += pitchInterval;
		}
	}

	//BGMのピッチを下げる
	public static void pitchDown(){
		Debug.Log("currentPitch = " + bgm.pitch + "##################### minPitch = " + minPitch);
		if(bgm.pitch > minPitch){
			bgm.pitch -= pitchInterval;
		}
	}

	//BGMのピッチを指定する
	public static void pitchSet(float n){
		bgm.pitch = n;
	}
}
 

