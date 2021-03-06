﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class ObjectController : MonoBehaviour {
    [SerializeField]
    //private Transform m_target = null;
    public Transform[] m_target;

    private int currnetTartget;

    private NavMeshAgent m_navAgent = null;

    private BGMController bgmController;

    private bool isSpeedUp;
    private bool isSpeedDown;

    private void Awake () {
        GetNavMeshAgent ();
    }

    public void GoButton () {
        m_navAgent.speed += 0.008f;
        bgmController.pitchUp (); //bgmを速くする
    }
    public void StopButton () {
        m_navAgent.speed -= 0.008f;
        bgmController.pitchDown (); //bgmを遅くする
    }

    public void SpeedUp (bool isUp) {
        isSpeedUp = isUp;
    }

    public void SpeeDown (bool isDown) {
        isSpeedDown = isDown;
    }

    // 速度セット
    public void SetSpeed (float nspeed) {
        m_navAgent.speed = nspeed;
    }

    private void Start () {
        currnetTartget = 0;

        if (m_target != null) {
            m_navAgent.destination = m_target[currnetTartget].position;
        }

        bgmController = GameObject.Find ("BGM").GetComponent<BGMController> (); //BGMの速度を変更するやつ
    }

    private void Update () {
        if (isSpeedUp) {
            //isSpeedDown = false;
            GoButton ();
            Debug.Log ("ta");
        }
        if (isSpeedDown) {
            //isSpeedUp = false;
            StopButton ();
            Debug.Log ("su");
        }

        Debug.Log ("up" + isSpeedUp);
        Debug.Log ("down " + isSpeedDown);

        Vector3 pos = m_target[currnetTartget].position;
        //Debug.Log("dist; " + Vector3.Distance(this.transform.position, pos));

        if (Vector3.Distance (this.transform.position, pos) < 0.065f) {
            //Debug.Log ("次");
            currnetTartget = (currnetTartget < m_target.Length - 1) ? currnetTartget + 1 : 0;
        }

        m_navAgent.destination = m_target[currnetTartget].position;

    }

    private void GetNavMeshAgent () {
        m_navAgent = this.GetComponent<NavMeshAgent> ();
    }
}