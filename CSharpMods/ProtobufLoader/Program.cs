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
using LitJson;
using System.Collections;
using static b1.AutoQA.QASimulateWindowsOperations;
using ILRuntime.Mono.Cecil.Cil;
using Google.Protobuf.Collections;
#nullable enable
namespace ProtobufLoader
{
    public class JsonField
    {
        public string Name = "";
        public string DisplayName = "";
        public string Type = "";
        public string ToolTip = "";
    }
    public static class Config
    {
        public static bool ShutUp = false;
        public static bool ShuutUp = false;
        public static bool ShuuutUp = false;
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
                var type = typeof(Config);
                foreach (JsonData jsonFieldObject in jsonDoc["Config"])
                    if (jsonFieldObject.IsObject)
                    {
                        var tmp = JsonMapper.ToObject<JsonField>(JsonMapper.ToJson(jsonFieldObject));
                        var fieldInfo = type.GetField(tmp.Name, BindingFlags.Public | BindingFlags.Static);
                        if (fieldInfo == null)
                        {
                            MyExten.Error($"Can't Set Field {tmp.Name}");
                            continue;
                        }
                        JsonData? value = null;
                        if (jsonFieldObject.Keys.Contains("CurrentValue") && jsonFieldObject["CurrentValue"] != null)
                            value = jsonFieldObject["CurrentValue"];
                        else if (jsonFieldObject.Keys.Contains("DefaultValue"))
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
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}",1);
                        }
                        else if (fieldInfo.FieldType == typeof(int))
                        {
                            if (tmp.Type.ToLower() == "int" && value.IsInt)
                            {
                                fieldInfo.SetValue(null, (int)value);
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}",1);
                        }
                        else if (fieldInfo.FieldType == typeof(bool))
                        {
                            if (tmp.Type.ToLower() == "bool" && value.IsBoolean)
                            {
                                fieldInfo.SetValue(null, (bool)value);
                            }
                            else
                                MyExten.Log($"{fieldInfo.FieldType.Name} no match {tmp.Type}",1);
                        }
                    }
                MyExten.Log($"Config loaded {Config.ShutUp} {Config.ShuutUp} {Config.ShuuutUp}",0);
            }
            catch (Exception)
            {
                MyExten.Error("Fail to Parse Config File");
                return;
            }
        }
    }
    public class MyMod : ICSharpMod
    {
        public string Name => MyExten.Name;
        public string Version => "1.2";
        // private readonly Harmony harmony;
        public Dictionary<Type,Dictionary<int, Google.Protobuf.IMessage?>> RecordBackup = new Dictionary<Type, Dictionary<int, IMessage?>>();

        void Log(string i,int verLevel=0) { MyExten.Log(i,verLevel); }
        void Error(string i, int verLevel=0) { MyExten.Error(i,verLevel); }
        void DebugLog(string i) { MyExten.DebugLog(i); }
        public MyMod()
        {
            // harmony = new Harmony(Name);
            // Harmony.DEBUG = true;
        }
        public void Init()
        {
            Config.LoadConfig();
            Log("MyMod::Init called.Start Timer");
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.F7, ResetAndLoadAllDataFiles);
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.F8, () => ResetAll(true));
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.F9, SuperReset);
            ResetAndLoadAllDataFiles();
            /*
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.F9, () =>
            {
                var t = typeof(BGW_GameDB);
                var fi=t.GetField("sMapPassiveSkill", BindingFlags.Static | BindingFlags.NonPublic);
                var dict=fi.GetValue(null) as Dictionary<int, Dictionary<int, FUStPassiveSkillDesc>>;
                var desc1 = BGW_GameDB.GetPassiveSkillDescDic(12345);
                var desc2 = BGW_GameDB.GetPassiveSkillDescDic(23456);
                var desc3 = BGW_GameDB.GetPassiveSkillDescDic(34567);
                if (desc1 != null)
                    Log($"Desc1 {desc1.Count}");
                if (desc2 != null)
                    Log($"Desc1 {desc2.Count}");
                if (desc3 != null)
                    Log($"Desc1 {desc3.Count}");
                Log($"{BGW_GameDB.GetAllPassiveSkillDesc().Count} {dict!.Count} {dict.ContainsKey(12345)}");
                foreach(var pair in BGW_GameDB.GetAllPassiveSkillDesc())
                if(pair.Value.PassiveSkillID==12345|| pair.Value.PassiveSkillID == 23456|| pair.Value.PassiveSkillID == 34567)
                {
                   Log($" Find {pair.Value.PassiveSkillID} {pair.Key}");
                }
                foreach(var config in BGW_GameDB.GetAllGlobalConfigDesc())
                    if (config.Value.ConfigInfo.AliasName==B1GlobalFNames.NORMAL_DASHENG_DURATION.ToString())
                    {
                        Log($"Desc4 {config.Value.ConfigInfo.ConfigValue}  {config.Key}");

                    }
            });*/

            // hook
            // harmony.PatchAll();
            /*
             * Fuck encoding
            foreach (var i in Encoding.GetEncodings())
                Log($"{i.Name}");
                Log($"{Encoding.Default.CodePage}");
            {
                var enc = Encoding.GetEncoding("GB18030");
                //var encb = Encoding.GetEncoding(936);
                foreach(var i in Encoding.GetEncodings())
                {
                    try
                    {
                        var encb = i.GetEncoding();
                        var bytes = Encoding.Convert(encb, enc, enc.GetBytes("啦啦啦"));
                        var bytes2 = Encoding.Convert(enc, encb, enc.GetBytes("啦啦啦"));
                        var bytes3 = Encoding.Convert(encb, enc, encb.GetBytes("啦啦啦"));
                        var bytes4 = Encoding.Convert(enc, encb, encb.GetBytes("啦啦啦"));
                        var bytes5 = encb.GetBytes("啦啦啦");
                        var bytes6 = encb.GetBytes("啦啦啦");
                        Log(enc.GetString(bytes));
                        Log(encb.GetString(bytes));
                        Log(enc.GetString(bytes2));
                        Log(encb.GetString(bytes2));
                        Log(enc.GetString(bytes3));
                        Log(encb.GetString(bytes3));
                        Log(enc.GetString(bytes4));
                        Log(encb.GetString(bytes4));
                        Log(enc.GetString(bytes5));
                        Log(encb.GetString(bytes5));
                        Log(enc.GetString(bytes6));
                        Log(encb.GetString(bytes6));
                        Log($"{encb.CodePage} --- {bytes.Length} {bytes[0]}");
                    }
                    catch (Exception ex) 
                    {
                    }
                }
            }
            */
        }
        public void DeInit() 
        {
            ResetAll();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        public void SuperReset()
        {
            //直接重新从pak里读所有数据，包含了Static和Dynamic
            BGUFunctionLibraryCS.RefreshGameDB();
            Log("Super Reset Done");
        }
        public void ResetAndLoadAllDataFiles()
        {
            ResetAll(false);
            int success_ct = 0;
            int total_ct = 0;
            foreach (var f in Directory.GetDirectories($"CSharpLoader\\Mods\\{MyExten.Name}"))
            {
                if (LoadDataFilesInDir(f))
                    success_ct++;
                total_ct++;
            }
            if (total_ct > 0)
                Log($"Load {success_ct}/{total_ct} Folders Successfully");
            RefreshDBCache();
        }
        public void RefreshDBCache()
        {
            Log("Refresh DBCache",1);
            {//Static要在前
                var t = typeof(BGW_GameDB);
                foreach (var methodname in new List<string> { "InitPartRuleUnitMap", "InitsMapAttackHitFX_ID", "InitsMapBeAttackedFX_ID", "InitCameraGroupUnitMap", "InitStraightCamUnitMap", "InitGiantCamUnitMap", "InitDiagonalCamUnitMap", "InitPassiveSkillMap", "InitUnitDeadMap", "InitSoulSkillMimicryMap", "InitHitSceneItemPerformMap", "InitFeatureFilterMap", "InitBuffTickRuleBySimpleStateData", "InitOnlineScreenMsgConfDict", "InitInteractMappingDict", "InitAiInteractMappingDict", "InitCustomStateMachineDict", "InitGuideAssetConfigDict", "InitActionNameTriggerEventIdDict", "InitGlobalConfigDesc", "InitsChallengeDescDict", "InitCollectionSpawnInfoDict", "InitBossRoomDict", "InitGlobalCannotDeadExtraCacheDict", "InitBuffDispMap", "InitBuffRuleMap", "InitElementDmgRatioLevelMapping", "InitAbnormalCommConfig", "InitBeAttackedDispInfo", "InitMapSymbolDescInfo", "InitGlobalAlchemyList", "InitPigsyStoryIAndRLibrary", "InitDOPerformMapping", "InitDefeatSlowTimeConfig", "InitCameraConversionParamConfig", "InitPotentialEnergyMap", "InitBossDict", "InitAbnormalDispMap", "InitAICrowdDetourlevelConfigDict", "InitBeAttackedStiffLevelMapping", "InitDialogue_FacialAnimPreloadMap", "InitLevelSequenceClearBattleItemConfig", "InitAkMarkerDesc", "InitFacialResourceMap", "InitSeqAudioJumpMap" })
                {
                    var methodInfo = t.GetMethod(methodname, BindingFlags.NonPublic| BindingFlags.Public | BindingFlags.Static);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(null, new object[] { });
                        DebugLog($"Invoke {methodInfo.Name}");
                    }
                    else
                        Error($"Can't Invoke {methodname}");
                }
            }
            {
                var t = typeof(GameDBRuntime);
                var methodInfo = t.GetMethod("BuildAllDescToDict", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                if (methodInfo!=null)
                {
                    methodInfo.Invoke(null, new object[] { });
                    DebugLog($"Invoke {methodInfo.Name}");
                }
                else
                    Error($"Can't Invoke BuildAllDescToDict");
            }
        }
        public bool LoadDataFilesInDir(string dir)
        {
            //load *.data
            int totalFileCount = 0;
            int success_ct = 0;
            {
                List<string> files = new List<string>();
                foreach (var f in Directory.GetFiles(dir, "*Desc*.data").ToList<string>())
                    if (f.EndsWith(".bak.data"))
                        Log($"Ignore {f} because end with .bak",1);
                    else if(!f.EndsWith(".insert.data"))
                        files.Add(f);
                files.Sort();
                Log($"Find {files.Count} .data files.Start Load",1);
                foreach (var file in files)
                    if (LoadDataFile(file,false))
                        success_ct++;
                totalFileCount += files.Count;
            }
            //load *.data
            {
                List<string> files = new List<string>();
                foreach (var f in Directory.GetFiles(dir, "*Desc*.insert.data").ToList<string>())
                   files.Add(f);
                files.Sort();
                if(files.Count > 0)
                {
                    Log($"Find {files.Count} .insert.data files.Start Load",1);
                    foreach (var file in files)
                        if (LoadDataFile(file, true))
                            success_ct++;
                    totalFileCount += files.Count;
                }
            }

            if (totalFileCount > 0)
                Log($"Load {success_ct}/{totalFileCount} successfully in Dir {dir}",0);
            return success_ct == totalFileCount;//空文件夹算成功
        }
        public bool LoadDataFile(string filepath, bool isInsertMode)
        {
            var fileInfo = new FileInfo(filepath);
            var filename = fileInfo.Name;
            var typename = filename.Substring(0, filename.IndexOf("Desc") + 4);//GetFiles时已经确保能找到Desc
            //NonRuntime
            switch (typename)
            {
                //BG_ProtobufDataAPI<(.*)>[^\n\r]*
                //case "$1": return LoadNoneRuntimeDataImp<$1>(filepath, filename, typename);
                //BGW_GameDB.LoadRes
                case "FUStInteractiveUnitCommDesc": return LoadNoneRuntimeDataImp<FUStInteractiveUnitCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStInteractionMappingDesc": return LoadNoneRuntimeDataImp<FUStInteractionMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAiInteractionMappingDesc": return LoadNoneRuntimeDataImp<FUStAiInteractionMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffDesc": return LoadNoneRuntimeDataImp<FUStBuffDesc>(filepath, filename, typename, isInsertMode);
                case "FUStChargeSkillSDesc": return LoadNoneRuntimeDataImp<FUStChargeSkillSDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDropItemDesc": return LoadNoneRuntimeDataImp<FUStDropItemDesc>(filepath, filename, typename, isInsertMode);
                case "FUStHitVEffectDesc": return LoadNoneRuntimeDataImp<FUStHitVEffectDesc>(filepath, filename, typename, isInsertMode);
                case "FUStQTEDesc": return LoadNoneRuntimeDataImp<FUStQTEDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSummonCommDesc": return LoadNoneRuntimeDataImp<FUStSummonCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPhysicalHitBoneRuleDesc": return LoadNoneRuntimeDataImp<FUStPhysicalHitBoneRuleDesc>(filepath, filename, typename, isInsertMode);
                case "FUStRebirthPointDesc": return LoadNoneRuntimeDataImp<FUStRebirthPointDesc>(filepath, filename, typename, isInsertMode);
                case "FUStStraightCamDesc": return LoadNoneRuntimeDataImp<FUStStraightCamDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMultiPointLockCameraConfigDesc": return LoadNoneRuntimeDataImp<FUStMultiPointLockCameraConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDiagonalCamDesc": return LoadNoneRuntimeDataImp<FUStDiagonalCamDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGiantLockCameraDesc": return LoadNoneRuntimeDataImp<FUStGiantLockCameraDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitCollisionHitMoveDesc": return LoadNoneRuntimeDataImp<FUStUnitCollisionHitMoveDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffDispDesc": return LoadNoneRuntimeDataImp<FUStBuffDispDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffLayerDispDesc": return LoadNoneRuntimeDataImp<FUStBuffLayerDispDesc>(filepath, filename, typename, isInsertMode);
                case "FUStExAnimDataDesc": return LoadNoneRuntimeDataImp<FUStExAnimDataDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitCommDesc": return LoadNoneRuntimeDataImp<FUStUnitCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitBattleInfoExtendDesc": return LoadNoneRuntimeDataImp<FUStUnitBattleInfoExtendDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitPassiveSkillInfoExtendDesc": return LoadNoneRuntimeDataImp<FUStUnitPassiveSkillInfoExtendDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitEnvMaskConfigDesc": return LoadNoneRuntimeDataImp<FUStUnitEnvMaskConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSkillAIDesc": return LoadNoneRuntimeDataImp<FUStSkillAIDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSkillEffectDesc": return LoadNoneRuntimeDataImp<FUStSkillEffectDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSkillSDesc": return LoadNoneRuntimeDataImp<FUStSkillSDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSkillSMappingDesc": return LoadNoneRuntimeDataImp<FUStSkillSMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStEnvironmentSwitchDesc": return LoadNoneRuntimeDataImp<FUStEnvironmentSwitchDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBulletSwitchDesc": return LoadNoneRuntimeDataImp<FUStBulletSwitchDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitAIDesc": return LoadNoneRuntimeDataImp<FUStUnitAIDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitTransCommDesc": return LoadNoneRuntimeDataImp<FUStUnitTransCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStRollSkillDesc": return LoadNoneRuntimeDataImp<FUStRollSkillDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitDropDesc": return LoadNoneRuntimeDataImp<FUStUnitDropDesc>(filepath, filename, typename, isInsertMode);
                case "FUStNianhuiNameListDesc": return LoadNoneRuntimeDataImp<FUStNianhuiNameListDesc>(filepath, filename, typename, isInsertMode);
                case "FUStNianhuiAwardDesc": return LoadNoneRuntimeDataImp<FUStNianhuiAwardDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBeAttackedInfoDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBeAttackedDispInfoDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedDispInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStScarInfoDesc": return LoadNoneRuntimeDataImp<FUStScarInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerSkillCtrlDesc": return LoadNoneRuntimeDataImp<FUStPlayerSkillCtrlDesc>(filepath, filename, typename, isInsertMode);
                case "FUStOverlyingSkillSDesc": return LoadNoneRuntimeDataImp<FUStOverlyingSkillSDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffGroupDesc": return LoadNoneRuntimeDataImp<FUStBuffGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffRuleDesc": return LoadNoneRuntimeDataImp<FUStBuffRuleDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMandatoryAITaskDesc": return LoadNoneRuntimeDataImp<FUStMandatoryAITaskDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttackHitAudioInfoDesc": return LoadNoneRuntimeDataImp<FUStAttackHitAudioInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerCameraDesc": return LoadNoneRuntimeDataImp<FUStPlayerCameraDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerTransAttrDesc": return LoadNoneRuntimeDataImp<FUStPlayerTransAttrDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttackHitFXMapDesc": return LoadNoneRuntimeDataImp<FUStAttackHitFXMapDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBeAttackedFXMapDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedFXMapDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerCommDesc": return LoadNoneRuntimeDataImp<FUStPlayerCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitDeadDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitDeadSwitchToPhysicDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadSwitchToPhysicDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitDeadOldDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadOldDesc>(filepath, filename, typename, isInsertMode);
                case "FUStHitSceneItemPerformDesc": return LoadNoneRuntimeDataImp<FUStHitSceneItemPerformDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitFootstepDesc": return LoadNoneRuntimeDataImp<FUStUnitFootstepDesc>(filepath, filename, typename, isInsertMode);
                case "FUStEQSSettingDesc": return LoadNoneRuntimeDataImp<FUStEQSSettingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPassiveSkillDesc": return LoadNoneRuntimeDataImp<FUStPassiveSkillDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAISkillBasicActionDesc": return LoadNoneRuntimeDataImp<FUStAISkillBasicActionDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitAudioBankMapDesc": return LoadNoneRuntimeDataImp<FUStUnitAudioBankMapDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAISkillTagsDesc": return LoadNoneRuntimeDataImp<FUStAISkillTagsDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAIThinkDesc": return LoadNoneRuntimeDataImp<FUStAIThinkDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAIActionDesc": return LoadNoneRuntimeDataImp<FUStAIActionDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAIFeatureDesc": return LoadNoneRuntimeDataImp<FUStAIFeatureDesc>(filepath, filename, typename, isInsertMode);
                case "FUStProjectileCommDesc": return LoadNoneRuntimeDataImp<FUStProjectileCommDesc>(filepath, filename, typename, isInsertMode);
                case "FUStProjectileDispDesc": return LoadNoneRuntimeDataImp<FUStProjectileDispDesc>(filepath, filename, typename, isInsertMode);
                case "FUStProjectileMoveDesc": return LoadNoneRuntimeDataImp<FUStProjectileMoveDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBulletExpandDesc": return LoadNoneRuntimeDataImp<FUStBulletExpandDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMagicFieldExpandDesc": return LoadNoneRuntimeDataImp<FUStMagicFieldExpandDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBulletWindFieldExpandDesc": return LoadNoneRuntimeDataImp<FUStBulletWindFieldExpandDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitSpecialMoveDesc": return LoadNoneRuntimeDataImp<FUStUnitSpecialMoveDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitLevelUpDesc": return LoadNoneRuntimeDataImp<FUStUnitLevelUpDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPartRuleInfoDesc": return LoadNoneRuntimeDataImp<FUStPartRuleInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttackerHitFXMappingDesc": return LoadNoneRuntimeDataImp<FUStAttackerHitFXMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttackerHitAudioEventMappingDesc": return LoadNoneRuntimeDataImp<FUStAttackerHitAudioEventMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffTickRuleBySimpleStateDesc": return LoadNoneRuntimeDataImp<FUStBuffTickRuleBySimpleStateDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitPhysicalAnimationDesc": return LoadNoneRuntimeDataImp<FUStUnitPhysicalAnimationDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMovieSequenceDesc": return LoadNoneRuntimeDataImp<FUStMovieSequenceDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSuitDesc": return LoadNoneRuntimeDataImp<FUStSuitDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCameraGroupDesc": return LoadNoneRuntimeDataImp<FUStCameraGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStFixFunctionDesc": return LoadNoneRuntimeDataImp<FUStFixFunctionDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGroupAISDesc": return LoadNoneRuntimeDataImp<FUStGroupAISDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDialogueDesc": return LoadNoneRuntimeDataImp<FUStDialogueDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDialogueIDMappingDesc": return LoadNoneRuntimeDataImp<FUStDialogueIDMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUIWordDesc": return LoadNoneRuntimeDataImp<FUStUIWordDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTalentDisplayDesc": return LoadNoneRuntimeDataImp<FUStTalentDisplayDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTalentLvUpCfgDesc": return LoadNoneRuntimeDataImp<FUStTalentLvUpCfgDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAiConversationEventDesc": return LoadNoneRuntimeDataImp<FUStAiConversationEventDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAiConversationContentDesc": return LoadNoneRuntimeDataImp<FUStAiConversationContentDesc>(filepath, filename, typename, isInsertMode);
                case "FUStOnlineScreenMsgConfDesc": return LoadNoneRuntimeDataImp<FUStOnlineScreenMsgConfDesc>(filepath, filename, typename, isInsertMode);
                case "FUStRichTextIconDesc": return LoadNoneRuntimeDataImp<FUStRichTextIconDesc>(filepath, filename, typename, isInsertMode);
                case "FUStIronBodyConfigDesc": return LoadNoneRuntimeDataImp<FUStIronBodyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStImmobilizeSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStImmobilizeSkillConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSealingSpellSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStSealingSpellSkillConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTransQiTianDaShengConfigDesc": return LoadNoneRuntimeDataImp<FUStTransQiTianDaShengConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPhantomRushSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStPhantomRushSkillConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerInputSkillMappingDesc": return LoadNoneRuntimeDataImp<FUStPlayerInputSkillMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTaskStageDesc": return LoadNoneRuntimeDataImp<FUStTaskStageDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTaskLineDesc": return LoadNoneRuntimeDataImp<FUStTaskLineDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSuperArmorLevelDesc": return LoadNoneRuntimeDataImp<FUStSuperArmorLevelDesc>(filepath, filename, typename, isInsertMode);
                //case "FUStCollectionSpawnInfoDesc": return LoadNoneRuntimeDataImp<FUStCollectionSpawnInfoDesc>(filepath, filename, typename,isInsertMode);
                case "FUStCollectionSpawnGroupDesc": return LoadNoneRuntimeDataImp<FUStCollectionSpawnGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCollectionEventProbabilityDesc": return LoadNoneRuntimeDataImp<FUStCollectionEventProbabilityDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCustomStateMachineDesc": return LoadNoneRuntimeDataImp<FUStCustomStateMachineDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGuideAssetConfigDesc": return LoadNoneRuntimeDataImp<FUStGuideAssetConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStNPCBaseInfoDesc": return LoadNoneRuntimeDataImp<FUStNPCBaseInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStEnhancedInputActionDesc": return LoadNoneRuntimeDataImp<FUStEnhancedInputActionDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSkillDamageExpandDesc": return LoadNoneRuntimeDataImp<FUStSkillDamageExpandDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPotentialEnergyConfigDesc": return LoadNoneRuntimeDataImp<FUStPotentialEnergyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGlobalConfigDesc": return LoadNoneRuntimeDataImp<FUStGlobalConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTeamRelationConfigDesc": return LoadNoneRuntimeDataImp<FUStTeamRelationConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAiConversationGroupDesc": return LoadNoneRuntimeDataImp<FUStAiConversationGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAssociationUnitInfoSDesc": return LoadNoneRuntimeDataImp<FUStAssociationUnitInfoSDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitIntelligenceInfoDesc": return LoadNoneRuntimeDataImp<FUStUnitIntelligenceInfoDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCBGTemplateDesc": return LoadNoneRuntimeDataImp<FUStCBGTemplateDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTamerStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStTamerStrategyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTROStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStTROStrategyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSummonCopySkillDesc": return LoadNoneRuntimeDataImp<FUStSummonCopySkillDesc>(filepath, filename, typename, isInsertMode);
                case "FUStUnitChangeMaterialByAttrDesc": return LoadNoneRuntimeDataImp<FUStUnitChangeMaterialByAttrDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCCGCastSkillMappingRuleDesc": return LoadNoneRuntimeDataImp<FUStCCGCastSkillMappingRuleDesc>(filepath, filename, typename, isInsertMode);
                case "FUStWeakPerformConfigDesc": return LoadNoneRuntimeDataImp<FUStWeakPerformConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStFollowPartnerConfigDesc": return LoadNoneRuntimeDataImp<FUStFollowPartnerConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStChallengeDesc": return LoadNoneRuntimeDataImp<FUStChallengeDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBossRoomConfigDesc": return LoadNoneRuntimeDataImp<FUStBossRoomConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGlobalCannotDeadExtraConfigDesc": return LoadNoneRuntimeDataImp<FUStGlobalCannotDeadExtraConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStEnvironmentSurfaceEffectDesc": return LoadNoneRuntimeDataImp<FUStEnvironmentSurfaceEffectDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSweepCheckDesc": return LoadNoneRuntimeDataImp<FUStSweepCheckDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAudioExtendDesc": return LoadNoneRuntimeDataImp<FUStAudioExtendDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffIconDesc": return LoadNoneRuntimeDataImp<FUStBuffIconDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPartHitExpandDesc": return LoadNoneRuntimeDataImp<FUStPartHitExpandDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDetonateConfigDesc": return LoadNoneRuntimeDataImp<FUStDetonateConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttachedNiagaraByHitDesc": return LoadNoneRuntimeDataImp<FUStAttachedNiagaraByHitDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAbnormalStateUIBlackListDesc": return LoadNoneRuntimeDataImp<FUStAbnormalStateUIBlackListDesc>(filepath, filename, typename, isInsertMode);
                case "FUStElementDmgRatioLevelDesc": return LoadNoneRuntimeDataImp<FUStElementDmgRatioLevelDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAbnormalCommConfigDesc": return LoadNoneRuntimeDataImp<FUStAbnormalCommConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStStreamingLevelStateDesc": return LoadNoneRuntimeDataImp<FUStStreamingLevelStateDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMapSymbolDesc": return LoadNoneRuntimeDataImp<FUStMapSymbolDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAttrCopyConfigDesc": return LoadNoneRuntimeDataImp<FUStAttrCopyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPlayerTransUnitConfDesc": return LoadNoneRuntimeDataImp<FUStPlayerTransUnitConfDesc>(filepath, filename, typename, isInsertMode);
                case "FUStLifeSavingHairConfigDesc": return LoadNoneRuntimeDataImp<FUStLifeSavingHairConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPigsyStoryLibraryDesc": return LoadNoneRuntimeDataImp<FUStPigsyStoryLibraryDesc>(filepath, filename, typename, isInsertMode);
                case "FUStPigsyStoryIAndRLibraryDesc": return LoadNoneRuntimeDataImp<FUStPigsyStoryIAndRLibraryDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGuideGroupDesc": return LoadNoneRuntimeDataImp<FUStGuideGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStGuideNodeDesc": return LoadNoneRuntimeDataImp<FUStGuideNodeDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDynamicObstaclePerformanceDesc": return LoadNoneRuntimeDataImp<FUStDynamicObstaclePerformanceDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDefeatSlowTimeConfigDesc": return LoadNoneRuntimeDataImp<FUStDefeatSlowTimeConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBuffDispGroupDesc": return LoadNoneRuntimeDataImp<FUStBuffDispGroupDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSoulSkillMimicryDesc": return LoadNoneRuntimeDataImp<FUStSoulSkillMimicryDesc>(filepath, filename, typename, isInsertMode);
                case "FUStCameraConversionParamConfigDesc": return LoadNoneRuntimeDataImp<FUStCameraConversionParamConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStEffectiveHitProjectileEffectDesc": return LoadNoneRuntimeDataImp<FUStEffectiveHitProjectileEffectDesc>(filepath, filename, typename, isInsertMode);
                case "FUStTransActiveStateDesc": return LoadNoneRuntimeDataImp<FUStTransActiveStateDesc>(filepath, filename, typename, isInsertMode);
                case "FUStMovementOptStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStMovementOptStrategyConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAbnormalDispAttackerMapDesc": return LoadNoneRuntimeDataImp<FUStAbnormalDispAttackerMapDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAbnormalDispVictimMapDesc": return LoadNoneRuntimeDataImp<FUStAbnormalDispVictimMapDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAICrowdDetourLevelConfigDesc": return LoadNoneRuntimeDataImp<FUStAICrowdDetourLevelConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStRebirthAreaDesc": return LoadNoneRuntimeDataImp<FUStRebirthAreaDesc>(filepath, filename, typename, isInsertMode);
                case "FUStLevelSequenceClearBattleItemConfigDesc": return LoadNoneRuntimeDataImp<FUStLevelSequenceClearBattleItemConfigDesc>(filepath, filename, typename, isInsertMode);
                case "FUStBeAttackedStiffLevelMappingDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedStiffLevelMappingDesc>(filepath, filename, typename, isInsertMode);
                case "FUStAkEventMarkerDesc": return LoadNoneRuntimeDataImp<FUStAkEventMarkerDesc>(filepath, filename, typename, isInsertMode);
                case "FUStSeqAudioJumpLengthDesc": return LoadNoneRuntimeDataImp<FUStSeqAudioJumpLengthDesc>(filepath, filename, typename, isInsertMode);
                case "FUStDeadSeqUnitConfigDesc": return LoadNoneRuntimeDataImp<FUStDeadSeqUnitConfigDesc>(filepath, filename, typename, isInsertMode);
                default: break;
            }
            //Runtime,根据Runtime文件夹下的所有文件生成
            switch (typename)
            {
                case "AchievementDesc": return LoadRuntimeDataImp<TBAchievementDesc, AchievementDesc>(filepath, filename, isInsertMode);
                case "AlchemyOutputDesc": return LoadRuntimeDataImp<TBAlchemyOutputDesc, AlchemyOutputDesc>(filepath, filename, isInsertMode);
                case "ArmorEnhanceConsumeDesc": return LoadRuntimeDataImp<TBArmorEnhanceConsumeDesc, ArmorEnhanceConsumeDesc>(filepath, filename, isInsertMode);
                case "ArmorEnhanceDesc": return LoadRuntimeDataImp<TBArmorEnhanceDesc, ArmorEnhanceDesc>(filepath, filename, isInsertMode);
                case "ArtBookDesc": return LoadRuntimeDataImp<TBArtBookDesc, ArtBookDesc>(filepath, filename, isInsertMode);
                case "AttrItemDesc": return LoadRuntimeDataImp<TBAttrItemDesc, AttrItemDesc>(filepath, filename, isInsertMode);
                case "BloodHudDesc": return LoadRuntimeDataImp<TBBloodHudDesc, BloodHudDesc>(filepath, filename, isInsertMode);
                case "CardDesc": return LoadRuntimeDataImp<TBCardDesc, CardDesc>(filepath, filename, isInsertMode);
                case "ChapterDesc": return LoadRuntimeDataImp<TBChapterDesc, ChapterDesc>(filepath, filename, isInsertMode);
                case "CollectionDropDesc": return LoadRuntimeDataImp<TBCollectionDropDesc, CollectionDropDesc>(filepath, filename, isInsertMode);
                //case "CombatSkillDesc": return LoadRuntimeDataImp<TBCombatSkillDesc, CombatSkillDesc>(filepath,filename,isInsertMode);
                case "CommDropRuleDesc": return LoadRuntimeDataImp<TBCommDropRuleDesc, CommDropRuleDesc>(filepath, filename, isInsertMode);
                case "CommLogicCfgDesc": return LoadRuntimeDataImp<TBCommLogicCfgDesc, CommLogicCfgDesc>(filepath, filename, isInsertMode);
                case "CommonErrorUITipsDesc": return LoadRuntimeDataImp<TBCommonErrorUITipsDesc, CommonErrorUITipsDesc>(filepath, filename, isInsertMode);
                case "ConsumeDesc": return LoadRuntimeDataImp<TBConsumeDesc, ConsumeDesc>(filepath, filename, isInsertMode);
                case "CricketBattleUnitDesc": return LoadRuntimeDataImp<TBCricketBattleUnitDesc, CricketBattleUnitDesc>(filepath, filename, isInsertMode);
                case "CricketUnitAttrDesc": return LoadRuntimeDataImp<TBCricketUnitAttrDesc, CricketUnitAttrDesc>(filepath, filename, isInsertMode);
                case "DestructionDropDesc": return LoadRuntimeDataImp<TBDestructionDropDesc, DestructionDropDesc>(filepath, filename, isInsertMode);
                case "EchoDesc": return LoadRuntimeDataImp<TBEchoDesc, EchoDesc>(filepath, filename, isInsertMode);
                case "EditionAwardDesc": return LoadRuntimeDataImp<TBEditionAwardDesc, EditionAwardDesc>(filepath, filename, isInsertMode);
                case "EquipAttrDesc": return LoadRuntimeDataImp<TBEquipAttrDesc, EquipAttrDesc>(filepath, filename, isInsertMode);
                case "EquipDesc": return LoadRuntimeDataImp<TBEquipDesc, EquipDesc>(filepath, filename, isInsertMode);
                case "EquipFaBaoAttrDesc": return LoadRuntimeDataImp<TBEquipFaBaoAttrDesc, EquipFaBaoAttrDesc>(filepath, filename, isInsertMode);
                case "EquipPositionConfDesc": return LoadRuntimeDataImp<TBEquipPositionConfDesc, EquipPositionConfDesc>(filepath, filename, isInsertMode);
                case "EquipSeriesDesc": return LoadRuntimeDataImp<TBEquipSeriesDesc, EquipSeriesDesc>(filepath, filename, isInsertMode);
                case "GMMonsterTeleportDesc": return LoadRuntimeDataImp<TBGMMonsterTeleportDesc, GMMonsterTeleportDesc>(filepath, filename, isInsertMode);
                case "HistoricDesc": return LoadRuntimeDataImp<TBHistoricDesc, HistoricDesc>(filepath, filename, isInsertMode);
                case "HuluDesc": return LoadRuntimeDataImp<TBHuluDesc, HuluDesc>(filepath, filename, isInsertMode);
                case "IncreaseConfigDesc": return LoadRuntimeDataImp<TBIncreaseConfigDesc, IncreaseConfigDesc>(filepath, filename, isInsertMode);
                case "InteractionFuncDesc": return LoadRuntimeDataImp<TBInteractionFuncDesc, InteractionFuncDesc>(filepath, filename, isInsertMode);
                case "ItemDesc": return LoadRuntimeDataImp<TBItemDesc, ItemDesc>(filepath, filename, isInsertMode);
                case "ItemRecipeDesc": return LoadRuntimeDataImp<TBItemRecipeDesc, ItemRecipeDesc>(filepath, filename, isInsertMode);
                case "LevelDesc": return LoadRuntimeDataImp<TBLevelDesc, LevelDesc>(filepath, filename, isInsertMode);
                case "LinkBloodDesc": return LoadRuntimeDataImp<TBLinkBloodDesc, LinkBloodDesc>(filepath, filename, isInsertMode);
                case "LoadingTipsDesc": return LoadRuntimeDataImp<TBLoadingTipsDesc, LoadingTipsDesc>(filepath, filename, isInsertMode);
                case "LoadingTipsWeightDesc": return LoadRuntimeDataImp<TBLoadingTipsWeightDesc, LoadingTipsWeightDesc>(filepath, filename, isInsertMode);
                case "LockMantraDesc": return LoadRuntimeDataImp<TBLockMantraDesc, LockMantraDesc>(filepath, filename, isInsertMode);
                case "LotteryAwardDesc": return LoadRuntimeDataImp<TBLotteryAwardDesc, LotteryAwardDesc>(filepath, filename, isInsertMode);
                case "MantraBuildupDesc": return LoadRuntimeDataImp<TBMantraBuildupDesc, MantraBuildupDesc>(filepath, filename, isInsertMode);
                case "MantraDesc": return LoadRuntimeDataImp<TBMantraDesc, MantraDesc>(filepath, filename, isInsertMode);
                case "MantraWeightDesc": return LoadRuntimeDataImp<TBMantraWeightDesc, MantraWeightDesc>(filepath, filename, isInsertMode);
                case "MapAreaConfigDesc": return LoadRuntimeDataImp<TBMapAreaConfigDesc, MapAreaConfigDesc>(filepath, filename, isInsertMode);
                case "MapFragmentDesc": return LoadRuntimeDataImp<TBMapFragmentDesc, MapFragmentDesc>(filepath, filename, isInsertMode);
                case "MedicineAwardDesc": return LoadRuntimeDataImp<TBMedicineAwardDesc, MedicineAwardDesc>(filepath, filename, isInsertMode);
                case "MeditationPointDesc": return LoadRuntimeDataImp<TBMeditationPointDesc, MeditationPointDesc>(filepath, filename, isInsertMode);
                case "MovieAndSubtitleDesc": return LoadRuntimeDataImp<TBMovieAndSubtitleDesc, MovieAndSubtitleDesc>(filepath, filename, isInsertMode);
                case "MultiplayerDropRuleDesc": return LoadRuntimeDataImp<TBMultiplayerDropRuleDesc, MultiplayerDropRuleDesc>(filepath, filename, isInsertMode);
                case "MuseumMVDesc": return LoadRuntimeDataImp<TBMuseumMVDesc, MuseumMVDesc>(filepath, filename, isInsertMode);
                case "NewGamePlusDesc": return LoadRuntimeDataImp<TBNewGamePlusDesc, NewGamePlusDesc>(filepath, filename, isInsertMode);
                case "NPCInteractConversationDesc": return LoadRuntimeDataImp<TBNPCInteractConversationDesc, NPCInteractConversationDesc>(filepath, filename, isInsertMode);
                case "PastMemoryDesc": return LoadRuntimeDataImp<TBPastMemoryDesc, PastMemoryDesc>(filepath, filename, isInsertMode);
                case "PlatformAchievementDesc": return LoadRuntimeDataImp<TBPlatformAchievementDesc, PlatformAchievementDesc>(filepath, filename, isInsertMode);
                case "PlatformAchievementLiteDesc": return LoadRuntimeDataImp<TBPlatformAchievementLiteDesc, PlatformAchievementLiteDesc>(filepath, filename, isInsertMode);
                case "PlayerLevelDesc": return LoadRuntimeDataImp<TBPlayerLevelDesc, PlayerLevelDesc>(filepath, filename, isInsertMode);
                //case "ActivityDesc": return LoadRuntimeDataImp<TBActivityDesc, ActivityDesc>(filepath,filename,isInsertMode);
                //case "ActivityTaskDesc": return LoadRuntimeDataImp<TBActivityTaskDesc, ActivityTaskDesc>(filepath,filename,isInsertMode);
                case "RoleDataConfigDesc": return LoadRuntimeDataImp<TBRoleDataConfigDesc, RoleDataConfigDesc>(filepath, filename, isInsertMode);
                case "SceneMonsterNameplateDesc": return LoadRuntimeDataImp<TBSceneMonsterNameplateDesc, SceneMonsterNameplateDesc>(filepath, filename, isInsertMode);
                //case "ScrollDesc": return LoadRuntimeDataImp<TBScrollDesc, ScrollDesc>(filepath,filename,isInsertMode);
                case "SeedCollectionAwardDesc": return LoadRuntimeDataImp<TBSeedCollectionAwardDesc, SeedCollectionAwardDesc>(filepath, filename, isInsertMode);
                case "SeedDesc": return LoadRuntimeDataImp<TBSeedDesc, SeedDesc>(filepath, filename, isInsertMode);
                case "ShopDesc": return LoadRuntimeDataImp<TBShopDesc, ShopDesc>(filepath, filename, isInsertMode);
                case "ShopItemDesc": return LoadRuntimeDataImp<TBShopItemDesc, ShopItemDesc>(filepath, filename, isInsertMode);
                case "ShopItemGroupDesc": return LoadRuntimeDataImp<TBShopItemGroupDesc, ShopItemGroupDesc>(filepath, filename, isInsertMode);
                case "ShopRefreshDesc": return LoadRuntimeDataImp<TBShopRefreshDesc, ShopRefreshDesc>(filepath, filename, isInsertMode);
                case "ShrineShowNpcConfigDesc": return LoadRuntimeDataImp<TBShrineShowNpcConfigDesc, ShrineShowNpcConfigDesc>(filepath, filename, isInsertMode);
                case "SoulSkillDesc": return LoadRuntimeDataImp<TBSoulSkillDesc, SoulSkillDesc>(filepath, filename, isInsertMode);
                case "SoulSkillDropDesc": return LoadRuntimeDataImp<TBSoulSkillDropDesc, SoulSkillDropDesc>(filepath, filename, isInsertMode);
                case "SoundTrackDesc": return LoadRuntimeDataImp<TBSoundTrackDesc, SoundTrackDesc>(filepath, filename, isInsertMode);
                case "SpellDesc": return LoadRuntimeDataImp<TBSpellDesc, SpellDesc>(filepath, filename, isInsertMode);
                case "SurpriseDesc": return LoadRuntimeDataImp<TBSurpriseDesc, SurpriseDesc>(filepath, filename, isInsertMode);
                case "TakePhotoCustomSettingDesc": return LoadRuntimeDataImp<TBTakePhotoCustomSettingDesc, TakePhotoCustomSettingDesc>(filepath, filename, isInsertMode);
                case "TalentRankDesc": return LoadRuntimeDataImp<TBTalentRankDesc, TalentRankDesc>(filepath, filename, isInsertMode);
                case "TalentSDesc": return LoadRuntimeDataImp<TBTalentSDesc, TalentSDesc>(filepath, filename, isInsertMode);
                case "TeamConfigDesc": return LoadRuntimeDataImp<TBTeamConfigDesc, TeamConfigDesc>(filepath, filename, isInsertMode);
                case "TransInputUITipsDesc": return LoadRuntimeDataImp<TBTransInputUITipsDesc, TransInputUITipsDesc>(filepath, filename, isInsertMode);
                case "UISettingConfigDesc": return LoadRuntimeDataImp<TBUISettingConfigDesc, UISettingConfigDesc>(filepath, filename, isInsertMode);
                //case "UISettingControlDesc": return LoadRuntimeDataImp<TBUISettingControlDesc, UISettingControlDesc>(filepath,filename,isInsertMode);
                case "UISettingDeviceConfigDesc": return LoadRuntimeDataImp<TBUISettingDeviceConfigDesc, UISettingDeviceConfigDesc>(filepath, filename, isInsertMode);
                case "UnitDropNumDesc": return LoadRuntimeDataImp<TBUnitDropNumDesc, UnitDropNumDesc>(filepath, filename, isInsertMode);
                case "UnitDropRuleDesc": return LoadRuntimeDataImp<TBUnitDropRuleDesc, UnitDropRuleDesc>(filepath, filename, isInsertMode);
                case "WeaponBuildDesc": return LoadRuntimeDataImp<TBWeaponBuildDesc, WeaponBuildDesc>(filepath, filename, isInsertMode);
                case "WineDesc": return LoadRuntimeDataImp<TBWineDesc, WineDesc>(filepath, filename, isInsertMode);
            }
            Error($"Not Supported Table {typename}");
            return false;
        }
        public bool LoadNoneRuntimeDataImp<T>(string filepath,string filename,string typename, bool isInsertMode) where T : class, Google.Protobuf.IMessage,new()
        {
            bool WithOutId = false;//决定载入到list还是dict里，根据BGW_GameDB.LoadRes中的参数设置
            if (typeof(T) == typeof(FUStCollectionSpawnInfoDesc))//目前只有这一个表用的list
                WithOutId = true;
            try
            {
                if (UGSE_FileFuncLib.LoadFileToArray(filepath, out var FileData))
                {
                    Log($"Start Load {filename} to {typename} Static Table",0);
                    MemoryStream memoryStream = new MemoryStream(FileData.ToArray());
                    string text = typeof(T).ToString();
                    text.IndexOf('.');
                    //Type.GetType不能直接获取到类型，需要指定Assembly
                    object obj = Activator.CreateInstance(typeof(T).Assembly.GetType(text.Insert(text.IndexOf('.') + 1, "TB"), throwOnError: true, ignoreCase: true));
                    object value = obj.GetType().GetProperty("Parser").GetValue(obj);
                    MethodInfo method = value.GetType().GetMethod("ParseFrom", new Type[1] { typeof(Stream) });
                    var _messageObject = method.Invoke(value, new object[1] { memoryStream });
                    IEnumerable<T>? parseResult = _messageObject.GetType().GetProperty("List").GetValue(_messageObject) as IEnumerable<T>;
                    if(parseResult==null)
                    {
                        Error($"Fail to ParseFrom file {filepath}");
                        return false;
                    }
                    if (WithOutId)
                    {
                        //Not Implemented!!
                        Error($"Insert for List is Not Implemented!!");
                        /*
                        var apiInstance = BG_ProtobufDataAPI<T>.Get();
                        var _dataList=apiInstance.GetFieldOrProperty<List<T>>("_dataList");
                        foreach (T item in value2)
                        {
                            _dataList.Add(item);
                        }*/
                    }
                    else
                    {
                        var apiInstance = BG_ProtobufDataAPI<T>.Get();
                        var _dataDict = apiInstance.GetFieldOrProperty<Dictionary<int, T>>("_dataDict");
                        var idPropertyName = apiInstance.GetFieldOrProperty<string>("_propertyID");
                        if(_dataDict is null)
                        {
                            Error($"Can't Find _DataList {typeof(T).Name}");
                            return false;
                        }
                        foreach (T item in parseResult)
                        {
                            //无视DataGuard
                            int? id = item.GetType().GetProperty(idPropertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.GetValue(item) as int?;
                            if(id==null)
                            {
                                Error($"Can't find id.Ignore item ");
                                continue;
                            }
                            if (!RecordBackup.ContainsKey(typeof(T)))
                                RecordBackup.Add(typeof(T), new Dictionary<int, IMessage?>());
                            if (_dataDict.ContainsKey(id.Value))
                            {
                                if (isInsertMode)//Change id to insert to end.
                                {
                                    //TODO : Optimize!!!!!!!!!!!!!!!!
                                    while (_dataDict.ContainsKey(id.Value)) id++;
                                    if(idPropertyName!=null)//修改item成员的id
                                        item.SetFieldOrProperty(idPropertyName!,id);
                                    RecordBackup[typeof(T)].Add(id.Value,null);
                                    Log($"Insert {(int)id} in {typename}",2);
                                }
                                else//Override
                                {
                                    if(typeof(T).GetInterfaces().Any(x =>x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDeepCloneable<>)))//是否可以Clone
                                        RecordBackup[typeof(T)].Add(id.Value, ((_dataDict[(int)id]) as IDeepCloneable<T>)!.Clone());
                                    else
                                    {
                                        Log($"Override a record not clonable in {typename}.It won't be reset.",2);
                                    }
                                    Log($"Override {(int)id} in {typename}",2);
                                }
                            }
                            else
                            {
                                RecordBackup[typeof(T)].Add(id.Value, null);
                                Log($"Add {(int)id} in {typename}",2);
                            }
                            _dataDict[(int)id] = item;
                        }
                    }
                    Log($"{filename} Done",1);
                    if (RecordBackup.ContainsKey(typeof(T)) && RecordBackup[typeof(T)].Count == 0)
                        RecordBackup.Remove(typeof(T));
                    return true;
                }
                else
                    Error($"Fail To Load File {filepath}");
            }
            catch (Exception e)
            {
                MyExten.Error($"Fail to LoadNoneRuntimeDataImp from {filepath}:{e.Message}");
                return false;
            }
            return false;
        }
        public bool LoadRuntimeDataImp<TB,T>(string filepath,string filename, bool isInsertMode) where TB : Google.Protobuf.IMessage, Google.Protobuf.IMessage<TB>, new() where T : Google.Protobuf.IMessage
        {
            try
            {
                if (UGSE_FileFuncLib.LoadFileToArray(filepath, out var FileData))
                {
                    Log($"Start Load {filename} to {typeof(T).Name} Dynamic Table",1);
                    MemoryStream input = new MemoryStream(FileData.ToArray());
                    var typename = typeof(T).Name;

                    var apiInstance = GSProtobufRuntimeAPI<TB, T>.Get();
                    var _dataDict = apiInstance.GetFieldOrProperty<Dictionary<int, T>>("_dataDict");
                    var _tbList=apiInstance.GetTBList();
                    if (_dataDict == null||_tbList==null)
                    {
                        Error($"Can't Find dataDict or tblist in {typeof(TB).Name}-{typename}");
                        return false;
                    }
                    var idPropertyName = apiInstance.GetFieldOrProperty<string>("_propertyID");
                    var _tmpTBList = new TB();
                    _tmpTBList.MergeFrom(input);
                    var parseResult= typeof(TB).GetProperty("List").GetValue(_tmpTBList) as IEnumerable<T>;
                    if(parseResult == null)
                    {
                        Error($"Fail to Parse {filepath}");
                        return false;
                    }
                    if (idPropertyName != "")
                    {
                        foreach (T item in parseResult)
                        {
                            object? _id = item.GetType().GetProperty(idPropertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.GetValue(item);
                            if (_id == null)
                            {
                                Error($"Can't find id.Ignore item ");
                                continue;
                            }
                            int id = Convert.ToInt32(_id);
                            if (!RecordBackup.ContainsKey(typeof(T)))
                                RecordBackup.Add(typeof(T), new Dictionary<int, IMessage?>());
                            if (_dataDict.ContainsKey((int)id))
                            {
                                if (isInsertMode)//Change id to insert to end.
                                {
                                    //TODO : Optimize!!!!!!!!!!!!!!!!
                                    while (_dataDict.ContainsKey(id)) id++;
                                    if (idPropertyName != null)
                                        item.SetFieldOrProperty(idPropertyName!, id);
                                    Log($"Insert {(int)id} in {typename}", 2);
                                    RecordBackup[typeof(T)].Add(id, null);

                                }
                                else//Override
                                {
                                    if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDeepCloneable<>)))//是否可以Clone
                                        RecordBackup[typeof(T)].Add(id, ((_dataDict[id]) as IDeepCloneable<T>)!.Clone());
                                    else
                                        Log($"Override a record not clonable in {typename}.It won't be reset.", 2);
                                    Log($"Override {(int)id} in {typename}", 2);
                                }
                            }
                            else
                            {
                                RecordBackup[typeof(T)].Add(id, null);
                                Log($"Add {(int)id} in {typename}", 2);
                            }
                            _dataDict[(int)id] = item;
                        }
                        //TBList也要修改，有些初始化例如InitTalentSUnitMap是从TBList里取值的
                        if(true)
                        {
                            //从LoadDataFromFile/TryAddToDictionary来看，一定具有名为List的Property且List的内容dataDict严格一一对应
                            var list = _tbList.GetFieldOrProperty<RepeatedField<T>>("List");
                            if (list == null)
                            {
                                Error($"Can't Find tbList.List in {typename}");
                                return false;
                            }
                            list.Clear();
                            //根据dict重建list
                            foreach (var item in _dataDict!)
                                list.Add(item.Value);
                            Log($"Rebuild {typename} TBList: {list.Count}", 2);
                        }
                        else
                        {
                            //也可以直接merge,T是引用类型，此时id的修改已经反映到tmpTBList上
                            //Log($"Before Merge {_tbList.CalculateSize()} {_tmpTBList.CalculateSize()}");
                            _tbList!.MergeFrom(_tmpTBList);
                            Log($"Merge {typename} TBList: {_tbList.CalculateSize()}", 2);
                            //Log($"After Merge {_tbList.CalculateSize()}");
                        }
                    }
                    else
                    {
                        Error($"Can't find id property name for type {typename}");
                        return false;
                    }
                    if (RecordBackup.ContainsKey(typeof(T)) && RecordBackup[typeof(T)].Count == 0)
                        RecordBackup.Remove(typeof(T));
                    return true;
                }
                else
                    Error($"Can't Load File{filename}");
            }
            catch (Exception e) {
                MyExten.Error($"Fail to LoadRuntimeDataImp from {filepath}:{e.Message}");
                return false;
            }
            return false;
        }
        public void ResetAll(bool refreshCache=true)
        {
            if (RecordBackup.Count == 0) return;
            int success_ct = 0;
            foreach(var pair in RecordBackup)
            {
                var typename = pair.Key.Name;
                if (ResetTable(typename))
                    success_ct++;
            }
            Log($"Reset {success_ct}/{RecordBackup.Count} tables.",0);
            RecordBackup.Clear();
            if(refreshCache)
                RefreshDBCache();
        }
        public bool ResetTable(string typename)
        {
            Log($"Start Reset Table {typename}",1);
            //NonRuntime
            switch (typename)
            {
                //BG_ProtobufDataAPI<(.*)>[^\n\r]*
                //case "$1": return RemoveInserted<$1>(filepath, filename, typename);
                //BGW_GameDB.LoadRes
                case "FUStInteractiveUnitCommDesc": return ResetTableNonRuntimeImp<FUStInteractiveUnitCommDesc>();
                case "FUStInteractionMappingDesc": return ResetTableNonRuntimeImp<FUStInteractionMappingDesc>();
                case "FUStAiInteractionMappingDesc": return ResetTableNonRuntimeImp<FUStAiInteractionMappingDesc>();
                case "FUStBuffDesc": return ResetTableNonRuntimeImp<FUStBuffDesc>();
                case "FUStChargeSkillSDesc": return ResetTableNonRuntimeImp<FUStChargeSkillSDesc>();
                case "FUStDropItemDesc": return ResetTableNonRuntimeImp<FUStDropItemDesc>();
                case "FUStHitVEffectDesc": return ResetTableNonRuntimeImp<FUStHitVEffectDesc>();
                case "FUStQTEDesc": return ResetTableNonRuntimeImp<FUStQTEDesc>();
                case "FUStSummonCommDesc": return ResetTableNonRuntimeImp<FUStSummonCommDesc>();
                case "FUStPhysicalHitBoneRuleDesc": return ResetTableNonRuntimeImp<FUStPhysicalHitBoneRuleDesc>();
                case "FUStRebirthPointDesc": return ResetTableNonRuntimeImp<FUStRebirthPointDesc>();
                case "FUStStraightCamDesc": return ResetTableNonRuntimeImp<FUStStraightCamDesc>();
                case "FUStMultiPointLockCameraConfigDesc": return ResetTableNonRuntimeImp<FUStMultiPointLockCameraConfigDesc>();
                case "FUStDiagonalCamDesc": return ResetTableNonRuntimeImp<FUStDiagonalCamDesc>();
                case "FUStGiantLockCameraDesc": return ResetTableNonRuntimeImp<FUStGiantLockCameraDesc>();
                case "FUStUnitCollisionHitMoveDesc": return ResetTableNonRuntimeImp<FUStUnitCollisionHitMoveDesc>();
                case "FUStBuffDispDesc": return ResetTableNonRuntimeImp<FUStBuffDispDesc>();
                case "FUStBuffLayerDispDesc": return ResetTableNonRuntimeImp<FUStBuffLayerDispDesc>();
                case "FUStExAnimDataDesc": return ResetTableNonRuntimeImp<FUStExAnimDataDesc>();
                case "FUStUnitCommDesc": return ResetTableNonRuntimeImp<FUStUnitCommDesc>();
                case "FUStUnitBattleInfoExtendDesc": return ResetTableNonRuntimeImp<FUStUnitBattleInfoExtendDesc>();
                case "FUStUnitPassiveSkillInfoExtendDesc": return ResetTableNonRuntimeImp<FUStUnitPassiveSkillInfoExtendDesc>();
                case "FUStUnitEnvMaskConfigDesc": return ResetTableNonRuntimeImp<FUStUnitEnvMaskConfigDesc>();
                case "FUStSkillAIDesc": return ResetTableNonRuntimeImp<FUStSkillAIDesc>();
                case "FUStSkillEffectDesc": return ResetTableNonRuntimeImp<FUStSkillEffectDesc>();
                case "FUStSkillSDesc": return ResetTableNonRuntimeImp<FUStSkillSDesc>();
                case "FUStSkillSMappingDesc": return ResetTableNonRuntimeImp<FUStSkillSMappingDesc>();
                case "FUStEnvironmentSwitchDesc": return ResetTableNonRuntimeImp<FUStEnvironmentSwitchDesc>();
                case "FUStBulletSwitchDesc": return ResetTableNonRuntimeImp<FUStBulletSwitchDesc>();
                case "FUStUnitAIDesc": return ResetTableNonRuntimeImp<FUStUnitAIDesc>();
                case "FUStUnitTransCommDesc": return ResetTableNonRuntimeImp<FUStUnitTransCommDesc>();
                case "FUStRollSkillDesc": return ResetTableNonRuntimeImp<FUStRollSkillDesc>();
                case "FUStUnitDropDesc": return ResetTableNonRuntimeImp<FUStUnitDropDesc>();
                case "FUStNianhuiNameListDesc": return ResetTableNonRuntimeImp<FUStNianhuiNameListDesc>();
                case "FUStNianhuiAwardDesc": return ResetTableNonRuntimeImp<FUStNianhuiAwardDesc>();
                case "FUStBeAttackedInfoDesc": return ResetTableNonRuntimeImp<FUStBeAttackedInfoDesc>();
                case "FUStBeAttackedDispInfoDesc": return ResetTableNonRuntimeImp<FUStBeAttackedDispInfoDesc>();
                case "FUStScarInfoDesc": return ResetTableNonRuntimeImp<FUStScarInfoDesc>();
                case "FUStPlayerSkillCtrlDesc": return ResetTableNonRuntimeImp<FUStPlayerSkillCtrlDesc>();
                case "FUStOverlyingSkillSDesc": return ResetTableNonRuntimeImp<FUStOverlyingSkillSDesc>();
                case "FUStBuffGroupDesc": return ResetTableNonRuntimeImp<FUStBuffGroupDesc>();
                case "FUStBuffRuleDesc": return ResetTableNonRuntimeImp<FUStBuffRuleDesc>();
                case "FUStMandatoryAITaskDesc": return ResetTableNonRuntimeImp<FUStMandatoryAITaskDesc>();
                case "FUStAttackHitAudioInfoDesc": return ResetTableNonRuntimeImp<FUStAttackHitAudioInfoDesc>();
                case "FUStPlayerCameraDesc": return ResetTableNonRuntimeImp<FUStPlayerCameraDesc>();
                case "FUStPlayerTransAttrDesc": return ResetTableNonRuntimeImp<FUStPlayerTransAttrDesc>();
                case "FUStAttackHitFXMapDesc": return ResetTableNonRuntimeImp<FUStAttackHitFXMapDesc>();
                case "FUStBeAttackedFXMapDesc": return ResetTableNonRuntimeImp<FUStBeAttackedFXMapDesc>();
                case "FUStPlayerCommDesc": return ResetTableNonRuntimeImp<FUStPlayerCommDesc>();
                case "FUStUnitDeadDesc": return ResetTableNonRuntimeImp<FUStUnitDeadDesc>();
                case "FUStUnitDeadSwitchToPhysicDesc": return ResetTableNonRuntimeImp<FUStUnitDeadSwitchToPhysicDesc>();
                case "FUStUnitDeadOldDesc": return ResetTableNonRuntimeImp<FUStUnitDeadOldDesc>();
                case "FUStHitSceneItemPerformDesc": return ResetTableNonRuntimeImp<FUStHitSceneItemPerformDesc>();
                case "FUStUnitFootstepDesc": return ResetTableNonRuntimeImp<FUStUnitFootstepDesc>();
                case "FUStEQSSettingDesc": return ResetTableNonRuntimeImp<FUStEQSSettingDesc>();
                case "FUStPassiveSkillDesc": return ResetTableNonRuntimeImp<FUStPassiveSkillDesc>();
                case "FUStAISkillBasicActionDesc": return ResetTableNonRuntimeImp<FUStAISkillBasicActionDesc>();
                case "FUStUnitAudioBankMapDesc": return ResetTableNonRuntimeImp<FUStUnitAudioBankMapDesc>();
                case "FUStAISkillTagsDesc": return ResetTableNonRuntimeImp<FUStAISkillTagsDesc>();
                case "FUStAIThinkDesc": return ResetTableNonRuntimeImp<FUStAIThinkDesc>();
                case "FUStAIActionDesc": return ResetTableNonRuntimeImp<FUStAIActionDesc>();
                case "FUStAIFeatureDesc": return ResetTableNonRuntimeImp<FUStAIFeatureDesc>();
                case "FUStProjectileCommDesc": return ResetTableNonRuntimeImp<FUStProjectileCommDesc>();
                case "FUStProjectileDispDesc": return ResetTableNonRuntimeImp<FUStProjectileDispDesc>();
                case "FUStProjectileMoveDesc": return ResetTableNonRuntimeImp<FUStProjectileMoveDesc>();
                case "FUStBulletExpandDesc": return ResetTableNonRuntimeImp<FUStBulletExpandDesc>();
                case "FUStMagicFieldExpandDesc": return ResetTableNonRuntimeImp<FUStMagicFieldExpandDesc>();
                case "FUStBulletWindFieldExpandDesc": return ResetTableNonRuntimeImp<FUStBulletWindFieldExpandDesc>();
                case "FUStUnitSpecialMoveDesc": return ResetTableNonRuntimeImp<FUStUnitSpecialMoveDesc>();
                case "FUStUnitLevelUpDesc": return ResetTableNonRuntimeImp<FUStUnitLevelUpDesc>();
                case "FUStPartRuleInfoDesc": return ResetTableNonRuntimeImp<FUStPartRuleInfoDesc>();
                case "FUStAttackerHitFXMappingDesc": return ResetTableNonRuntimeImp<FUStAttackerHitFXMappingDesc>();
                case "FUStAttackerHitAudioEventMappingDesc": return ResetTableNonRuntimeImp<FUStAttackerHitAudioEventMappingDesc>();
                case "FUStBuffTickRuleBySimpleStateDesc": return ResetTableNonRuntimeImp<FUStBuffTickRuleBySimpleStateDesc>();
                case "FUStUnitPhysicalAnimationDesc": return ResetTableNonRuntimeImp<FUStUnitPhysicalAnimationDesc>();
                case "FUStMovieSequenceDesc": return ResetTableNonRuntimeImp<FUStMovieSequenceDesc>();
                case "FUStSuitDesc": return ResetTableNonRuntimeImp<FUStSuitDesc>();
                case "FUStCameraGroupDesc": return ResetTableNonRuntimeImp<FUStCameraGroupDesc>();
                case "FUStFixFunctionDesc": return ResetTableNonRuntimeImp<FUStFixFunctionDesc>();
                case "FUStGroupAISDesc": return ResetTableNonRuntimeImp<FUStGroupAISDesc>();
                case "FUStDialogueDesc": return ResetTableNonRuntimeImp<FUStDialogueDesc>();
                case "FUStDialogueIDMappingDesc": return ResetTableNonRuntimeImp<FUStDialogueIDMappingDesc>();
                case "FUStUIWordDesc": return ResetTableNonRuntimeImp<FUStUIWordDesc>();
                case "FUStTalentDisplayDesc": return ResetTableNonRuntimeImp<FUStTalentDisplayDesc>();
                case "FUStTalentLvUpCfgDesc": return ResetTableNonRuntimeImp<FUStTalentLvUpCfgDesc>();
                case "FUStAiConversationEventDesc": return ResetTableNonRuntimeImp<FUStAiConversationEventDesc>();
                case "FUStAiConversationContentDesc": return ResetTableNonRuntimeImp<FUStAiConversationContentDesc>();
                case "FUStOnlineScreenMsgConfDesc": return ResetTableNonRuntimeImp<FUStOnlineScreenMsgConfDesc>();
                case "FUStRichTextIconDesc": return ResetTableNonRuntimeImp<FUStRichTextIconDesc>();
                case "FUStIronBodyConfigDesc": return ResetTableNonRuntimeImp<FUStIronBodyConfigDesc>();
                case "FUStImmobilizeSkillConfigDesc": return ResetTableNonRuntimeImp<FUStImmobilizeSkillConfigDesc>();
                case "FUStSealingSpellSkillConfigDesc": return ResetTableNonRuntimeImp<FUStSealingSpellSkillConfigDesc>();
                case "FUStTransQiTianDaShengConfigDesc": return ResetTableNonRuntimeImp<FUStTransQiTianDaShengConfigDesc>();
                case "FUStPhantomRushSkillConfigDesc": return ResetTableNonRuntimeImp<FUStPhantomRushSkillConfigDesc>();
                case "FUStPlayerInputSkillMappingDesc": return ResetTableNonRuntimeImp<FUStPlayerInputSkillMappingDesc>();
                case "FUStTaskStageDesc": return ResetTableNonRuntimeImp<FUStTaskStageDesc>();
                case "FUStTaskLineDesc": return ResetTableNonRuntimeImp<FUStTaskLineDesc>();
                case "FUStSuperArmorLevelDesc": return ResetTableNonRuntimeImp<FUStSuperArmorLevelDesc>();
                //case "FUStCollectionSpawnInfoDesc": return RemoveInserted<FUStCollectionSpawnInfoDesc>(filepath, filename, typename,isInsertMode);
                case "FUStCollectionSpawnGroupDesc": return ResetTableNonRuntimeImp<FUStCollectionSpawnGroupDesc>();
                case "FUStCollectionEventProbabilityDesc": return ResetTableNonRuntimeImp<FUStCollectionEventProbabilityDesc>();
                case "FUStCustomStateMachineDesc": return ResetTableNonRuntimeImp<FUStCustomStateMachineDesc>();
                case "FUStGuideAssetConfigDesc": return ResetTableNonRuntimeImp<FUStGuideAssetConfigDesc>();
                case "FUStNPCBaseInfoDesc": return ResetTableNonRuntimeImp<FUStNPCBaseInfoDesc>();
                case "FUStEnhancedInputActionDesc": return ResetTableNonRuntimeImp<FUStEnhancedInputActionDesc>();
                case "FUStSkillDamageExpandDesc": return ResetTableNonRuntimeImp<FUStSkillDamageExpandDesc>();
                case "FUStPotentialEnergyConfigDesc": return ResetTableNonRuntimeImp<FUStPotentialEnergyConfigDesc>();
                case "FUStGlobalConfigDesc": return ResetTableNonRuntimeImp<FUStGlobalConfigDesc>();
                case "FUStTeamRelationConfigDesc": return ResetTableNonRuntimeImp<FUStTeamRelationConfigDesc>();
                case "FUStAiConversationGroupDesc": return ResetTableNonRuntimeImp<FUStAiConversationGroupDesc>();
                case "FUStAssociationUnitInfoSDesc": return ResetTableNonRuntimeImp<FUStAssociationUnitInfoSDesc>();
                case "FUStUnitIntelligenceInfoDesc": return ResetTableNonRuntimeImp<FUStUnitIntelligenceInfoDesc>();
                case "FUStCBGTemplateDesc": return ResetTableNonRuntimeImp<FUStCBGTemplateDesc>();
                case "FUStTamerStrategyConfigDesc": return ResetTableNonRuntimeImp<FUStTamerStrategyConfigDesc>();
                case "FUStTROStrategyConfigDesc": return ResetTableNonRuntimeImp<FUStTROStrategyConfigDesc>();
                case "FUStSummonCopySkillDesc": return ResetTableNonRuntimeImp<FUStSummonCopySkillDesc>();
                case "FUStUnitChangeMaterialByAttrDesc": return ResetTableNonRuntimeImp<FUStUnitChangeMaterialByAttrDesc>();
                case "FUStCCGCastSkillMappingRuleDesc": return ResetTableNonRuntimeImp<FUStCCGCastSkillMappingRuleDesc>();
                case "FUStWeakPerformConfigDesc": return ResetTableNonRuntimeImp<FUStWeakPerformConfigDesc>();
                case "FUStFollowPartnerConfigDesc": return ResetTableNonRuntimeImp<FUStFollowPartnerConfigDesc>();
                case "FUStChallengeDesc": return ResetTableNonRuntimeImp<FUStChallengeDesc>();
                case "FUStBossRoomConfigDesc": return ResetTableNonRuntimeImp<FUStBossRoomConfigDesc>();
                case "FUStGlobalCannotDeadExtraConfigDesc": return ResetTableNonRuntimeImp<FUStGlobalCannotDeadExtraConfigDesc>();
                case "FUStEnvironmentSurfaceEffectDesc": return ResetTableNonRuntimeImp<FUStEnvironmentSurfaceEffectDesc>();
                case "FUStSweepCheckDesc": return ResetTableNonRuntimeImp<FUStSweepCheckDesc>();
                case "FUStAudioExtendDesc": return ResetTableNonRuntimeImp<FUStAudioExtendDesc>();
                case "FUStBuffIconDesc": return ResetTableNonRuntimeImp<FUStBuffIconDesc>();
                case "FUStPartHitExpandDesc": return ResetTableNonRuntimeImp<FUStPartHitExpandDesc>();
                case "FUStDetonateConfigDesc": return ResetTableNonRuntimeImp<FUStDetonateConfigDesc>();
                case "FUStAttachedNiagaraByHitDesc": return ResetTableNonRuntimeImp<FUStAttachedNiagaraByHitDesc>();
                case "FUStAbnormalStateUIBlackListDesc": return ResetTableNonRuntimeImp<FUStAbnormalStateUIBlackListDesc>();
                case "FUStElementDmgRatioLevelDesc": return ResetTableNonRuntimeImp<FUStElementDmgRatioLevelDesc>();
                case "FUStAbnormalCommConfigDesc": return ResetTableNonRuntimeImp<FUStAbnormalCommConfigDesc>();
                case "FUStStreamingLevelStateDesc": return ResetTableNonRuntimeImp<FUStStreamingLevelStateDesc>();
                case "FUStMapSymbolDesc": return ResetTableNonRuntimeImp<FUStMapSymbolDesc>();
                case "FUStAttrCopyConfigDesc": return ResetTableNonRuntimeImp<FUStAttrCopyConfigDesc>();
                case "FUStPlayerTransUnitConfDesc": return ResetTableNonRuntimeImp<FUStPlayerTransUnitConfDesc>();
                case "FUStLifeSavingHairConfigDesc": return ResetTableNonRuntimeImp<FUStLifeSavingHairConfigDesc>();
                case "FUStPigsyStoryLibraryDesc": return ResetTableNonRuntimeImp<FUStPigsyStoryLibraryDesc>();
                case "FUStPigsyStoryIAndRLibraryDesc": return ResetTableNonRuntimeImp<FUStPigsyStoryIAndRLibraryDesc>();
                case "FUStGuideGroupDesc": return ResetTableNonRuntimeImp<FUStGuideGroupDesc>();
                case "FUStGuideNodeDesc": return ResetTableNonRuntimeImp<FUStGuideNodeDesc>();
                case "FUStDynamicObstaclePerformanceDesc": return ResetTableNonRuntimeImp<FUStDynamicObstaclePerformanceDesc>();
                case "FUStDefeatSlowTimeConfigDesc": return ResetTableNonRuntimeImp<FUStDefeatSlowTimeConfigDesc>();
                case "FUStBuffDispGroupDesc": return ResetTableNonRuntimeImp<FUStBuffDispGroupDesc>();
                case "FUStSoulSkillMimicryDesc": return ResetTableNonRuntimeImp<FUStSoulSkillMimicryDesc>();
                case "FUStCameraConversionParamConfigDesc": return ResetTableNonRuntimeImp<FUStCameraConversionParamConfigDesc>();
                case "FUStEffectiveHitProjectileEffectDesc": return ResetTableNonRuntimeImp<FUStEffectiveHitProjectileEffectDesc>();
                case "FUStTransActiveStateDesc": return ResetTableNonRuntimeImp<FUStTransActiveStateDesc>();
                case "FUStMovementOptStrategyConfigDesc": return ResetTableNonRuntimeImp<FUStMovementOptStrategyConfigDesc>();
                case "FUStAbnormalDispAttackerMapDesc": return ResetTableNonRuntimeImp<FUStAbnormalDispAttackerMapDesc>();
                case "FUStAbnormalDispVictimMapDesc": return ResetTableNonRuntimeImp<FUStAbnormalDispVictimMapDesc>();
                case "FUStAICrowdDetourLevelConfigDesc": return ResetTableNonRuntimeImp<FUStAICrowdDetourLevelConfigDesc>();
                case "FUStRebirthAreaDesc": return ResetTableNonRuntimeImp<FUStRebirthAreaDesc>();
                case "FUStLevelSequenceClearBattleItemConfigDesc": return ResetTableNonRuntimeImp<FUStLevelSequenceClearBattleItemConfigDesc>();
                case "FUStBeAttackedStiffLevelMappingDesc": return ResetTableNonRuntimeImp<FUStBeAttackedStiffLevelMappingDesc>();
                case "FUStAkEventMarkerDesc": return ResetTableNonRuntimeImp<FUStAkEventMarkerDesc>();
                case "FUStSeqAudioJumpLengthDesc": return ResetTableNonRuntimeImp<FUStSeqAudioJumpLengthDesc>();
                case "FUStDeadSeqUnitConfigDesc": return ResetTableNonRuntimeImp<FUStDeadSeqUnitConfigDesc>();
                default: break;
            }
            //Runtime,根据Runtime文件夹下的所有文件生成
            switch (typename)
            {
                case "AchievementDesc": return ResetTableRuntimeImp<TBAchievementDesc, AchievementDesc>();
                case "AlchemyOutputDesc": return ResetTableRuntimeImp<TBAlchemyOutputDesc, AlchemyOutputDesc>();
                case "ArmorEnhanceConsumeDesc": return ResetTableRuntimeImp<TBArmorEnhanceConsumeDesc, ArmorEnhanceConsumeDesc>();
                case "ArmorEnhanceDesc": return ResetTableRuntimeImp<TBArmorEnhanceDesc, ArmorEnhanceDesc>();
                case "ArtBookDesc": return ResetTableRuntimeImp<TBArtBookDesc, ArtBookDesc>();
                case "AttrItemDesc": return ResetTableRuntimeImp<TBAttrItemDesc, AttrItemDesc>();
                case "BloodHudDesc": return ResetTableRuntimeImp<TBBloodHudDesc, BloodHudDesc>();
                case "CardDesc": return ResetTableRuntimeImp<TBCardDesc, CardDesc>();
                case "ChapterDesc": return ResetTableRuntimeImp<TBChapterDesc, ChapterDesc>();
                case "CollectionDropDesc": return ResetTableRuntimeImp<TBCollectionDropDesc, CollectionDropDesc>();
                //case "CombatSkillDesc": return RemoveInserted<TBCombatSkillDesc, CombatSkillDesc>(filepath,filename,isInsertMode);
                case "CommDropRuleDesc": return ResetTableRuntimeImp<TBCommDropRuleDesc, CommDropRuleDesc>();
                case "CommLogicCfgDesc": return ResetTableRuntimeImp<TBCommLogicCfgDesc, CommLogicCfgDesc>();
                case "CommonErrorUITipsDesc": return ResetTableRuntimeImp<TBCommonErrorUITipsDesc, CommonErrorUITipsDesc>();
                case "ConsumeDesc": return ResetTableRuntimeImp<TBConsumeDesc, ConsumeDesc>();
                case "CricketBattleUnitDesc": return ResetTableRuntimeImp<TBCricketBattleUnitDesc, CricketBattleUnitDesc>();
                case "CricketUnitAttrDesc": return ResetTableRuntimeImp<TBCricketUnitAttrDesc, CricketUnitAttrDesc>();
                case "DestructionDropDesc": return ResetTableRuntimeImp<TBDestructionDropDesc, DestructionDropDesc>();
                case "EchoDesc": return ResetTableRuntimeImp<TBEchoDesc, EchoDesc>();
                case "EditionAwardDesc": return ResetTableRuntimeImp<TBEditionAwardDesc, EditionAwardDesc>();
                case "EquipAttrDesc": return ResetTableRuntimeImp<TBEquipAttrDesc, EquipAttrDesc>();
                case "EquipDesc": return ResetTableRuntimeImp<TBEquipDesc, EquipDesc>();
                case "EquipFaBaoAttrDesc": return ResetTableRuntimeImp<TBEquipFaBaoAttrDesc, EquipFaBaoAttrDesc>();
                case "EquipPositionConfDesc": return ResetTableRuntimeImp<TBEquipPositionConfDesc, EquipPositionConfDesc>();
                case "EquipSeriesDesc": return ResetTableRuntimeImp<TBEquipSeriesDesc, EquipSeriesDesc>();
                case "GMMonsterTeleportDesc": return ResetTableRuntimeImp<TBGMMonsterTeleportDesc, GMMonsterTeleportDesc>();
                case "HistoricDesc": return ResetTableRuntimeImp<TBHistoricDesc, HistoricDesc>();
                case "HuluDesc": return ResetTableRuntimeImp<TBHuluDesc, HuluDesc>();
                case "IncreaseConfigDesc": return ResetTableRuntimeImp<TBIncreaseConfigDesc, IncreaseConfigDesc>();
                case "InteractionFuncDesc": return ResetTableRuntimeImp<TBInteractionFuncDesc, InteractionFuncDesc>();
                case "ItemDesc": return ResetTableRuntimeImp<TBItemDesc, ItemDesc>();
                case "ItemRecipeDesc": return ResetTableRuntimeImp<TBItemRecipeDesc, ItemRecipeDesc>();
                case "LevelDesc": return ResetTableRuntimeImp<TBLevelDesc, LevelDesc>();
                case "LinkBloodDesc": return ResetTableRuntimeImp<TBLinkBloodDesc, LinkBloodDesc>();
                case "LoadingTipsDesc": return ResetTableRuntimeImp<TBLoadingTipsDesc, LoadingTipsDesc>();
                case "LoadingTipsWeightDesc": return ResetTableRuntimeImp<TBLoadingTipsWeightDesc, LoadingTipsWeightDesc>();
                case "LockMantraDesc": return ResetTableRuntimeImp<TBLockMantraDesc, LockMantraDesc>();
                case "LotteryAwardDesc": return ResetTableRuntimeImp<TBLotteryAwardDesc, LotteryAwardDesc>();
                case "MantraBuildupDesc": return ResetTableRuntimeImp<TBMantraBuildupDesc, MantraBuildupDesc>();
                case "MantraDesc": return ResetTableRuntimeImp<TBMantraDesc, MantraDesc>();
                case "MantraWeightDesc": return ResetTableRuntimeImp<TBMantraWeightDesc, MantraWeightDesc>();
                case "MapAreaConfigDesc": return ResetTableRuntimeImp<TBMapAreaConfigDesc, MapAreaConfigDesc>();
                case "MapFragmentDesc": return ResetTableRuntimeImp<TBMapFragmentDesc, MapFragmentDesc>();
                case "MedicineAwardDesc": return ResetTableRuntimeImp<TBMedicineAwardDesc, MedicineAwardDesc>();
                case "MeditationPointDesc": return ResetTableRuntimeImp<TBMeditationPointDesc, MeditationPointDesc>();
                case "MovieAndSubtitleDesc": return ResetTableRuntimeImp<TBMovieAndSubtitleDesc, MovieAndSubtitleDesc>();
                case "MultiplayerDropRuleDesc": return ResetTableRuntimeImp<TBMultiplayerDropRuleDesc, MultiplayerDropRuleDesc>();
                case "MuseumMVDesc": return ResetTableRuntimeImp<TBMuseumMVDesc, MuseumMVDesc>();
                case "NewGamePlusDesc": return ResetTableRuntimeImp<TBNewGamePlusDesc, NewGamePlusDesc>();
                case "NPCInteractConversationDesc": return ResetTableRuntimeImp<TBNPCInteractConversationDesc, NPCInteractConversationDesc>();
                case "PastMemoryDesc": return ResetTableRuntimeImp<TBPastMemoryDesc, PastMemoryDesc>();
                case "PlatformAchievementDesc": return ResetTableRuntimeImp<TBPlatformAchievementDesc, PlatformAchievementDesc>();
                case "PlatformAchievementLiteDesc": return ResetTableRuntimeImp<TBPlatformAchievementLiteDesc, PlatformAchievementLiteDesc>();
                case "PlayerLevelDesc": return ResetTableRuntimeImp<TBPlayerLevelDesc, PlayerLevelDesc>();
                //case "ActivityDesc": return RemoveInserted<TBActivityDesc, ActivityDesc>(filepath,filename,isInsertMode);
                //case "ActivityTaskDesc": return RemoveInserted<TBActivityTaskDesc, ActivityTaskDesc>(filepath,filename,isInsertMode);
                case "RoleDataConfigDesc": return ResetTableRuntimeImp<TBRoleDataConfigDesc, RoleDataConfigDesc>();
                case "SceneMonsterNameplateDesc": return ResetTableRuntimeImp<TBSceneMonsterNameplateDesc, SceneMonsterNameplateDesc>();
                //case "ScrollDesc": return RemoveInserted<TBScrollDesc, ScrollDesc>(filepath,filename,isInsertMode);
                case "SeedCollectionAwardDesc": return ResetTableRuntimeImp<TBSeedCollectionAwardDesc, SeedCollectionAwardDesc>();
                case "SeedDesc": return ResetTableRuntimeImp<TBSeedDesc, SeedDesc>();
                case "ShopDesc": return ResetTableRuntimeImp<TBShopDesc, ShopDesc>();
                case "ShopItemDesc": return ResetTableRuntimeImp<TBShopItemDesc, ShopItemDesc>();
                case "ShopItemGroupDesc": return ResetTableRuntimeImp<TBShopItemGroupDesc, ShopItemGroupDesc>();
                case "ShopRefreshDesc": return ResetTableRuntimeImp<TBShopRefreshDesc, ShopRefreshDesc>();
                case "ShrineShowNpcConfigDesc": return ResetTableRuntimeImp<TBShrineShowNpcConfigDesc, ShrineShowNpcConfigDesc>();
                case "SoulSkillDesc": return ResetTableRuntimeImp<TBSoulSkillDesc, SoulSkillDesc>();
                case "SoulSkillDropDesc": return ResetTableRuntimeImp<TBSoulSkillDropDesc, SoulSkillDropDesc>();
                case "SoundTrackDesc": return ResetTableRuntimeImp<TBSoundTrackDesc, SoundTrackDesc>();
                case "SpellDesc": return ResetTableRuntimeImp<TBSpellDesc, SpellDesc>();
                case "SurpriseDesc": return ResetTableRuntimeImp<TBSurpriseDesc, SurpriseDesc>();
                case "TakePhotoCustomSettingDesc": return ResetTableRuntimeImp<TBTakePhotoCustomSettingDesc, TakePhotoCustomSettingDesc>();
                case "TalentRankDesc": return ResetTableRuntimeImp<TBTalentRankDesc, TalentRankDesc>();
                case "TalentSDesc": return ResetTableRuntimeImp<TBTalentSDesc, TalentSDesc>();
                case "TeamConfigDesc": return ResetTableRuntimeImp<TBTeamConfigDesc, TeamConfigDesc>();
                case "TransInputUITipsDesc": return ResetTableRuntimeImp<TBTransInputUITipsDesc, TransInputUITipsDesc>();
                case "UISettingConfigDesc": return ResetTableRuntimeImp<TBUISettingConfigDesc, UISettingConfigDesc>();
                //case "UISettingControlDesc": return RemoveInserted<TBUISettingControlDesc, UISettingControlDesc>(filepath,filename,isInsertMode);
                case "UISettingDeviceConfigDesc": return ResetTableRuntimeImp<TBUISettingDeviceConfigDesc, UISettingDeviceConfigDesc>();
                case "UnitDropNumDesc": return ResetTableRuntimeImp<TBUnitDropNumDesc, UnitDropNumDesc>();
                case "UnitDropRuleDesc": return ResetTableRuntimeImp<TBUnitDropRuleDesc, UnitDropRuleDesc>();
                case "WeaponBuildDesc": return ResetTableRuntimeImp<TBWeaponBuildDesc, WeaponBuildDesc>();
                case "WineDesc": return ResetTableRuntimeImp<TBWineDesc, WineDesc>();
            }
            Error($"Unsupported Table {typename}");
            return false;
        }
        public bool ResetDataDictImp<T>(Dictionary<int, T>? _dataDict) where T : class, Google.Protobuf.IMessage, new()
        {
            try
            {
                var type = typeof(T);
                if (!RecordBackup.ContainsKey(type)) return false;
                if (!typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDeepCloneable<>)))//是否可以Clone
                {
                    Error($"Unknown Error: {typeof(T).Name} is not clonable");
                    return false;
                }
                if (_dataDict == null)
                {
                    Error($"Can't Find dataDict in {type.Name}");
                    return false;
                }
                foreach (var pair in RecordBackup[type])
                {
                    int id = pair.Key;
                    if (_dataDict.ContainsKey(id))
                    {
                        if (pair.Value == null)//插入操作
                        {
                            _dataDict.Remove(id);
                            Log($"Remove {id} from {type.Name}",2);
                        }
                        else
                        {
                            _dataDict[id] = (pair.Value as T)!;
                            Log($"Revert {id} in {type.Name}",2);
                        }
                    }
                }
                Log($"Reset {type.Name} Dict Done",2);
                return true;
            }
            catch (Exception e)
            {
                MyExten.Error($"Fail to Reset {typeof(T).Name}:{e.Message}");
                return false;
            }
            
        }
        public bool ResetTableNonRuntimeImp<T>() where T : class, Google.Protobuf.IMessage, new()
        {
            bool WithOutId = false;
            if (typeof(T) == typeof(FUStCollectionSpawnInfoDesc))
                WithOutId = true;
            if (WithOutId)
            {
                //Not Implemented;
                Error($"{typeof(T).Name} Not Supported");
                return false;
            }
            try
            {
                var apiInstance = BG_ProtobufDataAPI<T>.Get();
                var _dataDict = apiInstance.GetFieldOrProperty<Dictionary<int, T>>("_dataDict");
                return ResetDataDictImp(_dataDict);
            }
            catch (Exception e)
            {
                MyExten.Error($"Fail to Reset {typeof(T).Name}:{e.Message}");
                return false;
            }
            //return false;
        }
        public bool ResetTableRuntimeImp<TB,T>() where TB : Google.Protobuf.IMessage, Google.Protobuf.IMessage<TB>, new() where T : class, Google.Protobuf.IMessage, new()
        {
            try
            {
                var apiInstance = GSProtobufRuntimeAPI<TB, T>.Get();
                var _dataDict = apiInstance.GetFieldOrProperty<Dictionary<int, T>>("_dataDict");
                if (!ResetDataDictImp(_dataDict))
                    return false;
                {
                    var type = typeof(T);
                    var tbList=apiInstance.GetTBList();
                    if (tbList == null)
                    {
                        Error($"Can't Find tbList in {type.Name}");
                        return false;
                    }
                    var list = tbList.GetFieldOrProperty<RepeatedField<T>>("List");
                    if (list == null)
                    {
                        Error($"Can't Find tbList.List in {type.Name}");
                        return false;
                    }
                    list.Clear();
                    //根据dict重建list
                    //从TryAddToDictionary来看，Dict和List是严格一一对应的
                    foreach (var item in _dataDict!)
                        list.Add(item.Value);
                    Log($"Reset {type.Name} TBList Done: {list.Count}", 2);
                }
                return true;
            }
            catch (Exception e)
            {
                MyExten.Error($"Fail to Reset {typeof(T).Name}:{e.Message}");
                return false;
            }
        }
    }
}
