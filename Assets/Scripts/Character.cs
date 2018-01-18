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
            return this.direction;
        }
    }
}

public abstract class Character : MonoBehaviour 
{
    protected abstract Rigidbody2D GetRB();
    protected abstract Transform GetGroundChecker();
    protected abstract LayerMask GetGroundMask();

    // Horizontal movement
    protected FaceDirection faceDirection;
    protected bool isRunning = false;

    // Jumping
    protected bool isOnGround = false; // let game decide if char is on ground
    protected float groundCheckerRadius = 0.7f; // can override this in child Start

    public delegate void FaceDirectionChangedHandler(MonoBehaviour sender, FaceDirectionChangedEventArgs e);
    public event FaceDirectionChangedHandler FaceDirectionChanged;

    public FaceDirection FaceDirection 
    {
        get {
            return this.faceDirection;
        }
        set {
            if (this.faceDirection != value) {
                this.faceDirection = value;

                var e = new FaceDirectionChangedEventArgs(value);
                this.FaceDirectionChanged(this, e);
            }
        }
    }

    public bool IsFalling 
    {
        get {
            return !this.isOnGround && this.GetRB().velocity.y < 0;
        }
    }

    public bool IsAscending 
    {
        get {
            return !this.isOnGround && this.GetRB().velocity.y > 0;
        }
    }

    protected void Start() 
    {
        this.FaceDirectionChanged += OnFaceDirectionChanged;
    }

    protected bool IsRunPressed() 
    {
        return Input.GetButton("Run");
    }

    protected float GetHorizontalAxis() 
    {
        return Input.GetAxis("Horizontal");
    }

    protected virtual void Move(float speed) 
    {
        Rigidbody2D rb = this.GetRB();
        rb.velocity = new Vector2(speed, rb.velocity.y);
        if (speed > 0) {
            this.FaceDirection = FaceDirection.Right;
        } else if (speed < 0) {
            this.FaceDirection = FaceDirection.Left;
        }
    }

    protected bool IsJumpPressed() 
    {
        return Input.GetButton("Jump");
    }

    protected virtual void Jump(float force) 
    {
        if (isOnGround) {
            isOnGround = false;
            this.GetRB().AddForce(new Vector2(0, force));
        } else {
            isOnGround = Physics2D.OverlapCircle(this.GetGroundChecker().position, groundCheckerRadius, this.GetGroundMask());
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
