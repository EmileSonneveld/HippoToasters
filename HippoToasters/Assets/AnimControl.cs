using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour {
    public float horizontalMove = 0f;
    public Animator animator;
	// Use this for initialization
	void Start () {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
