using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class PatrolBehaviour : NetworkBehaviour {

    public List<Transform> patrolPoints;

    NavMeshAgent agent;

    int nextIndex = 0;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        patrolPoints.Shuffle();
    }

    private void Update()
    {
        if(!isServer)
        {
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void GoToNextPoint()
    {
        if(patrolPoints.Count < 1)
        {
            return;
        }

        agent.SetDestination(patrolPoints[nextIndex].position);

        if(++nextIndex >= patrolPoints.Count)
        {
            patrolPoints.Shuffle();
            nextIndex = 0;
        }
    }
}
