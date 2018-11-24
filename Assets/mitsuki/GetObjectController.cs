using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetObjectController : MonoBehaviour {

    // Use this for initialization

    private GameObject TrainObject; //動的に生成された電車オブジェクト
    private ObjectController objectcontroller; //電車についているObjectController

    public Button SpeedUpButton; //UIのボタン
    public Button SpeedDownButton; //UIのボタン

    [HideInInspector] public bool CheckFrag = true; //CheckTrainを一度だけ実行させる
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        CheckTrain ();

    }

    private void CheckTrain () {
        if (RailCreateManager.Instance.trainExistence && CheckFrag) {
            //電車を取得する
            CreateTrain createtrain = this.gameObject.GetComponent<CreateTrain> ();

            TrainObject = createtrain.GetTrainInst ();
            Debug.Log ("電車" + TrainObject);

            //電車についたObjectControllerを持ってくる
            objectcontroller = TrainObject.GetComponent<ObjectController> ();

            // イベントが重複しないように削除
            SpeedUpButton.onClick.RemoveAllListeners ();
            SpeedDownButton.onClick.RemoveAllListeners ();

            //ボタンが押された時に実行する関数の指定
            SpeedUpButton.onClick.AddListener (() => objectcontroller.GoButtonDown ());
            SpeedDownButton.onClick.AddListener (() => objectcontroller.StopButtonDown ());

            CheckFrag = false;
        }
    }
}