using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationIOControl : MonoBehaviour {
    public Animator animIo;
    
	// Use this for initialization
	void Start () {
        animIo.SetBool("jumpIo", false) ;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
            animIo.SetBool("jumpIo", true);
        }
        if (Input.GetKeyUp(KeyCode.Space) )
        {
            animIo.SetBool("jumpIo", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift));
    }
  
}
