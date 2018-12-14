using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InteractableSpawner : NetworkBehaviour {
    public GameObject cratePrefab;

    public List<ObjectiveType> supportedObjectives;

    public int numberOfCrates = 5;

    public int minRange = -20;

    public int maxRange = 20;

    public List<CrateInteractable> crates;

    public static InteractableSpawner _instance;

    public static InteractableSpawner Instance
    {
        get
        {
            return _instance ?? FindObjectOfType<InteractableSpawner>();
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public override void OnStartServer()
    {
        Reset();
    }

    public void Reset()
    {
        if(crates != null)
        {
            foreach (var crate in crates)
            {
                NetworkServer.Destroy(crate.gameObject);
            }
            crates.Clear();
        }

        crates = new List<CrateInteractable>();

        for (int i = 0; i < numberOfCrates; i++)
        {
            var crateObject = (GameObject)Instantiate(cratePrefab);

            NetworkServer.Spawn(crateObject);

            RespawnPosition(crateObject.transform);

            var crate = crateObject.GetComponent<CrateInteractable>();
            crate.id = i;

            crates.Add(crate);
        }
    }

    public void RespawnPosition(Transform crate)
    {
        crate.position = new Vector3(Random.Range(minRange, maxRange), 1, Random.Range(minRange, maxRange));

        var randomObjectiveType = supportedObjectives[Random.Range(0, supportedObjectives.Count)];
        crate.GetComponent<CrateInteractable>().SetObjectiveType(randomObjectiveType);
    }
}
