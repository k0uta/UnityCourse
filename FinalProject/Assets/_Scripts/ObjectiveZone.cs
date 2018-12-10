using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class ObjectiveZone : NetworkBehaviour {

    public InteractableSpawner interactableSpawner;

    public ObjectiveType objectiveType;

    public GameObject baseObject;

    public GameObject zoneObject;

    NavMeshAgent agent;
    
	void Start () {
        agent = GetComponent<NavMeshAgent>();

        baseObject.GetComponent<MeshRenderer>().materials[2].color = objectiveType.color;

        zoneObject.GetComponent<Renderer>().material.color = new Color(objectiveType.color.r, objectiveType.color.g, objectiveType.color.b, 0.25f);
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
            if(crate.objectiveType == this.objectiveType)
            {
                player.score += crate.scoreValue;

                crate.Detach();

                agent.speed += crate.speedModifier * 2.0f;

                interactableSpawner.RespawnPosition(crate.transform);
            }
        }
    }
}
