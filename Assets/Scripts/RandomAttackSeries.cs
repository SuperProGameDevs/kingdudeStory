using System;
using UE = UnityEngine;

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