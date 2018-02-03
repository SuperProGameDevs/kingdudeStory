using System;
using System.Collections;
using System.Collections.Generic;
using UE = UnityEngine;

public interface IAttackSeries<T> where T : struct, IConvertible, IFormattable, IComparable
{
    T Next();
    void Reset();
}

public class RandomAttackSeries<T> : IAttackSeries<T> where T : struct, IConvertible, IFormattable, IComparable
{
    T[] types;

    public RandomAttackSeries(T[] types)
    {
        this.types = types;
    }

    public T Next()
    {
        return types[UE.Random.Range(0, types.Length - 1)];
    }

    public void Reset() 
    {
        // Do nothing
    }
}

public class ComboAttackSeries<T> : IAttackSeries<T> where T : struct, IConvertible, IFormattable, IComparable
{
    T[] types;
    int comboCounter;

    public ComboAttackSeries(T[] types) 
    {
        this.types = types;
        comboCounter = 0;
    }

    public T Next() 
    {
        if (comboCounter >= types.Length) {
            Reset();
        }

        T next = types[comboCounter];
        comboCounter++;
        return next;
    }

    public void Reset()
    {
        comboCounter = 0;
    }
}
