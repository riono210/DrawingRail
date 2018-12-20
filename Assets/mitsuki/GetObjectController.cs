using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetObjectController : MonoBehaviour {

    // Use this for initialization

    private GameObject TrainObject; //動的に生成された電車オブジェクト
    private ObjectController objectcontroller; //電車についているObjectController

    public GameObject SpeedUpButton; //UIのボタン
    public GameObject SpeedDownButton; //UIのボタン
    public GameObject SpeedMeter; //メーター

    public Text headText; // ヘッダーテキスト

    [HideInInspector] public bool CheckFrag = true; //CheckTrainを一度だけ実行させる
    void Start () {
        SpeedUpButton.SetActive (false);
        SpeedDownButton.SetActive (false);
        SpeedMeter.SetActive (false);
        headText.text = ("カメラをゆかにむけよう");
    }

    // Update is called once per frame
    void Update () {
        CheckTrain ();

    }

    private void CheckTrain () {
        if (RailCreateManager.Instance.trainExistence && CheckFrag) {
            SpeedUpButton.SetActive (true);
            SpeedDownButton.SetActive (true);
            SpeedMeter.SetActive (true);

            headText.text = ("はしらせよう");
            //電車を取得する
            CreateTrain createtrain = this.gameObject.GetComponent<CreateTrain> ();

            TrainObject = createtrain.GetTrainInst ();
            Debug.Log ("電車" + TrainObject);

            //電車についたObjectControllerを持ってくる
            objectcontroller = TrainObject.GetComponent<ObjectController> ();

            // 初速度
            objectcontroller.SetSpeed (0.05f);

            // イベントトリガー取得
            EventTrigger SpUpBtnTrig = SpeedUpButton.GetComponent<EventTrigger> ();
            EventTrigger SpDownBtnTrig = SpeedDownButton.GetComponent<EventTrigger> ();

            // イベントが重複しないように削除
            SpUpBtnTrig.triggers = null;
            SpDownBtnTrig.triggers = null;

            // イベントトリガーに登録
            EventTrigger.Entry entryGo = new EventTrigger.Entry ();
            entryGo.eventID = EventTriggerType.PointerDown;
            entryGo.callback.AddListener ((x) => objectcontroller.GoButton ());
            SpUpBtnTrig.triggers.Add (entryGo);

            EventTrigger.Entry entryStop = new EventTrigger.Entry ();
            entryStop.eventID = EventTriggerType.PointerDown;
            entryStop.callback.AddListener ((x) => objectcontroller.StopButton ());
            SpDownBtnTrig.triggers.Add (entryStop);

            // イベントが重複しないように削除
            // SpeedUpButton.onClick.RemoveAllListeners ();
            // SpeedDownButton.onClick.RemoveAllListeners ();

            //ボタンが押された時に実行する関数の指定
            // SpeedUpButton.onClick.AddListener (() => objectcontroller.GoButton ());
            // SpeedDownButton.onClick.AddListener (() => objectcontroller.StopButton ());

            CheckFrag = false;
        }
    }
}