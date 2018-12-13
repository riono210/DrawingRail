using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


/*
	使い方(BGMをリセットする)
	private BGMController bgmController;
	void Start(){
		bgmController = GameObject.Find("BGM").GetComponent<BGMController>();
	}
	bgmController.bgmReset();
 */
 
public class BGMController : MonoBehaviour {
	public AudioSource bgm;
	public AudioClip titleBGM;
	public AudioClip trainBGM;
	public AudioMixer masterMixer;

	private string beforeScene = "TitleScene";

	private float initSpeed = 0.5f; //電車が走るシーンでのbgmの初期スピード
	private float initPitch = 2.0f; //電車が走るシーンでのbgmの初期ピッチ

	private float speed = 1.0f;
	private float pitch = 1.0f;
	private float bgmInterval = 1.08f;

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
        if (beforeScene == "WriteLineScene" && nextScene.name == "ARTestScene" || beforeScene == "WriteLineScene" && nextScene.name == "CreateRailScene") {
            bgm.Stop ();
            bgm.clip = trainBGM;    //流すクリップを切り替える
            bgm.Play ();
			speed = initSpeed;
			pitch = initPitch;
			setAudioMixerParam(speed, pitch);
        }
        //電車が走るシーンから戻った時
        if (beforeScene == "ARTestScene" || beforeScene == "CreateRailScene" ){
            bgm.Stop ();
            bgm.clip = titleBGM;    //流すクリップを切り替える
            bgm.Play ();
			speed = 1.0f;
			pitch = 1.0f;
			setAudioMixerParam(speed, pitch);
        }
		beforeScene = nextScene.name; //prevSceneはNullが入るので、こちらで代用
    }

	//AudioMixerのPitch(speed)とpitchShifter.pitch(pitch)を変更する関数
	public void setAudioMixerParam(float speed, float pitch){
		masterMixer.SetFloat("speed", speed);
		masterMixer.SetFloat("pitch", pitch);
	}

	//BGMを速くする関数
	public void pitchUp(){
		speed *= bgmInterval;
		pitch /= bgmInterval;
		setAudioMixerParam(speed, pitch);
	}

	//BGMを遅くする関数
	public void pitchDown(){
		if (speed > initSpeed){
			speed /= bgmInterval;
			pitch *= bgmInterval;
			setAudioMixerParam(speed, pitch);
		}
	}

	//BGMをリセットする関数
	public void bgmReset(){
		speed = initSpeed;
		pitch = initPitch;
		setAudioMixerParam(speed, pitch);
		bgm.Stop();
		bgm.Play();
	}
}
 

