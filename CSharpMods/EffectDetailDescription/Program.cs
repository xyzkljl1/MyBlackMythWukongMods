using System;
using b1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using ResB1;
// using HarmonyLib;
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
//怎么能在不同文件间共用using语句？？？
using CommB1;
using System.Text.RegularExpressions;
namespace EffectDetailDescription
{
    public static class MyExten
    {
        public static string Name => "EffectDetailDescription";
        public static Data.LanguageIndex currentLanguage = Data.LanguageIndex.SimpleCN;
        public static void Log(string msg)
        {
            Console.WriteLine($"[{Name}]: {msg}");
        }
        public static void Error(string msg)
        {
            Console.WriteLine($"[{Name}] Error! : {msg}");
        }

        public static FieldType? GetField<FieldType>(this object obj, String field_name) where FieldType : class
        {
            var t = obj.GetType();
            var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
            if (field is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {field_name}");
                return default(FieldType);
            }
            return field.GetValue(obj) as FieldType;
        }
        public static object? CallPrivateFunc(this object obj, String method_name, object[] paras)
        {
            var t = obj.GetType();
            var methodInfo = t.GetMethod(method_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
                return null;
            }
            //return null;
            return methodInfo.Invoke(obj, paras);
        }
        public static object? CallPrivateGenericFunc(this object obj, String method_name, Type[] para_type4search, Type[] generic_types, object[] paras)
        {
            var t = obj.GetType();
            foreach (var methodInfo in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                if (methodInfo.IsGenericMethod && methodInfo.Name == method_name)
                {
                    var para_types = methodInfo.GetParameters();
                    if (para_types.Length == para_type4search.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < para_type4search.Length; i++)//根据参数类型搜索
                            if (para_types[i].ParameterType.Name != para_type4search[i].Name)
                            {
                                isMatch = false;
                                //Console.WriteLine($"Not Match:{para_types[i].ParameterType.Name}/{para_type4search[i].Name}");
                                break;
                            }
                        if (isMatch)
                        {
                            //Console.WriteLine($"Find: {method_name}");
                            var insMethodInfo = methodInfo.MakeGenericMethod(generic_types);//传入泛型参数获得泛型方法实例
                            if (insMethodInfo is null)
                                Console.WriteLine($"{Name} Fatal Error: Can't get instance of generic: {method_name}");
                            else
                                return insMethodInfo.Invoke(obj, paras);
                        }
                    }

                }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
            return null;
        }
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
            return source;
        }
        public static List<T> ToListMy<T>(this T obj) where T: Google.Protobuf.IMessage
        {
            return new List<T> { obj };
        }
        public class MyMod : ICSharpMod
        {
            public string Name => MyExten.Name;
            public string Version => "1.2";
            // private readonly Harmony harmony;
            //记录上一个bind过的对象的id，防止重复绑定
            public int lastRZDDetailID = -1;

            public System.Timers.Timer bindEventTimer = new System.Timers.Timer(3000);
            public System.Timers.Timer initDescTimer = new System.Timers.Timer(5000);

            public MyMod()
            {
                // harmony = new Harmony(Name);
                // Harmony.DEBUG = true;
            }
            public void Log(string msg)
            {
                MyExten.Log(msg);
            }
            public void Error(string msg)
            {
                MyExten.Error(msg);
            }

            public void DebugLog(string msg)
            {
#if DEBUG
            Console.WriteLine($"[{Name}]: {msg}");
#endif
            }

            public void Init()
            {
                Log("MyMod::Init called.Start Timer");
                //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
                //Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, FindPlayer);

                PreFillDict();//对Data.XXXDict的预处理

                bindEventTimer.Stop();
                initDescTimer.Start();
                //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
                initDescTimer.Elapsed += (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { TryInitDesc(); });
                bindEventTimer.Elapsed += (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { TryBindEvent(); });
                // hook
                // harmony.PatchAll();
            }
            public void PreFillDict()
            {
                //精魂消耗
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
                    Log($"Init Spirit Cost Dict {Data.SpiritCost.Count}");
                }

                //珍玩和法宝
                {
                    int ct = 0;
                    foreach (var descList in new List<DescDict> { Data.EquipDesc, Data.FabaoAttrDesc })
                        foreach (var mydesc in descList.Copy())
                        {
                            var desc = GameDBRuntime.GetEquipDesc(mydesc.Key);
                            if (desc == null) continue;
                            if (desc.EquipPosition != EquipPosition.Accessory && desc.EquipPosition != EquipPosition.Fabao)
                                continue;
                            if (!mydesc.Value.IsTrEmpty())//珍玩里有一些自动生成效果并不好，原来有值的统统跳过生成
                                continue;
                            //只考虑仅一个效果的情况，直接赋值不拼接
                            if (desc.AttrEffectId > 0 && desc.SuitId == 0 && desc.EquipEffectId == 0)
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
                    Log($"Generate {ct} Translation For EquipDesc");
                }
                //精魂
                {
                    int ct = 0;
                    var descList = Data.SpiritDesc;
                    var generalFormat = descList[-1];
                    foreach (var mydesc in descList.Copy())
                        if (mydesc.Key >= 0)
                        {
                            //假定精魂各等级只有value不一样，格式都一样,假定只有1/4/6级产生变化
                            var desc = GameDBRuntime.GetSoulSkillDesc(mydesc.Key);
                            var desc2 = GameDBRuntime.GetSoulSkillDesc(mydesc.Key + 300);
                            var desc3 = GameDBRuntime.GetSoulSkillDesc(mydesc.Key + 500);
                            if (desc == null || desc2 == null || desc3 == null) continue;
                            bool changed = false;
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
                                if (!talentTr.IsTrEmpty())
                                    changed=true;
                                descList[mydesc.Key].ConcatWith(talentDescList.ToTr(), ",");
                            }
                            ct += changed ? 1 : 0;
                            for (int i = 0; i < generalFormat.Count && i < descList[mydesc.Key].Count; i++)
                                descList[mydesc.Key][i] = String.Format(generalFormat[i], descList[mydesc.Key][i]);
                        }
                    Log($"Generate {ct} Translation For Vigor Passive");
                }


