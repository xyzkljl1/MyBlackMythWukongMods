using System;
using b1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using ResB1;
using HarmonyLib;
using GSE.GSSdk;
using B1UI.GSUI;
using GSE.GSUI;
using System.Reflection;
using UnrealEngine.UMG;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using EffectDetailDescription;
using b1.Localization;
using System.Linq;
using UnrealEngine.Engine;
using static UnrealEngine.Runtime.HotReload;
using UnrealEngine.Runtime;
using System.Threading;
using System.Timers;
using BtlB1;
using System.Reflection.Emit;
using b1.Protobuf.DataAPI;
using Mono.Unix.Native;
using Google.Protobuf.Collections;
#nullable enable
using CommB1;
using System.Text.RegularExpressions;
using OssB1;
namespace EffectDetailDescription
{
    public static class MyExten
    {
        private static UWorld? _world;
        public static string Name => "EffectDetailDescription";
        public static Data.LanguageIndex CurrentLanguage = Data.LanguageIndex.SimpleCN;
        public static void Log(string msg)
        {
            Console.WriteLine($"[{Name}]: {msg}");
        }
        public static void Error(string msg)
        {
            Console.WriteLine($"[{Name}] Error! : {msg}");
        }
        public static void DebugLog(string msg)
        {
            Console.WriteLine($"[{Name}]Debug: {msg}");
        }


