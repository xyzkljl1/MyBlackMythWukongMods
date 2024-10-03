using b1;
using BtlB1;
using BtlShare;
using CSharpModBase;
using Google.Protobuf.Collections;
using ResB1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Mono.Security.X509.X520;
#nullable enable
namespace EffectDetailDescription
{
    public class Desc:List<string>
    {
        public static Desc Default { get { return Data.DefaultDesc.Copy(); } }
        public static Desc Empty { get { return Data.EmptyDesc.Copy(); } }
        public bool IsTrEmpty()
        {
            return GetTr(false) == "";
        }
        public bool HasPercent()
        {
            return GetTr().Contains("%");
        }
        public string GetTr(bool useBracket = true)
        {
            return GetTr((int)MyExten.currentLanguage, useBracket);
        }
        //从List获取第i条翻译项，如果没有第i条则尝试返回English项目，如果English也没有返回最后一个，如果List为空返回空。
        public string GetTr(int i, bool useBracket = true)
        {
            if (Count == 0) return "";
            if (Count > i) return useBracket ? $"({this[i]})" : this[i];
            if (Count > (int)Data.LanguageIndex.English) return useBracket ? $"({this[(int)Data.LanguageIndex.English]})" : this[(int)Data.LanguageIndex.English];
            return useBracket ? $"({this.Last()})" : this.Last();
        }
        public static Desc operator +(Desc l, Desc r)
        {
            var ret = l.Copy();
            for (int i = 0; i < l.Count && i < r.Count; ++i)
                ret[i] += r[i];
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
        public Desc ConcatWith(Desc r,string connectionStr)
        {
            for (int i = 0; i < Count && i < r.Count; ++i)
                if (this[i] == "")
                    this[i] += r[i];
                else if (r[i]!="")
                    this[i] += connectionStr + r[i];
            return this;
        }
    }
    public static class DescTr
    {
        public static void Log(string msg) {MyExten.Log(msg);}
        public static void Error(string msg) {MyExten.Error(msg);}
        public static List<int> spellIdList = new List<int> { 10505, 10507, 10509, 10095, 10518, 10519, 10520, 10521, 10516 };
        public static Dictionary<EGSBuffAndSkillEffectActiveCondition, int> ContinousSkillEffectActiveConditionList = new Dictionary<EGSBuffAndSkillEffectActiveCondition, int> {
            { EGSBuffAndSkillEffectActiveCondition.HasAnyBuff,1},
            { EGSBuffAndSkillEffectActiveCondition.HasBuff,1},
            { EGSBuffAndSkillEffectActiveCondition.HasTalent,1},
            { EGSBuffAndSkillEffectActiveCondition.HasAnyTalent,1},
            { EGSBuffAndSkillEffectActiveCondition.ByAttr,1},
            { EGSBuffAndSkillEffectActiveCondition.TargetByAttr,1},
            { EGSBuffAndSkillEffectActiveCondition.TargetHasAnyBuff,1},
            { EGSBuffAndSkillEffectActiveCondition.TargetNotHasBuff,1},
            { EGSBuffAndSkillEffectActiveCondition.NotHasBuff,1},
            { EGSBuffAndSkillEffectActiveCondition.CheckPhysMat,1},//96022，在水中
        };

