using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformChecker : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		var crate = other.GetComponent<CrateInteractable>();
		if (crate && other.transform.root == transform.root)
		{
			crate.Detach();
		}
	}
}
