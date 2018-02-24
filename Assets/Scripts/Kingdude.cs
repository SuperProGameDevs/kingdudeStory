using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;
using Aliases;

public class Kingdude : Character, IAttacking
{
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
    Attack currentAttack;
    AttackGroup currentAttackGroup;

    // Punching
    IAttackSeries<Attack> punchCombo;
    SequentialClickTimer punchTimer;
    // Sword
    IAttackSeries<Attack> swordCombo;
    SequentialClickTimer swordTimer;
    // SwordDash
    [SerializeField] float dashSpeed = 30;

    // Use this for initialization
    protected new void Start()
    {
        isAttacking = false;

        punchCombo = new ComboAttackSeries<Attack>(new[] { Attack.KingdudePunch1, Attack.KingdudePunch1, Attack.KingdudePunch2 });
        punchTimer = new SequentialClickTimer(1);

        swordCombo = new RandomAttackSeries<Attack>(new[] { Attack.KingdudeSword1, Attack.KingdudeSword2 });
        swordTimer = new SequentialClickTimer(1);

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

        // Dash is an attack with movement, so move kingdude fast in facing direction
        if (isAttacking && currentAttack == Attack.KingdudeSwordDash) {
            if (this.FaceDirection == FaceDirection.Right) {
                this.Move(dashSpeed);
            } else if (this.FaceDirection == FaceDirection.Left) {
                this.Move(-dashSpeed);
            }
        } else {
            // Disable all movement and jumping during dash
            HandleMovement();
            HandleJumping();
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

    // All movement logic is handled here (running, walking and movements during attacks)
    protected void HandleMovement()
    {
        isRunning = this.IsRunPressed();
        float attackSlowDown = (isAttacking) ? 0.2f : 1;
        float xSpeed = this.GetHorizontalAxis() * ((isRunning) ? maxRunSpeed : maxWalkSpeed) * attackSlowDown;

        this.Move(xSpeed);
    }

    // All jumping logic is handled here
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

    // All attacking logic is handled here
    protected void HandleAttack()
    {
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("attack");

        // Punching timer to reset attack combination
        punchTimer.Update(Time.deltaTime);
        if (punchTimer.IsExpired()) {
            punchTimer.Reset();
        }

        // Start next attack only when previous attack is finished
        if (!isAttacking) {
            currentAttack = Attack.None;
            currentAttackGroup = AttackGroup.None;

            if (Input.GetKeyDown(KeyCode.J)) {
                // Sword attaks
                currentAttackGroup = AttackGroup.KingdudeSword;
                currentAttack = swordCombo.Next();
            } else if (Input.GetKeyDown(KeyCode.K)) {
                // Punch attacks
                currentAttackGroup = AttackGroup.KingdudePunch;
                if (!punchTimer.IsClickSequential) {
                    punchCombo.Reset();
                }
                currentAttack = punchCombo.Next();
                punchTimer.RegisterSequencialClick();
            } else if (Input.GetKeyDown(KeyCode.L)) {
                // Sword dash
                currentAttackGroup = AttackGroup.KingdudeDash;
                currentAttack = Attack.KingdudeSwordDash;
            }

            if (currentAttack != Attack.None) {
                this.animator.SetInteger("attackGroup", (int)currentAttackGroup);
                this.animator.SetTrigger("attack");
                this.animator.SetInteger("attackType", (int)currentAttack);
            }
        }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public Attack CurrentAttack 
    {
        get { return currentAttack; }
    }

    public AttackGroup CurrentAttackGroup
    {
        get { return currentAttackGroup; }
    }
}
