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
#nullable enable
namespace Test
{
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        public bool enable=false;

        //not used
        public System.Timers.Timer initDescTimer= new System.Timers.Timer(1000);

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void OnShowDamageNumber(DamageNumParam param)
        {
            Log("Triggered");

            foreach (object @object in UObject.GetObjects<BUI_BattleInfoCS>())
            {
                BUI_BattleInfoCS? bUI_BattleInfoCS = @object as BUI_BattleInfoCS;
                if (bUI_BattleInfoCS is null) continue;
                bUI_BattleInfoCS.CallPrivateFunc("ShowHPChangeNum",new object[] { param});
            }
            foreach (object @object in UObject.GetObjects<BUI_MSimNum>())
            {
                BUI_MSimNum? numWidget = @object as BUI_MSimNum;
                Log($"Find2 ___ {numWidget == null}");
                if (numWidget != null)
                {
                    var map = numWidget.GetFieldOrProperty<Dictionary<DamageTypeEnum, DamageNumUIInfo>>("DamageNumUIInfoMap");
                    if (map != null)
                    {
                        foreach (var dumageNumUIInfo in map.Values)
                            foreach (var widget in dumageNumUIInfo.Widgets)
                            {
                                UTextBlock uTextBlock = widget as UTextBlock;
                                if (uTextBlock != null)
                                {
                                    Log($"TextBlock");
                                    //uTextBlock.SetText(FText.FromString("菜"));
                                }
                                else
                                    Log($"Other Type {widget.GetFullName()}");

                            }
                        numWidget.CallPrivateFunc("UpdateDamageNum", new object[] { "菜" });
                    }
                    //break;
                }
            }
        }
        public void Init()
        {
            foreach (object @object in UObject.GetObjects<BUI_BattleInfoCS>())
            {
                BUI_BattleInfoCS? bUI_BattleInfoCS = @object as BUI_BattleInfoCS;
                if (bUI_BattleInfoCS is null) continue;
                Log($"Find ___ {bUI_BattleInfoCS == null}");
                BGW_UIEventCollection bGW_UIEventCollection = BGW_UIEventCollection.Get(bUI_BattleInfoCS);
                //bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Combine(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(ShowHPChangeNum));
                bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Combine(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(OnShowDamageNumber));
                Delegate? tmp= null;
                foreach(var func in bGW_UIEventCollection.Evt_UI_ShowHPChangeNum.GetInvocationList())
                {
                    Log($"{func.Method.Name}");
                    if(func.Method.Name== "ShowHPChangeNum")
                        tmp= func;
                }
                if(tmp != null)
                    bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Remove(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, tmp);
            }


            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            Utils.RegisterKeyBind(Key.O, delegate {
            var t = typeof(EBGUAttrFloat);
                BGUFunctionLibraryCS.BGUSetAttrValue(MyExten.GetControlledPawn(),EBGUAttrFloat.StaminaRecoverBase,0);
                //enable = !enable;
                //foreach (var pair in BGW_GameDB.GetAllBuffDesc())
                //    if (pair.Value.BuffActiveCondition.ConditionType == EGSBuffAndSkillEffectActiveCondition.HasTalent)
                //        if (pair.Value.BuffActiveCondition.ConditionParams=="106025")
                    {
                            //Log($"FFFFuck {pair.Value.ID}");
                    }


                foreach (object @object in UObject.GetObjects<BUI_MSimNum>())
                {
                    BUI_MSimNum? numWidget = @object as BUI_MSimNum;
                    Log($"Find2 ___ {numWidget == null}");
                    if(numWidget != null )
                    {
                        var map=numWidget.GetFieldOrProperty<Dictionary<DamageTypeEnum, DamageNumUIInfo>>("DamageNumUIInfoMap");
                        if(map != null )
                        {
                         foreach(var dumageNumUIInfo in map.Values)
                            foreach(var widget in dumageNumUIInfo.Widgets)
                                {
                                    UTextBlock uTextBlock = widget as UTextBlock;
                                    if (uTextBlock != null)
                                    {
                                        Log($"TextBlock");
                                        //uTextBlock.SetText(FText.FromString("菜"));
                                    }
                                    else
                                        Log($"Other Type {widget.GetFullName()}");

                                }
                            numWidget.CallPrivateFunc("UpdateDamageNum", new object[] { "菜" });
                        }
                        //break;
                    }
                }
                //Log($"{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.SpecialEnergy)}/{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(),EBGUAttrFloat.SpecialEnergyMax)}");
            });

            initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate {
                if(enable)
                {
                    Log($"{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.Stamina)}" +
                        $"/{BGUFunctionLibraryCS.BGUGetFloatAttr(MyExten.GetControlledPawn(), EBGUAttrFloat.StaminaMax)}");
                }
            });
            // hook
            // harmony.PatchAll();
        }
        public void DeInit() 
        {
            foreach (object @object in UObject.GetObjects<BUI_BattleInfoCS>())
            {
                BUI_BattleInfoCS? bUI_BattleInfoCS = @object as BUI_BattleInfoCS;
                if (bUI_BattleInfoCS is null) continue;
                Log($"Find ___ {bUI_BattleInfoCS == null}");
                BGW_UIEventCollection bGW_UIEventCollection = BGW_UIEventCollection.Get(bUI_BattleInfoCS);
                //bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Combine(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(ShowHPChangeNum));
                bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Remove(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(OnShowDamageNumber));
            }
            initDescTimer.Dispose();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        //unused
    }
}
