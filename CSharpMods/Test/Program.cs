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
#nullable enable
namespace Test
{
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        //记录上一个bind过的对象的id，防止重复绑定
        public int lastRZDDetailID = -1;

        //not used
        public System.Timers.Timer initDescTimer= new System.Timers.Timer(15000);

        void Log(string i) { MyExten.Log(i); }
        void Error(string i) { MyExten.Error(i); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public int FindMostStableTalentID(BindDictInt_Int list)
        {
            int vigorPassive = -1;
            int talent = -1;
            int equip = -1;
            foreach (var id in list)
                if(id.Key>=200101&&id.Key<=200604)//首选根器
                    return id.Key;
                else if(id.Key>=303600&&id.Key<400000)//精魂被动
                    vigorPassive=id.Key;
                else if (id.Key <= 101505 && id.Key > 100000)//天赋
                    talent = id.Key;
                else if (id.Key >= 300000 && id.Key < 303600)//天赋
                    talent = id.Key;
                else
                    equip= id.Key;
            if (vigorPassive >= 0) return vigorPassive;
            if(talent >= 0) return talent;
            return equip;
        }
        public void Init()
        {
            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            //Utils.RegisterKeyBind(ModifierKeys.Control, Key.F7, LoadAllDataFiles);
            Utils.RegisterKeyBind(Key.O, delegate {
            });

            //initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            //initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { LoadAllDataFiles(); });
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
        private void TryDelayInit()
        {
            //似乎直接执行也可以，但为了保险起见等进入游戏再执行
            if (MyExten.GetPlayerController() is null) return;
            var pawn = MyExten.GetControlledPawn();
            if (pawn is null) return;
            //Log($"{pawn.GetFullName()}");
            initDescTimer.Stop();
        }
    }
}
