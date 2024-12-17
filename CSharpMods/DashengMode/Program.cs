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
using Mono.Unix;
using b1.Protobuf.DataAPI;
using System.ComponentModel;
using UnrealEngine.Runtime;
using Google.Protobuf;
using OssB1;
#nullable enable
namespace DashengMode
{
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.4";
        // private readonly Harmony harmony;
        public EDaShengStage target=EDaShengStage.DaShengMode;
        //not used
        public System.Timers.Timer initDescTimer= new System.Timers.Timer(3000);

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
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.O, delegate {
                switch (target)
                {
                    case EDaShengStage.DaShengMode:
                        target = EDaShengStage.LittleMonkey;
                        Log($"Normal Mode On");
                        break;
                    case EDaShengStage.PreStage:
                        target = EDaShengStage.DaShengMode;
                        Log($"Dasheng Mode On");
                        break;
                    case EDaShengStage.LittleMonkey:
                        target = EDaShengStage.PreStage;
                        Log($"Pre Dasheng Mode On");
                        break;
                }
                CheckOnTick();
            });
            Utils.RegisterKeyBind(ModifierKeys.Shift, Key.O, delegate {
                //只在pre和dasheng间切换
                switch (target)
                {
                    case EDaShengStage.DaShengMode:
                        target = EDaShengStage.LittleMonkey;
                        CheckOnTick();
                        target = EDaShengStage.PreStage;
                        Log($"Pre Mode On");
                        break;
                    case EDaShengStage.PreStage:
                    case EDaShengStage.LittleMonkey:
                        target = EDaShengStage.DaShengMode;
                        Log($"Dasheng Mode On");
                        break;
                }
                CheckOnTick();
            });

            initDescTimer.Start();
            initDescTimer.Elapsed += (Object source, ElapsedEventArgs e) => CheckOnTick();
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
        private void CheckOnTick()
        {
            if (MyExten.GetPlayerController() is null) return;
            var character = MyExten.GetBGUPlayerCharacterCS();
            if (character is null) return;
            var compList = character.ActorCompContainerCS.GetFieldOrProperty<List<UActorCompBaseCS>>("CompCSs");
            if (compList is null) return;
            BUS_QiTianDaShengComp? dashengComp = null;
            foreach (var comp in compList)
                if (comp.GetType() == typeof(BUS_QiTianDaShengComp))
                    dashengComp = (comp as BUS_QiTianDaShengComp)!;
            if (dashengComp is null) return;
            var dashengData = dashengComp.GetFieldOrProperty<BUC_QiTianDaShengData>("QiTianDaShengData");
            if (dashengData is null) return;
            
            {
                int NORMAL_DASHENG_CONFIG_ID = MyExten.GetFieldOrProperty<BUS_QiTianDaShengComp, int>("NORMAL_DASHENG_CONFIG_ID");
                FUStTransQiTianDaShengConfigDesc daShengConfigDesc = BGW_GameDB.GetTransQiTianDaShengConfigDesc(NORMAL_DASHENG_CONFIG_ID, character);
                if (daShengConfigDesc.Duration != 10000000)
                {
                    daShengConfigDesc.Duration = 1000000;
                    Log($"Change NORMAL_DASHENG_DURATION");
                }
            }

            //如果需要切换到大圣/PreStage,需要设置equipIdList和TalentIDList
            if ((this.target == EDaShengStage.DaShengMode && dashengData.DaShengStage != this.target)
                ||(this.target == EDaShengStage.PreStage && dashengData.DaShengStage != this.target)) 
            {
                var equipData = dashengComp.GetFieldOrProperty<IBUC_EquipData>("EquipData");
                var roleData = dashengComp.GetFieldOrProperty<IBPC_RoleBaseData>("RoleBaseData");
                if (equipData is null || roleData is null) return;
                if (!equipData.SelfEquipMap.ContainsKey(EquipPosition.Weapon)) return;
                var curWeapon = equipData.SelfEquipMap[EquipPosition.Weapon];
                if (roleData.TalenList.Count == 0)
                {
                    Log($"No Talent.Ignore.");
                    return;
                }
                var talentId = FindMostStableTalentID(roleData.TalenList);
                //foreach (var t in roleData.TalenList)
                //Log($"T{t} {talentId}");
                if (talentId < 0)
                {
                    Error($"Can't Find any Valid Talent {roleData.TalenList.Count}");
                    return;
                }
                {
                    //InitDaShengConfig时从BGW_GameDB把所有数据拷贝进了QiTianDaShengData
                    dashengData.RelatedTalentIDList.Clear();
                    dashengData.RelatedTalentIDList.Add(talentId);
                    dashengData.RelatedEquipIDList.Clear();
                    dashengData.RelatedEquipIDList.Add(curWeapon);
                    Log($"Reset Tran Config to {curWeapon} {talentId}");
                }
            }
            else if (this.target == EDaShengStage.LittleMonkey && dashengData.DaShengStage != this.target)//如果需要还原到基础模式，则要清空TalentIDList
                //如果不clear，由于之前设置的条件仍然满足，即使回到LittleMonkey也会自动进入PreStage(表现为多一段棍势)
                dashengData.RelatedEquipIDList.Clear();

            if (this.target == EDaShengStage.DaShengMode&& dashengData.DaShengStage != this.target)//进入大圣模式
            {
                //正常流程下是满足条件时先从LittleMonkey进入PreStage获得第五段棍势条，然后蓄力棍进入DashengMode，这里不管PreStage
                Log($"Trans to Qitiandasheng from {dashengData.DaShengStage.ToString()}");
                dashengComp.CallPrivateFunc("TrySwitch2DaShengMode", new object[] { dashengData.DaShengStage });
            }
            else if (this.target == EDaShengStage.PreStage && dashengData.DaShengStage ==EDaShengStage.DaShengMode) //进入PreStage,如果之前是LittleMonkey会自动进入PreStage不需要主动操作
            {
                Log($"Trans to PreStage from {dashengData.DaShengStage.ToString()}");
                dashengComp.CallPrivateFunc("TrySwitch2PreStage", new object[] { dashengData.DaShengStage });
            }
            else if (this.target == EDaShengStage.LittleMonkey && dashengData.DaShengStage == EDaShengStage.DaShengMode)
            {
                Log($"Trans to LittleMonkey from {dashengData.DaShengStage.ToString()}");
                dashengComp.CallPrivateFunc("Reset2LittleMonkey", new object[] { });
            }
            //Log($"---{dashengData.DaShengStage.ToString()}");
        }
    }
}
