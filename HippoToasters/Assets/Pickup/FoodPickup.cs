using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickup : Pickup
{
    public override int maxStackable
    {
        get
        {
            return 100;
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
