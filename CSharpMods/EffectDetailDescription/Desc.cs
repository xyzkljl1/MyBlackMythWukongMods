using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using b1;
using System;
using CSharpModBase;

#nullable enable
namespace EffectDetailDescription;
public class Desc:List<string>
{
    public static readonly Desc DefaultDesc = ["{0}", "{0}"];
    public static readonly Desc EmptyDesc = ["", ""];
    public static readonly Desc NoneDesc = ["", ""];
    public static Desc Default => DefaultDesc.Copy();
    public static Desc Empty => EmptyDesc.Copy();
    public static Desc None => NoneDesc.Copy();

    public new string this[int index]
    {
        get => Get(index);
        set => base[index] = value;
    }

    public bool IsTrNull()
    {
        return GetTr(false) == "";
    }
    public bool HasPercent()
    {
        return GetTr().Contains("%");
    }
    public string GetTr(bool useBracket = true)
    {
        return GetTr((int)MyExten.CurrentLanguage, useBracket);
    }
    //从List获取第i条翻译项，如果没有第i条则尝试返回English项目，如果English也没有返回最后一个，如果List为空返回空。
    public string GetTr(int i, bool useBracket = true)
    {
        var tmp = this[i];
        if (tmp != ""&& useBracket) 
            return $"({tmp})";
        return tmp;
    }
    public string Get(int i)
    {
        if (Count > i) return base[i];
        if (Count > (int)Data.LanguageIndex.English) return base[(int)Data.LanguageIndex.English];
        if(Count>0)
            return this.Last();
        return "";
    }
    public void AddOrSet(int i,string value)
    {
        while (i >= Count+1)
            Add(Count > 0 ? this.First() : value);
        if(i>=Count)
            Add(value);
        else
            this[i] = value;
    }

    public Desc TrimBracket()
    {
        for(int i=0;i<Count;++i)
            if (this[i] != "")
                this[i] = $"({this[i]})";
        return this;
    }

    public static Desc operator +(Desc l, Desc r)
    {
        var ret = l.Copy();
        for (int i = 0; i < l.Count; ++i)
            ret[i]+= r.GetTr(i,false);
        for (int i = l.Count; i < r.Count; ++i)
            ret.Add(ret.GetTr(i,false)+r[i]);
        return ret;
    }
    public static Desc operator +(Desc l, string r)
    {
        var ret = l.Copy();
        for (int i = 0; i < l.Count; ++i)
            ret[i] += r;
        return ret;
    }
    public static Desc operator +(string l, Desc r)
    {
        var ret = r.Copy();
        for (int i = 0; i < r.Count; ++i)
            ret[i] = l+ret[i];
        return ret;
    }
    //注意！！！！！
    //以下函数虽然返回this，但是是原地修改this
    public Desc FormatWith(object para)
    {
        for (int i = 0; i < Count; ++i)
            this[i] = DescTr.MoveFormat(this[i], para);
        return this;
    }
    public Desc FormatWith(Desc para)
    {
        return FormatWithList<string>(para);
    }
    public Desc FormatWithList<T>(List<T> para)
    {
        for (int i = 0; i < Count && i < para.Count; ++i)
            this[i] = DescTr.MoveFormat(this[i], para[i]!);
        return this;
    }
    public Desc FormattedBy(Desc formatList)
    {
        for (int i = 0; i < Count; ++i)
        {
            var format = formatList.GetTr(i, false);
            this[i] = DescTr.MoveFormat(format, this[i]);
        }
        return this;
    }
    public Desc FormattedBy(DescDict dict, int key)
    {
        if (dict.ContainsKey(key))
            FormattedBy(dict[key]);
        return this;
    }

    public bool HasTag(string tag)
    {
        if(this.Count== 0) return false;
        return this.First().Contains($"<{tag}>");
    }
    //移除指定<tag></tag>
    public void RemoveTag(string tag,bool removeContent=true)
    {
        if(removeContent)
            for (int i = 0; i < Count; ++i)
                this[i] = Regex.Replace(this[i], $"<{tag}>.*?</{tag}>", "");
        else
            for (int i = 0; i < Count; ++i)
            {
                this[i] = Regex.Replace(this[i], $"<[/]*{tag}>", "");
                this[i]=this[i].Replace($"<{tag}>", "").Replace($"</{tag}>", "");
            }
    }
    public void RemoveAllTag(bool removeContent = true)
    {
        if (removeContent)
            for (int i = 0; i < Count; ++i)
                this[i] = Regex.Replace(this[i], "<.*?>.*?</.*?>", "");
        else
            for (int i = 0; i < Count; ++i)
                this[i] = Regex.Replace(this[i], "<.*?>", "");
    }
    //将palceholder替换回{0}
    public void ReversePlaceHolder()
    {
        for (int i = 0; i < Count; ++i)
            this[i] = this[i].Replace(Data.PlaceHolder, "{0}");
    }
    //使用该分隔符拼接
    public Desc ConcatWith(Desc r, string connectionStr = ",")
    {
        for (int i = 0; i < Count || i < r.Count; ++i)
            if (this[i] == "")
                AddOrSet(i,this[i]+r[i]);
            else if (r[i] != "")
                AddOrSet(i,this[i]+connectionStr+r[i]);
        return this;
    }
    public Desc ConcatWith(Desc r, Desc connectionStr)
    {
        for (int i = 0; i < Count||i<r.Count; ++i)
            if (this[i] == "")
                AddOrSet(i,this[i]+r[i]);
            else if (r[i] != "")
                AddOrSet(i,this[i]+connectionStr[i]+r[i]);
        return this;
    }

