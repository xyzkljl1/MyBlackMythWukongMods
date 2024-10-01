
using b1;
using UnrealEngine.Engine;
using UnrealEngine.Runtime;
using GSE.GSSdk;
using GSE;
using b1.Protobuf.DataAPI;
using ResB1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using System.Reflection;
using System;
using LitJson;
using ILRuntime.Runtime;
#nullable enable
namespace DashengMode
{
    public static class MyExten
    {
        private static UWorld? world;
        public static string Name => "DashengMode";
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

        public static ObjectType? CreateObjectFromJson<ObjectType>(JsonData json) where ObjectType :class ,new()
        {
            var ret = new ObjectType();
            if (json is null || json.IsObject == false) return null;
            foreach(string key in json.Keys)
            {
                var val = json[key];
                var fieldType = ret.GetFieldOrPropertyType(key);
                if (fieldType == null) continue;
                if (fieldType == typeof(string))
                {
                    if (val.IsString)
                        ret.SetFieldOrProperty(key, val.ToString());
                    else
                        Log($"Ignore {key}.Need String");
                }
                else if (fieldType == typeof(int) || fieldType == typeof(Int16) || fieldType == typeof(bool))
                {
                    if (val.IsInt)
                        ret.SetFieldOrProperty(key, (int)val);
                    else
                        Log($"Ignore {key}.Need Int");
                }
                else if (fieldType.IsEnum)
                {
                    if(val.IsInt)
                        ret.SetFieldOrProperty(key, (int)val);
                    else if(val.IsString)
                    {
                        bool find=false;
                        foreach(var i in fieldType.GetEnumValues())
                            if(fieldType.GetEnumName(i)== val.ToString())
                            {
                                ret.SetFieldOrProperty(key, i);
                                find = true;
                                break;
                            }
                        if (!find)
                            Log($"Ignore {val.ToString()}.Can't find in enum {fieldType.Name}");
                    }
                    else
                        Log($"Ignore {key}.Need Int or String for enum");
                }
                else if (fieldType == typeof(float) || fieldType == typeof(double))
                {
                    if (val.IsInt)
                        ret.SetFieldOrProperty(key, (int)val);
                    else if(val.IsDouble)
                        ret.SetFieldOrProperty(key, (double)val);
                    else
                        Log($"Ignore {key}.Need float");
                }
                else
                {
                    Log($"Ignore {key}.Not supported type {fieldType.Name}");
                }                
            }
            return ret;
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

        public static T LoadAsset<T>(string asset) where T : UObject
        {
            return b1.BGW.BGW_PreloadAssetMgr.Get(GetWorld()).TryGetCachedResourceObj<T>(asset, b1.BGW.ELoadResourceType.SyncLoadAndCache, b1.BGW.EAssetPriority.Default, null, -1, -1);
        }

        public static UClass LoadClass(string asset)
        {
            return LoadAsset<UClass>(asset);
        }

        public static AActor? SpawnActor(string classAsset)
        {
            var controlledPawn = GetControlledPawn();
            FVector actorLocation = controlledPawn.GetActorLocation();
            FVector b = controlledPawn.GetActorForwardVector() * 1000.0f;
            FVector start = actorLocation + b;
            FRotator frotator = UMathLibrary.FindLookAtRotation(start, actorLocation);
            UClass uClass = LoadClass($"PrefabricatorAsset'{classAsset}'");
            if (uClass == null)
            {
                return null;
            }
            return BGUFunctionLibraryCS.BGUSpawnActor(controlledPawn.World, uClass, start, frotator);
        }

        public static AActor GetActorOfClass(string classAsset)
        {
            return UGameplayStatics.GetActorOfClass(GetWorld(), LoadAsset<UClass>(classAsset));
        }
    }
}
