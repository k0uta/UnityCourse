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

    public float baseSpeed = 10;

    public float baseAcceleration = 20;

    public AudioClip scoreAudioClip;

    public AudioClip pickupAudioClip;

    AudioSource audioSource;

    private void Start()
	{
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
		interactableLayer = LayerMask.NameToLayer("Interactable");

        animator = GetComponent<Animator>();

        if (isLocalPlayer)
        {
            cam.GetComponent<SmoothFollow>().target = this.transform;

            CmdSetPlayerName(StageManager.Instance.playerNameInput.text);

            audioSource = GetComponent<AudioSource>();
        } else if(isServer)
        {
            Reset();
        }
	}

    public void Reset()
    {
        this.transform.position = Vector3.zero;
        score = 0;

        this.crates = new List<CrateInteractable>();
        agent.speed = baseSpeed;
        agent.acceleration = baseAcceleration;
        agent.SetDestination(this.transform.position);
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

	// Update is called once per frame
	void Update () {
        if(isServer) {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        if (!isLocalPlayer)
        {
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
	}

	private void OnTriggerEnter(Collider other)
	{
        if(!isServer)
        {
            return;
        }

		if (other.gameObject.layer == interactableLayer)
		{
            //other.GetComponent<InteractableBehaviour>().AttachTo(this);
            var netId = other.GetComponent<NetworkIdentity>().netId;
            RpcAttachCrate(netId);
        }
	}

    [ClientRpc]
    void RpcAttachCrate(NetworkInstanceId netId)
    {
        var crate = ClientScene.FindLocalObject(netId);
        crate.gameObject.GetComponent<InteractableBehaviour>().AttachTo(this);
    }

    [ClientRpc]
    public void RpcDetachCrate(NetworkInstanceId netId)
    {
        var crate = ClientScene.FindLocalObject(netId);
        crate.gameObject.GetComponent<InteractableBehaviour>().Detach();
    }

    void OnChangeScore(int _score)
    {
        score = _score;
        if (!isLocalPlayer)
        {
            return;
        }

        if (_score > 0)
        {
            audioSource.PlayOneShot(scoreAudioClip);
        }

        StageManager.Instance.scoreText.text = string.Format("x {0}", _score);
    }
}
