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
    }

    // 仮の台オブジェクトの子供にLineRendererを設定
    public void AddLineRenderer () {
        // 追加するオブジェクトをインスタンス
        lineObject = new GameObject ();
        lineObject.transform.parent = filedObj.transform;
#if UNITY_EDITOR
        lineObject.transform.localPosition = new Vector3 (0, 1, 0);
#else
        lineObject.transform.localPosition = new Vector3 (0, 1.5f, 0);
#endif
        lineObject.transform.localScale = new Vector3 (0.5f, 3, 0.5f);

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
        foreach (var value in linePoints) {
            Vector3 newPos = value;

            // 位置調整
            Vector3 diff = RailCreateManager.Instance.positionDiff;

            //newPos = new Vector3 (newPos.x - xDiffRange, newPos.y + 0.05f, newPos.z - zDiffRange);
            newPos = new Vector3 (newPos.x - diff.x, newPos.y + 0.05f, newPos.z - diff.z);
            Debug.Log ("x:" + diff.x + " z:" + diff.z);

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

            // 角度の計算
            float xzAngle = BetweenAngleXZ (currentChild.position, nextChild.position);
            // 二点間の中間位置を計算
            Vector3 newPosition = HalfPoint (currentChild.position, nextChild.position);

            // インスタンス生成
            GameObject raileInstance = Instantiate (railPrefab,
                newPosition,
                Quaternion.Euler (0, -xzAngle, 0),
                lineObject.transform);

            SetSize (currentChild.position, newPosition, raileInstance.transform);
            raileInstance.name = "rail_" + i;
        }
    }

    // 二点間の中心点
    private Vector3 HalfPoint (Vector3 origin, Vector3 next) {
        float xPos = (next.x + origin.x) / 2;
        float yPos = (next.y + origin.y) / 2;
        float zPos = (next.z + origin.z) / 2;
        return new Vector3 (xPos, yPos, zPos);
    }

    // レールのサイズ調整
    private void SetSize (Vector3 origin, Vector3 middle, Transform rail) {
        float xSize = Vector3.Distance (origin, middle) * 3.8f;
        Vector3 railSize = rail.transform.localScale;
        rail.transform.localScale = new Vector3 (railSize.x + xSize, railSize.y, railSize.z);
    }

    // 二点間の角度を求める
    private float BetweenAngleXZ (Vector3 origin, Vector3 next) {
        float dx = next.x - origin.x;
        float dz = next.z - origin.z;

        float rad = Mathf.Atan2 (dz, dx);
        return rad * Mathf.Rad2Deg;
    }
}