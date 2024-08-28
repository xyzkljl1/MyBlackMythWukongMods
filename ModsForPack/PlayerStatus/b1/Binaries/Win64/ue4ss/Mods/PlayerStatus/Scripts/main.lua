local ModName="[PlayerStatus] "
--author:xyzkljl1
local config = require("playerstatus-config")
local FuncLib=nil
local MyMod=nil
local player=nil
local inited=false
local enabled=true
local TextsWidgetInited=true
local interval=math.floor(1000/config.freq)
local TextsWidget=nil

local isTrans=false

local AttrEnumMap={}
local HPID=0
local HPMAXID=0
local MPID=0
local MPMAXID=0
local STID=0
local STMAXID=0
local FPID=0
local FPMAXID=0
local SPID=0
local SPMAXID=0
local TRID=0
local TRMAXID=0
local ENID=0
local ENMAXID=0

RegisterHook("/Script/Engine.PlayerController:ClientRestart", function(Context,pawn)
    local pawnname=pawn:get():GetFullName()
    if pawnname:find("DefaultEmptyPawn_C") then-- trash when start game
        return
    elseif pawnname:find("Unit_Player_Wukong_C") then -- player
        player=Context:get()
        isTrans=false
        print(ModName.."IsPlayer.....")
    elseif pawnname:find("Unit_player") then-- transformation eg.Unit_player_gycy_lang_C
        player=Context:get()
        isTrans=true
        print(ModName.."IsTrans.....")
    else
        print(ModName.."Unknown Pawn:"..pawnname)
        return --Unknown
    end
    --print(tostring(Context:get():GetFullName()))
    --print(tostring(pawn:get():GetFullName()))
    ExecuteWithDelay(3000,Init)
end)

function Init()
    if not inited then
        FuncLib=StaticFindObject("/Script/b1-Managed.Default__BGUFunctionLibraryCS")
        if not FuncLib:IsValid() then return end
        enum=StaticFindObject("/Script/GSE-ProtobufDB.EBGUAttrFloat")
        AttrEnumMap={}
        if not enum:IsValid() then return end
        enum:ForEachName(function(name,value)
            AttrEnumMap[name:ToString()]=value
        end)
        HPID=AttrEnumMap["EBGUAttrFloat::Hp"]
        HPMAXID=AttrEnumMap["EBGUAttrFloat::HpMax"]
        MPID=AttrEnumMap["EBGUAttrFloat::Mp"]
        MPMAXID=AttrEnumMap["EBGUAttrFloat::MpMax"]
        STID=AttrEnumMap["EBGUAttrFloat::Stamina"]
        STMAXID=AttrEnumMap["EBGUAttrFloat::StaminaMax"]
        FPID=AttrEnumMap["EBGUAttrFloat::Pevalue"]
        FPMAXID=AttrEnumMap["EBGUAttrFloat::PevalueMax"]
        SPID=AttrEnumMap["EBGUAttrFloat::VigorEnergy"]
        SPMAXID=AttrEnumMap["EBGUAttrFloat::VigorEnergyMax"]
        TRID=AttrEnumMap["EBGUAttrFloat::FabaoEnergy"]
        TRMAXID=AttrEnumMap["EBGUAttrFloat::FabaoEnergyMax"]
        ENID=AttrEnumMap["EBGUAttrFloat::CurEnergy"]
        ENMAXID=AttrEnumMap["EBGUAttrFloat::TransEnergyMax"]
        inited=true
        print(ModName.."Init Once")
    end
    FindMyMod()
    TextsWidgetInited=false
    print(ModName.."Reset")
    --setfontandcolor/settext causes crash.Why????
    --MyMod:SetFontColor(config.fontsize,config.color[1],config.color[2],config.color[3],config.color[4])
end

