using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;

public enum KingdudeAnimation { Idle, Walk, Run, Ascend, Descend, PunchWeak, PunchStrong, Sword }

public class Kingdude : Character
{
    enum AttackType
    {
        Sword = 1,
        Punch = 2
    }

    [SerializeField] float maxWalkSpeed = 10;
    [SerializeField] float maxRunSpeed = 20;

    Rigidbody2D dudeRB;
    CustomAnimator<KingdudeAnimation> animator;

        // Jumping
    [SerializeField] float jumpHeight = 10;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float jumpAmplificationRatio = 2;
    [SerializeField] float fallAmplificationRatio = 2.5f;

    bool isAttacking = false;
    int sameAttackCount = 0;

    // Use this for initialization
    protected new void Start()
    {
        this.dudeRB = this.GetComponent<Rigidbody2D>();

        CustomAnimatorSettings<KingdudeAnimation> settings =
            new CustomAnimatorSettings<KingdudeAnimation>(GetAnimations(), KingdudeAnimation.Idle, 100);
        animator = new CustomAnimator<KingdudeAnimation>(this.GetComponent<UnityArmatureComponent>(), settings);
        //this.animator = this.GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(GetRB().velocity.x) > 0.01) {
            if (isRunning && animator.CurrentAnimation != KingdudeAnimation.Run) {
                this.animator.Play(KingdudeAnimation.Run);
            } else if (animator.CurrentAnimation != KingdudeAnimation.Walk) {
                this.animator.Play(KingdudeAnimation.Walk);
            }
        }
        //this.animator.SetFloat("xSpeed", Mathf.Abs(this.dudeRB.velocity.x));
        //this.animator.SetFloat("ySpeed", this.dudeRB.velocity.y);
        //this.animator.SetBool("isRunning", this.isRunning);
        //this.animator.SetBool("isOnGround", this.isOnGround);
        this.animator.Resolve();
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
        float xSpeed = this.GetHorizontalAxis() * ((isRunning) ? maxRunSpeed : maxWalkSpeed);

        this.Move(xSpeed);
    }

    protected void HandleJumping()
    {
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

    protected void HandleAttack()
    {
        //if (Input.GetKeyDown(KeyCode.J)) {
        //    this.animator.SetTrigger("attack");
        //    this.animator.SetInteger("attackType", (int) AttackType.Sword);
        //} else if (Input.GetKeyDown(KeyCode.K)) {
        //    this.animator.SetTrigger("attack");
        //    this.animator.SetInteger("attackType", (int) AttackType.Punch);
        //    if (sameAttackCount == 0) {
        //        this.animator.SetTrigger("comboSwitch");
        //    }
        //}
    }

    protected Dictionary<KingdudeAnimation, CustomAnimation> GetAnimations()
    {
        CustomAnimation Idle = new CustomAnimation("Idle", 0);
        CustomAnimation Walk = new CustomAnimation("Walking", 10, 1);
        CustomAnimation Run = new CustomAnimation("Running", 10);

        return new Dictionary<KingdudeAnimation, CustomAnimation> {
            { KingdudeAnimation.Idle, Idle },
            { KingdudeAnimation.Walk, Walk },
            { KingdudeAnimation.Run, Run }
        };
    }
}
