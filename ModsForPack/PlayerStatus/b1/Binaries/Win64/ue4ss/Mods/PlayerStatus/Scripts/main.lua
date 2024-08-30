local ModName="[PlayerStatus] "
--author:xyzkljl1
local _Debug=false
local config = require("playerstatus-config")
local FuncLib=nil
local MyMod=nil
local player=nil
local inited=false
local enabled=true
local TextsWidgetInited=true
local interval=math.floor(1000/config.freq)

local TextsWidget=nil
local AttrWidgetsNum=6
local AttrWidgets={}
local AttrWidgetsOriginalText={}
local getAttrWidgetCt=0
local AttrWidgetsKeys={
    [1]="EBGUAttrFloat::HpMax",
    [2]="EBGUAttrFloat::MpMax",
    [3]="EBGUAttrFloat::StaminaMax",
    [4]="EBGUAttrFloat::Atk",
    [5]="EBGUAttrFloat::Def",
    [6]="EBGUAttrFloat::StaminaRecover"
    --[7]="EBGUAttrFloat::CritRate",
    --[8]="EBGUAttrFloat::CritMultiplier",
}

local penable=true
local hooked=false
local BuffSourceEnumMap={}

local isTrans=false

local AttrEnumMap={}
local AttrEnumMapRev={}
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
    if (not player) or (not player:IsValid()) then 	player=FindFirstOf("BP_B1PlayerController_C") end
    if (not player) or (not player:IsValid()) then return end
    if not inited then
        FuncLib=StaticFindObject("/Script/b1-Managed.Default__BGUFunctionLibraryCS")
        if not FuncLib:IsValid() then return end
        enum=StaticFindObject("/Script/GSE-ProtobufDB.EBGUAttrFloat")
        AttrEnumMap={}
        AttrEnumMapRev={}
        if not enum:IsValid() then return end
        enum:ForEachName(function(name,value)
            AttrEnumMap[name:ToString()]=value
            AttrEnumMapRev[value]=name:ToString()
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
    if _Debug==true and not hooked then        
        RegisterHook("/Script/b1-Managed.BUS_GSEventCollection:Evt_BuffAdd_Multicast_Invoke", function(Context,BuffID,Caster,RCaster,Duration,bufftype)
            if penable==true and BuffID:get()~=127 and BuffID:get()~=128 and BuffID:get()~=1015 then
    	        print("Add "..tostring(BuffID:get()).." t"..tostring(BuffSourceEnumMap[bufftype:get()]).." T:"..tostring(Duration:get()).." C"..tostring(Caster:get():GetFullName()).. " "..tostring(RCaster:get():GetFullName()))
            end
        end)
        RegisterHook("/Script/b1-Managed.BUS_GSEventCollection:Evt_BuffRemove_Multicast_Invoke", function(Context,BuffID)
            if penable==true and BuffID:get()~=127 and BuffID:get()~=128 and BuffID:get()~=1015 then
    	        print("Remove "..tostring(BuffID:get()))
            end
        end)
        hooked=true
        BuffSourceEnumMap={}
        enum=StaticFindObject("/Script/b1-Managed.BuffSourceType")
        enum:ForEachName(function(name,value)
            BuffSourceEnumMap[value]=name:ToString()
        end)
        LoopAsync(1000,function()
            if penable==true and false then
           		FuncLib=StaticFindObject("/Script/b1-Managed.Default__BGUFunctionLibraryCS")
		        ch=FindFirstOf("UNIT_GYCY_Lang_01_C") or {}
                if ch:IsValid() then
			        local hp=tostring(FuncLib:BGUGetFloatAttr(v,122))
			        local hpmax=tostring(FuncLib:BGUGetFloatAttr(v,172))
			        print("Def--"..tostring(hp).." "..tostring(hpmax).." ")
                end
            end
            return false
        end)
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
    if not enabled then return false end
    if (not player) or (not player:IsValid()) then return false end
    if (not MyMod) or (not MyMod:IsValid()) then return false end
    if (not FuncLib) or (not FuncLib:IsValid()) then return false end
    CreateWidget()
    if (not TextsWidget) or (not TextsWidget:IsValid()) then return false end 
    --print(tostring(hp))
    local text={}
    if config.show_hp then
        --text[1]=string.format("%.5f/%.5f",FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::StaminaCostMultiperBase"]), FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::StaminaCostMultiper"]))
        text[1]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,HPID),FuncLib:BGUGetFloatAttr(player.Pawn,HPMAXID))        
    end
    if config.show_mp then
        text[2]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,MPID),FuncLib:BGUGetFloatAttr(player.Pawn,MPMAXID))
    end
    if config.show_st then
        text[3]=string.format("%.1f/%.1f",FuncLib:BGUGetFloatAttr(player.Pawn,STID),FuncLib:BGUGetFloatAttr(player.Pawn,STMAXID))
        if _Debug==true and penable==true then
            print("DefMul "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::DefMul"]).." Df:"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::DmgDef"]).." "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::DefBase"]) .." St:"..tostring(FuncLib:BGUGetFloatAttr(player.Pawn,STID)).." FP:"..FuncLib:BGUGetFloatAttr(player.Pawn,FPID)
                .."Cr "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritRateBase"])
                .."Cr "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritMultiplierBase"])
                .."Cr "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritRate"])
                .."Cr "..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritMultiplier"])
                .."Atk"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::Atk"])
                .."AtkBase"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::AtkBase"])
                .."AtkMul"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::AtkMul"])
                .."EnBase"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::EnergyIncreaseSpeedBase"])
                .."EnMul"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::EnergyIncreaseSpeedMul"])
                .."En"..FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::EnergyIncreaseSpeed"])
                .."En"..FuncLib:BGUGetFloatAttr(player.Pawn,ENID)
                )
        end
    end
    --DmgDef：减伤率，200=2%
    if config.show_fp then
        fpmax=FuncLib:BGUGetFloatAttr(player.Pawn,FPMAXID)
        --变身时用的也是pevalue，pevaluemax会随之变化,寅虎上限是300，通过蓄力只能蓄到150，防御可以到300
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

    MyMod:SetValue(TextsWidget,text[1] or "",text[2] or "",text[3] or "",text[4] or "",text[5] or "",text[6] or "",text[7] or "")


    --属性界面
    --get attr name widgets
    --txtothernum会在左边被裁剪，使用txtotherName
    if AttrWidgets[1]==nil or (not AttrWidgets[1]:IsValid()) and getAttrWidgetCt<=0 then
        getAttrWidgetCt=1000/interval
    	local Menu=FindFirstOf("BUI_EquipMain_C")
	    if not Menu:IsValid() then return false end
        local url=Menu:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.BI_RoleAttr.WidgetTree.BI_SecRoleAttrUnfold_%d.WidgetTree.TxtOtherName" -- BattleMainUI object name
        for i=0,AttrWidgetsNum-1 do
            --GSScaleText /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147476104.WidgetTree.BUI_EquipMain_C_2147462105.WidgetTree.BI_RoleAttr.WidgetTree.BI_SecRoleAttrUnfold_1.WidgetTree.TxtOtherName
            local path=string.format(url,i)
            local tmp=StaticFindObject(path)
            if not tmp:IsValid() then break end
            AttrWidgets[i+1]=tmp
            AttrWidgetsOriginalText[i+1]=tmp:GetText():ToString()
        end
    end

    if AttrWidgets[1]==nil or (not AttrWidgets[1]:IsValid()) then
        getAttrWidgetCt=getAttrWidgetCt-1
        return false
    end
    for i=1,AttrWidgetsNum do 
        if AttrWidgetsKeys[i] then
            local detail=""
            detail=string.format("%.2f*%.2f",FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap[AttrWidgetsKeys[i].."Base"]),
                                    1+FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap[AttrWidgetsKeys[i].."Mul"])/10000)
            if i<=3 then
                detail=string.format("%.2f/",FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap[AttrWidgetsKeys[i]:sub(1,-4)]))..detail
            end
            AttrWidgets[i]:SetText(FText(AttrWidgetsOriginalText[i].."("..detail..")"))
        end
    end

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

if _Debug==true then
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
        if not player then 	
        	player=FindFirstOf("BP_B1PlayerController_C")
        end
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::AtkMul"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::AtkBase"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::Atk"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritRate"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritMultiplier"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::StaminaRecoverMul"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::StaminaRecoverBase"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::StaminaRecover"])))
        print(tostring(FuncLib:BGUGetFloatAttr(player.Pawn,AttrEnumMap["EBGUAttrFloat::CritMultiplier"])))
        print("--")
        if false then
            for v,k in pairs(AttrEnumMapRev) do
                if true then
                    --value=FuncLib:BGUGetFloatAttr(player.Pawn,v)
                    if true and k~=182 and k~=172 and k~=175 and k~=75 and k>0 then
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

if _Debug==true then
    RegisterKeyBind(Key.F7,function()
        penable=not penable
        print(tostring(penable))
    end)
end