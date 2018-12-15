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
    public GameObject entityPrefab; // 実体レールプレファブ
    [HideInInspector] public GameObject railParent;
    // 実体のレールの親
    [HideInInspector] public GameObject entityRailParet;

    // Use this for initialization
    void Start () {
        makeRoot ();
        AddLineRenderer ();
        AddNewPoints ();
        CreateRailInstance ();
        RailCreateManager.Instance.railExistence = true;
    }

    // Update is called once per frame
    void Update () {
        if (RailCreateManager.Instance.shapeRail) {
            AdjustRail ();
        }
    }

    // 前のシーンで描いたLineRendererを取得
    public void makeRoot () {
        linePoints = RailCreateManager.Instance.linePoints;
    }

    // 仮の台オブジェクトの子供にLineRendererを設定
    public void AddLineRenderer () {
        // 追加するオブジェクトをインスタンス
        lineObject = new GameObject ();
        lineObject.name = "LineParent";
        lineObject.transform.parent = filedObj.transform;
#if UNITY_EDITOR
        lineObject.transform.localPosition = new Vector3 (0, 1, 0);
#else
        lineObject.transform.localPosition = Vector3.zero;
#endif
        //lineObject.transform.localScale = new Vector3 (0.5f, 3, 0.5f);

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

            newPos = new Vector3 (newPos.x - diff.x, newPos.y + 0.05f, newPos.z - diff.z);

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
        // 線の名前と親,位置を設定
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
        // レールの親を生成
        railParent = new GameObject ();
        railParent.name = "RailParent";
        railParent.transform.parent = filedObj.transform;
        int railCount;

        // レールの実体の親を生成
        entityRailParet = new GameObject ();
        entityRailParet.name = "EntityRailParent";
        entityRailParet.transform.parent = filedObj.transform;

        for (railCount = 0; railCount < totalCount; railCount++) {
            // float childIndex = childTransfrom.GetComponent<PointToObject> ().lineIndex;
            // 今と次の子供を取得
            Transform currentChild = lineObject.transform.GetChild (railCount);
            Transform nextChild = lineObject.transform.GetChild ((railCount + 1) % totalCount);

            // 角度の計算
            float xzAngle = BetweenAngleXZ (currentChild.position, nextChild.position);
            // 二点間の中間位置を計算
            Vector3 newPosition = HalfPoint (currentChild.position, nextChild.position);

            // インスタンス生成
            GameObject raileInstance = Instantiate (railPrefab,
                newPosition,
                Quaternion.Euler (0, -xzAngle, 0),
                railParent.transform);

            SetSize (currentChild.position, nextChild.position, raileInstance.transform);
            raileInstance.name = "rail_" + railCount;

            //if (railCount % 2 == 0 && railCount < totalCount) {
                // 実体インスタンス
                GameObject entityRailInstanc = Instantiate (entityPrefab,
                    newPosition,
                    Quaternion.Euler (90, -xzAngle, 0),
                    entityRailParet.transform);

                EntitySetSize (currentChild.position, nextChild.position, entityRailInstanc.transform);
                entityRailInstanc.name = "entityRail_" + railCount;
            //}
        }

        RailCreateManager.Instance.railNum = railCount;

        lineObject.SetActive (false);
    }

    // 二点間の中心点
    private Vector3 HalfPoint (Vector3 origin, Vector3 next) {
        float xPos = (next.x + origin.x) / 2;
        float yPos = (next.y + origin.y) / 2;
        float zPos = (next.z + origin.z) / 2;
        return new Vector3 (xPos, yPos, zPos);
    }

    // レールのサイズ(長さ)調整
    private void SetSize (Vector3 origin, Vector3 next, Transform rail) {

        float diff = Vector3.Distance (origin, next);
        Vector3 railSize = rail.transform.localScale;
        //Debug.Log ("diff:" + diff + " rail:" + railSize.x);

        // 二点間の距離がレールの元サイズよりも大きいとき
        if (diff > railSize.x) {
            // 調整値を計算
            float xSize = (diff - railSize.x);
            // NavMeshができるよう調整
            xSize *= 1.2f;

            rail.transform.localScale = new Vector3 (railSize.x + xSize, railSize.y, railSize.z);

        } else if (diff <= railSize.x) { // 二点間の距離がレールの元サイズよりも小さい時
            // 調整値を計算
            float xSize = (railSize.x - diff);
            // NavMeshができるよう調整
            xSize *= 0.5f;

            rail.transform.localScale = new Vector3 (railSize.x - xSize, railSize.y, railSize.z);
        }
    }

    // レールのサイズ(長さ)調整
    private void EntitySetSize (Vector3 origin, Vector3 next, Transform rail) {

        float diff = Vector3.Distance (origin, next);
        Vector3 railSize = rail.transform.localScale;
        //Debug.Log ("diff:" + diff + " rail:" + railSize.x);

        // 二点間の距離がレールの元サイズよりも大きいとき
        if (diff > railSize.x) {
            // 調整値を計算
            float xSize = (diff - railSize.x);
            // NavMeshができるよう調整
            xSize *= 1.2f;

            rail.transform.localScale = new Vector3 (railSize.x + xSize, railSize.y, railSize.z);

        } else if (diff <= railSize.x) { // 二点間の距離がレールの元サイズよりも小さい時
            // 調整値を計算
            float xSize = (railSize.x - diff);
            // NavMeshができるよう調整
            xSize *= 0.7f;

            rail.transform.localScale = new Vector3 (railSize.x - xSize, railSize.y, railSize.z);
        }
    }

    // 二点間の角度を求める
    private float BetweenAngleXZ (Vector3 origin, Vector3 next) {
        float dx = next.x - origin.x;
        float dz = next.z - origin.z;

        float rad = Mathf.Atan2 (dz, dx);
        return rad * Mathf.Rad2Deg;
    }

    // レール幅の再調整　NavMesh適応後
    private void AdjustRail () {

        // var railChiled = railParent.transform.GetComponentsInChildren<Transform> ();
        // foreach (var value in railChiled) {
        //     // 0番目にrailObj自体が入ってしまうのでコライダーの有無で判断
        //     if (value.GetComponent<BoxCollider> ()) {
        //         Vector3 chiledScale = value.transform.localScale;
        //         value.transform.localScale =
        //             new Vector3 (chiledScale.x, chiledScale.y, 0.08f);
        //         // Debug.Log (value);
        //     }
        // }
        RailCreateManager.Instance.shapeRail = false;
        RailCreateManager.Instance.rootExistence = true;
    }

}