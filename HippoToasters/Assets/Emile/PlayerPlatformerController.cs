using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

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
                Debug.Log("Transiton 0" + animator.GetNextAnimatorStateInfo(0).fullPathHash);
            }
        }
        targetVelocity = move * maxSpeed;
    }
}