        public static string F2Str(float value, bool div100,bool noSign=false, bool usePlaceHolder = false)
        {
            if (usePlaceHolder)
                return Data.PlaceHolder;
            string valueStr = "";
            if (div100) value /= 100;
            valueStr = $"{value:F2}".TrimEnd('0').TrimEnd('.');//最多保留两位小数，去掉末尾的0,再去掉末尾的.(即整数)
            if (value > 0.01f&&noSign==false)
                valueStr = "+" + valueStr;
            //else if (value <= 0.01f)
            //    valueStr = valueStr;
            return valueStr;
        }
        public static string F2Str(IEnumerable<float> value, bool isPercent, bool usePlaceHolder=false)
        {
            if (usePlaceHolder)
                return Data.PlaceHolder;
            if (value.Count() < 1) return "";
            if (value.Count() == 1)
                return $"{F2Str(value.First(), isPercent)}";
            string ret = "";
            //多于一个值的统统加[]
            foreach(var v in value)
            {
                ret += $"[{F2Str(v, isPercent)}]";
                if (isPercent)
                    ret += "%";
                ret += "/";
            }
            //去掉末尾的/和%
            ret=ret.TrimEnd('/').TrimEnd('%');
            return ret;
        }
        public static string F2Str(float value, float value2, float value3, bool isPercent)
        {
            return F2Str(new List<float> { value, value2, value3 },isPercent);
        }
        public static bool IsPercent(this FUStBuffEffectAttr desc)
        {
            var t = desc.EffectType;
            if (t == EBuffAndSkillEffectType.RecoverAttr)
            {
                //EBGUAttrFloat
                //if (Data.EBGUAttrFloatDictConst.ContainsKey(desc.EffectParams[0]))
                //return Data.EBGUAttrFloatDictConst[desc.EffectParams[0]].GetTr().Contains("%");
                //参考BUEffectRecoverAttr.ApplyByBuff_Implement
                //由buff施加时，paramInt[3]为1时为百分比，否则为绝对值? 参考21465(蛇司药百分比回复生命) 96016(定颜珠增加生命上限并回复固定生命)
                if (desc.EffectParams.Count() > 3 && desc.EffectParams[3] > 0)
                    return true;
                return false;
            }
            else if(t==EBuffAndSkillEffectType.CostAttr)
            {
                //BANS_GSCostAttrByBuff:GSNotifyBeginCS
                //EAttrCostType
                return false;
            }
            return false;
        }
        //调用String.Format但是只format第一个参数，后面的参数前移

        public static string MoveFormat(string format, object para)
        {
            if (format.Contains("{2}"))
                return String.Format(format, para, "{0}", "{1}");
            if (format.Contains("{1}"))
                return String.Format(format, para, "{0}");
            return String.Format(format, para);
        }

        //分别根据每个talent找到唯一一个条件为HasTalent且要求该talent的buff
        //talent列表中有任意talent找到0个或多于1个对应buff就算失败
        //失败是返回空列表
        public static List<FUStBuffDesc?> GetUniqueBuffList(this List<TalentSDesc> talentDescList)
        {
            List<FUStBuffDesc?> buffDescList = new List<FUStBuffDesc?>();
            for (int i = 0; i < talentDescList.Count; ++i)
                buffDescList.Add(null);
            bool fail = false;
            foreach (var pair in BGW_GameDB.GetAllBuffDesc())
                if (fail == false && pair.Value.BuffActiveCondition.ConditionType == EGSBuffAndSkillEffectActiveCondition.HasTalent)
                {
                    for (int i = 0; i < buffDescList.Count; i++)
                        if (pair.Value.BuffActiveCondition.ConditionParams == talentDescList[i].Id.ToString())
                        {
                            if (buffDescList[i] == null)
                                buffDescList[i] = pair.Value;
                            else
                                fail = true;
                        }
                }
            if (fail)
                return new List<FUStBuffDesc?>();
            if (buffDescList.All(ele => ele != null))
                return buffDescList;
            return new List<FUStBuffDesc?>();
        }
        public static List<FUStSkillEffectDesc?> GetUniqueSkillEffectList(this List<TalentSDesc> talentDescList)
        {
            List<FUStSkillEffectDesc?> skilleffectDescList = new List<FUStSkillEffectDesc?>();
            for (int i = 0; i < talentDescList.Count; ++i)
                skilleffectDescList.Add(null);
            bool fail = false;
            foreach (var pair in BGW_GameDB.GetAllSkillEffectDesc())
                if (fail == false && pair.Value.EffectActiveCondition.ConditionType == EGSBuffAndSkillEffectActiveCondition.HasTalent)
                {
                    for (int i = 0; i < skilleffectDescList.Count; i++)
                        if (pair.Value.EffectActiveCondition.ConditionParams == talentDescList[i].Id.ToString())
                        {
                            if (skilleffectDescList[i] == null)
                                skilleffectDescList[i] = pair.Value;
                            else
                                fail = true;
                        }
                }
            if (fail)
                return new List<FUStSkillEffectDesc?>();
            if (skilleffectDescList.All(ele => ele != null))
                return skilleffectDescList;
            return new List<FUStSkillEffectDesc?>();
        }

