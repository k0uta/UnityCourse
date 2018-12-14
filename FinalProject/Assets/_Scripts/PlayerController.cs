using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityStandardAssets.Utility;

public class PlayerController : NetworkBehaviour {
    public Camera cam;

    public NavMeshAgent agent;

    public Transform boxPlatform;
    
    public List<CrateInteractable> crates;

    [SyncVar]
    public string playerName;

    [SyncVar(hook = "OnChangeScore")]
    public int score = 0;

    private int interactableLayer;

    Animator animator;

    MeshRenderer meshRenderer;

    [SyncVar]
    private bool visibility = true;

    private void Start()
	{
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
		interactableLayer = LayerMask.NameToLayer("Interactable");

        animator = GetComponent<Animator>();

        meshRenderer = GetComponent<MeshRenderer>();

        if (isLocalPlayer)
        {
            cam.GetComponent<SmoothFollow>().target = this.transform;

            // TEMP
            CmdSetPlayerName(string.Format("Player {0}", FindObjectsOfType<PlayerController>().Length));
        } else
        {
            meshRenderer.enabled = visibility;
        }
	}

    [Command]
    void CmdSetPlayerName(string _playerName)
    {
        playerName = _playerName;
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
        animator.SetFloat("Speed", agent.velocity.magnitude);
        if (!isLocalPlayer)
        {
            meshRenderer.enabled = visibility;
            return;
        }

		if (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1.0f);
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
        if(!isServer)
        {
            return;
        }

		if (other.gameObject.layer == interactableLayer)
		{
			other.gameObject.GetComponent<InteractableBehaviour>().AttachTo(this);
		}
	}

    void OnChangeScore(int _score)
    {
        score = _score;
        if (!isLocalPlayer)
        {
            return;
        }

        StageManager.Instance.scoreText.text = string.Format("x {0}", _score);
    }
}
