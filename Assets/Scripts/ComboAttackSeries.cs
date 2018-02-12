using System;

public class ComboAttackSeries<T> : IAttackSeries<T> where T : struct, IConvertible, IFormattable, IComparable
{
    T[] types;
    int comboCounter;

    public ComboAttackSeries(T[] types)
    {
        this.types = types;
        comboCounter = 0;
    }

    public int Length { 
        get { return this.types.Length; }
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
