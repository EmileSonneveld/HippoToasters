using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingsword : MonoBehaviour {
    public GameObject player1;
    public GameObject arrow;
	// Use this for initialization
	void Start () {
        player1 = GameObject.FindGameObjectWithTag("Player");
        arrow.GetComponent<Rigidbody2D>().simulated = false;
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") 
        {
            arrow.GetComponent<Rigidbody2D>().simulated = true;
        }
    }
}
