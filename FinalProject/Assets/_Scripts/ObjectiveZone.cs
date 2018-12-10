using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class ObjectiveZone : NetworkBehaviour {

    public InteractableSpawner interactableSpawner;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer)
        {
            return;
        }

        var player = other.GetComponent<PlayerController>();

        if(!player)
        {
            return;
        }

        var crates = player.crates;

        for (int i = crates.Count - 1; i >= 0; i--)
        {
            var crate = crates[i];
            player.score += crate.scoreValue;
            crate.Detach();
            interactableSpawner.RespawnPosition(crate.transform);
            agent.speed += crate.speedModifier * 2.0f;
        }
    }
}
