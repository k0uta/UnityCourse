using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerController : NetworkBehaviour {
	public Camera cam;

	public NavMeshAgent agent;

	public Transform boxPlatform;

	private int objectiveLayer;

    private void Start()
	{
        cam = Camera.main;
		objectiveLayer = LayerMask.NameToLayer("Objective");

        if(isLocalPlayer)
        {
            cam.GetComponent<SmoothFollow>().target = this.transform;
        }
	}

    [Command]
    void CmdSetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer)
        {
            return;
        }

		if (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
                CmdSetDestination(hit.point);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == objectiveLayer)
		{
			other.gameObject.GetComponent<ObjectiveBehaviour>().AttachTo(this);
		}
	}
}
