using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyamenTest : MonoBehaviour {
    public GameObject Thomas;
    public int speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody ThomasRb = Thomas.GetComponent<Rigidbody>();
        ThomasRb.AddForce(Thomas.transform.forward * speed);
		
	}
}