        public static Desc ToTr(this FUStBuffEffectActiveCondition desc)
        {
            var ret = Desc.Default;
            //see BGUFunctionLibraryCS.BGUCheckBuffEffectActiveCondition
            if (desc.ConditionType == EGSBuffAndSkillEffectActiveCondition.ByAttr)//条件
            {
                var conditions = desc.ConditionParams.Split(',').Select(ele => int.Parse(ele)).ToList();
                if (conditions.Count == 5)
                {
                    var attr1 = conditions[1];
                    var attr2 = conditions[2];
                    bool flag = conditions[3]>0;
                    var target = conditions[4];
                    //ret =attr1/attr2*10000>target
                    //flag>0且ret=true，返回true
                    //flag>0且ret=false，返回false
                    //flag=0且ret=true,返回false
                    //flag=0且ret=false,返回true
                    //所以flag>0时，条件为attr1>attr2*target,反之为attr1<=attr2*target
                    if (!Data.EBGUAttrFloatDictConst.ContainsKey(attr1)) return ret;
                    if (!Data.EBGUAttrFloatDictConst.ContainsKey(attr2)) return ret;

                    bool attr1SameTypeWithAttr2 = ((EBGUAttrFloat)attr2).ToString().Contains(((EBGUAttrFloat)attr1).ToString());
                    //特别的，"高于99.99%"且attr1和attr2同类型时，如"生命高于99.99%生命上限"，直接换成"满生命上限"
                    if (target==9999 && flag && attr1SameTypeWithAttr2)
                    {
                        ret = Data.FullFormat;
                        ret.FormatWith(Data.EBGUAttrFloatDictConst[attr1].FormatWith(""));
                    }
                    else
                    {
                        ret = flag ? Data.GreaterFormat.Copy() : Data.LesserFormat.Copy();
                        var attr2Format = Data.EBGUAttrFloatDictConst[attr2];
                        attr2Format.FormatWith(F2Str(target, div100: true, noSign: true) + (ret.HasPercent() ? "" : "%"));
                        //如果attr1和attr2具有同样前缀，例如"生命高于25%生命上限"的情况，省略attr1
                        if (attr1SameTypeWithAttr2)
                            ret.FormatWith("");
                        else
                            ret.FormatWith(Data.EBGUAttrFloatDictConst[attr1].FormatWith(""));
                        ret.FormatWith(attr2Format);
                    }
                }
            }
            return ret;
        }

