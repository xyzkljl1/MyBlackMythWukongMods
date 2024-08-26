local ModName="[HUDAdjustX] "
--author:xyzkljl1
local config = require("playerstatus-config")
local FuncLib=nil
local MyMod=nil
local player=nil
local inited=false
local enabled=true

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

RegisterHook("/Script/Engine.PlayerController:ClientRestart", function(Context)
    Init()
end)

function Init()
    if not inited then
        FuncLib=StaticFindObject("/Script/b1-Managed.Default__BGUFunctionLibraryCS")
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
        inited=true
        print(ModName.."Init Once")
    end
  	player=FindFirstOf("BP_B1PlayerController_C")
    FindMyMod()
    MyMod:SetPosition(config.hp[1],config.hp[2],
                      config.mp[1],config.mp[2],
                      config.st[1],config.st[2],
                      config.fp[1],config.fp[2],
                      config.sp[1],config.sp[2],
                      config.tr[1],config.tr[2]
                      )
    print(ModName.."Reset")
    --setfontandcolor/settext causes crash.Why????
    --MyMod:SetFontColor(config.fontsize,config.color[1],config.color[2],config.color[3],config.color[4])
end

function Refresh()
    --print("1")
    if not enabled then return end
    if not player then return end
    if not player:IsValid() then return end
    if not MyMod then return end
    if not MyMod:IsValid() then return end
    --if not FuncLib then return end
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
    if config.show_fp then
        fpmax=FuncLib:BGUGetFloatAttr(player.Pawn,FPMAXID)
        text[4]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,FPID),fpmax)
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
    MyMod:SetValue(text[1],text[2],text[3],text[4],text[5],text[6],"")

    return false --LoopAsync,loop forever
end

function FindMyMod()    
    local modActors = FindAllOf("ModActor_C");
    for idx, modActor in ipairs(modActors) do
        if modActor:IsA("/Game/Mods/PlayerStatus/ModActor.ModActor_C") then
            MyMod=modActor
        end
    end
end

if false then
    RegisterKeyBind(Key.F6,function()
        for k,v in pairs(AttrEnumMap) do
            if k:find("Yin") or k:find("Yang") then
            else
                value=FuncLib:BGUGetFloatAttr(player.Pawn,v)
                if value~=0.0 then
                    print(k)
                    print(tostring(value))
                end
            end
        end
    end)
end
RegisterKeyBind(Key.F4,function()
    if not MyMod then return end
    if not MyMod:IsValid() then return end
    MyMod:Switch()
    enabled=not enabled
end)

if config.freq >50 then config.freq=50 
elseif config.freq<0.1 then config.freq=0.1 end

LoopAsync(math.floor(1000/config.freq), Refresh)
Init()