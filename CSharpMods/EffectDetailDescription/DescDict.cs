using System;
using System.Collections.Generic;

namespace EffectDetailDescription;

public class DescDict : Dictionary<int, Desc>
{
    public new virtual Desc this[int index] //屏蔽基类[],改成虚函数
    {
        get => base[index];
        set => base[index] = value;
    }
}
public class ConstDescDict : DescDict
{
    public override Desc this[int index] 
    {
        get => base[index].Copy();
        set => throw new NotImplementedException();
    }
}