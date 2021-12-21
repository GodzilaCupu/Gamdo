using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject[] wayPoints;
    [SerializeField] private NavMeshAgent agent;

    [Header("Target")]
    [SerializeField] private GameObject playerObj;
    [SerializeField] private bool isCarryingPackage;

    [Header("Depandency")]
    Animator npcAnimator;
    KardusController boxControl;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        npcAnimator = this.GetComponent<Animator>();

        playerObj =  GameObject.Find("Player");

        int d = Random.Range(0, wayPoints.Length);
        agent.SetDestination(wayPoints[d].transform.position);
        npcAnimator.SetBool("Walk", true);

    }

    // Update is called once per frame
    void Update()
    {
        CheckKardus();
        CheckNgejar();
    }

    private void CheckKardus()
    {
        GameObject GrabPos = GameObject.Find("GrabPos");

        if(GrabPos.transform.childCount > 0)
        {
            boxControl = GrabPos.transform.GetChild(0).GetComponent<KardusController>();
            isCarryingPackage = boxControl._IsCarried;
        }
    }

    private void CheckNgejar()
    {
        int d = Random.Range(0, wayPoints.Length);


        float distancePlayer = Vector3.Distance(this.gameObject.transform.position, playerObj.transform.position);

        float _lastWayPoint = Vector3.Distance(this.gameObject.transform.position, wayPoints[d].transform.position);

        if (distancePlayer < 4f && isCarryingPackage == true)
        {
            npcAnimator.SetBool("Walk", true);
            if (_lastWayPoint < 8)
            {
                agent.SetDestination(playerObj.transform.position);
                Debug.LogWarning("Cek 1");
                if (agent.remainingDistance < 0.6f)
                {

                    agent.SetDestination(wayPoints[d].transform.position);
                    Debug.LogWarning("Cek 2");
                }
            }
            else
            {
                Debug.LogWarning("Cek 3");
                agent.SetDestination(wayPoints[d].transform.position);

            }

        }
        else
        {

            if (agent.remainingDistance < 0.6f)
            {
                Debug.LogWarning("Cek 4");
                agent.SetDestination(wayPoints[d].transform.position);
            }
            npcAnimator.SetBool("Walk", true);
        }

        Debug.Log("Wp Target" + d);
        Debug.Log("Player" + distancePlayer);
        Debug.Log("Wp" + _lastWayPoint);
    }

}
