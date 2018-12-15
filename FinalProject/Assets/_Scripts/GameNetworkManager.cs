using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameNetworkManager : NetworkManager {

    public InputField clientText;

    public InputField playerNameText;

    public CanvasGroup networkUI;

	public void Host()
    {
        if(playerNameText.text.Length < 1)
        {
            Debug.LogError("No name");
            return;
        }

        StartHost();

        HideNetworkUI();
    }

    public void Join()
    {
        if (playerNameText.text.Length < 1)
        {
            Debug.LogError("No name");
            return;
        }

        if (clientText.text.Length < 1)
        {
            Debug.LogError("No client");
            return;
        }

        networkAddress = clientText.text;

        StartClient();

        HideNetworkUI();
    }

    void HideNetworkUI()
    {
        networkUI.alpha = 0;
    }
}
