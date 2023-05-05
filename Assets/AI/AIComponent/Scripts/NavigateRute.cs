using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigateRute : MonoBehaviour
{
    [SerializeField] Transform route;

    [SerializeField] int currentWayPointIndex = 0;

    NavMeshAgent agent;
    [SerializeField] float reachingDistance=2.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Debug.Log(currentWayPointIndex);
        Vector3 currentPoint = route.GetChild(currentWayPointIndex).position;
        agent.SetDestination(currentPoint);
        Debug.Log(Vector3.Distance(transform.position, currentPoint));
        if (Vector3.Distance(transform.position, currentPoint) < reachingDistance)
        {
            currentWayPointIndex++;
            Debug.Log(currentWayPointIndex);
            if (currentWayPointIndex >= route.childCount)
            {
                currentWayPointIndex = 0;
            }
        }
    }
}
