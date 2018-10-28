using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ObjectMaker : MonoBehaviour {
    public GameObject obj;

    void CreateObj(Vector3 atPosition) {
        GameObject newBall = Instantiate(obj, atPosition, Quaternion.identity);
    }

    void Update() {
        // タッチ入力確認
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                ARPoint point = new ARPoint {
                    x = screenPosition.x,
                    y = screenPosition.y
                };
                // スクリーンの座標をWorld座標に変換
                List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
                if (hitResults.Count > 0) {
                    foreach (var hitResult in hitResults) {
                        Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                        CreateObj(new Vector3(position.x, position.y, position.z));
                        break;
                    }
                }
            }
        }
    }
}