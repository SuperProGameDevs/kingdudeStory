using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kingdude : Character 
{
    [SerializeField] private float maxWalkSpeed = 10;
    [SerializeField] private float maxRunSpeed = 20;

    private Rigidbody2D dudeRB;
    private Animator animator;

    // Jumping
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpAmplificationRatio = 2;
    [SerializeField] private float fallAmplificationRatio = 2.5f;

    private bool isAttacking = false;

    // Use this for initialization
    protected new void Start()
    {
        this.dudeRB = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        base.Start();
    }
	
	// Update is called once per frame
	void Update () 
    {
        this.animator.SetFloat("xSpeed", Mathf.Abs(this.dudeRB.velocity.x));
        this.animator.SetFloat("ySpeed", this.dudeRB.velocity.y);
        this.animator.SetBool("isRunning", this.isRunning);
        this.animator.SetBool("isOnGround", this.isOnGround);
        Attack();
	}

    // Same as Update, but called every fixed period of time
    private void FixedUpdate()
    {
        isRunning = this.IsRunPressed();
        float xSpeed = this.GetHorizontalAxis() * ((isRunning) ? maxRunSpeed : maxWalkSpeed);

        this.Move(xSpeed);

        if (this.IsJumpPressed() || !isOnGround) {
            // All jump logic is inside Jump function
            // Checking if character is on ground is handled
            this.Jump(jumpHeight);
        }

        Rigidbody2D rb = this.GetRB();
        // Multiplying by deltaTime is to apply gravity per second, not per FixedUpdate (roughly per frame)
        if (this.IsFalling) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallAmplificationRatio - 1) * Time.deltaTime;
        } else if (this.IsAscending && !this.IsJumpPressed()) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpAmplificationRatio - 1) * Time.deltaTime;
        }
    }

    protected override Rigidbody2D GetRB()
    {
        return this.dudeRB;
    }

    protected override LayerMask GetGroundMask()
    {
        return this.groundMask;
    }

    protected override Transform GetGroundChecker()
    {
        return this.groundChecker;
    }

    protected void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            this.animator.SetTrigger("attack");
        }
    }
}
