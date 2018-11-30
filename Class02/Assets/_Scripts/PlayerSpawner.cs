using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : MonoBehaviour {

    List<Transform> availableSpawns;

	// Use this for initialization
	void Start () {
        availableSpawns = new List<Transform>();
        foreach (var networkSpawn in FindObjectsOfType<NetworkStartPosition>())
        {
            availableSpawns.Add(networkSpawn.transform);
        }
    }
	
	public Transform GetRandomSpawn()
    {
        int randomIndex = Random.Range(0, availableSpawns.Count);
        return availableSpawns[randomIndex];
    }
}
