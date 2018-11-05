using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChar : MonoBehaviour {

    public Camera mainCamera;
    public GameObject character;
    private int ScreensizeX;
    private float startPos;
    private float movePos;

    private float defoltRotate;



    //private float calc;
    // Use this for initialization
    void Start() {
        ScreensizeX = Screen.currentResolution.width;
        defoltRotate = character.transform.rotation.eulerAngles.y;

    }

    // Update is called once per frame
    void Update() {
        Rotate();
    }


    public void Rotate() {
        float screenHalf = ScreensizeX;
        float rate = 360 / (screenHalf);
        float startRotate = 0;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) {

            //startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            // 開始位置
            startPos = Input.mousePosition.x;

            // 開始時角度
            startRotate = character.transform.eulerAngles.y;
        }

        if (Input.GetMouseButton(0)) {
            // 移動量
            movePos = Input.mousePosition.x;
            Debug.Log(movePos);

            // 回転計算
            float dist = Mathf.Abs(startPos - movePos);
            float calc = ((dist * rate) / 10);


            if (startPos > movePos) {
                character.transform.Rotate(new Vector3(0, startRotate + calc, 0));
            } else if (startPos < movePos) {
                character.transform.Rotate(new Vector3(0, startRotate - calc, 0));
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            Debug.Log("up");
            startPos = 0;
            movePos = 0;
        }


#elif UNITY_IOS
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                // 開始位置
                //startPos = Input.mousePosition.x;
                //startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
                startPos = touch.position.x;

                // 開始時角度
                startRotate = character.transform.eulerAngles.y;
            }

            if (touch.phase == TouchPhase.Moved) {
                // 移動量
                //movePos = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
                movePos = touch.position.x;

                // 回転計算
                float dist = Mathf.Abs(startPos - movePos);
                float calc = ((dist * rate) / 10);


                if (startPos > movePos) {
                    character.transform.Rotate(new Vector3(0, startRotate + calc, 0));
                } else if (startPos < movePos) {
                    character.transform.Rotate(new Vector3(0, startRotate - calc, 0));
                }
            }

            if (touch.phase == TouchPhase.Ended) {
                Debug.Log("up");
                startPos = 0;
                movePos = 0;
            }

        }
   
#endif
    }
}
