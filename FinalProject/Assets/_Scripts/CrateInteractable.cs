using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateInteractable : InteractableBehaviour {
	float speedModifier = 1.0f;
	public override void AttachTo(PlayerController player)
	{
		player.agent.speed = player.agent.speed / speedModifier;
		player.agent.acceleration = player.agent.acceleration / speedModifier;
		
		this.transform.SetParent(player.boxPlatform);
		this.transform.position = player.boxPlatform.position + new Vector3(0, 1 * player.boxPlatform.childCount);
		base.AttachTo(player);
	}

	public override void Detach()
	{
		var player = this.transform.root.GetComponent<PlayerController>();
		player.agent.speed = player.agent.speed * speedModifier;
		player.agent.acceleration = player.agent.acceleration * speedModifier;
		
		this.transform.SetParent(null);
		base.Detach();
	}
}
