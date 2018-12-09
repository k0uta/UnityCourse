using UnityEngine;
using UnityEngine.Networking;

public class InteractableSpawner : NetworkBehaviour {
    public GameObject cratePrefab;

    public int numberOfCrates = 5;

    public int minRange = -20;

    public int maxRange = 20;

    public override void OnStartServer()
    {
        for (int i = 0; i < numberOfCrates; i++)
        {
            var crate = (GameObject) Instantiate(cratePrefab);
            NetworkServer.Spawn(crate);

            RespawnPosition(crate.transform);
        }
    }

    public void RespawnPosition(Transform crate)
    {
        crate.position = new Vector3(Random.Range(minRange, maxRange), 1, Random.Range(minRange, maxRange));
    }
}
