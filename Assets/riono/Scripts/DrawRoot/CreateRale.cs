using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRale : MonoBehaviour {

    public FreeHand freeHand;
    private List<Vector3> linePoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void makeRoot(){
        linePoints = freeHand.GetPosList();

        foreach(var value in linePoints){
            Debug.Log(value);
        }
    }
}
