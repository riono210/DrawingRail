using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreeHand : MonoBehaviour {

    //private List<LineRenderer> lineRendererList; // 描く線のコンポーネントリスト
    private LineRenderer lineRenderer;
    private List<Vector3> linePoints; // 頂点の保存
    public Material lineMaterial; // 描く線のマテリアル
    public Color lineColor; // 描く線の色
    [Range (0, 10)] public float lineWidth; // 描く線の太さ
    //private List<GameObject> lineInst; // lineインスタンス
    private GameObject lineInst;

    public GameObject CameraObj; // カメラオブジェクト
    public FieldRange fieldRange; // rangeスクリプト
    private float[] xRange;
    private float[] zRange;

    [HideInInspector] public bool resetFlg; // リセットフラグ
    private bool clickFlg; // ボタンクリックフラグ
    private Vector3 startPoint; // 開始位置

    void Start () {

        if (CameraObj == null) {
            CameraObj = GameObject.Find ("Main Camera");
        }

        linePoints = new List<Vector3> ();
        resetFlg = false;
        clickFlg = false;

        xRange = fieldRange.GetRange ("x");
        zRange = fieldRange.GetRange ("z");
    }

    void Update () {

#if UNITY_EDITOR
        // ボタンが押された時に線オブジェクトの追加を行う
        if (Input.GetMouseButtonDown (0) && !resetFlg) {
            this.AddLineObject ();
        }

        // ボタンが押されている時、LineRendererに位置データの設定を指定していく
        if (Input.GetMouseButton (0) && !resetFlg) {
            this.AddPositionDataToLineRendererList ();
        }

        if (Input.GetMouseButtonUp (0) && !resetFlg && !clickFlg) {
            // LineFin();
            resetFlg = true;
        }
#elif UNITY_IOS

        AddPositonWithIOS ();
#endif
    }

    /// <summary>
    /// 線オブジェクトの追加を行うメソッド
    /// </summary>
    private void AddLineObject () {
        clickFlg = false;

        // 追加するオブジェクトをインスタンス
        GameObject lineObject = new GameObject ();
        lineObject.transform.parent = this.transform;

        // オブジェクトにLineRendererを取り付ける
        lineObject.AddComponent<LineRenderer> ();

        // 描く線のコンポーネントリストに追加する
        lineRenderer = lineObject.GetComponent<LineRenderer> ();

        // 線と線をつなぐ点の数を0に初期化
        lineRenderer.positionCount = 0;

        // マテリアルを初期化
        lineRenderer.material = this.lineMaterial;

        // 線の色を初期化
        lineRenderer.material.color = this.lineColor;

        // 線の太さを初期化
        lineRenderer.startWidth = this.lineWidth;
        lineRenderer.endWidth = this.lineWidth;
    }

    /// <summary>
    /// 描く線のコンポーネントリストに位置情報を登録していく
    /// </summary>
    private void AddPositionDataToLineRendererList () {
        // filedとカメラとの距離の差
        float diff = Mathf.Abs (CameraObj.transform.position.y - transform.position.y);
        // 軸はx,y平面　高さがz軸
        Vector3 screenPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, diff);
        // 座標の変換を行いマウス位置を取得
        var mousePosition = Camera.main.ScreenToWorldPoint (screenPosition);

        // 範囲制限
        float xNewPos = Mathf.Clamp (mousePosition.x, xRange[0], xRange[1]);
        float zNewPos = Mathf.Clamp (mousePosition.z, zRange[0], zRange[1]);
        mousePosition = new Vector3 (xNewPos, transform.position.y + 0.01f, zNewPos);

        // 差異を記録
        RailCreateManager.Instance.positionDiff = this.transform.position;
        //Debug.Log ("moushPos " + mousePosition);

        int count = lineRenderer.positionCount;

        // 頂点数の制限比較
        Vector3 old;
        if (count - 1 < 0) {
            old = Vector3.zero;
        } else {
            old = lineRenderer.GetPosition (count - 1);
        }
        float dist = Vector3.Distance (mousePosition, old) * 100;

        // 頂点数の制限
        if (dist > 5f) {
            //Debug.Log("pos;" + mousePosition);
            linePoints.Add (mousePosition);

            // 線と線をつなぐ点の数を更新
            lineRenderer.positionCount += 1;
            //Debug.Log("pos:" + mousePosition);

            // 描く線のコンポーネントリストを更新
            lineRenderer.SetPosition (lineRenderer.positionCount - 1, mousePosition);
        }
    }

    /// <summary>
    /// 各リストのリセット
    /// </summary>
    public void Reset () {
        if (lineRenderer != null && resetFlg) {
            lineRenderer.positionCount = 0;
            linePoints.Clear ();
            resetFlg = false;
            clickFlg = true;
            Debug.Log ("reset " + resetFlg);
            Destroy (lineInst);
        }
    }

    // 頂点配列を受け渡す
    public void CreateRail () {
        RailCreateManager.Instance.linePoints = linePoints;
        SceneManager.LoadScene (1);
    }

    // 頂点のリストを返す
    public List<Vector3> GetPosList () {
        return linePoints;
    }

    /// <summary>
    /// スマホの場合
    /// </summary>
    public void AddPositonWithIOS () {
        if (Input.touchCount > 0) {

            Touch touch = Input.GetTouch (0);

            if (touch.phase == TouchPhase.Began && !resetFlg) {

                AddLineObject ();
            }

            if (touch.phase == TouchPhase.Moved && !resetFlg) {
                // filedとカメラとの距離の差
                float diff = Mathf.Abs (CameraObj.transform.position.y - transform.position.y);
                // 軸はx,y平面　高さがz軸
                Vector3 screenPosition = new Vector3 (touch.position.x, touch.position.y, diff);
                // 座標の変換を行いマウス位置を取得
                var mousePosition = Camera.main.ScreenToWorldPoint (screenPosition);

                // 範囲制限
                float xNewPos = Mathf.Clamp (mousePosition.x, xRange[0], xRange[1]);
                float zNewPos = Mathf.Clamp (mousePosition.z, zRange[0], zRange[1]);
                mousePosition = new Vector3 (xNewPos, transform.position.y + 0.01f, zNewPos);

                // 差異を記録
                RailCreateManager.Instance.positionDiff = this.transform.position;

                linePoints.Add (mousePosition);

                // 線と線をつなぐ点の数を更新
                lineRenderer.positionCount += 1;

                // 描く線のコンポーネントリストを更新
                lineRenderer.SetPosition (lineRenderer.positionCount - 1, mousePosition);
            }

            if (touch.phase == TouchPhase.Ended && !resetFlg && !clickFlg) {
                resetFlg = true;
            }
        }
    }
}