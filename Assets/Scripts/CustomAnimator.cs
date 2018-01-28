using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class CustomAnimator<T> where T : struct, IConvertible
{
    UnityArmatureComponent uac;
    CustomAnimatorSettings<T> settings;
    Queue<T> animationQueue;

    public CustomAnimator(UnityArmatureComponent uac, CustomAnimatorSettings<T> settings)
    {
        this.uac = uac;
        this.settings = settings;
        this.animationQueue = new Queue<T>(settings.QueueLimit);
        CurrentAnimation = this.settings.DefaultAnimation;
    }

    public T CurrentAnimation { get; private set; }

    public CustomAnimatorSettings<T> Settings
    {
        get { return settings; }
    }

    public UnityArmatureComponent UAC
    {
        get { return uac; }
    }

    public void Play(T animation)
    {
        animationQueue.Enqueue(animation);
    }

    public void Resolve()
    {
        T next = settings.DefaultAnimation;
        bool isFromQueue = false;
        if (animationQueue.Count > 0) {
            if (settings.CanBeInterupted(next, animationQueue.Peek())) {
                next = animationQueue.Peek();
                isFromQueue = true;
            }
        }
        if (!CurrentAnimation.Equals(next) && (settings.CanBeInterupted(CurrentAnimation, next) || UAC.animation.isCompleted)) {
            if (isFromQueue) {
                animationQueue.Dequeue();
            }
            CurrentAnimation = next;
            CustomAnimation nextAnimation = settings[next];
            Debug.Log(nextAnimation.Name);
            UAC.animation.timeScale = nextAnimation.PlaySpeed;
            UAC.animation.FadeIn(nextAnimation.Name, nextAnimation.FadeInTime, nextAnimation.PlayTimes);
        }
    }
}