        public static TFieldType? GetField<TFieldType>(this object obj, String fieldName) where TFieldType : class
        {
            var t = obj.GetType();
            var field = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
            if (field is null)
                field = t.GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
            if (field is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {fieldName}");
                return default(TFieldType);
            }
            return field.GetValue(obj) as TFieldType;
        }
        public static object? CallPrivateFunc(this object obj, String methodName, object[] paras)
        {
            var t = obj.GetType();
            var methodInfo = t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {methodName}");
                return null;
            }
            //return null;
            return methodInfo.Invoke(obj, paras);
        }
        public static object? CallPrivateGenericFunc(this object obj, String methodName, Type[] paraType4Search, Type[] genericTypes, object[] paras)
        {
            var t = obj.GetType();
            foreach (var methodInfo in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                if (methodInfo.IsGenericMethod && methodInfo.Name == methodName)
                {
                    var para_types = methodInfo.GetParameters();
                    if (para_types.Length == paraType4Search.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < paraType4Search.Length; i++)//根据参数类型搜索
                            if (para_types[i].ParameterType.Name != paraType4Search[i].Name)
                            {
                                isMatch = false;
                                //Console.WriteLine($"Not Match:{para_types[i].ParameterType.Name}/{para_type4search[i].Name}");
                                break;
                            }
                        if (isMatch)
                        {
                            //Console.WriteLine($"Find: {method_name}");
                            var insMethodInfo = methodInfo.MakeGenericMethod(genericTypes);//传入泛型参数获得泛型方法实例
                            if (insMethodInfo is null)
                                Console.WriteLine($"{Name} Fatal Error: Can't get instance of generic: {methodName}");
                            else
                                return insMethodInfo.Invoke(obj, paras);
                        }
                    }

                }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {methodName}");
            return null;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
            return source;
        }
        public static List<T> ToListMy<T>(this T obj) where T : Google.Protobuf.IMessage
        {
            return new List<T> { obj };
        }
        public static List<T?> ToListMyNullable<T>(this T obj) where T : Google.Protobuf.IMessage
        {
            return new List<T?> { obj };
        }
        public static UWorld? GetWorld()
        {
            if (_world == null)
            {
                UObjectRef uobjectRef = GCHelper.FindRef(FGlobals.GWorld);
                _world = uobjectRef?.Managed as UWorld;
            }
            return _world;
        }
    }
    [HarmonyPatch(typeof(VITreasureDetail), "OnEquipIdChange")]
    class PatchFabaoCost
    {
        static void Postfix(VITreasureDetail __instance, ChangeReason Reason, int OldValue, int NewValue)
        {

            if (Data.SpiritCost.ContainsKey(NewValue))
            {
                var textCost = __instance.FindChildWidget("TxtCost") as UTextBlock;//非成员变量，靠FindChildWidget获取
                if (textCost != null)
                    textCost.SetText(FText.FromString(GSB1UIUtil.GetUIWordDescFText(EUIWordID.SOULSKILL_ENERGY_CAST_HIGH).ToString() + Data.SpiritCost[NewValue].GetTr()));
            }
            //else
                //MyExten.Log($"NotFind Treasure {NewValue}");
        }
    }
    [HarmonyPatch(typeof(VIRZDDetail), "OnChangeItemId")]
    class PatchVigorCost
    {
        static void Postfix(VIRZDDetail __instance, ChangeReason Reason, int OldValue, int NewValue)
        {
            if (Data.SpiritCost.ContainsKey(NewValue))
            {
                var textCost = __instance.GetField<UTextBlock>("TxtCost");
                if (textCost != null)
                    textCost.SetText(FText.FromString(textCost.GetText() + Data.SpiritCost[NewValue].GetTr()));
            }
            //else
            //MyExten.Log($"NotFind{NewValue}");
        }
    }
    [HarmonyPatch(typeof(BGUPlayerCharacterCS), nameof(BGUPlayerCharacterCS.AfterInitAllComp))]
    class PatchInit
    {
        static void Postfix(BGUPlayerCharacterCS __instance)
        {
            try
            {
                MyMod.PreFillDict();
                MyMod.InitDescProtobufAndLanguage();
            }
            catch (Exception e) 
            {
                MyExten.Error(e.Message);
                MyExten.Error(e.InnerException.Message);
            }
        }
    }

    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.7.1";
        private readonly Harmony harmony;
        //Ctrl F5重新加载mod时，类会重新加载，静态变量也会重置
        public static Boolean inited = false;//InitDescProtobufAndLanugage called
        public static Boolean preInited = false;//prefilldict called

        public MyMod()
        {
            harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        static public void Log(string msg)
        {
            MyExten.Log(msg);
        }
        static public void Error(string msg)
        {
            MyExten.Error(msg);
        }
        static public void DebugLog(string msg)
        {
#if DEBUG
            MyExten.DebugLog(msg);
#endif
        }

        public void Init()
        {
            Log("MyMod::Init called");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            //Utils.RegisterKeyBind(ModifierKeys.Control, Key.F12, () => {
            //    MyExten.Log("Manually Init"); 
            //    PreFillDict();
            //    InitDescProtobufAndLanugage(); 
            //});

            //如果此时游戏已经初始化完成(以GSLocalization.IsInit为准)则立刻进行预处理，否则在InitDescProtobufAndLanugage前进行
            if (MyExten.GetWorld() != null && GSLocalization.IsInit)
            {
                PreFillDict();//对Data.XXXDict的预处理
                InitDescProtobufAndLanguage();
            }
            else
                Log("Not Ready.Delay Pre Init");


            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            //不要随便用RunOnGameThread!!!!调用累计超过12672次后会导致游戏卡住，只在必要时调用
            //或创建一个Comp绑定到主角，然后在Comp的tick里执行，需要setTickEnable，reCalTick？
            //initDescTimer.Elapsed += (Object source, ElapsedEventArgs e) => TryInitDesc();
            // hook
            harmony.PatchAll();
        }

        
        public static void PreFillDict()
        {
            if (preInited)
            {
                Log("Skip Dup preInit");
                return;
            }
            //精魂和法宝消耗
            {
                var cost = new List<int> { 0, 0, 0, 0, 0, 0 };
                var costDict = new Dictionary<int, float>();
                foreach (var desc in GSProtobufRuntimeAPI<TBSoulSkillDesc, SoulSkillDesc>.Get().GetAll().List)
                    costDict.Add(desc.Id, desc.CastEnergy);
                foreach (var pair in costDict)
                {
                    int id = pair.Key;
                    int level = (id / 100) % 10;//形如8011->8111->8211...取百位
                    if (level != 0) continue;
                    float cost1 = costDict.ContainsKey(id) ? costDict[id] : 0;
                    float cost2 = costDict.ContainsKey(id + 100) ? costDict[id + 100] : 0;
                    float cost3 = costDict.ContainsKey(id + 200) ? costDict[id + 200] : 0;
                    float cost4 = costDict.ContainsKey(id + 400) ? costDict[id + 400] : 0;
                    Data.SpiritCost.Add(id, new Desc { $"[{cost1:F1}]/{cost2:F1}/{cost3:F1}/{cost4:F1}" });
                    Data.SpiritCost.Add(id + 100, new Desc { $"{cost1:F1}/[{cost2:F1}]/{cost3:F1}/{cost4:F1}" });
                    Data.SpiritCost.Add(id + 200, new Desc { $"{cost1:F1}/{cost2:F1}/[{cost3:F1}]/{cost4:F1}" });
                    Data.SpiritCost.Add(id + 300, new Desc { $"{cost1:F1}/{cost2:F1}/[{cost3:F1}]/{cost4:F1}" });
                    Data.SpiritCost.Add(id + 400, new Desc { $"{cost1:F1}/{cost2:F1}/{cost3:F1}/[{cost4:F1}]" });
                    Data.SpiritCost.Add(id + 500, new Desc { $"{cost1:F1}/{cost2:F1}/{cost3:F1}/[{cost4:F1}]" });
                }
                foreach(var desc in GSProtobufRuntimeAPI<TBEquipFaBaoAttrDesc, EquipFaBaoAttrDesc>.Get().GetAll().List)
                    Data.SpiritCost.Add(desc.Id, new Desc { $"{desc.CastEnergy:F1}" });
                Log($"Init Vigor And Treasure Cost Dict {Data.SpiritCost.Count}");
            }
            //珍玩和法宝
            {

                int ct = 0;
                foreach (var descList in new List<DescDict> { Data.EquipDesc, Data.FabaoAttrDesc })
                    foreach (var mydesc in descList.Copy())
                    {
                        var desc = GameDBRuntime.GetEquipDesc(mydesc.Key);
                        if (desc == null) continue;
                        if (!mydesc.Value.IsTrNull())//有一些自动生成效果并不好，原来有值的统统跳过生成
                            continue;
                        if (desc.EquipPosition == EquipPosition.Accessory || desc.EquipPosition == EquipPosition.Fabao)
                        {
                            //只考虑仅一个效果的情况，直接赋值不拼接
                            if (desc is { AttrEffectId: > 0, SuitId: 0, EquipEffectId: 0 })
                            {
                                //EquipDesc武器护甲的attrEffect就是面板，葫芦的是酒数量上限，只有珍玩需要写在描述里
                                //法宝的attreffect也在EquipDesc，但是描述在别处
                                var attrDesc = GameDBRuntime.GetEquipAttrDesc(desc.AttrEffectId);
                                if (attrDesc != null)
                                {
                                    descList[mydesc.Key] = attrDesc.ToTr();
                                    ct++;
                                }
                                else
                                    Log($"Can't Find Equip Attr For {mydesc.Key}-{desc.AttrEffectId}");
                            }
                            else if (desc.EquipEffectId > 0)
                            {
                                var talentDesc = GameDBRuntime.GetTalentSDesc(desc.EquipEffectId);
                                descList[mydesc.Key] = talentDesc.ToListMy().ToTr();
                                ct++;
                            }
                        }
                        else if (desc.EquipPosition != EquipPosition.Hulu && desc.EquipPosition != EquipPosition.Weapon)
                        {
                            if (desc.EquipEffectId > 0)
                            {
                                var talentDesc = GameDBRuntime.GetTalentSDesc(desc.EquipEffectId);
                                //if(descList[mydesc.Key].IsTrEmpty())
                                descList[mydesc.Key] = talentDesc.ToListMy().ToTr();
                                //else
                                //    descList[mydesc.Key] += "///" + talentDesc.ToListMy().ToTr();
                                ct++;
                            }
                        }
                    }
                Log($"Generate {ct} Translation For EquipDesc");
            }
            //泡酒物
            {
                int ct = 0;
                var descList = Data.ItemDesc;
                foreach (var mydesc in descList.Copy())
                {
                    var desc = GameDBRuntime.GetConsumeDesc(mydesc.Key);
                    if (desc == null) continue;
                    if (!mydesc.Value.IsTrNull())//有一些自动生成效果并不好，原来有值的统统跳过生成
                        continue;
                    if (desc.ConsumeEffect.Count == 0)
                        continue;
                    //Wine类的回血也是buff，暂不考虑
                    //丹药类的统统把有条件才触发的buff，如根器的吃药加暴击、套装的吃药加棍势也算进去了,忽略要求hastalent的buff
                    if (desc.Type == ConsumeType.WinePartner || desc.Type == ConsumeType.Elixir)
                    {
                        foreach (var consumeEffect in desc.ConsumeEffect)
                            if (consumeEffect.EffectType == ConsumeEffectType.Buff && consumeEffect.EffectId > 0)
                            {
                                FUStBuffDesc? buff = BGW_GameDB.GetOriginalBuffDesc(consumeEffect.EffectId);
                                if (buff is null) continue;
                                if (buff.ID is 21065 or 21165 or 21465 or 21665)//精魂被动吃丹药回血，条件也是always，特殊处理
                                    continue;
                                if (buff.BuffActiveCondition.ConditionType == EGSBuffAndSkillEffectActiveCondition.HasTalent)
                                    continue;
                                //if (descList[mydesc.Key].IsTrNull())
                                //descList[mydesc.Key]=Desc.Empty;
                                descList[mydesc.Key] = buff.ToListMyNullable().ToTr();
                                ct++;
                                //Log($" {mydesc.Key} {buff.ID} {descList[mydesc.Key].GetTr(1)}");
                            }
                    }
                }
                Log($"Generate {ct} Translation For ItemDesc");
            }
            //精魂被动
            {
                int ct = 0;
                var descList = Data.SpiritDesc;
                var generalFormat = descList[-1];
                foreach (var mydesc in descList.Copy())
                    if (mydesc.Key >= 0
                        &&mydesc.Key!=8076)//巡山鬼特殊处理
                    {
                        //假定精魂各等级只有value不一样，格式都一样,假定只有1/4/6级产生变化
                        var desc = GameDBRuntime.GetSoulSkillDesc(mydesc.Key);
                        var desc2 = GameDBRuntime.GetSoulSkillDesc(mydesc.Key + 300);
                        var desc3 = GameDBRuntime.GetSoulSkillDesc(mydesc.Key + 500);
                        if (desc == null || desc2 == null || desc3 == null) continue;
                        var changed = false;
                        //蜻蜓精等是AttrEffectId和EffectTalentId都有的
                        if (desc.AttrEffectId > 0)
                        {
                            var attrDesc = GameDBRuntime.GetEquipAttrDesc(desc.AttrEffectId);
                            var attrDesc2 = GameDBRuntime.GetEquipAttrDesc(desc2.AttrEffectId);
                            var attrDesc3 = GameDBRuntime.GetEquipAttrDesc(desc3.AttrEffectId);
                            if (attrDesc != null && attrDesc2 != null && attrDesc3 != null)
                            {
                                //Data.SpiritDesc[mydesc.Key] = attrDesc.ToTr(attrDesc2,attrDesc3);
                                var tmp = attrDesc.ToTr(attrDesc2, attrDesc3);
                                for (int i = 0; i < tmp.Count; i++)
                                    if (descList[mydesc.Key].Count > i)
                                        descList[mydesc.Key][i] += tmp[i];
                                changed = true;
                            }
                            else
                                Log($"Can't Find Attr For Vigor {mydesc.Key}-{desc.AttrEffectId}");
                        }
                        if (desc.EffectTalentId > 0)
                        {
                            var talentDescList = new List<TalentSDesc> { GameDBRuntime.GetTalentSDesc(desc.EffectTalentId), GameDBRuntime.GetTalentSDesc(desc2.EffectTalentId), GameDBRuntime.GetTalentSDesc(desc3.EffectTalentId) };
                            var talentTr = talentDescList.ToTr();
                            if (!talentTr.IsTrNull())
                                changed = true;
                            descList[mydesc.Key].ConcatWith(talentDescList.ToTr(), ",");
                        }
                        ct += changed ? 1 : 0;
                        for (int i = 0; i < generalFormat.Count && i < descList[mydesc.Key].Count; i++)
                            descList[mydesc.Key][i] = String.Format(generalFormat[i], descList[mydesc.Key][i]);
                    }
                preInited = true;
                Log($"Generate {ct} Translation For Vigor Passive");
            }
            //精魂变身期间buff
            {
                int ct = 0;
                var descList = Data.ItemDesc;
                var generalFormat = descList[-1];
                foreach (var mydesc in descList.Copy())
                    if (mydesc.Key is >= 8000 and < 9000)
                    {
                        var soulSkillDescList = new List<SoulSkillDesc?>
                                { GameDBRuntime.GetSoulSkillDesc(mydesc.Key),
                                    GameDBRuntime.GetSoulSkillDesc(mydesc.Key+300),
                                    GameDBRuntime.GetSoulSkillDesc(mydesc.Key+500) };
                        if (soulSkillDescList.Any(ele=>ele==null)) continue;
                        var buffDescList = soulSkillDescList.Select(ele => ele!.BuffId > 0 ? GameDBRuntime.GetFUStBuffDesc(ele.BuffId) : null).ToList();
                        //目前看来只有一种0/20810/20820,由于1级是0不太好处理,替换成一个自建临时buff
                        if (buffDescList[0] is null&& buffDescList[1] != null && buffDescList[2] != null 
                            && buffDescList[1]!.ID == 20810 && buffDescList[2]!.ID==20820)
                        {
                            var tmpBuff = buffDescList[2].Copy();
                            foreach (var buffEffect in tmpBuff!.BuffEffects)
                                if(buffEffect.EffectParams.Count>1)
                                    buffEffect.EffectParams[1] = 0;//将效果数值设为0;
                            buffDescList[0] = tmpBuff;
                            descList[mydesc.Key] += buffDescList.ToTr().FormattedBy(generalFormat);
                            ct++;
                        }
                        //else//其他情况暂不处理
                    }
                Log($"Generate {ct} Vigor trasformation buff");
            }
            //精魂动作值
            {
                int ct = 0;
                var descList = Data.ItemDesc;
                var generalFormat = descList[-2];
                foreach (var mydesc in descList.Copy())
                    if (mydesc.Key is >= 8000 and < 9000)
                    {
                        //只考虑满级，SpiritActiveSkillId里用的是1级的精魂id，技能id写的是满级的
                        if (!Data.SpiritActiveSkillId.ContainsKey(mydesc.Key)) continue;
                        var tmp = Desc.Empty;
                        foreach(var pair in Data.SpiritActiveSkillId[mydesc.Key])
                        {
                            var skillDesc =BGW_GameDB.GetOriginalSkillEffectDesc(pair.Key);
                            if (skillDesc is null)
                            {
                                Error($"Can't Find Skill Effect {pair.Key}");
                                continue;
                            }
                            var hitCount = pair.Value;
                            tmp.ConcatWith(skillDesc.ToTr(hitCount),Data.ThenConnection);
                        }
//                        Log(tmp.GetTr());
                        descList[mydesc.Key] += tmp.FormattedBy(generalFormat);
                        ct++;
                    }
                Log($"Generate {ct} Vigor action rate");
                /*
                foreach (var mydesc in descList.Copy())
                    if (mydesc.Key is >= 8000 and < 9000)
                    {
                        var itemDesc = GameDBRuntime.GetItemDesc(mydesc.Key);
                        var name=itemDesc.Name.ToFTextRemoveRich();
                        Log($"{name}: {mydesc.Value.GetTr()}");
                    }
                */
            }
            //动作值
            {
                int ct = 2;//lightAttack和HeavyAttack
                foreach(var descList in new List<DescDict> { Data.TalentDisplayDesc,Data.EquipDesc})
                    foreach(var mydesc in descList.Keys)
                        ct+=descList[mydesc].ReplaceActionRate()?1:0;
                Data.LightAttackDesc.ReplaceActionRate();
                Data.HeavyAttackDesc.ReplaceActionRate();
                Log($"Generate {ct} Talent/Equip action rate");
            }

            //补充Desc,因为不同等级id不同,共用描述，在Data里只写1级的id，其它等级在此处补上
            foreach (var desc in Data.EquipDesc.Copy())
                if (desc.Key is >10000 and <13000)//护甲，17000+的是无法升级的单件，是连续排列的
                    for (int i = 10; i <= 40; i += 10)
                        Data.EquipDesc.Add(desc.Key + i, desc.Value);
            foreach (var desc in Data.SpiritDesc.Copy())
                if (desc.Key is >= 8000 and < 9000)//精魂被动描述
                {
                    var levelStrList = desc.Value.Remove2in3Bracket();
                    Data.SpiritDesc[desc.Key] = levelStrList[0];
                    Data.SpiritDesc.Add(desc.Key + 100, levelStrList[0]);
                    Data.SpiritDesc.Add(desc.Key + 200, levelStrList[0]);
                    Data.SpiritDesc.Add(desc.Key + 300, levelStrList[1]);
                    Data.SpiritDesc.Add(desc.Key + 400, levelStrList[1]);
                    Data.SpiritDesc.Add(desc.Key + 500, levelStrList[2]);
                }
            foreach (var desc in Data.ItemDesc.Copy())
                if (desc.Key is >= 8000 and < 9000)//精魂主动描述
                {
                    var levelStrList = desc.Value.Remove2in3Bracket();
                    Data.ItemDesc[desc.Key] = levelStrList[0];
                    Data.ItemDesc.Add(desc.Key + 100, levelStrList[0]);
                    Data.ItemDesc.Add(desc.Key + 200, levelStrList[0]);
                    Data.ItemDesc.Add(desc.Key + 300, levelStrList[1]);
                    Data.ItemDesc.Add(desc.Key + 400, levelStrList[1]);
                    Data.ItemDesc.Add(desc.Key + 500, levelStrList[2]);
                }
            DebugLog("Fill Dict Done");
        }
        public void DeInit()
        {            
            Log($"DeInit");
            harmony.UnpatchAll();
        }
        public static void InitDescProtobufAndLanguage()
        {
            if (inited)
            {
                Log("Skip Dup Init");
                return;
            }
            Log($"Start Init Desc");
            {
                string ingameLng = GSLocalization.GetCurrentCulture().ToLower();
                //steam版是zh-Hans/zh-Hant,wegame版是zh-Hans-CN/?，坑爹
                if (GSLocalization.IsZHCulture())
                    MyExten.CurrentLanguage = Data.LanguageIndex.SimpleCN;
                else if (ingameLng == "en")
                    MyExten.CurrentLanguage = Data.LanguageIndex.English;
                else//default
                    MyExten.CurrentLanguage = Data.LanguageIndex.English;
            }
            Log($"Use Language :{MyExten.CurrentLanguage.ToString()}");
            foreach (var pair in Data.ItemEqDesc)
            {
                var id = pair.Key;
                var desc = GameDBRuntime.GetItemDesc(id);
                //EffectDesc初始值形如ItemDesc.19001.EffectDesc 通过ToFTextFillPre转成本地化字符串，如果ToFTextFillPre找不到翻译会返回原值
                if (desc != null)
                    desc.EffectDesc =
                        $"ItemDesc.{id}.EffectDesc".ToFTextFillPre("SlotItemDetail_Desc") + pair.Value.GetTr();
                //originalItemDesc.EffectDesc.ToFTextFillPre("SlotItemDetail_Desc") + pair.Value.GetTr();
            }
            foreach (var pair in Data.ItemDesc)
            {
                var id = pair.Key;
                var desc = GameDBRuntime.GetItemDesc(id);
                //VIItemDetail.OnItemIdChange
                if (desc != null)
                    desc.EffectDesc =
                        $"ItemDesc.{id}.EffectDesc".ToFTextFillPre("ItemDetail_Desc") + pair.Value.GetTr();
            }
            foreach (var pair in Data.EquipDesc)
            {
                var id = pair.Key;
                var desc = GameDBRuntime.GetEquipDesc(id);
                //VIAccessoryDetail.OnAccessoryIdChanged
                //VIEquipDetail.OnEquipIdChange
                if (desc != null)
                    desc.EquipEffectDesc =
                        $"EquipDesc.{id}.EquipEffectDesc".ToFTextFillPre("Accessory_Desc") + pair.Value.GetTr();
            }
            foreach (var pair in BG_ProtobufDataAPI<FUStSuitDesc>.Get().GetAll())//GetAllList not work
            {
                var suitDesc = pair.Value;
                //VIEquipDetail.OnEquipIdChange
                //DisplayUITextContentHelper_Suit
                for (int i = 0; i < suitDesc.SuitInfo.Count; i++)
                {
                    var id = suitDesc.SuitInfo[i].AttrEffectID;
                    //分别查找attrEffectId\TalentID\SuitEffectID
                    //Log($"Search {suitDesc.ID}.{i}");
                    if (Data.SuitInfoDesc.ContainsKey(id))
                    {
                        var tmp = $"FUStSuitDesc.{suitDesc.ID}.SuitInfo__{i}.SuitEffectDesc".ToFTextRemoveRich().ToString();
                        if (tmp.StartsWith("FUStSuitDesc"))
                            continue;
                        //tmp = suitDesc.SuitInfo[i].SuitEffectDesc;
                        suitDesc.SuitInfo[i].SuitEffectDesc = tmp + Data.SuitInfoDesc[id].GetTr();
                        //Log($"Hit {suitDesc.ID}.{i} ::{Data.SuitInfoDesc[id].GetTr()}");
                    }
                    else
                    {
                        id = suitDesc.SuitInfo[i].SuitEffectID;
                        if (Data.SuitInfoDesc.ContainsKey(id))
                        {
                            var tmp = $"FUStSuitDesc.{suitDesc.ID}.SuitInfo__{i}.SuitEffectDesc".ToFTextRemoveRich().ToString();
                            if (tmp.StartsWith("FUStSuitDesc"))
                                continue;
                            //tmp = suitDesc.SuitInfo[i].SuitEffectDesc;
                            suitDesc.SuitInfo[i].SuitEffectDesc = tmp + Data.SuitInfoDesc[id].GetTr();
                            //Log($"Hit {suitDesc.ID}.{i} ::{Data.SuitInfoDesc[id].GetTr()}");
                        }
                    }
                }
                //RedQualityInfo即升到神珍解锁的效果
                if(suitDesc.RedQualityInfo!=null)
                {
                    var id = suitDesc.RedQualityInfo.AttrEffectID;
                    //分别查找attrEffectId\TalentID\SuitEffectID
                    if (Data.SuitInfoDesc.ContainsKey(id))
                    {
                        suitDesc.RedQualityInfo.RedQualityEffectDesc = $"FUStSuitDesc.{suitDesc.ID}.RedQualityEffectDesc".ToFTextRemoveRich().ToString() + Data.SuitInfoDesc[id].GetTr();
                        //Log($"Hit {suitDesc.ID}.R ::{Data.SuitInfoDesc[id].GetTr()}");
                    }
                    else
                    {
                        id = suitDesc.RedQualityInfo.TalentID;
                        if (Data.SuitInfoDesc.ContainsKey(id))
                        {
                            suitDesc.RedQualityInfo.RedQualityEffectDesc = $"FUStSuitDesc.{suitDesc.ID}.RedQualityInfo.RedQualityEffectDesc".ToFTextRemoveRich().ToString() + Data.SuitInfoDesc[id].GetTr();
                            //Log($"Hit {suitDesc.ID}.R ::{Data.SuitInfoDesc[id].GetTr()}");
                        }
                    }
                }
            }

            foreach (var pair in Data.TalentDisplayDesc)
            {
                var id = pair.Key;
                var pDesc = BGW_GameDB.GetTalentDisplayDesc(id);
                //VISpellDetail.UpadateLevelDesc
                //VILegacyTalentDesc.OnChangeLegacyTalentID
                if (pDesc != null)
                    foreach (var desc in pDesc.DisplayCfg)
                    {
                        //NextDesc是蓝字升级效果，EffectDesc是白字当前效果
                        //desc.Level从1开始，需要-1
                        //desc.NextDesc = BGW_GameDB.GetTalentDisplayDesc($"FUStTalentDisplayDesc.{id}.DisplayCfg__{Math.Max(desc.Level-1, pDesc.DisplayCfg.Count - 1)}.EffectDesc".ToFText(), desc.Level) + pair.Value.GetTr();
                        desc.EffectDesc = BGW_GameDB.GetTalentDisplayDesc($"FUStTalentDisplayDesc.{id}.DisplayCfg__{Math.Max(desc.Level - 1, pDesc.DisplayCfg.Count - 1)}.EffectDesc".ToFText(), desc.Level) + pair.Value.GetTr();
                    }
            }

            foreach (var pair in Data.FabaoAttrDesc)
            {
                var id = pair.Key;
                var desc = GameDBRuntime.GetEquipFaBaoAttrDesc(id);
                if (desc != null && desc.CarryEffectDesc.Count > 0)//似乎都只有0
                    desc.CarryEffectDesc[0] = $"EquipFaBaoAttrDesc.{id}.CarryEffectDesc__0".ToFTextFillPre("SlotItemDetail_Desc") + pair.Value.GetTr();
            }
            foreach (var pair in Data.SpiritDesc)
                if (pair.Key >= 0)
                {
                    var id = pair.Key;
                    var desc = GameDBRuntime.GetSoulSkillDesc(id);
                    if (desc != null)
                    {
                        //Log(GSLocalization.GetLocaliztionalFText($"SoulSkillDesc.{id}.EffectTalentDesc").ToString());
                        //Log($"SoulSkillDesc.{id}.EffectTalentDesc".ToFTextFillPre("SlotItemDetail_Desc").ToString());
                        //desc.EffectTalentDesc = GSLocalization.GetLocaliztionalFText($"SoulSkillDesc.{id}.EffectTalentDesc").ToString()+ pair.Value.GetTr();
                        desc.EffectTalentDesc = $"SoulSkillDesc.{id}.EffectTalentDesc".ToFTextFillPre("SlotItemDetail_Desc").ToString() + pair.Value.GetTr();
                    }
                }
            inited = true;
            DebugLog("initdesc done");
        }
    }
}