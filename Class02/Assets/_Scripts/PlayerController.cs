using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour {
    const float horizontalSpeed = 150.0f;

    const float verticalSpeed = 3.0f;

    const float bulletSpeed = 6.0f;

    const float bulletLifetime = 2.0f;

    public GameObject bulletPrefab;

    public GameObject messageBalloonPrefab;

    public Transform bulletSpawn;

    float speedMultiplier;

    [SyncVar(hook = "OnChangePlayerColor")]
    public Color playerColor;

    public override void OnStartLocalPlayer()
    {
        CmdChangeColor(Color.blue);
        FindObjectOfType<TextInputController>().playerController = this;
    }

    void OnChangePlayerColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = playerColor;
    }

    // Update is called once per frame
    void Update () {
        if(!isLocalPlayer)
        {
            return;
        }

        speedMultiplier = Input.GetKey(KeyCode.Q) ? 3.0f : 1.0f;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime *  horizontalSpeed * speedMultiplier;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed * speedMultiplier;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, z, 0);
        }

        //if(Input.GetKey(KeyCode.Tab))
        //{
        //    CmdFire();
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        if(Input.GetKey(KeyCode.Tab))
        {
            CmdChangeColor(new Color(Random.value, Random.value, Random.value));
        }
    }

    [Command]
    void CmdChangeColor(Color color)
    {
        playerColor = color;
    }

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed * speedMultiplier;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, bulletLifetime);
    }

    [Command]
    public void CmdSendMessage(string message)
    {
        if(!isServer)
        {
            return;
        }

        Debug.Log(message);

        RpcMessageReceived(message);
    }

    [ClientRpc]
    void RpcMessageReceived(string message)
    {
        Debug.Log(message);
        var messageBalloon = (GameObject)Instantiate(messageBalloonPrefab, this.transform);
        messageBalloon.transform.localPosition = new Vector3(0, 2, 0);

        messageBalloon.GetComponentInChildren<Text>().text = message;

        Destroy(messageBalloon, 2.0f);
    }
}
