using UnityEngine;
using System.Collections;

public class shoot : MonoBehaviour {
    public GameObject shell;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(shell, this.transform.position, this.transform.rotation);
        }
	}
}
