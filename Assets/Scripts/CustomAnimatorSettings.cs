using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct CustomAnimatorSettings<T> where T : struct, IConvertible
{
    // Map animations' enum with their string aliases
    Dictionary<T, CustomAnimation> animationMap;
    T defaultAnimation;
    int queueLimit;

    public CustomAnimatorSettings(Dictionary<T, CustomAnimation> animationMap, T defaultAnimation, int queueLimit)
    {
        this.animationMap = animationMap;
        this.defaultAnimation = defaultAnimation;
        this.queueLimit = queueLimit;
    }

    public Dictionary<T, CustomAnimation> AnimationMap
    {
        get { return this.animationMap; }
    }

    public T DefaultAnimation
    {
        get { return this.defaultAnimation; }
    }

    public int QueueLimit
    {
        get { return this.queueLimit; }
    }

    public bool CanBeInterupted(T interruptible, T interrupter)
    {
        return this[interruptible].Weight <= this[interrupter].Weight;
    }

    public CustomAnimation this[T animation]
    {
        get { return AnimationMap[animation]; }
    }
}
