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
#nullable enable
namespace NoRandomDamage
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
        public static bool Enable=true;
        public static float Min = 1.0f;
        public static float Max = 1.0f;
        public static float ReverseChance = 1.0f;

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
                    }
            }
            catch (Exception ) 
            {
                MyExten.Error("Fail to Parse Config File");
                return;
            }

            MyExten.Log($"Load Config.Random Range {Min}~{Max}");
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        public uint LastBattleInfoID = 0;
        public Delegate? LastRemovedDelegate = null;
        public Random rnd=new Random();

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }

        public System.Timers.Timer initTimer = new System.Timers.Timer(5000);

        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void OnShowDamageNumber(DamageNumParam Param)
        {
            DebugLog("Triggered");
        }
        public void Init()
        {
            Config.LoadConfig();
            Log("MyMod::Init called.Start Timer");
            initTimer.Start();
            initTimer.Elapsed += (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(HookEvent);
            // hook
            // harmony.PatchAll();
        }
        public void DeInit() 
        {
            initTimer.Dispose();
            UnHookEvent();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }

        public void HookEvent()
        {
            if (!Config.Enable) return;
            BUI_BattleInfoCS? bUI_BattleInfoCS = null;
            foreach (var obj in UObject.GetObjects<BUI_BattleInfoCS>())
                if(obj as BUI_BattleInfoCS is not null)
                {
                    bUI_BattleInfoCS=obj as BUI_BattleInfoCS;
                    break;
                }
            if (bUI_BattleInfoCS == null)
                return;
            if (LastBattleInfoID!=bUI_BattleInfoCS.GetUniqueID())
            {
                LastRemovedDelegate = null;
                LastBattleInfoID=bUI_BattleInfoCS.GetUniqueID();
                Log($"Start Hook Battle Info: {LastBattleInfoID}");
                BGW_UIEventCollection bGW_UIEventCollection = BGW_UIEventCollection.Get(bUI_BattleInfoCS);
                //删掉原来的Delegate用我的代替
                bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Combine(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(OnShowDamageNumber));
                foreach (var func in bGW_UIEventCollection.Evt_UI_ShowHPChangeNum.GetInvocationList())
                    if (func.Method.Name == "ShowHPChangeNum")
                    {
                        Log($"Start Replace Delegate {func.Method.Name}");
                        LastRemovedDelegate = func;
                        bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Remove(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, LastRemovedDelegate);
                        break;
                    }
            }
        }
        public void UnHookEvent()
        {
            if (LastBattleInfoID == 0) return;
            foreach (object @object in UObject.GetObjects<BUI_BattleInfoCS>())
            {
                BUI_BattleInfoCS? bUI_BattleInfoCS = @object as BUI_BattleInfoCS;
                if (bUI_BattleInfoCS is null) continue;
                if (bUI_BattleInfoCS.GetUniqueID() != LastBattleInfoID) continue;
                BGW_UIEventCollection bGW_UIEventCollection = BGW_UIEventCollection.Get(bUI_BattleInfoCS);
                Log($"Start Unhook:{LastBattleInfoID}");
                if(LastRemovedDelegate != null)
                {
                    bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Combine(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, LastRemovedDelegate);
                    LastRemovedDelegate = null;
                    Log($"Give Delegate Back:{LastBattleInfoID}");
                }
                bGW_UIEventCollection.Evt_UI_ShowHPChangeNum = (BGW_UIEventCollection.Del_UI_ShowHPChangeNum)Delegate.Remove(bGW_UIEventCollection.Evt_UI_ShowHPChangeNum, new BGW_UIEventCollection.Del_UI_ShowHPChangeNum(OnShowDamageNumber));
            }
        }
    }
}
