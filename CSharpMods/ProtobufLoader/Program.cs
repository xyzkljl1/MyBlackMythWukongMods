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
namespace ProtobufLoader
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
        public void Init()
        {
            Log("MyMod::Init called.Start Timer");
            //Utils.RegisterKeyBind(Key.ENTER, () => Console.WriteLine("Enter pressed"));
            Utils.RegisterKeyBind(ModifierKeys.Control, Key.F7, LoadAllDataFiles);
            //Utils.RegisterKeyBind(Key.O, delegate {});

            //initDescTimer.Start();
            //注意必须在GameThread执行，ToFTextFillPre/GetLocaliztionalFText等函数在Timer.Elapsed线程无法得到正确翻译，在RegisterKeyBind或Init或TryRunOnGameThread线程则可以
            //initDescTimer.Elapsed +=   (Object source, ElapsedEventArgs e) => Utils.TryRunOnGameThread(delegate { LoadAllDataFiles(); });
            // hook
            // harmony.PatchAll();
            LoadAllDataFiles();
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
            initDescTimer.Dispose();
            Log($"DeInit");
            // harmony.UnpatchAll();
        }
        public void LoadAllDataFiles()
        {
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
        }
        public bool LoadDataFilesInDir(string dir)
        {
            List<string> files= new List<string>();
            foreach (var f in Directory.GetFiles(dir, "*Desc*.data").ToList<string>())
                if (f.EndsWith(".bak.data"))
                    Log($"Ignore {f} because end with .bak");
                else
                    files.Add(f);
            files.Sort();
            Log($"Find {files.Count} data files.Start Load");
            int success_ct = 0;
            foreach (var file in files)
                if(LoadDataFile(file))
                    success_ct++;
            if (files.Count > 0)
                Log($"Load {success_ct}/{files.Count} successfully in {dir}");
            return success_ct == files.Count;//空文件夹算成功
        }
        public bool LoadNoneRuntimeDataImp<T>(string filepath,string filename,string typename) where T : class, Google.Protobuf.IMessage,new()
        {
            bool WithOutId = false;//决定载入到list还是dict里，根据BGW_GameDB.LoadRes中的参数设置
            if (typeof(T) == typeof(FUStCollectionSpawnInfoDesc))
                WithOutId = true;
            try
            {
                if (UGSE_FileFuncLib.LoadFileToArray(filepath, out var FileData))
                {
                    Log($"Start Load {filename} to {typename} Static Table");
                    MemoryStream memoryStream = new MemoryStream(FileData.ToArray());
                    string text = typeof(T).ToString();
                    text.IndexOf('.');
                    //Type.GetType不能直接获取到类型，需要指定Assembly
                    object obj = Activator.CreateInstance(typeof(T).Assembly.GetType(text.Insert(text.IndexOf('.') + 1, "TB"), throwOnError: true, ignoreCase: true));
                    object value = obj.GetType().GetProperty("Parser").GetValue(obj);
                    MethodInfo method = value.GetType().GetMethod("ParseFrom", new Type[1] { typeof(Stream) });
                    var _messageObject = method.Invoke(value, new object[1] { memoryStream });
                    object value2 = _messageObject.GetType().GetProperty("List").GetValue(_messageObject);
                    if (WithOutId)
                    {
                        var apiInstance = BG_ProtobufDataAPI<T>.Get();
                        var _dataList=apiInstance.GetFieldOrProperty<List<T>>("_dataList");
                        foreach (T item in value2 as IEnumerable<T>)
                        {
                            _dataList.Add(item);
                        }
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
                        foreach (T item2 in value2 as IEnumerable<T>)
                        {
                            //无视DataGuard
                            int? id = item2.GetType().GetProperty(idPropertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.GetValue(item2) as int?;
                            if(id==null)
                            {
                                Error($"Can't find id.Ignore item ");
                                continue;
                            }
                            if (_dataDict.ContainsKey((int)id))
                            {
                                Log($"Override {(int)id} in {typename}");
                                _dataDict[ (int)id ] = item2;
                            }
                            else
                            {
                                Log($"Add {(int)id} in {typename}");
                                _dataDict[(int)id] = item2;
                            }
                        }
                    }
                    Log($"{filename} Done");
                    return true;
                }
                else
                    Error($"Fail To Load File {filepath}");
            }
            catch (Exception e)
            {
                FMessage.OpenDialog("Handle protobuf data of " + filepath + " error!");
                USharpExceptionHandler.HandleException(e, EUSharpExceptionType.InvokeFunction);
            }
            return false;
        }
        public bool LoadDataFile(string filepath)
        {
            var fileInfo = new FileInfo(filepath);
            var filename = fileInfo.Name;
            var typename=filename.Substring(0,filename.IndexOf("Desc")+4);//GetFiles时已经确保能找到Desc
            //NonRuntime
            switch (typename)
            {
                //BG_ProtobufDataAPI<(.*)>[^\n\r]*
                //case "$1": return LoadNoneRuntimeDataImp<$1>(filepath, filename, typename);
                //BGW_GameDB.LoadRes
                case "FUStInteractiveUnitCommDesc": return LoadNoneRuntimeDataImp<FUStInteractiveUnitCommDesc>(filepath, filename, typename);
                case "FUStInteractionMappingDesc": return LoadNoneRuntimeDataImp<FUStInteractionMappingDesc>(filepath, filename, typename);
                case "FUStAiInteractionMappingDesc": return LoadNoneRuntimeDataImp<FUStAiInteractionMappingDesc>(filepath, filename, typename);
                case "FUStBuffDesc": return LoadNoneRuntimeDataImp<FUStBuffDesc>(filepath, filename, typename);
                case "FUStChargeSkillSDesc": return LoadNoneRuntimeDataImp<FUStChargeSkillSDesc>(filepath, filename, typename);
                case "FUStDropItemDesc": return LoadNoneRuntimeDataImp<FUStDropItemDesc>(filepath, filename, typename);
                case "FUStHitVEffectDesc": return LoadNoneRuntimeDataImp<FUStHitVEffectDesc>(filepath, filename, typename);
                case "FUStQTEDesc": return LoadNoneRuntimeDataImp<FUStQTEDesc>(filepath, filename, typename);
                case "FUStSummonCommDesc": return LoadNoneRuntimeDataImp<FUStSummonCommDesc>(filepath, filename, typename);
                case "FUStPhysicalHitBoneRuleDesc": return LoadNoneRuntimeDataImp<FUStPhysicalHitBoneRuleDesc>(filepath, filename, typename);
                case "FUStRebirthPointDesc": return LoadNoneRuntimeDataImp<FUStRebirthPointDesc>(filepath, filename, typename);
                case "FUStStraightCamDesc": return LoadNoneRuntimeDataImp<FUStStraightCamDesc>(filepath, filename, typename);
                case "FUStMultiPointLockCameraConfigDesc": return LoadNoneRuntimeDataImp<FUStMultiPointLockCameraConfigDesc>(filepath, filename, typename);
                case "FUStDiagonalCamDesc": return LoadNoneRuntimeDataImp<FUStDiagonalCamDesc>(filepath, filename, typename);
                case "FUStGiantLockCameraDesc": return LoadNoneRuntimeDataImp<FUStGiantLockCameraDesc>(filepath, filename, typename);
                case "FUStUnitCollisionHitMoveDesc": return LoadNoneRuntimeDataImp<FUStUnitCollisionHitMoveDesc>(filepath, filename, typename);
                case "FUStBuffDispDesc": return LoadNoneRuntimeDataImp<FUStBuffDispDesc>(filepath, filename, typename);
                case "FUStBuffLayerDispDesc": return LoadNoneRuntimeDataImp<FUStBuffLayerDispDesc>(filepath, filename, typename);
                case "FUStExAnimDataDesc": return LoadNoneRuntimeDataImp<FUStExAnimDataDesc>(filepath, filename, typename);
                case "FUStUnitCommDesc": return LoadNoneRuntimeDataImp<FUStUnitCommDesc>(filepath, filename, typename);
                case "FUStUnitBattleInfoExtendDesc": return LoadNoneRuntimeDataImp<FUStUnitBattleInfoExtendDesc>(filepath, filename, typename);
                case "FUStUnitPassiveSkillInfoExtendDesc": return LoadNoneRuntimeDataImp<FUStUnitPassiveSkillInfoExtendDesc>(filepath, filename, typename);
                case "FUStUnitEnvMaskConfigDesc": return LoadNoneRuntimeDataImp<FUStUnitEnvMaskConfigDesc>(filepath, filename, typename);
                case "FUStSkillAIDesc": return LoadNoneRuntimeDataImp<FUStSkillAIDesc>(filepath, filename, typename);
                case "FUStSkillEffectDesc": return LoadNoneRuntimeDataImp<FUStSkillEffectDesc>(filepath, filename, typename);
                case "FUStSkillSDesc": return LoadNoneRuntimeDataImp<FUStSkillSDesc>(filepath, filename, typename);
                case "FUStSkillSMappingDesc": return LoadNoneRuntimeDataImp<FUStSkillSMappingDesc>(filepath, filename, typename);
                case "FUStEnvironmentSwitchDesc": return LoadNoneRuntimeDataImp<FUStEnvironmentSwitchDesc>(filepath, filename, typename);
                case "FUStBulletSwitchDesc": return LoadNoneRuntimeDataImp<FUStBulletSwitchDesc>(filepath, filename, typename);
                case "FUStUnitAIDesc": return LoadNoneRuntimeDataImp<FUStUnitAIDesc>(filepath, filename, typename);
                case "FUStUnitTransCommDesc": return LoadNoneRuntimeDataImp<FUStUnitTransCommDesc>(filepath, filename, typename);
                case "FUStRollSkillDesc": return LoadNoneRuntimeDataImp<FUStRollSkillDesc>(filepath, filename, typename);
                case "FUStUnitDropDesc": return LoadNoneRuntimeDataImp<FUStUnitDropDesc>(filepath, filename, typename);
                case "FUStNianhuiNameListDesc": return LoadNoneRuntimeDataImp<FUStNianhuiNameListDesc>(filepath, filename, typename);
                case "FUStNianhuiAwardDesc": return LoadNoneRuntimeDataImp<FUStNianhuiAwardDesc>(filepath, filename, typename);
                case "FUStBeAttackedInfoDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedInfoDesc>(filepath, filename, typename);
                case "FUStBeAttackedDispInfoDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedDispInfoDesc>(filepath, filename, typename);
                case "FUStScarInfoDesc": return LoadNoneRuntimeDataImp<FUStScarInfoDesc>(filepath, filename, typename);
                case "FUStPlayerSkillCtrlDesc": return LoadNoneRuntimeDataImp<FUStPlayerSkillCtrlDesc>(filepath, filename, typename);
                case "FUStOverlyingSkillSDesc": return LoadNoneRuntimeDataImp<FUStOverlyingSkillSDesc>(filepath, filename, typename);
                case "FUStBuffGroupDesc": return LoadNoneRuntimeDataImp<FUStBuffGroupDesc>(filepath, filename, typename);
                case "FUStBuffRuleDesc": return LoadNoneRuntimeDataImp<FUStBuffRuleDesc>(filepath, filename, typename);
                case "FUStMandatoryAITaskDesc": return LoadNoneRuntimeDataImp<FUStMandatoryAITaskDesc>(filepath, filename, typename);
                case "FUStAttackHitAudioInfoDesc": return LoadNoneRuntimeDataImp<FUStAttackHitAudioInfoDesc>(filepath, filename, typename);
                case "FUStPlayerCameraDesc": return LoadNoneRuntimeDataImp<FUStPlayerCameraDesc>(filepath, filename, typename);
                case "FUStPlayerTransAttrDesc": return LoadNoneRuntimeDataImp<FUStPlayerTransAttrDesc>(filepath, filename, typename);
                case "FUStAttackHitFXMapDesc": return LoadNoneRuntimeDataImp<FUStAttackHitFXMapDesc>(filepath, filename, typename);
                case "FUStBeAttackedFXMapDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedFXMapDesc>(filepath, filename, typename);
                case "FUStPlayerCommDesc": return LoadNoneRuntimeDataImp<FUStPlayerCommDesc>(filepath, filename, typename);
                case "FUStUnitDeadDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadDesc>(filepath, filename, typename);
                case "FUStUnitDeadSwitchToPhysicDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadSwitchToPhysicDesc>(filepath, filename, typename);
                case "FUStUnitDeadOldDesc": return LoadNoneRuntimeDataImp<FUStUnitDeadOldDesc>(filepath, filename, typename);
                case "FUStHitSceneItemPerformDesc": return LoadNoneRuntimeDataImp<FUStHitSceneItemPerformDesc>(filepath, filename, typename);
                case "FUStUnitFootstepDesc": return LoadNoneRuntimeDataImp<FUStUnitFootstepDesc>(filepath, filename, typename);
                case "FUStEQSSettingDesc": return LoadNoneRuntimeDataImp<FUStEQSSettingDesc>(filepath, filename, typename);
                case "FUStPassiveSkillDesc": return LoadNoneRuntimeDataImp<FUStPassiveSkillDesc>(filepath, filename, typename);
                case "FUStAISkillBasicActionDesc": return LoadNoneRuntimeDataImp<FUStAISkillBasicActionDesc>(filepath, filename, typename);
                case "FUStUnitAudioBankMapDesc": return LoadNoneRuntimeDataImp<FUStUnitAudioBankMapDesc>(filepath, filename, typename);
                case "FUStAISkillTagsDesc": return LoadNoneRuntimeDataImp<FUStAISkillTagsDesc>(filepath, filename, typename);
                case "FUStAIThinkDesc": return LoadNoneRuntimeDataImp<FUStAIThinkDesc>(filepath, filename, typename);
                case "FUStAIActionDesc": return LoadNoneRuntimeDataImp<FUStAIActionDesc>(filepath, filename, typename);
                case "FUStAIFeatureDesc": return LoadNoneRuntimeDataImp<FUStAIFeatureDesc>(filepath, filename, typename);
                case "FUStProjectileCommDesc": return LoadNoneRuntimeDataImp<FUStProjectileCommDesc>(filepath, filename, typename);
                case "FUStProjectileDispDesc": return LoadNoneRuntimeDataImp<FUStProjectileDispDesc>(filepath, filename, typename);
                case "FUStProjectileMoveDesc": return LoadNoneRuntimeDataImp<FUStProjectileMoveDesc>(filepath, filename, typename);
                case "FUStBulletExpandDesc": return LoadNoneRuntimeDataImp<FUStBulletExpandDesc>(filepath, filename, typename);
                case "FUStMagicFieldExpandDesc": return LoadNoneRuntimeDataImp<FUStMagicFieldExpandDesc>(filepath, filename, typename);
                case "FUStBulletWindFieldExpandDesc": return LoadNoneRuntimeDataImp<FUStBulletWindFieldExpandDesc>(filepath, filename, typename);
                case "FUStUnitSpecialMoveDesc": return LoadNoneRuntimeDataImp<FUStUnitSpecialMoveDesc>(filepath, filename, typename);
                case "FUStUnitLevelUpDesc": return LoadNoneRuntimeDataImp<FUStUnitLevelUpDesc>(filepath, filename, typename);
                case "FUStPartRuleInfoDesc": return LoadNoneRuntimeDataImp<FUStPartRuleInfoDesc>(filepath, filename, typename);
                case "FUStAttackerHitFXMappingDesc": return LoadNoneRuntimeDataImp<FUStAttackerHitFXMappingDesc>(filepath, filename, typename);
                case "FUStAttackerHitAudioEventMappingDesc": return LoadNoneRuntimeDataImp<FUStAttackerHitAudioEventMappingDesc>(filepath, filename, typename);
                case "FUStBuffTickRuleBySimpleStateDesc": return LoadNoneRuntimeDataImp<FUStBuffTickRuleBySimpleStateDesc>(filepath, filename, typename);
                case "FUStUnitPhysicalAnimationDesc": return LoadNoneRuntimeDataImp<FUStUnitPhysicalAnimationDesc>(filepath, filename, typename);
                case "FUStMovieSequenceDesc": return LoadNoneRuntimeDataImp<FUStMovieSequenceDesc>(filepath, filename, typename);
                case "FUStSuitDesc": return LoadNoneRuntimeDataImp<FUStSuitDesc>(filepath, filename, typename);
                case "FUStCameraGroupDesc": return LoadNoneRuntimeDataImp<FUStCameraGroupDesc>(filepath, filename, typename);
                case "FUStFixFunctionDesc": return LoadNoneRuntimeDataImp<FUStFixFunctionDesc>(filepath, filename, typename);
                case "FUStGroupAISDesc": return LoadNoneRuntimeDataImp<FUStGroupAISDesc>(filepath, filename, typename);
                case "FUStDialogueDesc": return LoadNoneRuntimeDataImp<FUStDialogueDesc>(filepath, filename, typename);
                case "FUStDialogueIDMappingDesc": return LoadNoneRuntimeDataImp<FUStDialogueIDMappingDesc>(filepath, filename, typename);
                case "FUStUIWordDesc": return LoadNoneRuntimeDataImp<FUStUIWordDesc>(filepath, filename, typename);
                case "FUStTalentDisplayDesc": return LoadNoneRuntimeDataImp<FUStTalentDisplayDesc>(filepath, filename, typename);
                case "FUStTalentLvUpCfgDesc": return LoadNoneRuntimeDataImp<FUStTalentLvUpCfgDesc>(filepath, filename, typename);
                case "FUStAiConversationEventDesc": return LoadNoneRuntimeDataImp<FUStAiConversationEventDesc>(filepath, filename, typename);
                case "FUStAiConversationContentDesc": return LoadNoneRuntimeDataImp<FUStAiConversationContentDesc>(filepath, filename, typename);
                case "FUStOnlineScreenMsgConfDesc": return LoadNoneRuntimeDataImp<FUStOnlineScreenMsgConfDesc>(filepath, filename, typename);
                case "FUStRichTextIconDesc": return LoadNoneRuntimeDataImp<FUStRichTextIconDesc>(filepath, filename, typename);
                case "FUStIronBodyConfigDesc": return LoadNoneRuntimeDataImp<FUStIronBodyConfigDesc>(filepath, filename, typename);
                case "FUStImmobilizeSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStImmobilizeSkillConfigDesc>(filepath, filename, typename);
                case "FUStSealingSpellSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStSealingSpellSkillConfigDesc>(filepath, filename, typename);
                case "FUStTransQiTianDaShengConfigDesc": return LoadNoneRuntimeDataImp<FUStTransQiTianDaShengConfigDesc>(filepath, filename, typename);
                case "FUStPhantomRushSkillConfigDesc": return LoadNoneRuntimeDataImp<FUStPhantomRushSkillConfigDesc>(filepath, filename, typename);
                case "FUStPlayerInputSkillMappingDesc": return LoadNoneRuntimeDataImp<FUStPlayerInputSkillMappingDesc>(filepath, filename, typename);
                case "FUStTaskStageDesc": return LoadNoneRuntimeDataImp<FUStTaskStageDesc>(filepath, filename, typename);
                case "FUStTaskLineDesc": return LoadNoneRuntimeDataImp<FUStTaskLineDesc>(filepath, filename, typename);
                case "FUStSuperArmorLevelDesc": return LoadNoneRuntimeDataImp<FUStSuperArmorLevelDesc>(filepath, filename, typename);
                case "FUStCollectionSpawnInfoDesc": return LoadNoneRuntimeDataImp<FUStCollectionSpawnInfoDesc>(filepath, filename, typename);
                case "FUStCollectionSpawnGroupDesc": return LoadNoneRuntimeDataImp<FUStCollectionSpawnGroupDesc>(filepath, filename, typename);
                case "FUStCollectionEventProbabilityDesc": return LoadNoneRuntimeDataImp<FUStCollectionEventProbabilityDesc>(filepath, filename, typename);
                case "FUStCustomStateMachineDesc": return LoadNoneRuntimeDataImp<FUStCustomStateMachineDesc>(filepath, filename, typename);
                case "FUStGuideAssetConfigDesc": return LoadNoneRuntimeDataImp<FUStGuideAssetConfigDesc>(filepath, filename, typename);
                case "FUStNPCBaseInfoDesc": return LoadNoneRuntimeDataImp<FUStNPCBaseInfoDesc>(filepath, filename, typename);
                case "FUStEnhancedInputActionDesc": return LoadNoneRuntimeDataImp<FUStEnhancedInputActionDesc>(filepath, filename, typename);
                case "FUStSkillDamageExpandDesc": return LoadNoneRuntimeDataImp<FUStSkillDamageExpandDesc>(filepath, filename, typename);
                case "FUStPotentialEnergyConfigDesc": return LoadNoneRuntimeDataImp<FUStPotentialEnergyConfigDesc>(filepath, filename, typename);
                case "FUStGlobalConfigDesc": return LoadNoneRuntimeDataImp<FUStGlobalConfigDesc>(filepath, filename, typename);
                case "FUStTeamRelationConfigDesc": return LoadNoneRuntimeDataImp<FUStTeamRelationConfigDesc>(filepath, filename, typename);
                case "FUStAiConversationGroupDesc": return LoadNoneRuntimeDataImp<FUStAiConversationGroupDesc>(filepath, filename, typename);
                case "FUStAssociationUnitInfoSDesc": return LoadNoneRuntimeDataImp<FUStAssociationUnitInfoSDesc>(filepath, filename, typename);
                case "FUStUnitIntelligenceInfoDesc": return LoadNoneRuntimeDataImp<FUStUnitIntelligenceInfoDesc>(filepath, filename, typename);
                case "FUStCBGTemplateDesc": return LoadNoneRuntimeDataImp<FUStCBGTemplateDesc>(filepath, filename, typename);
                case "FUStTamerStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStTamerStrategyConfigDesc>(filepath, filename, typename);
                case "FUStTROStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStTROStrategyConfigDesc>(filepath, filename, typename);
                case "FUStSummonCopySkillDesc": return LoadNoneRuntimeDataImp<FUStSummonCopySkillDesc>(filepath, filename, typename);
                case "FUStUnitChangeMaterialByAttrDesc": return LoadNoneRuntimeDataImp<FUStUnitChangeMaterialByAttrDesc>(filepath, filename, typename);
                case "FUStCCGCastSkillMappingRuleDesc": return LoadNoneRuntimeDataImp<FUStCCGCastSkillMappingRuleDesc>(filepath, filename, typename);
                case "FUStWeakPerformConfigDesc": return LoadNoneRuntimeDataImp<FUStWeakPerformConfigDesc>(filepath, filename, typename);
                case "FUStFollowPartnerConfigDesc": return LoadNoneRuntimeDataImp<FUStFollowPartnerConfigDesc>(filepath, filename, typename);
                case "FUStChallengeDesc": return LoadNoneRuntimeDataImp<FUStChallengeDesc>(filepath, filename, typename);
                case "FUStBossRoomConfigDesc": return LoadNoneRuntimeDataImp<FUStBossRoomConfigDesc>(filepath, filename, typename);
                case "FUStGlobalCannotDeadExtraConfigDesc": return LoadNoneRuntimeDataImp<FUStGlobalCannotDeadExtraConfigDesc>(filepath, filename, typename);
                case "FUStEnvironmentSurfaceEffectDesc": return LoadNoneRuntimeDataImp<FUStEnvironmentSurfaceEffectDesc>(filepath, filename, typename);
                case "FUStSweepCheckDesc": return LoadNoneRuntimeDataImp<FUStSweepCheckDesc>(filepath, filename, typename);
                case "FUStAudioExtendDesc": return LoadNoneRuntimeDataImp<FUStAudioExtendDesc>(filepath, filename, typename);
                case "FUStBuffIconDesc": return LoadNoneRuntimeDataImp<FUStBuffIconDesc>(filepath, filename, typename);
                case "FUStPartHitExpandDesc": return LoadNoneRuntimeDataImp<FUStPartHitExpandDesc>(filepath, filename, typename);
                case "FUStDetonateConfigDesc": return LoadNoneRuntimeDataImp<FUStDetonateConfigDesc>(filepath, filename, typename);
                case "FUStAttachedNiagaraByHitDesc": return LoadNoneRuntimeDataImp<FUStAttachedNiagaraByHitDesc>(filepath, filename, typename);
                case "FUStAbnormalStateUIBlackListDesc": return LoadNoneRuntimeDataImp<FUStAbnormalStateUIBlackListDesc>(filepath, filename, typename);
                case "FUStElementDmgRatioLevelDesc": return LoadNoneRuntimeDataImp<FUStElementDmgRatioLevelDesc>(filepath, filename, typename);
                case "FUStAbnormalCommConfigDesc": return LoadNoneRuntimeDataImp<FUStAbnormalCommConfigDesc>(filepath, filename, typename);
                case "FUStStreamingLevelStateDesc": return LoadNoneRuntimeDataImp<FUStStreamingLevelStateDesc>(filepath, filename, typename);
                case "FUStMapSymbolDesc": return LoadNoneRuntimeDataImp<FUStMapSymbolDesc>(filepath, filename, typename);
                case "FUStAttrCopyConfigDesc": return LoadNoneRuntimeDataImp<FUStAttrCopyConfigDesc>(filepath, filename, typename);
                case "FUStPlayerTransUnitConfDesc": return LoadNoneRuntimeDataImp<FUStPlayerTransUnitConfDesc>(filepath, filename, typename);
                case "FUStLifeSavingHairConfigDesc": return LoadNoneRuntimeDataImp<FUStLifeSavingHairConfigDesc>(filepath, filename, typename);
                case "FUStPigsyStoryLibraryDesc": return LoadNoneRuntimeDataImp<FUStPigsyStoryLibraryDesc>(filepath, filename, typename);
                case "FUStPigsyStoryIAndRLibraryDesc": return LoadNoneRuntimeDataImp<FUStPigsyStoryIAndRLibraryDesc>(filepath, filename, typename);
                case "FUStGuideGroupDesc": return LoadNoneRuntimeDataImp<FUStGuideGroupDesc>(filepath, filename, typename);
                case "FUStGuideNodeDesc": return LoadNoneRuntimeDataImp<FUStGuideNodeDesc>(filepath, filename, typename);
                case "FUStDynamicObstaclePerformanceDesc": return LoadNoneRuntimeDataImp<FUStDynamicObstaclePerformanceDesc>(filepath, filename, typename);
                case "FUStDefeatSlowTimeConfigDesc": return LoadNoneRuntimeDataImp<FUStDefeatSlowTimeConfigDesc>(filepath, filename, typename);
                case "FUStBuffDispGroupDesc": return LoadNoneRuntimeDataImp<FUStBuffDispGroupDesc>(filepath, filename, typename);
                case "FUStSoulSkillMimicryDesc": return LoadNoneRuntimeDataImp<FUStSoulSkillMimicryDesc>(filepath, filename, typename);
                case "FUStCameraConversionParamConfigDesc": return LoadNoneRuntimeDataImp<FUStCameraConversionParamConfigDesc>(filepath, filename, typename);
                case "FUStEffectiveHitProjectileEffectDesc": return LoadNoneRuntimeDataImp<FUStEffectiveHitProjectileEffectDesc>(filepath, filename, typename);
                case "FUStTransActiveStateDesc": return LoadNoneRuntimeDataImp<FUStTransActiveStateDesc>(filepath, filename, typename);
                case "FUStMovementOptStrategyConfigDesc": return LoadNoneRuntimeDataImp<FUStMovementOptStrategyConfigDesc>(filepath, filename, typename);
                case "FUStAbnormalDispAttackerMapDesc": return LoadNoneRuntimeDataImp<FUStAbnormalDispAttackerMapDesc>(filepath, filename, typename);
                case "FUStAbnormalDispVictimMapDesc": return LoadNoneRuntimeDataImp<FUStAbnormalDispVictimMapDesc>(filepath, filename, typename);
                case "FUStAICrowdDetourLevelConfigDesc": return LoadNoneRuntimeDataImp<FUStAICrowdDetourLevelConfigDesc>(filepath, filename, typename);
                case "FUStRebirthAreaDesc": return LoadNoneRuntimeDataImp<FUStRebirthAreaDesc>(filepath, filename, typename);
                case "FUStLevelSequenceClearBattleItemConfigDesc": return LoadNoneRuntimeDataImp<FUStLevelSequenceClearBattleItemConfigDesc>(filepath, filename, typename);
                case "FUStBeAttackedStiffLevelMappingDesc": return LoadNoneRuntimeDataImp<FUStBeAttackedStiffLevelMappingDesc>(filepath, filename, typename);
                case "FUStAkEventMarkerDesc": return LoadNoneRuntimeDataImp<FUStAkEventMarkerDesc>(filepath, filename, typename);
                case "FUStSeqAudioJumpLengthDesc": return LoadNoneRuntimeDataImp<FUStSeqAudioJumpLengthDesc>(filepath, filename, typename);
                case "FUStDeadSeqUnitConfigDesc": return LoadNoneRuntimeDataImp<FUStDeadSeqUnitConfigDesc>(filepath, filename, typename);
                default:break;
            }
            //Runtime,根据Runtime文件夹下的所有文件生成
            switch (typename)
            {
                case "AchievementDesc": return LoadRuntimeDataImp<TBAchievementDesc, AchievementDesc>(filepath,filename);
                case "AlchemyOutputDesc": return LoadRuntimeDataImp<TBAlchemyOutputDesc, AlchemyOutputDesc>(filepath,filename);
                case "ArmorEnhanceConsumeDesc": return LoadRuntimeDataImp<TBArmorEnhanceConsumeDesc, ArmorEnhanceConsumeDesc>(filepath,filename);
                case "ArmorEnhanceDesc": return LoadRuntimeDataImp<TBArmorEnhanceDesc, ArmorEnhanceDesc>(filepath,filename);
                case "ArtBookDesc": return LoadRuntimeDataImp<TBArtBookDesc, ArtBookDesc>(filepath,filename);
                case "AttrItemDesc": return LoadRuntimeDataImp<TBAttrItemDesc, AttrItemDesc>(filepath,filename);
                case "BloodHudDesc": return LoadRuntimeDataImp<TBBloodHudDesc, BloodHudDesc>(filepath,filename);
                case "CardDesc": return LoadRuntimeDataImp<TBCardDesc, CardDesc>(filepath,filename);
                case "ChapterDesc": return LoadRuntimeDataImp<TBChapterDesc, ChapterDesc>(filepath,filename);
                case "CollectionDropDesc": return LoadRuntimeDataImp<TBCollectionDropDesc, CollectionDropDesc>(filepath,filename);
                //case "CombatSkillDesc": return LoadRuntimeDataImp<TBCombatSkillDesc, CombatSkillDesc>(filepath,filename);
                case "CommDropRuleDesc": return LoadRuntimeDataImp<TBCommDropRuleDesc, CommDropRuleDesc>(filepath,filename);
                case "CommLogicCfgDesc": return LoadRuntimeDataImp<TBCommLogicCfgDesc, CommLogicCfgDesc>(filepath,filename);
                case "CommonErrorUITipsDesc": return LoadRuntimeDataImp<TBCommonErrorUITipsDesc, CommonErrorUITipsDesc>(filepath,filename);
                case "ConsumeDesc": return LoadRuntimeDataImp<TBConsumeDesc, ConsumeDesc>(filepath,filename);
                case "CricketBattleUnitDesc": return LoadRuntimeDataImp<TBCricketBattleUnitDesc, CricketBattleUnitDesc>(filepath,filename);
                case "CricketUnitAttrDesc": return LoadRuntimeDataImp<TBCricketUnitAttrDesc, CricketUnitAttrDesc>(filepath,filename);
                case "DestructionDropDesc": return LoadRuntimeDataImp<TBDestructionDropDesc, DestructionDropDesc>(filepath,filename);
                case "EchoDesc": return LoadRuntimeDataImp<TBEchoDesc, EchoDesc>(filepath,filename);
                case "EditionAwardDesc": return LoadRuntimeDataImp<TBEditionAwardDesc, EditionAwardDesc>(filepath,filename);
                case "EquipAttrDesc": return LoadRuntimeDataImp<TBEquipAttrDesc, EquipAttrDesc>(filepath,filename);
                case "EquipDesc": return LoadRuntimeDataImp<TBEquipDesc, EquipDesc>(filepath,filename);
                case "EquipFaBaoAttrDesc": return LoadRuntimeDataImp<TBEquipFaBaoAttrDesc, EquipFaBaoAttrDesc>(filepath,filename);
                case "EquipPositionConfDesc": return LoadRuntimeDataImp<TBEquipPositionConfDesc, EquipPositionConfDesc>(filepath,filename);
                case "EquipSeriesDesc": return LoadRuntimeDataImp<TBEquipSeriesDesc, EquipSeriesDesc>(filepath,filename);
                case "GMMonsterTeleportDesc": return LoadRuntimeDataImp<TBGMMonsterTeleportDesc, GMMonsterTeleportDesc>(filepath,filename);
                case "HistoricDesc": return LoadRuntimeDataImp<TBHistoricDesc, HistoricDesc>(filepath,filename);
                case "HuluDesc": return LoadRuntimeDataImp<TBHuluDesc, HuluDesc>(filepath,filename);
                case "IncreaseConfigDesc": return LoadRuntimeDataImp<TBIncreaseConfigDesc, IncreaseConfigDesc>(filepath,filename);
                case "InteractionFuncDesc": return LoadRuntimeDataImp<TBInteractionFuncDesc, InteractionFuncDesc>(filepath,filename);
                case "ItemDesc": return LoadRuntimeDataImp<TBItemDesc, ItemDesc>(filepath,filename);
                case "ItemRecipeDesc": return LoadRuntimeDataImp<TBItemRecipeDesc, ItemRecipeDesc>(filepath,filename);
                case "LevelDesc": return LoadRuntimeDataImp<TBLevelDesc, LevelDesc>(filepath,filename);
                case "LinkBloodDesc": return LoadRuntimeDataImp<TBLinkBloodDesc, LinkBloodDesc>(filepath,filename);
                case "LoadingTipsDesc": return LoadRuntimeDataImp<TBLoadingTipsDesc, LoadingTipsDesc>(filepath,filename);
                case "LoadingTipsWeightDesc": return LoadRuntimeDataImp<TBLoadingTipsWeightDesc, LoadingTipsWeightDesc>(filepath,filename);
                case "LockMantraDesc": return LoadRuntimeDataImp<TBLockMantraDesc, LockMantraDesc>(filepath,filename);
                case "LotteryAwardDesc": return LoadRuntimeDataImp<TBLotteryAwardDesc, LotteryAwardDesc>(filepath,filename);
                case "MantraBuildupDesc": return LoadRuntimeDataImp<TBMantraBuildupDesc, MantraBuildupDesc>(filepath,filename);
                case "MantraDesc": return LoadRuntimeDataImp<TBMantraDesc, MantraDesc>(filepath,filename);
                case "MantraWeightDesc": return LoadRuntimeDataImp<TBMantraWeightDesc, MantraWeightDesc>(filepath,filename);
                case "MapAreaConfigDesc": return LoadRuntimeDataImp<TBMapAreaConfigDesc, MapAreaConfigDesc>(filepath,filename);
                case "MapFragmentDesc": return LoadRuntimeDataImp<TBMapFragmentDesc, MapFragmentDesc>(filepath,filename);
                case "MedicineAwardDesc": return LoadRuntimeDataImp<TBMedicineAwardDesc, MedicineAwardDesc>(filepath,filename);
                case "MeditationPointDesc": return LoadRuntimeDataImp<TBMeditationPointDesc, MeditationPointDesc>(filepath,filename);
                case "MovieAndSubtitleDesc": return LoadRuntimeDataImp<TBMovieAndSubtitleDesc, MovieAndSubtitleDesc>(filepath,filename);
                case "MultiplayerDropRuleDesc": return LoadRuntimeDataImp<TBMultiplayerDropRuleDesc, MultiplayerDropRuleDesc>(filepath,filename);
                case "MuseumMVDesc": return LoadRuntimeDataImp<TBMuseumMVDesc, MuseumMVDesc>(filepath,filename);
                case "NewGamePlusDesc": return LoadRuntimeDataImp<TBNewGamePlusDesc, NewGamePlusDesc>(filepath,filename);
                case "NPCInteractConversationDesc": return LoadRuntimeDataImp<TBNPCInteractConversationDesc, NPCInteractConversationDesc>(filepath,filename);
                case "PastMemoryDesc": return LoadRuntimeDataImp<TBPastMemoryDesc, PastMemoryDesc>(filepath,filename);
                case "PlatformAchievementDesc": return LoadRuntimeDataImp<TBPlatformAchievementDesc, PlatformAchievementDesc>(filepath,filename);
                case "PlatformAchievementLiteDesc": return LoadRuntimeDataImp<TBPlatformAchievementLiteDesc, PlatformAchievementLiteDesc>(filepath,filename);
                case "PlayerLevelDesc": return LoadRuntimeDataImp<TBPlayerLevelDesc, PlayerLevelDesc>(filepath,filename);
                //case "ActivityDesc": return LoadRuntimeDataImp<TBActivityDesc, ActivityDesc>(filepath,filename);
                //case "ActivityTaskDesc": return LoadRuntimeDataImp<TBActivityTaskDesc, ActivityTaskDesc>(filepath,filename);
                case "RoleDataConfigDesc": return LoadRuntimeDataImp<TBRoleDataConfigDesc, RoleDataConfigDesc>(filepath,filename);
                case "SceneMonsterNameplateDesc": return LoadRuntimeDataImp<TBSceneMonsterNameplateDesc, SceneMonsterNameplateDesc>(filepath,filename);
                //case "ScrollDesc": return LoadRuntimeDataImp<TBScrollDesc, ScrollDesc>(filepath,filename);
                case "SeedCollectionAwardDesc": return LoadRuntimeDataImp<TBSeedCollectionAwardDesc, SeedCollectionAwardDesc>(filepath,filename);
                case "SeedDesc": return LoadRuntimeDataImp<TBSeedDesc, SeedDesc>(filepath,filename);
                case "ShopDesc": return LoadRuntimeDataImp<TBShopDesc, ShopDesc>(filepath,filename);
                case "ShopItemDesc": return LoadRuntimeDataImp<TBShopItemDesc, ShopItemDesc>(filepath,filename);
                case "ShopItemGroupDesc": return LoadRuntimeDataImp<TBShopItemGroupDesc, ShopItemGroupDesc>(filepath,filename);
                case "ShopRefreshDesc": return LoadRuntimeDataImp<TBShopRefreshDesc, ShopRefreshDesc>(filepath,filename);
                case "ShrineShowNpcConfigDesc": return LoadRuntimeDataImp<TBShrineShowNpcConfigDesc, ShrineShowNpcConfigDesc>(filepath,filename);
                case "SoulSkillDesc": return LoadRuntimeDataImp<TBSoulSkillDesc, SoulSkillDesc>(filepath,filename);
                case "SoulSkillDropDesc": return LoadRuntimeDataImp<TBSoulSkillDropDesc, SoulSkillDropDesc>(filepath,filename);
                case "SoundTrackDesc": return LoadRuntimeDataImp<TBSoundTrackDesc, SoundTrackDesc>(filepath,filename);
                case "SpellDesc": return LoadRuntimeDataImp<TBSpellDesc, SpellDesc>(filepath,filename);
                case "SurpriseDesc": return LoadRuntimeDataImp<TBSurpriseDesc, SurpriseDesc>(filepath,filename);
                case "TakePhotoCustomSettingDesc": return LoadRuntimeDataImp<TBTakePhotoCustomSettingDesc, TakePhotoCustomSettingDesc>(filepath,filename);
                case "TalentRankDesc": return LoadRuntimeDataImp<TBTalentRankDesc, TalentRankDesc>(filepath,filename);
                case "TalentSDesc": return LoadRuntimeDataImp<TBTalentSDesc, TalentSDesc>(filepath,filename);
                case "TeamConfigDesc": return LoadRuntimeDataImp<TBTeamConfigDesc, TeamConfigDesc>(filepath,filename);
                case "TransInputUITipsDesc": return LoadRuntimeDataImp<TBTransInputUITipsDesc, TransInputUITipsDesc>(filepath,filename);
                case "UISettingConfigDesc": return LoadRuntimeDataImp<TBUISettingConfigDesc, UISettingConfigDesc>(filepath,filename);
                //case "UISettingControlDesc": return LoadRuntimeDataImp<TBUISettingControlDesc, UISettingControlDesc>(filepath,filename);
                case "UISettingDeviceConfigDesc": return LoadRuntimeDataImp<TBUISettingDeviceConfigDesc, UISettingDeviceConfigDesc>(filepath,filename);
                case "UnitDropNumDesc": return LoadRuntimeDataImp<TBUnitDropNumDesc, UnitDropNumDesc>(filepath,filename);
                case "UnitDropRuleDesc": return LoadRuntimeDataImp<TBUnitDropRuleDesc, UnitDropRuleDesc>(filepath,filename);
                case "WeaponBuildDesc": return LoadRuntimeDataImp<TBWeaponBuildDesc, WeaponBuildDesc>(filepath,filename);
                case "WineDesc": return LoadRuntimeDataImp<TBWineDesc, WineDesc>(filepath,filename);
            }
            Error($"Not Supported Table {typename}");
            return false;
        }
        public bool LoadRuntimeDataImp<TB,T>(string filepath,string filename) where TB : Google.Protobuf.IMessage, new() where T : Google.Protobuf.IMessage
        {
            if (UGSE_FileFuncLib.LoadFileToArray(filepath, out var FileData))
            {
                Log($"Start Load {filename} to {typeof(T).Name} Dynamic Table");
                MemoryStream input = new MemoryStream(FileData.ToArray());
                var typename = typeof(T).Name;

                var apiInstance = GSProtobufRuntimeAPI<TB, T>.Get();
                var _dataDict = apiInstance.GetFieldOrProperty<Dictionary<int, T>>("_dataDict");
                if (_dataDict == null) 
                {
                    Error($"Can't Find dataDict in {typeof(TB).Name}-{typename}");
                    return false;
                }
                var idPropertyName = apiInstance.GetFieldOrProperty<string>("_propertyID");
                var _messageTBList = new TB();
                _messageTBList.MergeFrom(input);
                if (idPropertyName != "")
                    foreach (T item in typeof(TB).GetProperty("List").GetValue(_messageTBList) as IEnumerable<T>)
                    {
                        object? _id = item.GetType().GetProperty(idPropertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public)?.GetValue(item);
                        if (_id == null)
                        {
                            Error($"Can't find id.Ignore item ");
                            continue;
                        }
                        int id = Convert.ToInt32(_id);
                        if (_dataDict.ContainsKey((int)id))
                        {
                            Log($"Override {(int)id} in {typename}");
                            _dataDict[(int)id] = item;
                        }
                        else
                        {
                            Log($"Add {(int)id} in {typename}");
                            _dataDict[(int)id] = item;
                        }
                    }
                else
                {
                    Error($"Can't find id property name for type {typename}");
                    return false;
                }
                return true;
            }
            else
                Error($"Can't Load File{filename}");
            return false;
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
            LoadAllDataFiles();
        }
    }
}