                //补充Desc,因为不同等级id不同,共用描述，在Data里只写1级的id，其它等级在此处补上
                foreach (var desc in Data.EquipDesc.Copy())
                    if (desc.Key >= 10000 && desc.Key < 13000)//护甲，17000+的是无法升级的单件，是连续排列的
                        for (int i = 10; i <= 40; i += 10)
                            Data.EquipDesc.Add(desc.Key + i, desc.Value);
                foreach (var desc in Data.SpiritDesc.Copy())
                    if (desc.Key >= 8000 && desc.Key < 9000)//精魂
                    {
                        var base_str = desc.Value;
                        var levelStrList = desc.Value.Remove2in3Bracket();
                        Data.SpiritDesc[desc.Key] = levelStrList[0];
                        Data.SpiritDesc.Add(desc.Key + 100, levelStrList[0]);
                        Data.SpiritDesc.Add(desc.Key + 200, levelStrList[0]);
                        Data.SpiritDesc.Add(desc.Key + 300, levelStrList[1]);
                        Data.SpiritDesc.Add(desc.Key + 400, levelStrList[1]);
                        Data.SpiritDesc.Add(desc.Key + 500, levelStrList[2]);
                    }
                DebugLog("Fill Dict Done");
            }
            public void DeInit()
            {
                initDescTimer.Dispose();
                bindEventTimer.Dispose();//注意Dispose
                Log($"DeInit");
                // harmony.UnpatchAll();
            }
            private void TryInitDesc()
            {
                //似乎直接执行也可以，但为了保险起见等进入游戏再执行
                if (MyUtils.GetPlayerController() is null) return;
                var pawn = MyUtils.GetControlledPawn();
                if (pawn is null) return;
                //Log($"{pawn.GetFullName()}");
                initDescTimer.Stop();
                InitDescProtobufAndLanugage();
                bindEventTimer.Start();
            }
            private void InitDescProtobufAndLanugage()
            {
                Log($"Start Init Desc");
                {
                    string ingameLng = GSLocalization.GetCurrentCulture();
                    if (ingameLng == "zh-Hans" || ingameLng == "zh-Hant")
                        MyExten.currentLanguage = Data.LanguageIndex.SimpleCN;
                    else if (ingameLng == "en")
                        MyExten.currentLanguage = Data.LanguageIndex.English;
                    else//default
                        MyExten.currentLanguage = Data.LanguageIndex.English;
                }
                Log($"Use Language :{MyExten.currentLanguage.ToString()}");
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
                DebugLog("initdesc done");
            }
            private void TryBindEvent()
            {
                var world = MyUtils.GetWorld();
                if (world is null) return;
                var equipMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.EquipMain);
                if (equipMain is null) return;
                var equipMainType = equipMain.GetType();
                var rzdDetail = equipMain.GetField<VIRZDDetail>("RZDDetail");
                if (rzdDetail is null) return;

                //每次销毁窗口后都需要重新bind
                if (lastRZDDetailID != rzdDetail.GetID())
                {
                    var dataStore = rzdDetail.GetField<DSRZDDetail>("DataStore");
                    if (dataStore is null) return;
                    Log($"Bind RZDDetail:{lastRZDDetailID}->{rzdDetail.GetID()}");
                    lastRZDDetailID = rzdDetail.GetID();
                    rzdDetail.CallPrivateGenericFunc("BindValueToCustom", new Type[] { typeof(GSUIBiProp<int>), typeof(Action<ChangeReason, int, int>) }, new Type[] { typeof(int) },
                                new object[] { dataStore.ItemId, (ChangeReason Reason, int OldValue, int NewValue) =>
                                                                {
                                                                    if(!Data.SpiritCost.ContainsKey(NewValue)) return;
                                                                    SoulSkillDesc soulSkillDesc = GameDBRuntime.GetSoulSkillDesc(NewValue);
                                                                    if(soulSkillDesc is null) return;
                                                                    var dataStore=rzdDetail.GetField<DSRZDDetail>("DataStore");
                                                                    if(dataStore is null) return;
                                                                    //Console.WriteLine($"!!!Set {NewValue} ");
                                                                    var text=GSB1UIUtil.GetUIWordDescFText((EUIWordID)(1437 + dataStore.GetEnergyLevel(soulSkillDesc.CastEnergy))).ToString()
                                                                                +$"{Data.SpiritCost[NewValue].GetTr()}";
                                                                    rzdDetail.GetField<UTextBlock>("TxtCost")!.SetText(FText.FromString(text));
                                                                }
                                });

                }
            }
        }
    }
}