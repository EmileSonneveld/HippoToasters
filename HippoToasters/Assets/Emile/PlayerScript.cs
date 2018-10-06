using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float lives = 100;
    public PhysicsObject test;

    public Transform[] pickupSlots;
    /// <summary>
    /// Pickups that the player has in his back
    /// </summary>
    public Pickup[] snappedPickups;

    private Transform mainCamera;

    void Awake()
    {
        test = GetComponent<PhysicsObject>();
        this.mainCamera = Camera.main.transform;
        snappedPickups = new Pickup[pickupSlots.Length];
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var tmp = mainCamera.position;
        tmp.x = transform.position.x;
        tmp.y = transform.position.y;
        mainCamera.position = tmp;

        if (lives < 0)
        {
            lives = 0;
            DieSequence();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.tag == "Pickup")
        {
            var p = col.collider.GetComponent<Pickup>();
            this.PickItUp(p);
        }
        else if (col.collider.tag == "DangerousCollider")
        {
            if (col.relativeVelocity.sqrMagnitude > 5)
            {
                Debug.Log("OnCollisionEnter2D" + col.relativeVelocity.magnitude);
                this.lives -= col.relativeVelocity.magnitude * 15; // random multiplier
            }
        }
        else
        {
            //Debug.Log(col.collider.tag);
        }
    }


    void DieSequence()
    {
        Debug.Log("We died!");

        DropAllPickups();
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(GetComponent<PlayerPlatformerController>());
        Destroy(GetComponent<PlayerScript>());

        PlayerSpawner.singleton.SpawnPlayer();
    }



    int GetEmptyPickupSLot()
    {
        for (int i = 0; i < snappedPickups.Length; i++)
        {
            if (snappedPickups[i] == null)
                return i;
        }
        return -1; // not found
    }

    void PickItUp(Pickup pickup)
    {
        var slotNr = GetEmptyPickupSLot();
        if (slotNr == -1) Debug.LogError("No slots available!");
        snappedPickups[slotNr] = pickup;

        pickup.SetState(PickupState.snappedToPlayer);
        pickup.transform.parent = pickupSlots[slotNr];
        pickup.transform.localPosition = new Vector3();
    }

    void DropAllPickups()
    {
        for (int i = 0; i < snappedPickups.Length; i++)
        {
            var p = snappedPickups[i];
            if (p)
            {
                PickItOf(i);
            }
        }
    }

    void PickItOf(int slot)
    {
        //Debug.Log("PickItOf " + slot);
        var pickup = snappedPickups[slot];
        snappedPickups[slot] = null;

        pickup.SetState(PickupState.floatingFree);
        pickup.transform.parent = null;
    }

}
