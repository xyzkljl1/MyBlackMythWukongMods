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
#nullable enable
namespace EffectDetailDescription
{
    public static class MyExten
    {
        public static string Name => "EffectDetailDescription";
        public static Data.LanguageIndex currentLanguage = Data.LanguageIndex.SimpleCN;
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
            return methodInfo.Invoke(obj,paras);
        }
        public static object? CallPrivateGenericFunc(this object obj, String method_name, Type[] para_type4search, Type[] generic_types, object[] paras)
        {
            var t = obj.GetType();
            foreach (var methodInfo in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                if(methodInfo.IsGenericMethod&&methodInfo.Name==method_name)
                {
                    var para_types = methodInfo.GetParameters();
                    if (para_types.Length==para_type4search.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < para_type4search.Length; i++)//根据参数类型搜索
                            if (para_types[i].ParameterType.Name!=para_type4search[i].Name)
                            { 
                                isMatch = false;
                                //Console.WriteLine($"Not Match:{para_types[i].ParameterType.Name}/{para_type4search[i].Name}");
                                break; 
                            }
                        if(isMatch)
                        {
                            //Console.WriteLine($"Find: {method_name}");
                            var insMethodInfo = methodInfo.MakeGenericMethod(generic_types);//传入泛型参数获得泛型方法实例
                            if(insMethodInfo is null)
                                Console.WriteLine($"{Name} Fatal Error: Can't get instance of generic: {method_name}");
                            else
                                return insMethodInfo.Invoke(obj, paras);
                        }
                    }

                }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
            return null;
        }
        public static string ToANSI(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Encoding.GetEncoding(936).GetString(bytes);
        }
        public static string GetTr(this List<String> descList)
        {
            if (descList.Count == 0) return "";
            if (descList.Count > (int)currentLanguage) return $"({descList[(int)currentLanguage]})";
            if (descList.Count > (int)Data.LanguageIndex.English) return $"({descList[(int)Data.LanguageIndex.English]})";
            return $"({descList.Last()})";
        }
        //形如[a]/[b]/[c]的数字,返回3个字符串，返回只留第i个[]的结果
        public static string Remove2in3Bracket(this string str,int i)
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
                ret=ret.Remove(pos,1);
                ret=ret.Remove(ret.IndexOf(']', start_index),1);//假定[]一定成对出现
                start_index = pos;
            }
            return ret;
        }
        public static List<List<string>> Remove2in3Bracket(this List<string> strList)
        {
            var ret = new List<List<string>>();
            for (int i = 0; i < 3; ++i)
            {
                var tmp=new List<string>();
                foreach (var str in strList)
                    tmp.Add(str.Remove2in3Bracket(i));
                ret.Add(tmp);
            }
            return ret;
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        //记录上一个bind过的对象的id，防止重复绑定
        public int lastRZDDetailID = -1;

        public System.Timers.Timer bindEventTimer= new System.Timers.Timer(3000);
        public System.Timers.Timer initDescTimer= new System.Timers.Timer(5000);

        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void Log(string msg)
        {
            Console.WriteLine($"[{Name}]: {msg}");
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
            Utils.RegisterKeyBind(Key.O, delegate {
                GameDBRuntime.GetItemDesc(2218).CarryMax = 100;
                var world = MyUtils.GetWorld();
                if (world is null) return;
                var equipMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.EquipMain);
                var bagMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.BagMain);
                if (equipMain is null) return;
                if(bagMain is null) return;
                var detail = bagMain.GetField<GSUIView>("ItemDetail");
                if (detail is null) return;
                var tb = detail.GetField<URichTextBlock>("TxtEffectDesc");
                if (tb is null) return;
                //Console.WriteLine($"detail.Text:{tb.GetText()}");
                //tb.SetText(FText.FromString("ddddd"));
            });

            PreFillDict();//对Data.XXXDict的预处理

            bindEventTimer.Stop();
            initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { TryInitDesc(); });
            bindEventTimer.Elapsed += (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { TryBindEvent(); });
            // hook
            // harmony.PatchAll();
        }
        public void PreFillDict()
        {
            //精魂消耗
            {
                var cost = new List<int>{ 0, 0, 0, 0, 0, 0 };
                var costDict=new Dictionary<int, float>();
                foreach (var desc in GSProtobufRuntimeAPI<TBSoulSkillDesc, SoulSkillDesc>.Get().GetAll().List)
                    costDict.Add(desc.Id, desc.CastEnergy);
                foreach(var pair in costDict)
                {
                    int id=pair.Key;
                    int level = (id / 100) % 10;//形如8011->8111->8211...取百位
                    if (level != 0) continue;
                    float cost1 = costDict.ContainsKey(id) ? costDict[id] : 0;
                    float cost2 = costDict.ContainsKey(id+100) ? costDict[id+100] : 0;
                    float cost3 = costDict.ContainsKey(id+200) ? costDict[id+200] : 0;
                    float cost4 = costDict.ContainsKey(id+400) ? costDict[id+400] : 0;
                    Data.SpiritCost.Add(id, new List<string> { $"[{cost1:F1}]/{cost2:F1}/{cost3:F1}/{cost4:F1}" });
                    Data.SpiritCost.Add(id+100, new List<string> { $"{cost1:F1}/[{cost2:F1}]/{cost3:F1}/{cost4:F1}" });
                    Data.SpiritCost.Add(id+200, new List<string> { $"{cost1:F1}/{cost2:F1}/[{cost3:F1}]/{cost4:F1}" });
                    Data.SpiritCost.Add(id+300, new List<string> { $"{cost1:F1}/{cost2:F1}/[{cost3:F1}]/{cost4:F1}" });
                    Data.SpiritCost.Add(id+400, new List<string> { $"{cost1:F1}/{cost2:F1}/{cost3:F1}/[{cost4:F1}]" });
                    Data.SpiritCost.Add(id+500, new List<string> { $"{cost1:F1}/{cost2:F1}/{cost3:F1}/[{cost4:F1}]" });
                }
                Log($"Init Spirit Cost Dict {Data.SpiritCost.Count}");
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
            foreach(var pair in BG_ProtobufDataAPI<FUStSuitDesc>.Get().GetAll())//GetAllList not work
            {
                var suitDesc=pair.Value;
                //VIEquipDetail.OnEquipIdChange
                //DisplayUITextContentHelper_Suit
                for (int i = 0;i< suitDesc.SuitInfo.Count;i++)
                {
                    var id = suitDesc.SuitInfo[i].AttrEffectID;
                    //分别查找attrEffectId\TalentID\SuitEffectID
                    //Log($"Search {suitDesc.ID}.{i}");
                    if (Data.SuitInfoDesc.ContainsKey(id))
                    {
                        suitDesc.SuitInfo[i].SuitEffectDesc = $"FUStSuitDesc.{suitDesc.ID}.SuitInfo__{i}.SuitEffectDesc".ToFTextRemoveRich().ToString() + Data.SuitInfoDesc[id].GetTr();
                        //Log($"Hit {suitDesc.ID}.{i} ::{Data.SuitInfoDesc[id].GetTr()}");

                    }
                    else
                    {
                        id=suitDesc.SuitInfo[i].SuitEffectID;
                        if (Data.SuitInfoDesc.ContainsKey(id))
                        {
                            suitDesc.SuitInfo[i].SuitEffectDesc = $"FUStSuitDesc.{suitDesc.ID}.SuitInfo__{i}.SuitEffectDesc".ToFTextRemoveRich().ToString() + Data.SuitInfoDesc[id].GetTr();
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
                        desc.EffectDesc = BGW_GameDB.GetTalentDisplayDesc($"FUStTalentDisplayDesc.{id}.DisplayCfg__{Math.Max(desc.Level-1, pDesc.DisplayCfg.Count - 1)}.EffectDesc".ToFText(), desc.Level) + pair.Value.GetTr();                        
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
            if(equipMain is null) return;
            var equipMainType = equipMain.GetType();
            var rzdDetail=equipMain.GetField<VIRZDDetail>("RZDDetail");
            if(rzdDetail is null) return;

            //每次销毁窗口后都需要重新bind
            if(lastRZDDetailID!=rzdDetail.GetID())
            {
                var dataStore = rzdDetail.GetField<DSRZDDetail>("DataStore");
                if (dataStore is null) return;
                Log($"Bind RZDDetail:{lastRZDDetailID}->{rzdDetail.GetID()}");
                lastRZDDetailID = rzdDetail.GetID();
                rzdDetail.CallPrivateGenericFunc("BindValueToCustom", new Type[] { typeof(GSUIBiProp<int>),typeof(Action<ChangeReason,int,int>) },new Type[] { typeof(int)},
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