        public static Desc ToTr(this List<TalentSDesc> talentDescList)
        {
            var ret=Desc.Empty;

            if (talentDescList.All(ele => ele != null))
            {
                //目前并不能完美处理Passive/buff,只有简单情况使用自动生成
                if (talentDescList.All(ele => ele.PassiveSkillIDs != "" && ele.AddBuffIDs == ""))
                {
                    var PassiveIDsList = talentDescList.Select(ele => ele.PassiveSkillIDs.Split(',').ToList())
                                        .ForEach(ele => ele.Sort()).ToList();
                    //                                    if (PassiveList.Count == 1 && descList[mydesc.Key].IsTrEmpty())
                    if (PassiveIDsList.Count>0 && PassiveIDsList.All(ele=>ele.Count == 1))
                        for (int i = 0; i < PassiveIDsList.First().Count; i++)
                        {
                            var passiveDictList = PassiveIDsList.Select(ele => BGW_GameDB.GetPassiveSkillDescDic(int.Parse(ele[i])));
                            if (passiveDictList.All(ele=>ele.Count == 1))//多于一个的暂不处理
                            {
                                List<FUStPassiveSkillDesc> fistPassiveSkillDescList = passiveDictList.Select(ele => ele.First().Value).ToList();
                                ret.ConcatWith(fistPassiveSkillDescList.ToTr(), ",");
                            }
                        }
                }
                if (talentDescList.All(ele => ele.AddBuffIDs != "" && ele.PassiveSkillIDs == ""))
                {
                    var buffIDsList = talentDescList.Select(ele => ele.AddBuffIDs.Split(',').ToList())
                                        .ForEach(ele => ele.Sort()).ToList();
                    if (buffIDsList.All(ele => ele.Count == 1) && (ret.IsTrEmpty() || true))
                        for (int i = 0; i < buffIDsList[0].Count; i++)//buffIDsList肯定不为空
                        {
                            //用静态的buffdesc
                            var buffDescList = buffIDsList.Select(ele => BGW_GameDB.GetOriginalBuffDesc(int.Parse(ele[i]))).ToList();
                            ret.ConcatWith(buffDescList!.ToTr(),",");
                        }
                }
                //if(talentDesc.AddBuffIDs==""&&talentDesc.PassiveSkillIDs=="")
                {
                    //有可能AddBuffIDs和PassiveSkillIDs都为空，但是buffdesc-fr里有一个buff要求HasTalent，例如蛇司药
                    List<FUStBuffDesc?> buffDescList = talentDescList.GetUniqueBuffList();
                    //如果每个talent都对应且仅对应一个buff
                    ret.ConcatWith(buffDescList.ToTr(), ",");
                }
                {
                    //还有一种情况是SkillEffect里有一个要求HasTalent的，例如定颜珠
                    List<FUStSkillEffectDesc?> skillEffectDescList = talentDescList.GetUniqueSkillEffectList();
                    //如果每个talent都对应且仅对应一个buff
                    //目前只有精魂需要List形式，而精魂暂无此种形式的翻译,所以不传list了
                    if(skillEffectDescList.Count==1)
                        ret.ConcatWith(skillEffectDescList.First()!.ToTr(), ",");
                }
            }
            return ret;
        }
        public static Desc ToTr(this List<FUStBuffEffectAttr> descList, FUStBuffDesc buffDesc,bool isChild = false,bool PlaceHolder=false,bool noDurationOrInterval=false)//isChild:被addbuff类效果触发的子buff
        {
            var ret = new Desc();
            bool isParent = false;
            if (descList.Count < 1) return ret;
            var desc = descList.First();
            bool needDuration = !noDurationOrInterval;
            if (desc.EffectType == EBuffAndSkillEffectType.RecoverAttr)
            {
                if (desc.EffectParams.Count < 2) return ret;
                int attrfloatType = desc.EffectParams[0];
                bool isPercent = descList.First().IsPercent();
                if (Data.EBGUAttrFloatDictConst.ContainsKey(attrfloatType))
                {
                    ret = Data.EBGUAttrFloatDictConst[attrfloatType].Copy();
                    ret.FormatWith(F2Str(descList.Select(ele => (float)ele.EffectParams[1]),isPercent:isPercent,usePlaceHolder:PlaceHolder)
                        +(isPercent?"%":""));//由于此处%不是由BGUAttrFloat的format提供，需要手动补上最后一个%
                }
                ret.FormattedBy(Data.BuffAndSkillEffectTypeDictConst, (int)desc.EffectType);
                ret.FormattedBy(Data.BuffEffectTriggerTypeDictConst, (int)desc.EffectTrigger);
            }
            else if (desc.EffectType == EBuffAndSkillEffectType.AddAttr && desc.EffectTrigger == EBuffEffectTriggerType.Generation)
            {
                if (desc.EffectParams.Count < 2) return ret;
                int attrfloatType = desc.EffectParams[0];
                if (Data.EBGUAttrFloatDictConst.ContainsKey(attrfloatType))
                {
                    ret = Data.EBGUAttrFloatDictConst[attrfloatType].Copy();
                    bool isPercent = ret.GetTr().Contains("%");
                    ret.FormatWith(F2Str(descList.Select(ele => (float)ele.EffectParams[1]), isPercent, usePlaceHolder: PlaceHolder));
                }
                ret.FormattedBy(Data.BuffAndSkillEffectTypeDictConst, (int)desc.EffectType);
            }
            else if (desc.EffectType == EBuffAndSkillEffectType.IncreasePevalue)
            {
                if (desc.EffectParamsFloat.Count < 1) return ret;
                ret = Data.EBGUAttrFloatDictConst[(int)EBGUAttrFloat.Pevalue];
                ret.FormatWith(F2Str(descList.Select(ele => (float)ele.EffectParamsFloat[0]), false, usePlaceHolder: PlaceHolder));
                ret.FormattedBy(Data.BuffAndSkillEffectTypeDictConst, (int)desc.EffectType);
                ret.FormattedBy(Data.BuffEffectTriggerTypeDictConst,(int)desc.EffectTrigger);
                ret.FormatWith(buffDesc.Interval / 1000.0f);
            }
            else if (desc.EffectType == EBuffAndSkillEffectType.AddBuff)
            {
                //父buff的Duration不需要
                needDuration = false;
                if (desc.EffectParams.Count < 1) return ret;
                //此处不传递noDuration参数，noDuration是为了一个buff的多条效果共用Duration/Interval,而父buff的多个子buff并不共用
                ret = descList.Select(ele => GameDBRuntime.GetFUStBuffDesc(ele.EffectParams[0])).ToList()!.ToTr(true);
                //根据条件决定是否显示子buff的Duration，例如受击时施加buff需要显示持续时间，低血量时则不需要
                if (desc.EffectTrigger == EBuffEffectTriggerType.Time)
                {
                    //满足条件时持续施加子buff,此时子buff的持续时间没有意义，移除
                    ret.RemoveTag("Duration", ContinousSkillEffectActiveConditionList.ContainsKey(buffDesc.BuffActiveCondition.ConditionType));
                    //Log($"TTT {buffDesc.ID}  {ret.GetTr(1)} ____ {buffDesc.BuffActiveCondition.ToTr().GetTr(1)}");
                    ret.FormattedBy(buffDesc.BuffActiveCondition.ToTr());
                }
                //Log($"2FFFFFFFF{ret.GetTr(1)}");
                isParent = true;
            }
            else if (desc.EffectType == EBuffAndSkillEffectType.ChangeMoveSpeed)
            {
                if (desc.EffectParamsFloat.Count < 3) return ret;
                //虽然放在float里，实际是百分比*100
                var spa = (int)desc.EffectParamsFloat[0];
                var spb = (int)desc.EffectParamsFloat[1];
                var spc = (int)desc.EffectParamsFloat[2];
                int targetIndex = -1;
                Desc postfix;
                if (spa == spb && spb == spc)//三种一样
                {
                    targetIndex = 0;
                    postfix = Data.SpeedNameDictConst[0b111];
                }
                else if (spa == spb && spc == 0)
                {
                    targetIndex = 1;
                    postfix = Data.SpeedNameDictConst[0b11];
                }
                else if (spb == spc && spa == 0)
                {
                    targetIndex = 2;
                    postfix = Data.SpeedNameDictConst[0b110];
                }
                else
                    return ret;
                ret = Data.BuffAndSkillEffectTypeDictConst[(int)desc.EffectType].Copy();
                ret.FormatWith(F2Str(descList.Select(ele => ele.EffectParamsFloat[targetIndex]) , true, usePlaceHolder: PlaceHolder) + "%");
                ret.FormatWithList(postfix);                
            }

            if (ret.Count == 0)
                return ret;

            //注意此时ret里可能还有没替换完的参数, 先检查Interval
            if (!isParent)//如果此效果是parent，间隔和时间都由子buff的effecttrigger提供，父buff不处理
            {
                if (ret.HasTag("Inverval")&& desc.EffectTrigger == EBuffEffectTriggerType.Time)//补全间隔
                {
                    if (buffDesc.Interval == 1000)
                        ret.FormatWith("");
                    else
                        ret.FormatWith(F2Str(buffDesc.Interval / 1000.0f, div100: false, noSign: false, usePlaceHolder: PlaceHolder));
                }
                else
                    ret.RemoveTag("Interval",true);
            }
            else
                ret.RemoveTag("Interval",true);

            if (noDurationOrInterval)
                ret.RemoveTag("Interval", true);
            //然后再添加Duration
            if (needDuration&&buffDesc.Duration > 0)
            {
                ret.FormattedBy(Data.DurationFormat);
                ret.FormatWith(buffDesc.Duration / 1000.0f);
            }

            if (!isChild)//如果此buff是顶层buff,返回前移除所有尚未处理的tag
                ret.RemoveAllTag(false);
            return ret;
        }