local findBattleMainInterval=5000
function CreateWidget()
    if not ParentSetted then
        if findBattleMainInterval<=0 then
            findBattleMainInterval=1000

            TextsWidget=FindFirstOf("TextsWidget_C")
            if (not TextsWidget:IsValid()) then                
                MyMod:InitWidget(nil)
                TextsWidget=FindFirstOf("TextsWidget_C")
                MyMod:SetPosition(TextsWidget,config.hp[1],config.hp[2],
                        config.mp[1],config.mp[2],
                        config.st[1],config.st[2],
                        config.fp[1],config.fp[2],
                        config.sp[1],config.sp[2],
                        config.tr[1],config.tr[2],
                        config.en[1],config.en[2]
                        )
                --[[
                local BattleMain=FindFirstOf("BUI_BattleMain_C")
                if BattleMain and BattleMain:IsValid() then
                    --local ParentWidget=StaticFindObject(BattleMain:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.MainCon")
                    local ParentWidget=StaticFindObject(BattleMain:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.MainCon")
                    --local Root=FindFirstOf("BUI_B1_Root_V2_C")
                    --ParentWidget=StaticFindObject(Root:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.RootCanvas")
                    if ParentWidget and ParentWidget:IsValid() then
                        TextsWidget=FindFirstOf("TextsWidget_C")
                        if not TextsWidget:IsValid() then
                            --MyMod:InitWidget(ParentWidget)
                            MyMod:InitWidget(nil)
                            --ParentWidget:AddChild(TextsWidget)
                            TextsWidget=FindFirstOf("TextsWidget_C")
                            MyMod:SetPosition(TextsWidget,config.hp[1],config.hp[2],
                                  config.mp[1],config.mp[2],
                                  config.st[1],config.st[2],
                                  config.fp[1],config.fp[2],
                                  config.sp[1],config.sp[2],
                                  config.tr[1],config.tr[2],
                                  config.en[1],config.en[2]
                                  )
                        end

                        print(tostring(ParentWidget:GetFullName()))
                        print(tostring(TextsWidget:GetFullName()))
                        print(ModName.."Create TextsWidget")
                        TextsWidgetInited=true
                    end
                end
                ]]--
            end
        end
        findBattleMainInterval=findBattleMainInterval-interval
    end
end

function Refresh()
    if not enabled then return end
    if (not player) or (not player:IsValid()) then return end
    if (not MyMod) or (not MyMod:IsValid()) then return end
    if (not FuncLib) or (not FuncLib:IsValid()) then return end
    CreateWidget()
    if (not TextsWidget) or (not TextsWidget:IsValid()) then return end 
    --print(tostring(hp))
    local text={"","","","","",""}
    if config.show_hp then
        text[1]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,HPID),FuncLib:BGUGetFloatAttr(player.Pawn,HPMAXID))
    end
    if config.show_mp then
        text[2]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,MPID),FuncLib:BGUGetFloatAttr(player.Pawn,MPMAXID))
    end
    if config.show_st then
        text[3]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,STID),FuncLib:BGUGetFloatAttr(player.Pawn,STMAXID))
    end
    --DmgDef：减伤率，200=2%
    if config.show_fp then
        fpmax=FuncLib:BGUGetFloatAttr(player.Pawn,FPMAXID)
        if IsTrans then
            --变身时用的也是pevalue，pevaluemax会随之变化,但是最大值只有pevaluemax的一半
            fpmax=fpmax/2
        end
        text[4]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,FPID),fpmax)
        --[[
        text[4]=string.format("%.5f/%.6f/%.3f/%.3f/%.3f",
                FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::DmgDef"]),
                FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::EnergyIncreaseSpeed"]),
                FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::SpecialEnergyMax"]),
                FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CurEnergy"]),
                FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::DmgDef"])
                )
                ]]--
    end
    if config.show_sp then
        spmax=FuncLib:BGUGetFloatAttr(player.Pawn,SPMAXID)
        if spmax>0.0 then
            text[5]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,SPID),spmax)
        end
    end
    if config.show_tr then
        trmax=FuncLib:BGUGetFloatAttr(player.Pawn,TRMAXID)
        if trmax>0.0 then
            text[6]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,TRID),trmax)
        end
    end
    if config.show_en then
        enmax=FuncLib:BGUGetFloatAttr(player.Pawn,ENMAXID)
        if enmax>0.0 or true then
            text[7]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,ENID),enmax)
        end
    end

    if isTrans==true then --变身
        text[2]=text[7]--能量条显示在蓝条位置，不显示mp/st
        text[3]=""
        text[5]=""
        text[6]=""
        text[7]=""
    end

    MyMod:SetValue(TextsWidget,text[1],text[2],text[3],text[4],text[5],text[6],text[7])

    return false --LoopAsync,loop forever
end

function FindMyMod()    
    local modActors = FindAllOf("ModActor_C");
    for idx, modActor in ipairs(modActors or {}) do
        if modActor and modActor:IsA("/Game/Mods/PlayerStatus/ModActor.ModActor_C") then
            MyMod=modActor
        end
    end
end

if false then
    RegisterKeyBind(Key.F6,function()
        if false then
            local BattleMain=FindFirstOf("BUI_BattleMain_C")
            if BattleMain and BattleMain:IsValid() then
                local ParentWidget=StaticFindObject(BattleMain:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.MainCon")
                BattleMain:SetVisibility(2)
                --ParentWidget:RemoveChild(TextsWidget)
                --ParentWidget:RemoveChild(BattleMain)
                --ParentWidget:RemoveFromViewport()
                --TextsWidget:RemoveFromViewport()
                MyMod:Remove(BattleMain)
                --MyMod:Remove(TextsWidget)
                local slot=StaticFindObject("/Script/UMG.Default__WidgetLayoutLibrary"):SlotAsCanvasSlot(TextsWidget)
                ParentWidget:RemoveChild(slot)
                ParentWidget:RemoveChild(TextsWidget)
                --TextsWidget:RemoveFromParent()
                ParentWidget:ClearChildren()
                --TextsWidget=nil
            end
        end
        if true then
            for k,v in ipairs(AttrEnumMap) do
                if true then
                    --value=FuncLib:BGUGetFloatAttr(player.Pawn,v)
                    if true and k~=182 and k~=172 and k~=175 then
                        print(tostring(v)..tostring(k))
                        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,v)))
                    end
                end
            end
        end
    end)
end
RegisterKeyBind(Key.F4,function()
    if not MyMod then return end
    if not MyMod:IsValid() then return end
    if not TextsWidget then return end
    if not TextsWidget:IsValid() then return end
    enabled=not enabled
    MyMod:SetVisible(enabled,TextsWidget)
end)

if config.freq >50 then config.freq=50 
elseif config.freq<0.1 then config.freq=0.1 end

LoopAsync(interval, Refresh)
Init()