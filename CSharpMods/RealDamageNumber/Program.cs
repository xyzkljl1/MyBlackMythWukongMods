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
using System.Text.Json.Serialization;
using ILRuntime.CLR.Utils;
using b1.BGW;
using Novell.Directory.Ldap.Utilclass;
#nullable enable
namespace RealDamageNumber
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
        public static List<string> DamageNumbers=new List<string>();
        public static List<string> BigDamageNumbers = new List<string>();
        public static List<string> EnemyDamageNumbers = new List<string>();
        public static int BigDamageCap;
        public static bool Enable=true;

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
                            {
                                MyExten.Error($"No Value For {tmp.Name}");
                            }
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
                    }
            }
            catch (Exception e) 
            {
                MyExten.Error($"Fail to Parse Config File{e.Message}");
                return;
            }

            MyExten.Log($"Load Config Done :{DamageNumbers.Count} {BigDamageNumbers.Count} {EnemyDamageNumbers.Count}");
        }
    }
    [HarmonyPatch(typeof(BUI_MSimNum), nameof(BUI_MSimNum.SetDamageNumParam))]
    class Patch
    {
        static public Random rnd = new Random();
        static void Postfix(BUI_MSimNum __instance,DamageNumShowParam ShowParam, DamageNumParam Param, BGWDataAsset_DamageNumConfig DamageNumConfig)
        {
            string text = "";
            if (Param.AttackerTeamType == EDmgNumUITeamType.Enemy && Param.DamageNum != 0)
            {
                //注意DamagenNum是负数
                if ((-Param.DamageNum) > Config.BigDamageCap && Config.BigDamageNumbers.Count > 0)
                    text = Config.BigDamageNumbers[rnd.Next(Config.BigDamageNumbers.Count)];
                else if (Config.DamageNumbers.Count > 0)
                    text = Config.DamageNumbers[rnd.Next(Config.DamageNumbers.Count)];
            }
            else if (Param.AttackerTeamType == EDmgNumUITeamType.Hero && Config.EnemyDamageNumbers.Count > 0 && Param.DamageNum != 0)
                text = Config.EnemyDamageNumbers[rnd.Next(Config.EnemyDamageNumbers.Count)];
            if (text != "" && Config.Enable)
                __instance.CallPrivateFunc("UpdateDamageNum", new object[] { text });
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.1";
        private readonly Harmony harmony;

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }

        public MyMod()
        {
            harmony = new Harmony(Name);
            //harmony.DEBUG = true;
        }
        public void Init()
        {
            Config.LoadConfig();
            harmony.PatchAll();
        }
        public void DeInit() 
        {
            harmony.UnpatchAll();
        }
    }
}
