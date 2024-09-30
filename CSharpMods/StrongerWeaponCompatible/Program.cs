using System;
using b1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using BtlShare;
using CSharpModBase;
using CSharpModBase.Input;
using ResB1;
// using HarmonyLib;
using GSE.GSSdk;
using B1UI.GSUI;
using GSE.GSUI;
using System.Reflection;
using UnrealEngine.UMG;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using EffectDetailDescription;
using b1.Localization;
using System.Linq;
using UnrealEngine.Engine;
using static UnrealEngine.Runtime.HotReload;
using UnrealEngine.Runtime;
using System.Threading;
using System.Timers;
using BtlB1;
using System.Reflection.Emit;
using b1.Protobuf.DataAPI;
#nullable enable
namespace EffectDetailDescription
{
    public static class MyExten
    {
        public static string Name => "Stronger Weapon Compatible";
        public static FieldType? GetField<FieldType>(this object obj, String field_name) where FieldType : class
        {
            var t = obj.GetType();
            var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
            if (field is null)
                field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
            if (field is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {field_name}");
                return default(FieldType);
            }
            return field.GetValue(obj) as FieldType;
        }
        public static object? CallPrivateFunc(this object obj, String method_name, object[] paras)
        {
            var t = obj.GetType();
            var methodInfo = t.GetMethod(method_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
                return null;
            }
            //return null;
            return methodInfo.Invoke(obj,paras);
        }
        public static object? CallPrivateGenericFunc(this object obj, String method_name, Type[] para_type4search, Type[] generic_types, object[] paras)
        {
            var t = obj.GetType();
            foreach (var methodInfo in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                if(methodInfo.IsGenericMethod&&methodInfo.Name==method_name)
                {
                    var para_types = methodInfo.GetParameters();
                    if (para_types.Length==para_type4search.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < para_type4search.Length; i++)//根据参数类型搜索
                            if (para_types[i].ParameterType.Name!=para_type4search[i].Name)
                            { 
                                isMatch = false;
                                //Console.WriteLine($"Not Match:{para_types[i].ParameterType.Name}/{para_type4search[i].Name}");
                                break; 
                            }
                        if(isMatch)
                        {
                            //Console.WriteLine($"Find: {method_name}");
                            var insMethodInfo = methodInfo.MakeGenericMethod(generic_types);//传入泛型参数获得泛型方法实例
                            if(insMethodInfo is null)
                                Console.WriteLine($"{Name} Fatal Error: Can't get instance of generic: {method_name}");
                            else
                                return insMethodInfo.Invoke(obj, paras);
                        }
                    }

                }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
            return null;
        }
        public static string ToANSI(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            return Encoding.GetEncoding(936).GetString(bytes);
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.0";
        // private readonly Harmony harmony;
        //记录上一个bind过的对象的id，防止重复绑定
        public int lastRZDDetailID = -1;

        public System.Timers.Timer bindEventTimer= new System.Timers.Timer(3000);
        public System.Timers.Timer initDescTimer= new System.Timers.Timer(5000);

        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void Log(string msg)
        {
            Console.WriteLine($"[{Name}]: {msg}");
        }
        public void DebugLog(string msg)
        {
#if DEBUG
            Console.WriteLine($"[{Name}]: {msg}");
#endif
        }

        public void Init()
        {
            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            //Utils.RegisterKeyBind(ModifierKeys.Control, Key.ENTER, FindPlayer);
            Utils.RegisterKeyBind(Key.O, delegate {
                GameDBRuntime.GetItemDesc(2218).CarryMax = 100;
                var world = MyUtils.GetWorld();
                var suit=BGW_GameDB.GetSuitDesc(3);
                var suit2 = BGW_GameDB.GetSuitDesc(4);
                foreach (var suitInfo in suit2.SuitInfo)
                    suit.SuitInfo.Add(suitInfo.Clone());
            });

            initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { TryInitDesc(); });
            // hook
            // harmony.PatchAll();
        }
        public void DeInit() 
        {
            initDescTimer.Dispose();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        private void TryInitDesc()
        {
            //似乎直接执行也可以，但为了保险起见等进入游戏再执行
            if (MyUtils.GetPlayerController() is null) return;
            var pawn = MyUtils.GetControlledPawn();
            if (pawn is null) return;
            //Log($"{pawn.GetFullName()}");
            initDescTimer.Stop();
        }
    }
}
