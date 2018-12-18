using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class PatrolBehaviour : NetworkBehaviour {

    public List<Transform> patrolPoints;

    NavMeshAgent agent;

    int nextIndex = 0;

    public float baseSpeed = 4;

    public float baseAcceleration = 8;

    AudioSource audioSource;

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        if (!isServer)
        {
            return;
        }

        RpcResetSound();
        nextIndex = 0;
        //patrolPoints.Shuffle();
        transform.position = patrolPoints[nextIndex].position;

        agent.SetDestination(transform.position);

        agent.speed = baseSpeed;
        agent.acceleration = baseAcceleration;
    }

    [ClientRpc]
    void RpcResetSound()
    {
        audioSource.Play();
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
            //patrolPoints.Shuffle();
            nextIndex = 0;
        }
    }
}
