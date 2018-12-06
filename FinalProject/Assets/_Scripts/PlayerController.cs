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

	private int interactableLayer;

    MeshRenderer meshRenderer;

    [SyncVar]
    private bool visibility = true;

    private void Start()
	{
        cam = Camera.main;
		interactableLayer = LayerMask.NameToLayer("Interactable");

        meshRenderer = GetComponent<MeshRenderer>();

        if (isLocalPlayer)
        {
            cam.GetComponent<SmoothFollow>().target = this.transform;
        } else
        {
            meshRenderer.enabled = visibility;
        }
	}

    [Command]
    void CmdSetDestination(Vector3 position)
    {
        agent.SetDestination(position);
    }

    [Command]
    void CmdSetVisibility(bool _visibility)
    {
        visibility = _visibility;
    }

	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            meshRenderer.enabled = visibility;
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

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            CmdSetVisibility(!visibility);
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == interactableLayer)
		{
			other.gameObject.GetComponent<InteractableBehaviour>().AttachTo(this);
		}
	}
}
