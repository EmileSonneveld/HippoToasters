﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPlatformerController : PhysicsObject
{

    public float lives = 100;
    //public GameObject marker;


    public Transform[] pickupSlots;
    /// <summary>
    /// Pickups that the player has in his back
    /// </summary>
    Pickup[] snappedPickups;
    int[] snappedPickupsQuantity;

    private Transform mainCamera;
    public GameObject klimbingPickaxe;


    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    //private SpriteRenderer spriteRenderer;
    private Animator animator;


    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
        this.mainCamera = Camera.main.transform;
        snappedPickups = new Pickup[pickupSlots.Length];
        snappedPickupsQuantity = new int[pickupSlots.Length];

        //spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public bool isGrabbed = false;
    public float canDoAirJumpTime = 0;
    public void EnterAnimationState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("grab"))
        {
        }
    }

    protected override void ComputeVelocity()
    {
        var currState = animator.GetCurrentAnimatorStateInfo(0);
        bool jumpOrGrab = currState.IsName("jump") || currState.IsName("grab");

        if (currState.IsName("Tent"))
        {
            this.lives -= 0.5f * Time.deltaTime;
        }

        var wanneBeATent = animator.GetBool("wanneBeATent");
        Vector2 move = Vector2.zero;

        if (Input.GetButtonDown("Tent"))
        {

            if ((!wanneBeATent && HasTent()) || wanneBeATent)
                animator.SetBool("wanneBeATent", !wanneBeATent);
        }

        if (!wanneBeATent && !currState.IsName("Caca"))
        {
            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && (grounded || (jumpOrGrab && canDoAirJumpTime > 0)))
            {
                velocity.y = jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (Input.GetButton("Grab"))
            {
                this.animator.SetBool("wantToGrab", true);
                canDoAirJumpTime = 0.7f;
            }
            else
            {
                this.animator.SetBool("wantToGrab", false);
                this.klimbingPickaxe.SetActive(false);
                isGrabbed = false;
            }
            if (jumpOrGrab)
            {
                if (Input.GetButtonDown("Grab"))
                {
                    RaycastHit2D[] results = new RaycastHit2D[8];
                    int count = rb2d.Cast(new Vector2(1, 0), contactFilter, results, 3f + shellRadius); // targetVelocity
                    if (count == 0)
                        count = rb2d.Cast(new Vector2(-1, 0), contactFilter, results, 3f + shellRadius); // targetVelocity
                    if (count > 0)
                    {
                        //this.marker.transform.position = results[0].point;
                        this.klimbingPickaxe.SetActive(true);
                        isGrabbed = true;
                    }
                }
            }
        }

        bool flipSprite = ((transform.localScale.x < 0) ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            //spriteRenderer.flipX = !spriteRenderer.flipX;
            var tmp = transform.localScale;
            tmp.x *= -1;
            transform.localScale = tmp;
        }


        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        if (
            animator.IsInTransition(0)
            )
        {
        }
        targetVelocity = move * maxSpeed;

        if (isGrabbed)
            velocity.y = 0;
    }

    public GameObject cacaObj;
    private IEnumerator ShitSequence()
    {
        yield return new WaitForSeconds(3);
        var ui = FindObjectOfType<UiManager>();
        ui.shitAlert = true;
        yield return new WaitForSeconds(2);
        ui.shitAlert = false;
        animator.SetTrigger("Caca");
        yield return new WaitForSeconds(2);
        var c = Instantiate(cacaObj);
        c.transform.position = this.transform.position;
    }
    Vector2 cameraOffset = new Vector2(1.6f, 0.8f);

    public float foodBar = 100;

    public override void Update()
    {
        base.Update();
        canDoAirJumpTime -= Time.deltaTime;

        //if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftShift)) ;


        var currState = animator.GetCurrentAnimatorStateInfo(0);

        if (currState.IsName("Tent"))
        {
            this.lives += 5f * Time.deltaTime;
        }
        if (Input.GetButtonDown("Eat"))
        {
            bool foundFood = false;
            for (int i = 0; i < pickupSlots.Length; i++)
            {
                var p = snappedPickups[i];
                if (p == null) continue;
                if (p.GetType() == typeof(FoodPickup))
                {
                    foundFood = true;
                    foodBar += 20;
                    snappedPickupsQuantity[i] -= 1;
                    if (snappedPickupsQuantity[i] == 0) 
                        this.PickItOf(i);
                }
            }
            if (foundFood)
                StartCoroutine(ShitSequence());
            else
                Debug.Log("No food in inventory!");
        }
        foodBar -= 0.5f * Time.deltaTime;
        if (foodBar <= 0)
        {
            foodBar = 0;
            lives -= 5 * Time.deltaTime;
        }
        else if (foodBar > 100)
        {
            foodBar = 100;
        }

        var tmp = mainCamera.position;
        tmp.x = transform.position.x + cameraOffset.x;
        tmp.y = transform.position.y + cameraOffset.y;
        mainCamera.position = tmp;

        this.lives -= 0.5f * Time.deltaTime;
        if (lives < 0)
        {
            lives = 0;
            DieSequence();
        }
        else if (lives > 100)
        {
            lives = 100;
        }

        for (int i = 0; i < pickupSlots.Length; i++)
        {
            var h = this.pickupSlots[i];
            var tm = h.GetComponentInChildren<TextMesh>();
            string text = "";

            var p = snappedPickups[i];
            if (p)
            {
                text = p.name + ": " + this.snappedPickupsQuantity[i] + "/" + p.maxStackable;
            }

            tm.text = text;
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            //animator.SetTrigger("Caca");
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
                this.lives -= col.relativeVelocity.magnitude * 9; // random multiplier
            }
        }
        else
        {
            //Debug.Log(col.collider.tag);
        }
    }

    public void DieSequence()
    {
        Debug.Log("We died!");

        DropAllPickups();
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
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

    void PickItUp(Pickup pickup)
    {
        for (int i = 0; i < snappedPickups.Length; i++)
        {
            var p = snappedPickups[i];
            if (p == null) continue;
            if (p.GetType() == pickup.GetType())
            {
                if (snappedPickupsQuantity[i] < p.maxStackable)
                {
                    snappedPickupsQuantity[i] += 1;
                    Destroy(pickup.gameObject); // Will be regenerated when we die.
                    return;
                }
            }
        }

        var slotNr = GetEmptyPickupSLot();
        if (slotNr == -1) Debug.LogError("No slots available!");

        snappedPickups[slotNr] = pickup;
        snappedPickupsQuantity[slotNr] = 1;

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

        if (snappedPickupsQuantity[slot] <= 1)
            Destroy(pickup.gameObject);

        for (int i = 0; i < snappedPickupsQuantity[slot] - 1; i++)
        {
            var inst = Instantiate(pickup.gameObject);
            inst.transform.position = transform.position;
        }
        snappedPickupsQuantity[slot] = 0;

    }

    public bool HasTent()
    {
        for (int i = 0; i < snappedPickups.Length; i++)
        {
            var p = this.snappedPickups[i];
            if (p == null) continue;
            if (p.GetType() == typeof(TentPickup))
            {
                return true;
            }
        }
        return false;
    }
}