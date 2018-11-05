using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRale : MonoBehaviour {

    //public FreeHand freeHand; // 頂点リストを描画保持するクラス
    private List<Vector3> linePoints; // 頂点を保持したリスト

    public GameObject filedObj; // 親オブジェクト
    public GameObject lineObject; // 線オブジェクト
    private LineRenderer railRender; // 線を描くコンポーネント
    public Material lineMaterial; // 描く線のマテリアル
    public Color lineColor; // 描く線の色
    [Range (0, 10)] public float lineWidth; // 描く線の太さ

    private int totalCount; // 頂点の数
    public GameObject railPrefab; // レールプレファブ

    // Use this for initialization
    void Start () {
        makeRoot ();
        AddLineRenderer ();
        AddNewPoints ();
        CreateRailInstance ();
    }

    // Update is called once per frame
    void Update () {

    }

    // 前のシーンで描いたLineRendererを取得
    public void makeRoot () {
        linePoints = RailCreateManager.Instance.linePoints;

        // foreach (var value in linePoints) {
        //     Debug.Log (value);
        // }
    }

    // 仮の台オブジェクトの子供にLineRendererを設定
    public void AddLineRenderer () {
        // 追加するオブジェクトをインスタンス
        lineObject = new GameObject ();
        lineObject.transform.parent = filedObj.transform;

        // オブジェクトにLineRendererを取り付ける
        lineObject.AddComponent<LineRenderer> ();

        // 描く線のコンポーネントリストに追加する
        railRender = lineObject.GetComponent<LineRenderer> ();

        // 線と線をつなぐ点の数を0に初期化
        railRender.positionCount = 0;

        // マテリアルを初期化
        railRender.material = this.lineMaterial;

        // 線の色を初期化
        railRender.material.color = this.lineColor;

        // 線の太さを初期化
        railRender.startWidth = this.lineWidth;
        railRender.endWidth = this.lineWidth;
    }

    // 取得したLineRrendererから線を描画
    public void AddNewPoints () {
        int count = 0;
        float xDiffRange = 0;
        float zDiffRange = 0;
        foreach (var value in linePoints) {
            Vector3 newPos = value;

            // 初期値を保存して位置調整
            if (count == 0) {
                xDiffRange = newPos.x;
                zDiffRange = newPos.z;
            }
            newPos = new Vector3 (newPos.x - xDiffRange, newPos.y, newPos.z - zDiffRange);

            // 線と線をつなぐ点の数を更新
            railRender.positionCount += 1;
            //Debug.Log("pos:" + mousePosition);

            // 描く線のコンポーネントリストを更新
            railRender.SetPosition (railRender.positionCount - 1, newPos);

            CreatePointObj (count, newPos);

            count++;
        }
        totalCount = count;
        RailCreateManager.Instance.createRender = railRender;
    }

    // 頂点とオブジェクトを紐付け
    private void CreatePointObj (int count, Vector3 position) {
        // newObjectの名前と親,位置を設定
        GameObject　 pointObj = new GameObject ();
        pointObj.name = "point_" + count;
        pointObj.transform.parent = lineObject.transform;
        pointObj.transform.localPosition = railRender.GetPosition (count);

        // 頂点が移動すると点も移動するスクリプトをaddcomponent
        PointToObject pointToObj = pointObj.AddComponent<PointToObject> ();
        pointToObj.lineIndex = count;
    }

    // レールインスタンスを生成
    private void CreateRailInstance () {
        var childPoints = lineObject.transform.GetComponentInChildren<Transform> ();

        for (int i = 0; i < totalCount; i++) {
            // float childIndex = childTransfrom.GetComponent<PointToObject> ().lineIndex;

            Transform currentChild = lineObject.transform.GetChild (i);
            Transform nextChild = lineObject.transform.GetChild ((i + 1) % totalCount);

            // Debug.Log ("orig:" + currentChild.name + " next;" + nextChild.name);
            float xzAngle = BetweenAngleXZ (currentChild.position, nextChild.position);
            //Vector3 position = HalfPoint (currentChild.position, nextChild.position);
            Vector3 position = currentChild.position;
            //Debug.Log ("i:" + currentChild.position + " ne" + nextChild.position);
            // インスタンス生成
            GameObject raileInstance = Instantiate (railPrefab,
                position,
                Quaternion.Euler (0, -xzAngle, 0),
                lineObject.transform);

            raileInstance.name = "rail_" + i;
        }
    }

    // 二点間の中心点
    private Vector3 HalfPoint (Vector3 origin, Vector3 next) {
        float xPos = next.x - origin.x;
        float yPos = next.y - origin.y;
        float zPos = next.z - origin.z;
        Debug.Log ("x:" + xPos + " y:" + yPos + " z:" + zPos);
        return new Vector3 (xPos, yPos, zPos);
    }

    // 二点間の角度を求める
    private float BetweenAngleXZ (Vector3 origin, Vector3 next) {
        float dx = next.x - origin.x;
        float dz = next.z - origin.z;

        float rad = Mathf.Atan2 (dz, dx);
        return rad * Mathf.Rad2Deg;
    }
}