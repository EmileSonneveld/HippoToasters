using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentPickup : Pickup
{

    public override int maxStackable
    {
        get
        {
            return 1;
        }
    }

    public override void Start()
    {
        base.Start();
    }
    
    public override void Update()
    {
        base.Start();
    }
}
