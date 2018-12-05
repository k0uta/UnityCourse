using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveSpawner : NetworkBehaviour {
    public GameObject cratePrefab;

    public int numberOfCrates = 5;

    public int minRange = -20;

    public int maxRange = 20;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfCrates; i++)
        {
            var crate = (GameObject) Instantiate(cratePrefab);
            crate.transform.position = new Vector3(Random.Range(minRange, maxRange), 1, Random.Range(minRange, maxRange));
            NetworkServer.Spawn(crate);
        }
    }
}
