using System;

public interface IAttackSeries<T> where T : struct, IConvertible, IFormattable, IComparable
{
    T Next();
    void Reset();
}
