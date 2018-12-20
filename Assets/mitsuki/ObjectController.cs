using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class ObjectController : MonoBehaviour {
    [SerializeField]
    //private Transform m_target = null;
    public Transform[] m_target;

    private int currnetTartget;

    private NavMeshAgent m_navAgent = null;

    private BGMController bgmController;

    private void Awake () {
        GetNavMeshAgent ();
    }

    public void GoButton () {
        m_navAgent.speed += 0.05f;
        bgmController.pitchUp(); //bgmを速くする
    }
    public void StopButton () {
        m_navAgent.speed -= 0.05f;
        bgmController.pitchDown(); //bgmを遅くする
    }

    public void SetGo(bool isGo){

    }

    public void SetStop(bool isStop){
        
    }

    private void Start () {
        currnetTartget = 0;

        if (m_target != null) {
            m_navAgent.destination = m_target[currnetTartget].position;
        }

        bgmController = GameObject.Find("BGM").GetComponent<BGMController>();//BGMの速度を変更するやつ
    }

    private void Update () {
        // if (RailCreateManager.Instance.isDerail && m_navAgent == null) {
        //     GetNavMeshAgent ();
        // } else {
        //     RailCreateManager.Instance.isDerail = false;
        // }

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