    public bool TrEqual(Desc r)
    {
        for (int i = 0; i < (int)Data.LanguageIndex.Max; ++i)
            if (this.GetTr(i,false) != r.GetTr(i,false))
                return false;
        return true;
    }
}
public class ComboDesc
{
    public class ComboItem:List<int>
    {
        public int SkillId => this.Count>0?this[0]:-1;

        public int SkillCount => this.Count>1?this[1]:1;
        public bool IsBuff=>this.Count>2 && this[2]==1;
        public ComboItem() { }

        public Desc GetARDesc()
        {
            if (!IsBuff)
            {
                var skillEffectDesc = BGW_GameDB.GetOriginalSkillEffectDesc(SkillId);
                if (skillEffectDesc is null)
                {
                    MyExten.Error($"Cant get skillEffectDesc {SkillId}");
                    return Desc.Empty;
                }
                return skillEffectDesc.GetARDesc(SkillCount);
            }
            else
            {
                var buffDesc = BGW_GameDB.GetOriginalBuffDesc(SkillId);
                if (buffDesc is null)
                {
                    MyExten.Error($"Cant get buffDesc {SkillId}");
                    return Desc.Empty;
                }
                return buffDesc.GetARDesc(SkillCount);
            }
        }        
        public Desc GetAREleShortDesc()
        {
            if (!IsBuff)
            {
                var skillEffectDesc = BGW_GameDB.GetOriginalSkillEffectDesc(SkillId);
                if (skillEffectDesc is null)
                {
                    MyExten.Error($"Cant get skillEffectDesc {SkillId}");
                    return Desc.Empty;
                }
                return skillEffectDesc.GetAREleShortDesc();
            }
            else
            {
                var buffDesc = BGW_GameDB.GetOriginalBuffDesc(SkillId);
                if (buffDesc is null)
                {
                    MyExten.Error($"Cant get buffDesc {SkillId}");
                    return Desc.Empty;
                }
                return buffDesc.GetAREleShortDesc();
            }
        }
    }

    public virtual Desc Name
    {
        get => _name;
        set => _name = value;
    }
    protected Desc _name=Desc.Empty;
    public Desc Postfix=Desc.Empty;
    public Desc Prefix=Desc.Empty;
    public List<List<ComboItem>> SkillEffectIdList= new List<List<ComboItem>>();
    public ComboDesc() { }

    public ComboDesc(List<List<ComboItem>> _list)
    {
        SkillEffectIdList = _list; }

    public Desc ToDesc()
    {
        var ret = Desc.Empty;
        ret += Prefix!;
        ret += Name!;
        bool useGeneralElementType = true;
        //先过一遍看是否所有技能的元素类型都一致，如果都一致，就在name后面统一标注元素类型，否则在每个数字后面单独标
        {
            Desc? generalEleDesc = null;
            foreach (var comboItemList in SkillEffectIdList)
                if (useGeneralElementType)
                    foreach (var comboItem in comboItemList)
                        if (useGeneralElementType)
                        {
                            var desc = comboItem.GetAREleShortDesc();
                            if (generalEleDesc is null)
                                generalEleDesc = desc;
                            else if (!generalEleDesc.TrEqual(desc))
                                useGeneralElementType = false;
                        }

            if (generalEleDesc is null)
                useGeneralElementType = false;
            else if (useGeneralElementType)
                ret += generalEleDesc.TrimBracket();
        }
        //combo动作值
        {
            var middleDesc = Desc.Empty;
            foreach (var comboItemList in SkillEffectIdList)
            {
                var tmpDesc = Desc.Empty;
                foreach (var comboItem in comboItemList)
                {
                    var arDesc = comboItem.GetARDesc();
                    if (arDesc.IsTrNull()) continue;
                    tmpDesc.ConcatWith(arDesc, "+");
                    if (!useGeneralElementType)
                        tmpDesc += comboItem.GetAREleShortDesc().TrimBracket();
                }
                middleDesc.ConcatWith(tmpDesc, "/");
            }
            ret.ConcatWith(middleDesc, ":");
        }
        ret += Postfix!;
        return ret;
    }
}
public class ComboDescL : ComboDesc
{
    public override Desc Name=>Data.LightAttackPrefix;
}
public class ComboDescH : ComboDesc
{
    public override Desc Name=>Data.HeavyAttackPrefix;
}
public class ComboDescS : ComboDesc
{
    public override Desc Name=>Data.SpellAttackPrefix;
}
public class ComboDescEx : ComboDesc
{
    public override Desc Name=>Data.ExitAttackPrefix;
}
public class ComboDescEn : ComboDesc
{
    public override Desc Name=>Data.EnterAttackPrefix;
}
