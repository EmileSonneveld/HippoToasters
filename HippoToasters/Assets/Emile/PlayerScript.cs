using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float lives = 100;

    public Transform[] pickupSlots;
    /// <summary>
    /// Pickups that the player has in his back
    /// </summary>
    public Pickp[] snappedPickups;

    private Transform mainCamera;

    void Awake()
    {
        this.mainCamera = Camera.main.transform;
        snappedPickups = new Pickp[pickupSlots.Length];
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
        Debug.Log(col.collider.name);

        if (col.collider.tag == "Pickup")
        {
            var p = col.collider.GetComponent<Pickp>();
            this.PickItUp(p);
        }
        else if (col.collider.tag == "DangerousCollider")
        {
            if (col.relativeVelocity.sqrMagnitude > 5)
            {
                Debug.Log("OnCollisionEnter2D" + col.relativeVelocity.magnitude);
                this.lives -= col.relativeVelocity.magnitude * 10; // random multiplier
            }
        }
    }


    void DieSequence()
    {
        Debug.Log("We died!");

        Destroy(GetComponent<PlayerPlatformerController>());
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

    void PickItUp(Pickp pickup)
    {

        var slotNr = GetEmptyPickupSLot();
        if (slotNr == -1) Debug.LogError("No slots available!");
        snappedPickups[slotNr] = pickup;

        pickup.SetState(PickupState.snappedToPlayer);
        pickup.transform.parent = pickupSlots[slotNr];
        pickup.transform.localPosition = new Vector3();
    }

}
