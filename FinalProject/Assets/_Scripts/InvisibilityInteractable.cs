using UnityEngine;

public class InvisibilityInteractable : InteractableBehaviour {
    public override void AttachTo(PlayerController player)
    {
        player.GetComponent<MeshRenderer>().enabled = false;
        base.AttachTo(player);
    }
}
