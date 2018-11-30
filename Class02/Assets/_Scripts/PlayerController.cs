using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {
    const float horizontalSpeed = 150.0f;

    const float verticalSpeed = 3.0f;

    const float bulletSpeed = 6.0f;

    const float bulletLifetime = 2.0f;

    public GameObject bulletPrefab;

    public Transform bulletSpawn;

    float speedMultiplier;

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer)
        {
            return;
        }

        speedMultiplier = Input.GetKey(KeyCode.Q) ? 3.0f : 1.0f;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * horizontalSpeed * speedMultiplier;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * verticalSpeed * speedMultiplier;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, z, 0);
        }

        if(Input.GetKey(KeyCode.Tab))
        {
            CmdFire();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed * speedMultiplier;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, bulletLifetime);
    }
}
