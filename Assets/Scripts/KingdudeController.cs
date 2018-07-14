using System.Collections;
using System.Collections.Generic;
using DragonBones;
using UnityEngine;
using Transform = UnityEngine.Transform;
using Aliases;

[RequireComponent(typeof(Character))]
public class KingdudeController : MonoBehaviour, IAttacking
{
    [SerializeField] float maxWalkSpeed = 10;
    [SerializeField] float maxRunSpeed = 20;

    Character character;
    Animator animator;

    // Jumping
    [SerializeField] float jumpHeight = 10;
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

	private void Awake()
	{
        isAttacking = false;

        punchCombo = new ComboAttackSeries<Attack>(new[] { Attack.KingdudePunch1, Attack.KingdudePunch1, Attack.KingdudePunch2 });
        punchTimer = new SequentialClickTimer(1);

        swordCombo = new RandomAttackSeries<Attack>(new[] { Attack.KingdudeSword1, Attack.KingdudeSword2 });
        swordTimer = new SequentialClickTimer(1);

        character = GetComponent<Character>();
        animator = GetComponent<Animator>();
	}

	// Use this for initialization
	protected new void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.animator.SetFloat("xSpeed", Mathf.Abs(character.Velocity.x));
        this.animator.SetFloat("ySpeed", character.Velocity.y);
        this.animator.SetBool("isRunning", this.IsRunPressed());
        this.animator.SetBool("isOnGround", character.IsOnGround);
        HandleAttack();
    }

    // Same as Update, but called every fixed period of time
    void FixedUpdate()
    {
        // Dash is an attack with movement, so move kingdude fast in facing direction
        if (isAttacking && currentAttack == Attack.KingdudeSwordDash) {
            if (character.FaceDirection == FaceDirection.Right) {
                character.Move(dashSpeed);
            } else if (character.FaceDirection == FaceDirection.Left) {
                character.Move(-dashSpeed);
            }
        } else {
            // Disable all movement and jumping during dash
            HandleMovement();
            HandleJumping();
        }
    }

    // All movement logic is handled here (running, walking and movements during attacks)
    protected void HandleMovement()
    {
        float attackSlowDown = (isAttacking) ? 0.2f : 1;
        float xSpeed = this.GetHorizontalAxis() * ((this.IsRunPressed()) ? maxRunSpeed : maxWalkSpeed) * attackSlowDown;

        character.Move(xSpeed);
    }

    // All jumping logic is handled here
    protected void HandleJumping()
    {
        if (this.IsJumpPressed() || !character.IsOnGround) {
            // All jump logic is inside Jump function
            // Checking if character is on ground is handled
            character.Jump(jumpHeight);
        }

        // Multiplying by deltaTime is to apply gravity per second, not per FixedUpdate (roughly per frame)
        if (character.IsFalling) {
            character.Velocity += Vector2.up * Physics2D.gravity.y * (fallAmplificationRatio - 1) * Time.fixedDeltaTime;
        } else if (character.IsAscending && !this.IsJumpPressed()) {
            character.Velocity += Vector2.up * Physics2D.gravity.y * (jumpAmplificationRatio - 1) * Time.fixedDeltaTime;
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

    private bool IsRunPressed()
    {
        return Input.GetButton("Run");
    }

    private float GetHorizontalAxis()
    {
        return Input.GetAxis("Horizontal");
    }

    private bool IsJumpPressed()
    {
        return Input.GetButton("Jump");
    }
}
