using System;
using b1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using ResB1;
// using HarmonyLib;
using GSE.GSSdk;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using System.Timers;
using BtlB1;
using System.Reflection.Emit;
using System.IO;
using ILRuntime.Runtime;
using LitJson;
using Mono.Unix;
using b1.Protobuf.DataAPI;
using System.ComponentModel;
using UnrealEngine.Runtime;
using Google.Protobuf;
using OssB1;
using ArchiveB1;
using b1.UI.Comm;
using UnrealEngine.Engine;
using static System.Net.Mime.MediaTypeNames;
using UnrealEngine.UMG;
using static b1.BGW_UIEventCollection;
using System.Text.Json.Serialization;
using ILRuntime.CLR.Utils;
using b1.BGW;
using HarmonyLib;
using Diana.Common;
using B1UI.GSUI;
using GSE.GSUI;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
#nullable enable
namespace BattleLog
{

    public class JsonField
    {
        public string Name="";
        public string DisplayName="";
        public string Type="";
        public string ToolTip="";
    }
    public static class Config
    {
        public static bool LogOnScreen=true;
        public static bool LogOnFile = true;
        public static bool LogOnConsole = true;
        public static bool LogEverything = false;

        public static void LoadConfig()
        {
            var filepath = $"CSharpLoader\\Mods\\{MyExten.Name}\\config.json";
            if (!File.Exists(filepath)) 
            {
                MyExten.Error("No Config File");
                return;
            }
            try
            {
                var jsonDoc = JsonMapper.ToObject(File.ReadAllText(filepath));
                var type=typeof(Config);
                foreach (JsonData jsonFieldObject in jsonDoc["Config"])
                    if(jsonFieldObject.IsObject)
                    {
                        var tmp=JsonMapper.ToObject<JsonField>(JsonMapper.ToJson(jsonFieldObject));
                        var fieldInfo=type.GetField(tmp.Name,BindingFlags.Public | BindingFlags.Static);
                        if(fieldInfo==null)
                        {
                            MyExten.Error($"Can't Set Field {tmp.Name}");
                            continue;
                        }
                        JsonData? value=null;
                        if (jsonFieldObject.Keys.Contains("CurrentValue") && jsonFieldObject["CurrentValue"] != null)
                            value = jsonFieldObject["CurrentValue"];
                        else if(jsonFieldObject.Keys.Contains("DefaultValue"))
                            value = jsonFieldObject["DefaultValue"];
                        if (value == null)
                        {
                            MyExten.Error($"No Value For {tmp.Name}");
                            continue;
                        }
                        if (fieldInfo.FieldType == typeof(List<string>))
                        {
                            if (tmp.Type.ToLower() == "list<string>" && value.IsArray)
                            {
                                var x = JsonMapper.ToObject<List<string>>(JsonMapper.ToJson(value));
                                fieldInfo.SetValue(null, x);
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}");
                        }
                        else if (fieldInfo.FieldType == typeof(int))
                        {
                            if (tmp.Type.ToLower() == "int" && value.IsInt)
                            {
                                fieldInfo.SetValue(null, (int)value);
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}");
                        }
                        else if (fieldInfo.FieldType == typeof(bool))
                        {
                            if (tmp.Type.ToLower() == "bool" && value.IsBoolean)
                            {
                                fieldInfo.SetValue(null, (bool)value);
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}");
                        }
                    }
                MyExten.Log($"Config loaded {Config.LogOnScreen} {Config.LogOnFile} {Config.LogEverything}");
            }
            catch (Exception ) 
            {
                MyExten.Error("Fail to Parse Config File");
                return;
            }
        }
    }
    [HarmonyPatch(typeof(BGUFunctionLibraryCS), nameof(BGUFunctionLibraryCS.LogBattleInfo))]
    class PatchBattleLog
    {
        static void Postfix(AActor ContextActor, EBattleInfoType BattleInfoType, string BattleInfoLog, int BattleInfoLogOptions, EBGULogVerbosity BGULogVerbosity)
        {
            if(Config.LogEverything||BattleInfoType== EBattleInfoType.DamageCalc)
            //if(BattleInfoType== EBattleInfoType.AddBuff)
            {
                //MyExten.Log($"{BattleInfoType.ToString()}");
                //MyExten.Log($"{BattleInfoLog}");
                if (Config.LogOnScreen)
                {
                    var widget = MyMod.GetLogWidget();
                    if (widget != null)
                    {
                        //var log=Regex.Replace(BattleInfoLog, $"</><.*?>(.*?)</>", "$1");
                        var log = Regex.Replace(BattleInfoLog, $"<.*?>", "");
                        log = $"{BattleInfoType.ToString()}\n{log}\n\n";
                        //widget.SetText(FText.FromString(widget.GetText() + log));

                        var text = widget.GetText()+log;
                        while (text.Count(x => x == '\n') > 60)
                        {
                            for (int i = 0; i < 10; i++)
                                text = text.Substring(text.IndexOf('\n') + 1);
                        }
                        widget.SetText(FText.FromString(text));
                    }
                }
                if (Config.LogOnFile)
                {
                    var log = Regex.Replace(BattleInfoLog, $"<.*?>", "");
                    File.AppendAllText(MyMod.LogFilePath, $"{BattleInfoType.ToString()}\n{log}\n\n");
                }
                if (Config.LogOnConsole)
                {
                    var log = Regex.Replace(BattleInfoLog, $"<.*?>", "");
                    MyExten.Log($"{BattleInfoType.ToString()}\n{log}\n\n");
                }
            }
        }

    }
    /*
    [HarmonyPatch(typeof(BUS_BeAttackedComp), "DoDmg_B1_V2")]
    class Patch
    {
        static public void Log(string i) { MyExten.Log(i); }
        static void Postfix(BUS_BeAttackedComp __instance,AActor Attacker, bool IsCrit, float DmgNoiseMul, object DamageDynamicParam, object DamageDescParam,
            FSkillDamageConfig SkillDamageConfig, FBattleAttrSnapShot Attacker_AttrMemData, float FinalDamageValue, float FinalDmgForPart, float FinalElementDmgValue, bool bPrintLog)
        {
            Log("=================================");
            Log($"Final NormalDamage/PartDamage/ElementDamage: {FinalDamageValue} /{FinalDmgForPart}/{FinalElementDmgValue}");
            Log($"Attacker {Attacker.GetFullName()}");

            var VictimAttrCon = __instance.GetFieldOrProperty<IBUC_AttrContainer>("VictimAttrCon")!;

            var ActionRate = DamageDescParam.GetFieldOrProperty2<float>("BaseDamageRatio")!.Value;
            var FixDamage = DamageDescParam.GetFieldOrProperty2<float>("BaseDamage")!.Value/100;
            var BaseDamage = Attacker_AttrMemData.Attr_Atk * ActionRate / 10000 + FixDamage;
            Log($"Base Damage = {BaseDamage} = (FinalAttack {Attacker_AttrMemData.Attr_Atk} * ActionRate {ActionRate / 100}% + FixDamage {FixDamage}) ");

            var def = VictimAttrCon.GetFloatValue(EBGUAttrFloat.Def);
            var defDamageReducion = 1 - 0.48f * def / (90f + 0.52f * Math.Abs(def));
            var DamageBonusAndReduction = Math.Max(0.2f,(1- VictimAttrCon.GetFloatValue(EBGUAttrFloat.DmgDef)/10000)*(1+ Attacker_AttrMemData.Attr_DmgAddition/10000));
            Log($"Damage Bonus/Reduction Factor = {DamageBonusAndReduction} = Max(0.2,(1 - EnemyDamageReduction {VictimAttrCon.GetFloatValue(EBGUAttrFloat.DmgDef) / 100}%) * (1 + DamageBonus {Attacker_AttrMemData.Attr_DmgAddition/100}%) )");

            var YinYangMulti = (float)__instance.CallPrivateFunc("CalcYinYangDmgMultiplier", new object[] { Attacker, __instance.GetOwner() })!;
            BaseDamage *= DamageBonusAndReduction*YinYangMulti;
            Log($"Total Damage = {BaseDamage} = BaseDamage * Damage Bonus/Reduction Factor * YinYangMulti {YinYangMulti} ");


            float TrueDamageRatio = 0f;
            FUStUnitBattleInfoExtendDesc unitBattleInfoExtendDesc = BGW_GameDB.GetUnitBattleInfoExtendDesc(BGU_DataUtil.GetFinalBattleInfoExtendID(Attacker));
            if (unitBattleInfoExtendDesc != null)
                TrueDamageRatio = unitBattleInfoExtendDesc.TrueDamageRatio;
            Log($"True Damage Ratio = {TrueDamageRatio}");

            var ElementDamageLevel = DamageDescParam.GetFieldOrProperty2<int>("ElementDmgLevel")!.Value;
            var ElementDamageRatio = BGW_GameDB.GetElementDmgRatio(ElementDamageLevel) * (1f - TrueDamageRatio);
            Log($"Element Damage Ratio = {ElementDamageLevel} = FUStElementDmgRatioLevelDesc[ ElementDamageLevel {ElementDamageLevel}] * (1 - TrueDamageRatio)");
            if(false)
            {
                var EleType = (EAbnormalStateType)DamageDescParam.GetFieldOrProperty2<int>("ElemAtkType")!.Value;
                if(EleType!=EAbnormalStateType.None)
                {
                    float EleAtk = .0f;
                    float EleDef = .0f;
                    switch (EleType)
                    {
                        case EAbnormalStateType.Abnormal_Freeze:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.FreezeDef);
                            EleAtk = Attacker_AttrMemData.Attr_FreezeAtk;
                            break;
                        case EAbnormalStateType.Abnormal_Burn:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.BurnDef);
                            EleAtk = Attacker_AttrMemData.Attr_BurnAtk;
                            break;
                        case EAbnormalStateType.Abnormal_Poison:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.PoisonDef);
                            EleAtk = Attacker_AttrMemData.Attr_PoisonAtk;
                            break;
                        case EAbnormalStateType.Abnormal_Thunder:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.ThunderDef);
                            EleAtk = Attacker_AttrMemData.Attr_ThunderAtk;
                            break;
                        case EAbnormalStateType.Abnormal_Yin:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.YinDef);
                            break;
                        case EAbnormalStateType.Abnormal_Yang:
                            EleDef = VictimAttrCon.GetFloatValue(EBGUAttrFloat.YangDef);
                            break;
                    }
                    if (Attacker_AttrMemData.Attr_IgnoreTargetElemDef)
                        EleDef = .0f;
                    float FinalEleDef = EleDef-EleAtk;
                    Log($"ElementResist = {FinalEleDef} = EnemyElementResist {EleDef} - ElementAtk {EleAtk}");
                }
            }
            DebugConfig.IsOpenBattleInfoTool = true;

            Log($"Defense Factor = {defDamageReducion} = 1-0.48* DEF {def} /(90+0.52*abs(DEF {def})) ");

            Log("=================================");
        }
    }*/
    public class MyMod : CSharpModBase.ICSharpMod
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);

        public string Name => MyExten.Name;
        public virtual string Version => "1.0";
        protected readonly Harmony harmony;
        static public Random rnd = new Random();
        static public UTextBlock? LogTextblock=null;
        public static string LogFilePath = $"CSharpLoader\\Mods\\{MyExten.Name}\\BattleLog.txt";

        static public void Log(string i) { MyExten.Log(i); }
        static public void Error(string i) { MyExten.Error(i); }
        static public void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod() { harmony = new Harmony(Name); }
        public void Del_AddBattleInfoLog(EBattleInfoType BattleInfoType, string BattleInfoLog, int BattleInfoLogOptions, EBGULogVerbosity BGULogVerbosity)
        {
            Console.WriteLine(BattleInfoLog);
        }
        public static UTextBlock GetLogWidget()
        {
            if(LogTextblock!=null&& LogTextblock.IsValidLowLevel()) return LogTextblock;
            var world = MyExten.GetWorld();
            var battleMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.BattleMainCon) as UIBattleMainCon;
            if (battleMain == null || world == null)
                return null;
            var MainCon = battleMain.GetFieldOrProperty<UCanvasPanel>("MainCon");
            LogTextblock = UObject.NewObject<UTextBlock>();
            {
                var font=LogTextblock.Font;
                font.Size = 18;
                LogTextblock.SetFont(font);
            }
            (MainCon!.AddChild(LogTextblock) as UCanvasPanelSlot)!.SetPosition(new FVector2D(200 , 100 ));
            return LogTextblock;
        }
        public virtual void Init()
        {
            //var eventCollection=MyExten.GetBUS_GSEventCollection();
            //eventCollection.Evt_AddBattleInfoLog += Del_AddBattleInfoLog;
            Config.LoadConfig();
            if (Config.LogOnConsole)
            {
                SetConsoleCP(65001);
                SetConsoleOutputCP(65001);
                Log("将控制台代码页设置为UTF8!! 开启控制台中文输出");
            }
            DebugConfig.IsOpenBattleInfoTool = true;
            File.WriteAllText(LogFilePath, "");
            Log("MyMod::Init.");
            // hook
            harmony.PatchAll();
        }
        public virtual void DeInit()
        {
            if (LogTextblock != null && LogTextblock.IsValidLowLevel())
                LogTextblock.SetVisibility(ESlateVisibility.Hidden);
            //var eventCollection = MyExten.GetBUS_GSEventCollection();
            //eventCollection.Evt_AddBattleInfoLog -= Del_AddBattleInfoLog;
            Log("MyMod::DeInit.");
            harmony.UnpatchAll();
        }
    }
}
