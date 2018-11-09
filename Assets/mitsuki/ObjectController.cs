using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ObjectController : MonoBehaviour
{
    [SerializeField]
    //private Transform m_target = null;
    public Transform[] m_target;

    private int currnetTartget;

    private NavMeshAgent m_navAgent = null;

    private void Awake()
    {
        m_navAgent = this.GetComponent<NavMeshAgent>();
    }

    public void GoButtonDown()
    {
        m_navAgent.speed += 0.5f;
    }
    public void StopButtonDown()
    {
        m_navAgent.speed -= 0.8f;
    }

    private void Start()
    {
        currnetTartget = 0;

        if (m_target != null)
        {
            m_navAgent.destination = m_target[currnetTartget].position;
        }
    }

    private void Update()
    {
        Vector3 pos = m_target[currnetTartget].position;
        Debug.Log("dist; " + Vector3.Distance(this.transform.position, pos));

        if (Vector3.Distance(this.transform.position, pos) < 0.08f)
        {
            Debug.Log("次");
            currnetTartget = (currnetTartget < m_target.Length - 1) ? currnetTartget + 1 : 0;
        }

        m_navAgent.destination = m_target[currnetTartget].position;


    } // class ObjectController
}