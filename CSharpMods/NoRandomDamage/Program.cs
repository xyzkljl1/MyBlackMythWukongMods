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
        public static float Min = 1.0f;
        public static float Max = 1.0f;

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
                        else if (fieldInfo.FieldType == typeof(float))
                        {
                            if (tmp.Type.ToLower() == "float" && value.IsDouble)
                            {
                                fieldInfo.SetValue(null, (float)(double)value);
                            }
                            else if (tmp.Type.ToLower() == "float" && value.IsInt)
                            {
                                fieldInfo.SetValue(null, (float)(int)value);
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
            if (Min < 0) Min = 0;
            if (Max < Min) Max = Min;
            MyExten.Log($"Load Config.Random Range {Min} ~ {Max}");
        }
    }
    [HarmonyPatch(typeof(BUS_BeAttackedComp), "GetDmgNoiseMultiplier")]
    class Patch
    {
        static public Random rnd = new Random();
        static bool Prefix(ref float __result)
        {
            __result =(float)(MyMod.rnd.NextDouble()*(Config.Max-Config.Min)+Config.Min);
            //MyExten.Log($"Hook {__result:F2} {Config.Max} {Config.Min}");
            return false;
        }
    }
    public class MyMod : CSharpModBase.ICSharpMod
    {
        public string Name => MyExten.Name;
        public virtual string Version => "1.0";
        protected readonly Harmony harmony;
        static public Random rnd = new Random();

        static public void Log(string i) { MyExten.Log(i); }
        static public void Error(string i) { MyExten.Error(i); }
        static public void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod() { harmony = new Harmony(Name); }
        public virtual void Init()
        {
            Config.LoadConfig();
            Log("MyMod::Init.");
            // hook
            harmony.PatchAll();
        }
        public virtual void DeInit()
        {
            Log("MyMod::DeInit.");
            harmony.UnpatchAll();
        }
    }
}
