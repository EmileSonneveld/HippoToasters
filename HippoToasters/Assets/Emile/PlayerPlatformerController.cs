using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPlatformerController : PhysicsObject
{

    public float lives = 100;
    public GameObject marker;

    public Transform[] pickupSlots;
    /// <summary>
    /// Pickups that the player has in his back
    /// </summary>
    public Pickup[] snappedPickups;
    public int[] snappedPickupsQuantity;

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

    public void EnterAnimationState(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Player_grab"))
        {
            RaycastHit2D[] results = new RaycastHit2D[8];
            int count = rb2d.Cast(targetVelocity, contactFilter, results, 0.5f + shellRadius);
            if (count > 0)
            {
                Debug.Log("Grabbed contact!");
                this.marker.transform.position = results[0].point;
            }
        }
    }

    protected override void ComputeVelocity()
    {
        var becomeTent = animator.GetBool("becomeTent");
        Vector2 move = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.T))
        {
            if ((!becomeTent && HasTent()) || becomeTent)
                animator.SetBool("becomeTent", !becomeTent);
        }

        if (!becomeTent)
        {
            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && grounded)
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

            //if ()
            {
                this.animator.SetBool("wantToGrab", Input.GetKey(KeyCode.G));
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


        if (animator)
        {
            animator.SetBool("grounded", grounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            if (
                animator.IsInTransition(0)
                )
            {
                if (animator.GetNextAnimatorStateInfo(0).IsName("Player_grab"))
                {
                    Debug.Log("In grab state!");
                    // Todo raycast, and grab the rocks
                }
                //Debug.Log("Transiton 0" + animator.GetNextAnimatorStateInfo(0).fullPathHash);
            }
        }
        targetVelocity = move * maxSpeed;
    }

    public override void Update()
    {
        base.Update();
        var tmp = mainCamera.position;
        tmp.x = transform.position.x;
        tmp.y = transform.position.y;
        mainCamera.position = tmp;

        if (lives < 0)
        {
            lives = 0;
            DieSequence();
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

        this.lives -= 0.5f * Time.deltaTime;
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

        // TODO: Generate multiple wjen quantity > 1
        pickup.SetState(PickupState.floatingFree);
        pickup.transform.parent = null;

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
            if (p.GetType() == typeof(TentPickup))
            {
                return true;
            }
        }
        return false;
    }
}