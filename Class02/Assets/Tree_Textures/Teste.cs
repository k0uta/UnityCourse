using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Teste : MonoBehaviour {

    public GameObject treeObject;

    public Tree tree;

	// Use this for initialization
	void Start () {
        tree = treeObject.GetComponent<Tree>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(tree.data);
        }
    }
}
