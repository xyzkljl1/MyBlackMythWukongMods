using System;
using b1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using ResB1;
using HarmonyLib;
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
using B1UI.GSUI;
using GSE.GSUI;
using b1.UI;
#nullable enable
namespace Test
{
    /*
    [HarmonyPatch(typeof(BGUFunctionLibraryCS), nameof(BGUFunctionLibraryCS.BGUAddBuff))]
    class Patch_Init
    {
        static void Postfix(AActor Caster, AActor Target, int BuffID, EBuffSourceType BuffSourceType, float BuffDurationTimer)
        {
            MyExten.Log($"Add Buff {BuffID}");
        }
    }
    [HarmonyPatch(typeof(BUS_TalentComp), "OnActivateTalent")]
    class Patch_Init2
    {
        static void Postfix(BUS_TalentComp __instance,int TalentID, int ChangeLevel)
        {
            MyExten.Log($"Talent Change {TalentID} {ChangeLevel}");
            int actorResID = __instance.GetActorResID();
            TalentSDesc talentSDescByUnitResIDInMapCache = GameDBRuntime.GetTalentSDescByUnitResIDInMapCache(TalentID, actorResID);
            MyExten.Log($"{talentSDescByUnitResIDInMapCache.Id} {talentSDescByUnitResIDInMapCache.AddBuffIDs} end");
        }
    }

    [HarmonyPatch(typeof(BUS_BeAttackedComp), "IsAttackCrit")]
    class Patch2
    {
        static public void Log(string i) { MyExten.Log(i); }
        static void Postfix(BUS_BeAttackedComp __instance, AActor Attacker, object DamageDescParam, FBattleAttrSnapShot Attacker_AttrMemData)
        {
            var a = Attacker_AttrMemData.Attr_CritRate;
            var b= DamageDescParam.GetFieldOrProperty2<float>("CritRateAddition")!.Value;
            var VictimAttrCon = __instance.GetFieldOrProperty<IBUC_AttrContainer>("VictimAttrCon")!;
            var c = VictimAttrCon.GetFloatValue(EBGUAttrFloat.CritRateDef);
            var d= GSGameplayCVar.CVar_DmgCacl.GetValueInGameThread();
            Log($"On Crit {a} {b} {c} {d}");
        }
    }*/
    [HarmonyPatch(typeof(BUS_BeAttackedComp), "DoDmg_B1_V2")]
    class Patch
    {
        static public void Log(string i) { MyExten.Log(i); }
        static void Postfix(BUS_BeAttackedComp __instance,AActor Attacker, bool IsCrit, float DmgNoiseMul, object DamageDynamicParam, object DamageDescParam,
            FSkillDamageConfig SkillDamageConfig, FBattleAttrSnapShot Attacker_AttrMemData, float FinalDamageValue, float FinalDmgForPart, float FinalElementDmgValue, bool bPrintLog)
        {
            if (!Attacker.GetFullName().Contains("Unit_Player_Wukong_C"))
                return;
            Log("=================================");
            //Log($"Final NormalDamage/PartDamage/ElementDamage: {FinalDamageValue} /{FinalDmgForPart}/{FinalElementDmgValue}");
            Log($"Attacker {Attacker.GetFullName()}");
            //FSkillDamageConfig
            var VictimAttrCon = __instance.GetFieldOrProperty<IBUC_AttrContainer>("VictimAttrCon")!;

            var ActionRate = DamageDescParam.GetFieldOrProperty2<float>("BaseDamageRatio")!.Value;
            var FixDamage = DamageDescParam.GetFieldOrProperty2<float>("BaseDamage")!.Value/100;
            var BaseDamage = Attacker_AttrMemData.Attr_Atk * ActionRate / 10000 + FixDamage;
            //Log($"Base Damage = {BaseDamage} = (FinalAttack {Attacker_AttrMemData.Attr_Atk} * ActionRate {ActionRate / 100}% + FixDamage {FixDamage}) ");
            Log($"AR {ActionRate} FixDamage {FixDamage}) ");

            var def = VictimAttrCon.GetFloatValue(EBGUAttrFloat.Def);
            var defDamageReducion = 1 - 0.48f * def / (90f + 0.52f * Math.Abs(def));
            var DamageBonusAndReduction = Math.Max(0.2f,(1- VictimAttrCon.GetFloatValue(EBGUAttrFloat.DmgDef)/10000)*(1+ Attacker_AttrMemData.Attr_DmgAddition/10000));
            //Log($"Damage Bonus/Reduction Factor = {DamageBonusAndReduction} = Max(0.2,(1 - EnemyDamageReduction {VictimAttrCon.GetFloatValue(EBGUAttrFloat.DmgDef) / 100}%) * (1 + DamageBonus {Attacker_AttrMemData.Attr_DmgAddition/100}%) )");

            var YinYangMulti = (float)__instance.CallPrivateFunc("CalcYinYangDmgMultiplier", new object[] { Attacker, __instance.GetOwner() })!;
            BaseDamage *= DamageBonusAndReduction*YinYangMulti;
            //Log($"Total Damage = {BaseDamage} = BaseDamage * Damage Bonus/Reduction Factor * YinYangMulti {YinYangMulti} ");


            float TrueDamageRatio = 0f;
            FUStUnitBattleInfoExtendDesc unitBattleInfoExtendDesc = BGW_GameDB.GetUnitBattleInfoExtendDesc(BGU_DataUtil.GetFinalBattleInfoExtendID(Attacker));
            if (unitBattleInfoExtendDesc != null)
                TrueDamageRatio = unitBattleInfoExtendDesc.TrueDamageRatio;
            //Log($"True Damage Ratio = {TrueDamageRatio}");

            var ElementDamageLevel = DamageDescParam.GetFieldOrProperty2<int>("ElementDmgLevel")!.Value;
            var ElementDamageRatio = BGW_GameDB.GetElementDmgRatio(ElementDamageLevel) * (1f - TrueDamageRatio);
            //Log($"Element Damage Ratio = {ElementDamageLevel} = FUStElementDmgRatioLevelDesc[ ElementDamageLevel {ElementDamageLevel}] * (1 - TrueDamageRatio)");
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

            //Log($"Defense Factor = {defDamageReducion} = 1-0.48* DEF {def} /(90+0.52*abs(DEF {def})) ");

            Log("=================================");
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        private readonly Harmony harmony;
        public bool enable=false;

        public System.Timers.Timer initDescTimer= new System.Timers.Timer(1000);

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod()
        {
            harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
       
        public void Init()
        {

            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            Utils.RegisterKeyBind(Key.O, delegate {
                {
                    var t = typeof(GameDBRuntime);
                    var methodInfo = t.GetMethod("InitTalentSUnitMap", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(null, new object[] { });
                        Log($"Invoke {methodInfo.Name}");
                    }
                    else
                        Error($"Can't Invoke BuildAllDescToDict");
                }
                {
                    var desc = GameDBRuntime.GetTalentSDesc(106026);
                    MyExten.Log($"{desc.AddBuffIDs}");
                }
                {
                    var desc = GameDBRuntime.GetTalentSDesc(106035);
                    MyExten.Log($"{desc.AddBuffIDs} /{desc.PassiveSkillIDs}/");
                }
                {
                    //var desc = GameDBRuntime.GetFUStBuffDesc(96037);
                    //Log($"Buff {desc.BuffEffects.Count}");
                }
            });

            initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            /*initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate {
                if(enable)
                {
                    Log($"{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.CurEnergy)}" +
                        $"/{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.TransEnergyMax)}");
                }
            });
            */
            // hook
            harmony.PatchAll();
        }
        public void DeInit() 
        {
            initDescTimer.Dispose();
            Log($"DeInit");
            harmony.UnpatchAll();
        }
        //unused
    }
}
