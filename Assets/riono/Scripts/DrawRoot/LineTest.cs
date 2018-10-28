using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour {

    public LineRenderer line;
    public Camera camera;
    private List<Vector3> linePoints;


	// Use this for initialization
	void Start () {
        line = this.gameObject.AddComponent<LineRenderer>();
        LineSetUp();
        linePoints = new List<Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButton(0)){
            Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 mousePosition = Input.mousePosition;
           
            Debug.Log(mousePosition.x);

            mousePosition.z = line.transform.position.z;
            if(!linePoints.Contains(mousePosition)){
                linePoints.Add(mousePosition);
                line.positionCount = linePoints.Count;
                line.SetPosition(line.positionCount - 1, mousePosition);
            }
        }
	}


    private void LineSetUp(){
        line.positionCount = 0;
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.startColor = Color.white;
        line.endColor = Color.white;
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        line.useWorldSpace = true;
    }
}
