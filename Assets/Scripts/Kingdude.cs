using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;

public class Kingdude : Character
{
    enum AttackType
    {
        Sword = 1,
        Punch = 2
    }

    enum PunchType
    {
        Punch1 = 1,
        Punch2 = 2
    }

    [SerializeField] float maxWalkSpeed = 10;
    [SerializeField] float maxRunSpeed = 20;

    Rigidbody2D dudeRB;
    Animator animator;

        // Jumping
    [SerializeField] float jumpHeight = 10;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpAmplificationRatio = 2;
    [SerializeField] float fallAmplificationRatio = 2.5f;

    bool isAttacking;
    IAttackSeries<PunchType> punchCombo;
    SequentialClickTimer punchTimer;

    // Use this for initialization
    protected new void Start()
    {
        isAttacking = false;
        punchCombo = new ComboAttackSeries<PunchType>(new[] { PunchType.Punch1, PunchType.Punch1, PunchType.Punch2 });
        punchTimer = new SequentialClickTimer(1);

        this.dudeRB = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        this.animator.SetFloat("xSpeed", Mathf.Abs(GetRB().velocity.x));
        this.animator.SetFloat("ySpeed", GetRB().velocity.y);
        this.animator.SetBool("isRunning", this.isRunning);
        this.animator.SetBool("isOnGround", this.isOnGround);
        HandleAttack();
    }

    // Same as Update, but called every fixed period of time
    void FixedUpdate()
    {
        HandleMovement();
        HandleJumping();
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

    protected void HandleMovement()
    {
        isRunning = this.IsRunPressed();
        float attackSlowDown = (isAttacking) ? 0.2f : 1;
        float xSpeed = this.GetHorizontalAxis() * ((isRunning) ? maxRunSpeed : maxWalkSpeed) * attackSlowDown;

        this.Move(xSpeed);
    }

    protected void HandleJumping()
    {
        if (this.IsJumpPressed() || !isOnGround) {
            // All jump logic is inside Jump function
            // Checking if character is on ground is handled
            this.Jump(jumpHeight);
        }

        // Multiplying by deltaTime is to apply gravity per second, not per FixedUpdate (roughly per frame)
        if (this.IsFalling) {
            GetRB().velocity += Vector2.up * Physics2D.gravity.y * (fallAmplificationRatio - 1) * Time.fixedDeltaTime;
        } else if (this.IsAscending && !this.IsJumpPressed()) {
            GetRB().velocity += Vector2.up * Physics2D.gravity.y * (jumpAmplificationRatio - 1) * Time.fixedDeltaTime;
        }
    }

    protected void HandleAttack()
    {
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");

        punchTimer.Update(Time.deltaTime);
        if (punchTimer.IsExpired()) {
            punchTimer.Reset();
        }
        if (!isAttacking) {
            if (Input.GetKeyDown(KeyCode.J)) {
                this.animator.SetInteger("attackType", (int)AttackType.Sword);
                this.animator.SetTrigger("attack");
            } else if (Input.GetKeyDown(KeyCode.K)) {
                this.animator.SetInteger("attackType", (int)AttackType.Punch);
                this.animator.SetTrigger("attack");

                if (!punchTimer.IsClickSequential) {
                    punchCombo.Reset();
                }
                this.animator.SetInteger("punchType", (int)punchCombo.Next());
                punchTimer.RegisterSequencialClick();
            }
        }
    }
}
