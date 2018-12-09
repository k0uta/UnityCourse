using UnityEngine.Networking;

public class InteractableBehaviour : NetworkBehaviour
{
	public virtual void AttachTo(PlayerController player)
	{
	}

	public virtual void Detach()
	{
	}
}
