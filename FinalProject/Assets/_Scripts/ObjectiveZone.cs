using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveZone : NetworkBehaviour {

    public InteractableSpawner interactableSpawner;

	// Use this for initialization
	void Start () {
		
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

        var player = other.transform.root.GetComponent<PlayerController>();

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
        }

        //NetworkServer.Destroy()
    }
}
