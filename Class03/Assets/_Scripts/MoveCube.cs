using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{

    void Start()
    {
        float x = 0, z = 0;

        if(PlayerPrefs.HasKey("SphereX"))
            x = PlayerPrefs.GetFloat("SphereX");

        if(PlayerPrefs.HasKey("SphereZ"))
            z = PlayerPrefs.GetFloat("SphereZ");

        transform.position = new Vector3(x, 0, z);
    }

    void Update()
    {
        var h = Input.GetAxis("Horizontal") * Time.deltaTime;
        var v = Input.GetAxis("Vertical") * Time.deltaTime;
        transform.Translate(h, 0, v);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Save sphere position in to memory.");
            PlayerPrefs.SetFloat("SphereX", transform.position.x);
            PlayerPrefs.SetFloat("SphereZ", transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Erased sphere position from memory.");
            PlayerPrefs.DeleteKey("SphereX");
            PlayerPrefs.DeleteKey("SphereZ");
            PlayerPrefs.Save();
        }
    }
}
