using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCcontroller : MonoBehaviour
{
    NavMeshAgent Agent;
    GameObject QueueWaypoint;
    GameObject Waypoint;
    GameObject Waypoint2;
    GameObject Waypoint3;
    GameObject Waypoint4;
    GameObject Waypoint5;
    GameObject Waypoint6;
    GameObject Table1;
    GameObject Table2;

    Animator Anim;
    GameObject currentWaypoint;
    bool isWaiting = false;

    float startTime; // Tidmätningens starttid
    float queueTime; // Total kötid

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
        QueueWaypoint = GameObject.Find("QueueWaypoint");
        Waypoint = GameObject.Find("Waypoint");
        Waypoint2 = GameObject.Find("Waypoint2");
        Waypoint3 = GameObject.Find("Waypoint3");
        Waypoint4 = GameObject.Find("Waypoint4");
        Waypoint5 = GameObject.Find("Waypoint5");
        Waypoint6 = GameObject.Find("Waypoint6");
        Table1 = GameObject.Find("Table1");
        Table2 = GameObject.Find("Table2");

       
        Vector3 queuePosition = QueueWaypoint.transform.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        currentWaypoint = QueueWaypoint;
        Agent.SetDestination(queuePosition);

        // Starta tidmätning
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            Anim.SetBool("Walking", false);
            StartCoroutine(DelayedFunctionCall(Random.Range(1f, 5f)));
        }
        else if (Agent.remainingDistance > Agent.stoppingDistance)
        {
            Anim.SetBool("Walking", true);
        }
    }

    IEnumerator DelayedFunctionCall(float delay)
    {
        isWaiting = true;
        yield return new WaitForSeconds(delay);
        SetNextDestination();
        isWaiting = false;
    }

    void SetNextDestination()
    {
        
        if (currentWaypoint == QueueWaypoint)
        {
            
            currentWaypoint = Random.value > 0.5f ? Waypoint : Waypoint2;
            Vector3 nextPosition = currentWaypoint.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(nextPosition);
        }
        else if (currentWaypoint == Waypoint)
        {
            currentWaypoint = Waypoint3;
            Vector3 nextPosition = currentWaypoint.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(nextPosition);
        }
        else if (currentWaypoint == Waypoint2)
        {
            currentWaypoint = Waypoint4;
            Vector3 nextPosition = currentWaypoint.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(nextPosition);
        }
        else if (currentWaypoint == Waypoint3)
        {
            currentWaypoint = Waypoint5;
            Vector3 nextPosition = currentWaypoint.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(nextPosition);
        }
        else if (currentWaypoint == Waypoint4)
        {
            currentWaypoint = Waypoint6;
            Vector3 nextPosition = currentWaypoint.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(nextPosition);
        }
        else if (currentWaypoint == Waypoint5)
        {
            currentWaypoint = Table1;
            Vector3 tablePosition = Table1.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(tablePosition);

            // Stoppa tidmätning och beräkna total kötid
            queueTime = Time.time - startTime;
            Debug.Log($"Queue time for NPC: {queueTime} seconds.");
        }
        else if (currentWaypoint == Waypoint6)
        {
            currentWaypoint = Table2;
            Vector3 tablePosition = Table2.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Agent.SetDestination(tablePosition);

            // Stoppa tidmätning och beräkna total kötid
            queueTime = Time.time - startTime;
            Debug.Log($"Queue time for NPC: {queueTime} seconds.");
        }
    }
}
