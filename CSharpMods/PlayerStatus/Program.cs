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
using GSE.GSUI;
using b1.UI;
using B1UI.GSUI;
using System.Numerics;
using b1.Plugins.GSInput;
#nullable enable
namespace PlayerStatus
{
    public class JsonField
    {
        public string Name="";
        public string DisplayName="";
        public string Type="";
        public string ToolTip="";
        //public List<string> DefaultValue;
        //public List<string> CurrentValue;
    }
    public static class Config
    {
        public static bool Enable = true;
        public static float freq = 1.0f;
        public static FVector2D HPOffset = new FVector2D(0,0);
        public static FVector2D MPOffset = new FVector2D(0, 0);
        public static FVector2D StOffset = new FVector2D(0, 0);
        public static FVector2D VigorOffset = new FVector2D(0, 0);
        public static FVector2D FabaoOffset = new FVector2D(0, 0);
        public static FVector2D TransOffset = new FVector2D(0, 0);
        public static FVector2D FocusOffset = new FVector2D(0, 0);
        public static FVector2D CDOffset = new FVector2D(0, 0);

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
                            if (tmp.Name == "Enable")
                                Config.Enable = true;
                            else
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
                        else if (fieldInfo.FieldType == typeof(float))
                        {
                            if (tmp.Type.ToLower() == "float" && value.IsDouble)
                            {
                                fieldInfo.SetValue(null, (float)(double)value);
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
                        else if (fieldInfo.FieldType == typeof(FVector2D))
                        {
                            if (tmp.Type.ToLower() == "vec2" && value.IsArray)
                            {
                                var x = JsonMapper.ToObject<List<double>>(JsonMapper.ToJson(value));
                                if(x.Count==2)
                                {
                                    fieldInfo.SetValue(null, new FVector2D(x[0], x[1]));
                                }
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}");
                        }
                    }
            }
            catch (Exception ) 
            {
                MyExten.Error("Fail to Parse Config File");
                return;
            }

        }
    }
    public class TimerComp: UActorCompBaseCS
    {
        public float sumDelta = 0.0f;
        static public float targetDelta = 1.0f;
        UTextBlock? HPText = null;
        UTextBlock? MPText = null;
        UTextBlock? StText = null;
        UTextBlock? VigorText = null;
        UTextBlock? FabaoText = null;
        UTextBlock? TransText = null;
        UTextBlock? FocusText = null;
        UTextBlock? TransFocusText = null;
        List<UTextBlock>? CDTexts = null;

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public override void OnAttach()
        {
            SetCanTick(true);
            if (Config.freq < 0.01f) Config.freq = 0.01f;
            if (Config.freq > 100.0f) Config.freq = 100.0f;
            targetDelta = 1 / Config.freq;
            //RecalculateCanTick();
        }
        public override int GetTickGroupMask()
        {
            return 1;
        }
        public void Uninit()
        {
            SetCanTick(false);
            RecalculateCanTick();
            if(HPText!=null)
            {
                HPText.SetVisibility(ESlateVisibility.Hidden);
                MPText.SetVisibility(ESlateVisibility.Hidden);
                StText.SetVisibility(ESlateVisibility.Hidden);
                VigorText.SetVisibility(ESlateVisibility.Hidden);
                FabaoText.SetVisibility(ESlateVisibility.Hidden);
                TransText.SetVisibility(ESlateVisibility.Hidden);
                FocusText.SetVisibility(ESlateVisibility.Hidden);
                foreach(var widget in CDTexts)
                    widget.SetVisibility(ESlateVisibility.Hidden);
                TransFocusText.SetVisibility(ESlateVisibility.Hidden) ;
            }
        }
        public void InitWidgets()
        {
            var world = MyExten.GetWorld();
            var battleMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.BattleMainCon) as UIBattleMainCon;
            if (battleMain == null)
                throw new Exception("Fuck");
            var playerCon = battleMain.GetFieldOrProperty<UCanvasPanel>("PlayerStCon");
            var treasureCon = battleMain.GetFieldOrProperty<UCanvasPanel>("TreasureCon");
            var biTrans = battleMain.GetFieldOrProperty<BI_TransCS>("Trans");
            var ShortcutSpellCon = battleMain.GetFieldOrProperty<UCanvasPanel>("ShortcutSpellCon");
            //var ShortcutSpellCS = battleMain.GetFieldOrProperty<BI_ShortcutSpellCS>("ShortcutSpellCS");
            var SoulSkillCon = battleMain.GetFieldOrProperty<UCanvasPanel>("SoulSkillCon");
            var StickLevel = battleMain.GetFieldOrProperty<BI_StickLevelCS>("StickLevel");
            HPText = UObject.NewObject<UTextBlock>();
            MPText = UObject.NewObject<UTextBlock>();
            StText = UObject.NewObject<UTextBlock>();
            VigorText = UObject.NewObject<UTextBlock>();
            FabaoText = UObject.NewObject<UTextBlock>();
            TransText = UObject.NewObject<UTextBlock>();
            FocusText = UObject.NewObject<UTextBlock>();
            TransFocusText= UObject.NewObject<UTextBlock>();
            CDTexts = new List<UTextBlock> { UObject.NewObject<UTextBlock>(), UObject.NewObject<UTextBlock>(), UObject.NewObject<UTextBlock>() };
            foreach (var textblock in CDTexts)
            {
                var font = textblock.Font;
                font.Size = 16;
                textblock.SetFont(font);
            }
            (playerCon!.AddChild(HPText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(1000 + Config.HPOffset.X, -180 + Config.HPOffset.Y));
            (playerCon!.AddChild(MPText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(1000 + Config.MPOffset.X, -130 + Config.MPOffset.Y));
            (playerCon!.AddChild(StText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(1000 + Config.StOffset.X, -80 + +Config.StOffset.Y));
            (treasureCon!.AddChild(FabaoText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(0+Config.FabaoOffset.X, 0 + Config.FabaoOffset.Y));

            //手柄or键盘
            if(BGW_EnhancedInputMgrV2.GetCurrentInputType() == EGSInputType.Gamepad)
            {
                (ShortcutSpellCon!.AddChild(TransText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-210 + Config.CDOffset.X, -630 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[0]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-440 + Config.CDOffset.X, -630 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[1]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-290 + Config.CDOffset.X, -730 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[2]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-290 + Config.CDOffset.X, -530 + Config.CDOffset.Y));
            }
            else
            {
                (ShortcutSpellCon!.AddChild(TransText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-200 + Config.CDOffset.X, -600 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[0]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-560 + Config.CDOffset.X, -600 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[1]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-420 + Config.CDOffset.X, -600 + Config.CDOffset.Y));
                (ShortcutSpellCon!.AddChild(CDTexts[2]) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-305 + Config.CDOffset.X, -600 + Config.CDOffset.Y));
            }
            (SoulSkillCon!.AddChild(VigorText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(20 + Config.VigorOffset.X, 0 + Config.VigorOffset.Y));
            ((GSUIUtil.FindChildWidget(StickLevel, "RootCon") as UCanvasPanel)!.AddChild(FocusText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-340 + Config.FocusOffset.X, -350 + Config.FocusOffset.Y));
            var styleCon = (GSUIUtil.FindChildWidget(biTrans, "TransStyleCon") as UCanvasPanel);
            if(styleCon != null)
                (styleCon!.AddChild(TransFocusText) as UCanvasPanelSlot)!.SetPosition(new FVector2D(-300 + Config.FocusOffset.X, -300 + Config.FocusOffset.Y));
            else
                Error("Can't find Trans Style Con");
            Log($"Init Widgets");
        }
        public override void OnTickWithGroup(float DeltaTime, int TickGroup)
        {
            //MyExten.Log($"Tick {DeltaTime}");
            sumDelta += DeltaTime;
            if (sumDelta < targetDelta)
                return;
            sumDelta = 0;

            {
                if(!Config.Enable) return;
                var world = MyExten.GetWorld();
                if (world == null) return;

                var battleMain = GSUI.UIMgr.FindUIPage(world, (int)EUIPageID.BattleMainCon) as UIBattleMainCon;
                if(battleMain == null) return;

                if (HPText == null || !HPText.IsValidLowLevel())
                    InitWidgets();
                if (HPText == null || !HPText.IsValidLowLevel()) return;
                try
                {
                    var spellItemList = battleMain.GetFieldOrProperty<BI_ShortcutSpellCS>("ShortcutSpellCS")!.GamepadSpellList;
                    var battleMainData = battleMain.GetFieldOrProperty<DSBattleMain>("BattleMainData");


                    HPText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.Hp):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.HpMax):F1}"));
                    if(battleMainData!.IsTrans.Value)
                    {
                        MPText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.CurEnergy):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.TransEnergyMax):F1}"));
                        StText.SetText(FText.GetEmpty());
                        VigorText.SetText(FText.GetEmpty());
                        FabaoText.SetText(FText.GetEmpty());
                        TransText.SetText(FText.GetEmpty());
                        FocusText.SetText(FText.GetEmpty());
                        TransFocusText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.Pevalue):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.PevalueMax):F1}"));
                        TransFocusText.SetVisibility(ESlateVisibility.Visible);
                        for (int i = 0; i < 3; ++i)
                            CDTexts![i].SetText(FText.GetEmpty());
                    }
                    else
                    {
                        MPText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.Mp):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.MpMax):F1}"));
                        StText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.Stamina):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.StaminaMax):F1}"));
                        VigorText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.VigorEnergy):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.VigorEnergyMax):F1}"));
                        FabaoText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.FabaoEnergy):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.FabaoEnergyMax):F1}"));
                        TransText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.CurEnergy):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.TransEnergyMax):F1}"));
                        FocusText.SetText(FText.FromString($"{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.Pevalue):F1}/{BGUFunctionLibraryCS.GetAttrValue(Owner, EBGUAttrFloat.PevalueMax):F1}"));
                        TransFocusText.SetText(FText.GetEmpty());

                        var SkillInstData = BGU_DataUtil.GetUnPersistentReadOnlyData<BUC_SkillInstsData>(Owner);
                        //BUS_MagicSpellInfoComp.GetSpellState BGUFuncLibSkillCS.GetSkillCDTimePercent
                        for (int i = 0; i < spellItemList.Count && i < 3; ++i)
                        {
                            int? spellId = spellItemList[i].GetFieldOrProperty2<int>("BaseID");
                            if (spellId.HasValue)
                            {
                                var spellDesc = GameDBRuntime.GetSpellDesc(spellId.Value);
                                if (spellDesc is null) continue;
                                FUStSkillSDesc skillDesc = BGW_GameDB.GetSkillSDesc(spellDesc.SkillId, Owner);
                                if (skillDesc is null) continue;
                                float remainingCD = 0, remainingPreCD = 0;
                                SkillInstData.GetSkillCooldownTime(spellDesc.SkillId, out remainingCD, out remainingPreCD);
                                if (skillDesc == null)
                                    CDTexts![i].SetText(FText.FromString(""));
                                if (remainingCD < 0)
                                    CDTexts![i].SetText(FText.FromString($"{skillDesc.CooldownTime:F1}"));
                                else
                                    CDTexts![i].SetText(FText.FromString($"{skillDesc!.CooldownTime - remainingCD:F1}/{skillDesc.CooldownTime:F1}"));
                            }
                        }
                    }
                }
                catch (Exception e) {
                    Log($"{e.Message}");
                }
            }
            //MyExten.Log($"Tick {ct++}");
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        public uint LastCharacterID = 0;
        public TimerComp timerComp = new TimerComp();

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }

        public System.Timers.Timer initTimer = new System.Timers.Timer(3000);

        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void Init()
        {
            Config.LoadConfig();
            Log("MyMod::Init called.Start Timer");
            initTimer.Start();
            initTimer.Elapsed += (Object source, ElapsedEventArgs e) => RegisterComp();
            // hook
            // harmony.PatchAll();
        }
        public void DeInit() 
        {
            initTimer.Dispose();
            timerComp.Uninit();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        public void RegisterComp()
        {
            if (!Config.Enable)
                return;
            var pawn=MyExten.GetBGUPlayerCharacterCS();
            if (pawn == null)
                return;
            var character = MyExten.GetBGUPlayerCharacterCS();
            if (character == null)
                return;
            if(character.GetUniqueID()!= LastCharacterID)
            {
                var compList = character.ActorCompContainerCS.GetFieldOrProperty<List<UActorCompBaseCS>>("CompCSs");
                if (compList is null) return;
                TimerComp? myActorComp = null;
                foreach (var comp in compList)
                    if (comp.GetType() == typeof(TimerComp))
                        myActorComp = (comp as TimerComp)!;
                if(myActorComp == null)
                    Utils.TryRunOnGameThread(() =>
                    {
                        //character.ActorCompContainerCS.AddComp(new MyActorComp());
                        character.ActorCompContainerCS.AddComp(timerComp);
                        Log($"Register Comp To {character.GetUniqueID()}");
                        character.ActorCompContainerCS.RecalculateCanTick();
                    });
                else
                    Log($"Already Registered {character.GetUniqueID()}");
                LastCharacterID =character.GetUniqueID();
            }
        }
    }
}
