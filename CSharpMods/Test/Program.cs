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
    }*/
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
            initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate {
                if(enable)
                {
                    Log($"{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.CurEnergy)}" +
                        $"/{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.TransEnergyMax)}");
                }
            });
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
