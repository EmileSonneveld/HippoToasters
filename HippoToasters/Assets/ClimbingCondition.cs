using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingCondition : MonoBehaviour {

    public Animator animator;
    public GameObject[] player1;
    public Component gravite;
    

    
	// Use this for initialization
	void Start () {
        player1 = GameObject.FindGameObjectsWithTag("Player");
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "climbing")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                animator.SetBool("wantToGrab", true);

            }
        }
        else;
            
          
        
    }
}
