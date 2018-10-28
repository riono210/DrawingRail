using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldRange : MonoBehaviour {

    // 描画範囲
    public GameObject drowPlane;

    private float[] planePointsX;
    private float[] planePointsZ;

	// Use this for initialization
	void Awake () {
        //Vector3 points;
        //List<Vector2> dial = new List<Vector2> { { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };
        Vector3 planePos = drowPlane.transform.position;

        //planePoints.Add(planePos);

        float xRange = drowPlane.transform.localScale.x /2;
        float zRange = drowPlane.transform.localScale.z /2;

        planePointsX = new float[2] { (planePos.x - xRange) , (planePos.x + xRange)};
        planePointsZ = new float[2] { (planePos.z - zRange), (planePos.z + zRange) };
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 範囲の配列を返す
    /// </summary>
    /// <returns>The range.</returns>
    /// <param name="key">x or z</param>
    public float[] GetRange(string key){
        switch (key){
            case "x":
                return planePointsX;

            case "z":
                return planePointsZ;

            default:
                return null;
        }
    }
}
