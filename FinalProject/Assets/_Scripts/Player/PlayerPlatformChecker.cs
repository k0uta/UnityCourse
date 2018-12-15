using UnityEngine;
using UnityEngine.Networking;

public class PlayerPlatformChecker : NetworkBehaviour {

    PlayerController playerController;

    private void Start()
    {
        playerController = this.transform.root.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
	{
        if(!playerController.isServer)
        {
            return;
        }

		var crate = other.GetComponent<CrateInteractable>();
		if (crate && other.transform.root == transform.root)
		{
            playerController.RpcDetachCrate(crate.GetComponent<NetworkIdentity>().netId);
		}
	}
}
