using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection { Right, Left }

public class FaceDirectionChangedEventArgs : EventArgs 
{
    protected FaceDirection direction;

    public FaceDirectionChangedEventArgs(FaceDirection direction) 
    {
        this.direction = direction;
    }

    public FaceDirection FaceDirection 
    {
        get {
            return direction;
        }
    }
}

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    // Horizontal movement
    FaceDirection faceDirection;
    bool isRunning = false;

    // Jumping
    bool isOnGround = false; // let game decide if char is on ground
    float groundCheckerRadius = 0.7f; // can override this in child Start
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundMask;

    public delegate void FaceDirectionChangedHandler(MonoBehaviour sender, FaceDirectionChangedEventArgs e);

    public event FaceDirectionChangedHandler FaceDirectionChanged;

    public FaceDirection FaceDirection
    {
        get { return faceDirection; }
        set {
            if (faceDirection != value) {
                faceDirection = value;

                var e = new FaceDirectionChangedEventArgs(value);
                FaceDirectionChanged(this, e);
            }
        }
    }

    public bool IsFalling
    {
        get { return !isOnGround && rb.velocity.y < 0; }
    }

    public bool IsAscending
    {
        get { return !isOnGround && rb.velocity.y > 0; }
    }

    public bool IsOnGround
    {
        get { return isOnGround; }
    }

    // Called before Start
	private void Awake()
	{
        FaceDirectionChanged += OnFaceDirectionChanged;
        rb = GetComponent<Rigidbody2D>();
	}

	protected void Start()
    {
    }


    public void Move(float speed)
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        if (speed > 0) {
            FaceDirection = FaceDirection.Right;
        } else if (speed < 0) {
            FaceDirection = FaceDirection.Left;
        }
    }

    public void Jump(float force)
    {
        if (isOnGround) {
            isOnGround = false;
            rb.AddForce(new Vector2(0, force));
        } else {
            isOnGround = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundMask);
        }
    }

    // Changes facing direction of the character
    // Can be overriden for custom usage
    protected virtual void OnFaceDirectionChanged(MonoBehaviour sender, FaceDirectionChangedEventArgs e)
    {
        Vector3 localScale = sender.transform.localScale;
        if (e.FaceDirection == FaceDirection.Right) {
            localScale.x = Mathf.Abs(localScale.x);
        } else {
            localScale.x = -Mathf.Abs(localScale.x);
        }
        sender.transform.localScale = localScale;
    }
}
