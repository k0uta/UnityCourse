using UnityEngine;
using UnityEngine.Networking;

public class CrateInteractable : InteractableBehaviour {
    public int scoreValue = 10;

    public float speedModifier = 1.0f;

    public ObjectiveType objectiveType;

    public int id;

    [SyncVar]
    Color objectiveColor;

    Rigidbody rb;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = objectiveColor;
        rb = GetComponent<Rigidbody>();
    }

    public void SetObjectiveType(ObjectiveType _objectiveType)
    {
        objectiveType = _objectiveType;
        objectiveColor = objectiveType.color;
        GetComponent<MeshRenderer>().material.color = objectiveColor;
    }

	public override void AttachTo(PlayerController player)
	{
        if(this.transform.parent == player.boxPlatform)
        {
            return;
        }


		player.agent.speed = player.agent.speed * speedModifier;
		player.agent.acceleration = player.agent.acceleration * speedModifier;
		
		this.transform.SetParent(player.boxPlatform);
		this.transform.position = player.boxPlatform.position + new Vector3(0, (player.crates.Count + 1) * this.transform.localScale.magnitude);

        player.crates.Add(this);

        player.GetComponent<AudioSource>().PlayOneShot(player.pickupAudioClip);

        if(isServer)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        } else
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        base.AttachTo(player);        
    }

	public override void Detach()
	{
		var player = this.transform.root.GetComponent<PlayerController>();
		player.agent.speed = player.agent.speed / speedModifier;
		player.agent.acceleration = player.agent.acceleration / speedModifier;
		
		this.transform.SetParent(null);

        player.crates.Remove(this);

        rb.isKinematic = false;
        rb.useGravity = true;

        base.Detach();
	}
}
