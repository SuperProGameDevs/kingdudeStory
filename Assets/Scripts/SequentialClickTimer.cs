using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialClickTimer
{
    float awaitTime;
    float timePassed;
    bool isClickSequential;

    public SequentialClickTimer(float awaitTime)
    {
        this.awaitTime = awaitTime;
        this.timePassed = 0;
        isClickSequential = false;
    }

    public bool IsClickSequential
    {
        get { return isClickSequential; }
    }

    public void Update(float deltaTime)
    {
        this.timePassed += deltaTime;
    }

    public void Reset()
    {
        this.timePassed = 0;
        isClickSequential = false;
    }

    public void RegisterSequencialClick()
    {
        this.timePassed = 0;
        isClickSequential = true;
    }

    public bool IsExpired()
    {
        return this.timePassed >= awaitTime;
    }
}
