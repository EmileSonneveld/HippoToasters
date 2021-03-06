﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupState
{
    floatingFree,
    snappedToPlayer
}

public abstract class Pickup : MonoBehaviour
{
    public PickupState state;
    private Rigidbody2D rb2d;

    public virtual int maxStackable { get { return 1; } }

    public virtual void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public virtual void Update()
    {

    }

    public void SetState(PickupState s)
    {
        if (s == state) return;
        //Debug.Log("SetState");
        switch (s)
        {
            case PickupState.floatingFree:
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                rb2d.simulated = true;
                break;
            case PickupState.snappedToPlayer:
                rb2d.bodyType = RigidbodyType2D.Static;
                rb2d.simulated = false;
                // Should also disable collider?
                break;

        }
        this.state = s;
    }


}