        public static Desc ToTr(this List<FUStBuffDesc?> descList, bool isChild = false)
        {
            var ret = new Desc();
            descList = descList.Where(ele => ele is not null).ToList();
            if (descList.Count == 0) return ret;
            var desc=descList.First();
            if(descList.All(ele=>ele!.BuffEffects.Count==1))//对于复数desc(即精魂)，多于1个的不考虑
            {
                var effectDescList=descList.Select(ele => ele!.BuffEffects.First()).ToList()!;
                ret = effectDescList.ToTr(desc!,isChild);
            }
            else if(descList.Count == 1)//对于单个desc
            {
                ret = Desc.Empty;
                var effectList = desc!.BuffEffects.ToList();
                for(int i = 0 ;i < effectList.Count;++i)
                {
                    ret.ConcatWith(effectList[i].ToListMy().ToTr(desc!, isChild,noDurationOrInterval:i!=0),",");
                }
            }
            return ret;
        }
        //注意FUStPassiveSkillDesc中也有FUStSkillEffectDesc相关的部分
        //此函数是为了装备的AddBuff等FUStSkillEffectDesc准备的
        //因为精魂里没有需要单独翻译FUStSkillEffectDesc的情况,不需要使用List(暂定)
        public static Desc ToTr(this FUStSkillEffectDesc desc)
        {
            var ret = Desc.Empty;
            if(desc.EffectType== EBuffAndSkillEffectType.AddBuff)
                foreach(var buffId in desc.EffectParamsInt)
                {
                    var buffDesc=BGW_GameDB.GetOriginalBuffDesc(buffId);
                    if(buffDesc!=default(FUStBuffDesc))
                    {
                        ret.ConcatWith(buffDesc.ToListMy()!.ToTr(),";");
                    }
                }
            return ret;
        }
        public static Desc ToTr(this List<FUStPassiveSkillDesc> descList)
        {
            var ret = new Desc();
            var valueStr = "";
            if(descList.Count == 3)
            {
                var desc = descList[0];
                var desc2 = descList[1];
                var desc3 = descList[2];
                bool isPercent = desc.ValOp == EValOp.Mul || desc.ModifyMethod.ToString().Contains("Duration") || desc.ModifyMethod.ToString().Contains("Interval");
                valueStr = F2Str(desc.BaseValue, desc2.BaseValue, desc3.BaseValue, isPercent);
                return desc.ToTr(valueStr);
            }
            else if(descList.Count==1)
                return descList[0].ToTr();
            return ret;
        }
        public static Desc ToTr(this FUStPassiveSkillDesc desc, string valueStr = "")
        {
            var ret = new Desc();
            if (valueStr == "")
            {
                bool isPercent = desc.ValOp == EValOp.Mul || desc.ModifyMethod.ToString().Contains("Duration") || desc.ModifyMethod.ToString().Contains("Interval");
                valueStr = F2Str(desc.BaseValue, isPercent);
            }
            for (int i = 0; i < (int)Data.LanguageIndex.Max; ++i)
                ret.Add("");
            //ValOp部分
            if (Data.ValOpDictConst.ContainsKey((int)desc.ValOp))
                for (int i = 0; i < (int)Data.LanguageIndex.Max; ++i)
                {
                    var format = Data.ValOpDictConst[(int)desc.ValOp][i];
                    ret[i] += String.Format(format, valueStr);
                }
            //Modified Method和MainID部分
            {
                Desc? format = null;
                switch (desc.ModifyMethod)
                {
                    case EModifyMethod.SkillCooldown:
                        int contain_ct = 0;
                        foreach (var id in spellIdList)
                            contain_ct += (desc.MainID.Contains(id.ToString()) ? 1 : 0);
                        if (contain_ct == spellIdList.Count)
                            format = new Desc { "{0}法术冷却", "{0} Spell CoolDown" };
                        else
                            format = new Desc { $"{{0}} 法术冷却,影响{contain_ct}种法术", $"{{0}} CoolDown of {contain_ct} Spell" };
                        break;
                    case EModifyMethod.SkillEffectFloatN:
                        if (desc.MainID != "")//动作倍率、属性消耗/获得
                        {
                            //静态SkillDesc
                            var skillEffectList = desc.MainID.Split(',').Select(ele => BGW_GameDB.GetSkillEffectDesc(int.Parse(ele), null))
                                                                        .Where(ele=> ele!=default(FUStSkillEffectDesc)).ToList();
                            if (skillEffectList.Count() == 0)
                                break;
                            //找到所有skill共用的effecttype
                            EBuffAndSkillEffectType commonEffectType = skillEffectList.First().EffectType;
                            if(!skillEffectList.All(ele => ele.EffectType == commonEffectType))
                                commonEffectType=EBuffAndSkillEffectType.None;

                            //List<int> commonParamInt = skillEffectList.First().EffectParamsInt.ToList();
                            //if (!skillEffectList.All(ele => ele.EffectParamsInt.SequenceEqual(commonParamInt)))
                            //    commonParamInt.Clear();

                            //subID作用是？
                            //动作倍率
                            if (commonEffectType == EBuffAndSkillEffectType.SkillDamage && desc.SubID == 2)
                                format = new Desc { $"{{0}}动作倍率(影响{skillEffectList.Count}种动作)", $"{{0}} SkillEffect of {skillEffectList.Count} Actions" };
                            //动作获得/消耗属性
                            else if(commonEffectType==EBuffAndSkillEffectType.CostAttr && desc.SubID==0)
                            {
                                //BANS_GSCostAttrByBuff:GSNotifyBeginCS
                                Dictionary<EAttrCostType, int> tmpDict=new Dictionary<EAttrCostType, int>();
                                Desc tmpFormats = Desc.Default.FormatWith("");
                                //元气和法宝能量回复是放一起的，不能指望ParamsInt只有一种值
                                foreach(var skillEffect in skillEffectList)
                                    if(skillEffect.EffectParamsFloat.Count>0&&skillEffect.EffectParamsInt.Count>0)
                                    {
                                        var costAttrType = (EAttrCostType)skillEffect.EffectParamsInt[0];
                                        if (!tmpDict.ContainsKey(costAttrType))
                                        {
                                            if (tmpDict.Count > 0)
                                                tmpFormats += "&";
                                            tmpFormats.FormattedBy(Data.EAttrCostTypeDictConst, (int)costAttrType);
                                            //符号取第一个
                                            if (skillEffectList.First().EffectParamsFloat.First() > 0)
                                                tmpFormats.RemoveTag("Positive", true);
                                            else
                                                tmpFormats.RemoveTag("Negative", true);
                                            tmpFormats.RemoveTag("Positive", false);
                                            tmpFormats.RemoveTag("Negative", false);                                            
                                            tmpDict.Add(costAttrType,0);
                                        }
                                    }
                                ret += tmpFormats;
                            }
                        }
                        break;
                    case EModifyMethod.BuffEffectFloatN:
                        if (desc.MainID != "")
                        {
                            var buffList = desc.MainID.Split(',').Select(ele => BGW_GameDB.GetOriginalBuffDesc(int.Parse(ele)))
                                                                        .Where(ele => ele != default(FUStBuffDesc)).ToList();
                            if (buffList.Count() == 0)
                                break;
                            if(buffList.Count() == 1)
                                foreach(var buffDesc in buffList)
                                    if(buffDesc.BuffEffects.Count==1)
                                    {
                                        //生成buff的描述，但是不填入buff自身的数值而是用placeholder替代
                                        var buffEffectFormat=(new List<FUStBuffEffectAttr> { buffDesc.BuffEffects[0] }).ToTr(buffDesc,PlaceHolder:true);
                                        //把placeholder换成该passive的数值
                                        buffEffectFormat.ReversePlaceHolder();
                                        ret.FormattedBy(buffEffectFormat);
                                    }
                        }
                        break;
                }
                if (format == null) format = new Desc { "{0}", "{0}" };
                ret.FormattedBy(format);
            }
            return ret;
        }
        public static Desc ToTr(this EquipAttrDesc desc)
        {
            var ret = new Desc();
            var attrList = desc.GetField<RepeatedField<EffectAttrCfg>>("attr_");
            if (attrList == null || attrList.Count == 0)
                return ret;
            for (int i = 0; i < (int)Data.LanguageIndex.Max; ++i)
                ret.Add("");
            foreach (var attr in attrList)
                if (Data.EBGUAttrFloatDictConst.ContainsKey((int)attr.Type))
                {
                    var tmpList = Data.EBGUAttrFloatDictConst[(int)attr.Type];
                    for (int i = 0; i < ret.Count; ++i)
                    {
                        if (ret[i] != "")
                            ret[i] += i == (int)Data.LanguageIndex.SimpleCN ? "," : ".";
                        var format = tmpList.GetTr(i, false);
                        var value = attr.Value;
                        var valueStr = F2Str(value, format.Contains("%"));//百分比类要除100
                        ret[i] += String.Format(tmpList[i], valueStr);
                    }
                }
                else
                {
                    for (int i = 0; i < ret.Count; ++i)
                    {
                        if (ret[i] != "")
                            ret[i] += i == (int)Data.LanguageIndex.SimpleCN ? "," : ".";
                        ret[i] += String.Format("{0} {1}", attr.Value, attr.Type.ToString());
                    }
                }
            return ret;
        }
        public static Desc ToTr(this EquipAttrDesc desc, EquipAttrDesc desc2, EquipAttrDesc desc3)
        {
            var ret = new Desc();
            var attrList = desc.GetField<RepeatedField<EffectAttrCfg>>("attr_");
            var attrList2 = desc2.GetField<RepeatedField<EffectAttrCfg>>("attr_");
            var attrList3 = desc3.GetField<RepeatedField<EffectAttrCfg>>("attr_");
            if (attrList == null || attrList.Count == 0)
                return ret;
            if (attrList2 == null || attrList2.Count == 0)
                return ret;
            if (attrList3 == null || attrList3.Count == 0)
                return ret;
            if (attrList.Count != attrList2.Count || attrList3.Count != attrList2.Count)
            {
                Error($"Desc1/2/3 Attr List Count Not Same {desc.Id}");
                return ret;
            }

            for (int i = 0; i < (int)Data.LanguageIndex.Max; ++i)
                ret.Add("");
            for (int attrI = 0; attrI < attrList.Count; ++attrI)
            {
                int attrType = (int)attrList[attrI].Type;
                var value = attrList[attrI].Value;
                var value2 = attrList2[attrI].Value;
                var value3 = attrList3[attrI].Value;
                if (Data.EBGUAttrFloatDictConst.ContainsKey(attrType))
                {
                    var tmpList = Data.EBGUAttrFloatDictConst[attrType];
                    for (int i = 0; i < ret.Count; ++i)
                    {
                        if (ret[i] != "")
                            ret[i] += i == (int)Data.LanguageIndex.SimpleCN ? "," : ".";
                        var format = tmpList.GetTr(i, false);
                        var valueStr = F2Str(value, value2, value3, format.Contains("%"));
                        ret[i] += String.Format(tmpList[i], valueStr);
                    }
                }
                else
                {
                    for (int i = 0; i < ret.Count; ++i)
                    {
                        if (ret[i] != "")
                            ret[i] += i == (int)Data.LanguageIndex.SimpleCN ? "," : ".";
                        ret[i] += String.Format("{0} {3}", F2Str(value, value2, value3, false), attrList[attrI].Type.ToString());
                    }
                }
            }
            return ret;
        }
        //形如[a]/[b]/[c]的数字,返回3个字符串，返回只留第i个[]的结果
        public static string Remove2in3Bracket(this string str, int i)
        {
            i = i % 3;
            string ret = str.Copy();
            int ct = 0;
            int start_index = 0;
            while (true)
            {
                var pos = ret.IndexOf('[', start_index);
                if (pos < 0) break;
                if (ct % 3 == i)
                {
                    ct++;
                    start_index = ret.IndexOf(']', start_index) + 1;
                    continue;
                }
                ct++;
                ret = ret.Remove(pos, 1);
                ret = ret.Remove(ret.IndexOf(']', start_index), 1);//假定[]一定成对出现
                start_index = pos;
            }
            return ret;
        }
        public static List<Desc> Remove2in3Bracket(this Desc strList)
        {
            var ret = new List<Desc>();
            for (int i = 0; i < 3; ++i)
            {
                var tmp = new Desc();
                foreach (var str in strList)
                    tmp.Add(str.Remove2in3Bracket(i));
                ret.Add(tmp);
            }
            return ret;
        }
    }
    public class DescDict : Dictionary<int, Desc>
    {
        new virtual public Desc this[int index] //屏蔽基类[],改成虚函数
        {
            get { return base[index]; }
            set { base[index] = value; }
        }

    }
    public class ConstDescDict : DescDict
    {
        override public Desc this[int index] 
        {
            get { return base[index].Copy(); }
            set { throw new NotImplementedException(); }
        }
    }
}
