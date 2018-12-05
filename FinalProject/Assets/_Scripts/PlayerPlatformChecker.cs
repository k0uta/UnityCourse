using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformChecker : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
		var objective = other.GetComponent<ObjectiveBehaviour>();
		if (objective && other.transform.root == transform.root)
		{
			objective.Detach();
		}
	}
}
