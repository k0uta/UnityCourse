using UnityEngine;
using UnityEngine.Networking;

public class PlayerPlatformChecker : NetworkBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		var crate = other.GetComponent<CrateInteractable>();
		if (crate && other.transform.root == transform.root)
		{
			crate.Detach();
		}
	}
}
