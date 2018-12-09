using UnityEngine;

public class CrateInteractable : InteractableBehaviour {
    public int scoreValue = 10;

    public float speedModifier = 1.0f;

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

        base.AttachTo(player);        
    }

	public override void Detach()
	{
		var player = this.transform.root.GetComponent<PlayerController>();
		player.agent.speed = player.agent.speed / speedModifier;
		player.agent.acceleration = player.agent.acceleration / speedModifier;
		
		this.transform.SetParent(null);

        player.crates.Remove(this);

		//base.RpcDetach();
	}
}
