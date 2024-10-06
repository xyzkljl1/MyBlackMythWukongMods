﻿using b1;
using UnrealEngine.Engine;
using UnrealEngine.Runtime;
using GSE.GSSdk;
using GSE;
using ResB1;
using System.Reflection;
using System;
using ILRuntime.Runtime;
#nullable enable
namespace PlayerStatus
{
    public static class MyExten
    {
        private static UWorld? world;
        public static string Name => typeof(MyExten).Namespace;
        public static FieldType? GetFieldOrProperty<FieldType>(this object obj, String field_name) where FieldType : class
        {
            var t = obj.GetType();
            {
                var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return field.GetValue(obj) as FieldType;
            }
            {
                var field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return field.GetValue(obj) as FieldType;
            }

            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {field_name}");
                return default(FieldType);
            }
        }
        public static FieldType? GetFieldOrProperty2<FieldType>(this object obj, String field_name) where FieldType : unmanaged
        {
            var t = obj.GetType();
            {
                var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return (FieldType)field.GetValue(obj);
            }
            {
                var field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return (FieldType)field.GetValue(obj);
            }

            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {field_name}");
                return null;
            }
        }
        public static Type? GetFieldOrPropertyType(this object obj, String field_name)
        {
            var t = obj.GetType();
            {
                var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return field.FieldType;
            }
            //try find property
            {
                var field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                    return field.PropertyType;
            }
            Error($"GetFieldOrPropertyType : Can't Find {field_name}");
            return null;
        }
        public static void SetFieldOrProperty(this object obj, String field_name, object value)
        {
            var t = obj.GetType();
            {
                var field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetField(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                {
                    if (field.FieldType.Name != value.GetType().Name && value.GetType() == typeof(string))
                    {
                        if (field.FieldType == typeof(float))
                            field.SetValue(obj, float.Parse(value as string));
                        else if (field.FieldType == typeof(double))
                            field.SetValue(obj, double.Parse(value as string));
                        else if (field.FieldType == typeof(int))
                            field.SetValue(obj, int.Parse(value as string));
                        else if (field.FieldType == typeof(Int16))
                            field.SetValue(obj, Int16.Parse(value as string));
                        else if (field.FieldType == typeof(Int64))
                            field.SetValue(obj, Int64.Parse(value as string));
                        else
                            field.SetValue(obj, value);
                    }
                    else
                        field.SetValue(obj, value);
                    return;
                }
            }
            {
                var field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Instance);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.NonPublic | BindingFlags.Static);
                if (field is null)
                    field = t.GetProperty(field_name, BindingFlags.Public | BindingFlags.Static);
                if (field is not null)
                {
                    if (field.PropertyType.Name != value.GetType().Name && value.GetType() == typeof(string))
                    {
                        if (field.PropertyType == typeof(float))
                            field.SetValue(obj, float.Parse(value as string));
                        else if (field.PropertyType == typeof(double))
                            field.SetValue(obj, double.Parse(value as string));
                        else if (field.PropertyType == typeof(int))
                            field.SetValue(obj, int.Parse(value as string));
                        else if (field.PropertyType == typeof(Int16))
                            field.SetValue(obj, Int16.Parse(value as string));
                        else if (field.PropertyType == typeof(Int64))
                            field.SetValue(obj, Int64.Parse(value as string));
                        else
                            field.SetValue(obj, value);
                    }
                    else
                        field.SetValue(obj, value);
                    return;
                }

            }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {field_name}");
            return;
        }
        public static object? CallPrivateFunc(this object obj, String method_name, object[] paras)
        {
            var t = obj.GetType();
            var methodInfo = t.GetMethod(method_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo is null)
            {
                methodInfo = t.GetMethod(method_name, BindingFlags.NonPublic | BindingFlags.Static);
            }
            if (methodInfo is null)
            {
                Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
                return null;
            }
            //return null;
            return methodInfo.Invoke(obj, paras);
        }
        public static object? CallPrivateGenericFunc(this object obj, String method_name, Type[] para_type4search, Type[] generic_types, object[] paras)
        {
            var t = obj.GetType();
            foreach (var methodInfo in t.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                if (methodInfo.IsGenericMethod && methodInfo.Name == method_name)
                {
                    var para_types = methodInfo.GetParameters();
                    if (para_types.Length == para_type4search.Length)
                    {
                        bool isMatch = true;
                        for (int i = 0; i < para_type4search.Length; i++)//根据参数类型搜索
                            if (para_types[i].ParameterType.Name != para_type4search[i].Name)
                            {
                                isMatch = false;
                                //Console.WriteLine($"Not Match:{para_types[i].ParameterType.Name}/{para_type4search[i].Name}");
                                break;
                            }
                        if (isMatch)
                        {
                            //Console.WriteLine($"Find: {method_name}");
                            var insMethodInfo = methodInfo.MakeGenericMethod(generic_types);//传入泛型参数获得泛型方法实例
                            if (insMethodInfo is null)
                                Console.WriteLine($"{Name} Fatal Error: Can't get instance of generic: {method_name}");
                            else
                                return insMethodInfo.Invoke(obj, paras);
                        }
                    }

                }
            Console.WriteLine($"{Name} Fatal Error: Can't Find {method_name}");
            return null;
        }

        public static void Log(string msg)
        {
            Console.WriteLine($"[{Name}]: {msg}");
        }
        public static void Error(string msg)
        {
            Console.WriteLine($"Error! [{Name}]: {msg}");
        }
        public static void DebugLog(string msg)
        {
#if DEBUG
            Console.WriteLine($"[{Name}]: {msg}");
#endif
        }

        public static UWorld? GetWorld()
        {
            if (world == null)
            {
                UObjectRef uobjectRef = GCHelper.FindRef(FGlobals.GWorld);
                world = uobjectRef?.Managed as UWorld;
            }
            return world;
        }

        public static APawn GetControlledPawn()
        {
            return UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld()).GetControlledPawn();
        }

        public static BGUPlayerCharacterCS GetBGUPlayerCharacterCS()
        {
            return (GetControlledPawn() as BGUPlayerCharacterCS)!;
        }

        public static BGP_PlayerControllerB1 GetPlayerController()
        {
            return (BGP_PlayerControllerB1)UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld());
        }

        public static BUS_GSEventCollection GetBUS_GSEventCollection()
        {
            return BUS_EventCollectionCS.Get(GetControlledPawn());
        }

    }
}
