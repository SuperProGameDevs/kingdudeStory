using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct CustomAnimation
{
    string name;
    int playTimes;
    float playSpeed;
    float fadeInTime;
    int weight;

    public CustomAnimation(string name, int weight, int playTimes = -1, float playSpeed = 1.0f, float fadeInTime = -1.0f)
    {
        this.name = name;
        this.weight = weight;
        this.playTimes = playTimes;
        this.playSpeed = playSpeed;
        this.fadeInTime = fadeInTime;
    }

    public string Name
    {
        get { return name; }
    }

    public int Weight
    {
        get { return weight; }
    }

    public int PlayTimes
    {
        get { return playTimes; }
    }

    public float PlaySpeed
    {
        get { return playSpeed; }
    }

    public float FadeInTime
    {
        get { return fadeInTime; }
    }
}
