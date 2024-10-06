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
using B1UI.GSUI;
using GSE.GSUI;
using b1.UI;
#nullable enable
namespace Test
{
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        public bool enable=false;

        public System.Timers.Timer initDescTimer= new System.Timers.Timer(1000);

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        UTextBlock tmp;
        public void Init()
        {

            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            Utils.RegisterKeyBind(Key.O, delegate {
                var world = MyExten.GetWorld();
                var battlemain=GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.BattleMainCon) as UIBattleMainCon;
                var panel=battlemain.GetFieldOrProperty<UCanvasPanel>("PlayerStCon");
                tmp=UObject.NewObject<UTextBlock>();
                tmp.SetText(FText.FromString("111111"));
                Log($"{tmp.GetFullName()}");
                var panelslot=panel.AddChild(tmp) as UCanvasPanelSlot;
                panelslot.SetPosition(new FVector2D(1000,200));
                //var slot = UnrealEngine.UMG.UWidgetLayoutLibrary.SlotAsCanvasSlot(textblock!);
                /*
                var t = typeof(EBGUAttrFloat);
                //BGUFunctionLibraryCS.BGUSetAttrValue(MyExten.GetControlledPawn(),EBGUAttrFloat.StaminaRecoverBase,0);
                enable = !enable;
                //foreach (var pair in BGW_GameDB.GetAllBuffDesc())
                //    if (pair.Value.BuffActiveCondition.ConditionType == EGSBuffAndSkillEffectActiveCondition.HasTalent)
                //        if (pair.Value.BuffActiveCondition.ConditionParams=="106025")
                    {
                            //Log($"FFFFuck {pair.Value.ID}");
                    }
                //int ct=0;
                //1061301,1061401,1021301,1021401,1086301,1086401,1088301,1088401
                var tmpStr = "";
                foreach(var effect in BGW_GameDB.GetAllSkillEffectDesc())
                {
                    var id = effect.Key;
                    var effectDesc=effect.Value;
                    if(effectDesc.EffectType== EBuffAndSkillEffectType.SkillDamage)
                    {
                        if (effectDesc.EffectParamsStr.Count > 0 && effectDesc.EffectParamsStr[0].Contains("主角"))
                        {
                            tmpStr += $"{id} /";
                            foreach (var str in effectDesc.EffectParamsStr)
                                tmpStr += $"{str}/";
                            if (effectDesc.EffectParamsFloat.Count > 2)
                                tmpStr += $"({effectDesc.EffectParamsFloat[2] / 10000},{effectDesc.EffectParamsFloat[1]})/";
                            if (effectDesc.EffectActiveCondition.ConditionType != EGSBuffAndSkillEffectActiveCondition.Always)
                                tmpStr += $" Condition:{effectDesc.EffectActiveCondition.ConditionType.ToString()}/";
                            if (effectDesc.EffectParamsInt.Count > 5)
                            {
                                var ele = (EAbnormalStateType)effectDesc.EffectParamsInt[5];
                                if(ele!= EAbnormalStateType.None)
                                    tmpStr += $"{ele.ToString()}";                              
                            }
                            tmpStr += "\n";

                        }
                    }
                }
                File.WriteAllText("tmp.txt", tmpStr);
                */
                //Log($"{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.SpecialEnergy)}/{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(),EBGUAttrFloat.SpecialEnergyMax)}");
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
            // harmony.PatchAll();
        }
        public void DeInit() 
        {
            initDescTimer.Dispose();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        //unused
    }
}
