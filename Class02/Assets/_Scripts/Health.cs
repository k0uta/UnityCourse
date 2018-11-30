using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {
    public const int maxHealth = 100;

    public RectTransform healthBar;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public bool destroyOnDeath;

    PlayerSpawner playerSpawner;

	// Use this for initialization
	void Start () {
        playerSpawner = FindObjectOfType<PlayerSpawner>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentHealth = maxHealth;

            RpcRespawn();
        }

        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            if (isLocalPlayer)
            {
                transform.position = playerSpawner.GetRandomSpawn().position;
            }
        }
    }

    void OnChangeHealth(int health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}
