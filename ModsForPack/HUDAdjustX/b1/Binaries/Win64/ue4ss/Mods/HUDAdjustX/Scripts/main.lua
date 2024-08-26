local ModName="[HUDAdjustX] "
--author:xyzkljl1
--version:1.2
local config = require("hudadjustx-config")
local WidgetLib=nil
local MyMod=nil
local hooked=false

RegisterHook("/Script/Engine.PlayerController:ClientRestart", function(Context,pawn)
	if not hooked then
		--变身有好几种，用到时才会加载
		NotifyOnNewObject("/Script/b1-Managed.BI_TransStyleCS", function(ConstructedObject)
			--print(tostring(ConstructedObject:GetFullName()))
			local panelname=ConstructedObject:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree.StyleCon"
			ExecuteWithDelay(10000,function()
				local panel=StaticFindObject(panelname)
				if panel and panel:IsValid() then
					print(ModName.."Init transstyle:"..tostring(panel:GetFullName()))
					if (not MyMod) or (not MyMod:IsValid()) then
						MyMod=StaticFindObject("/Game/Mods/HUDAdjustX/ModActor.Default__ModActor_C")
					end 
					if (not MyMod) or (not MyMod:IsValid()) then
						print(ModName.."Can't Find MyMod when init transstyle")						
					end
					panel["Slots"]:ForEach(function(index,elem)
						--print(tostring(index))
						--print(tostring(elem:get():GetFullName()))
						MyMod:SetPosition(elem:get(),config.fpbar_x+400,config.fpbar_y+80)
					end)
				end
			end)
		end)
		hooked=true
	end

    if not pawn:get():GetFullName():find("Unit_Player_Wukong_C") then return end
	ExecuteWithDelay(config.init_delay*1000, function()
		if not SetParam("Delay Init On ClientRestart") then
			print(ModName.."Retry in 10 seconds")
			ExecuteWithDelay(10000, function()
				SetParam("Delay Init2 On ClientRestart")
			end)
		end
	end)
end)

function SetCanvasSlotPosition(widget,x,y,bSetOffset)
	if widget and widget:IsValid() then
		local slot=WidgetLib:SlotAsCanvasSlot(widget)
		--[[
		print(tostring(widget:GetFullName()))
		print(tostring(slot:GetFullName()))
		print(tostring(slot:GetPosition()["X"]))
		print(tostring(slot:GetPosition()["Y"]))
		print(tostring(slot["LayoutData"]["Offsets"]["Left"]))
		print(tostring(slot["LayoutData"]["Offsets"]["Top"]))
		print(tostring(slot["LayoutData"]["Offsets"]["Right"]))
		print(tostring(slot["LayoutData"]["Offsets"]["Bottom"]))
		print(tostring(slot:GetAnchors()["Minimum"]["X"]))
		print(tostring(slot:GetAnchors()["Minimum"]["Y"]))
		print(tostring(slot:GetAnchors()["Maximum"]["X"]))
		print(tostring(slot:GetAnchors()["Maximum"]["Y"]))
		print(tostring(slot:GetAlignment()["X"]))
		print(tostring(slot:GetAlignment()["Y"]))
		]]--
		if slot and slot:IsValid() then
			MyMod:SetPosition(slot,x,y)
			if bSetOffset then
				MyMod:SetOffset(slot,x,y,-x,0)
			end
		end
	end
end


function SetParam(message)
	print(ModName..message)

	BattleMainUI=FindFirstOf("BUI_BattleMain_C")
	if not BattleMainUI:IsValid() then
		print(ModName.."Game not ready,Abort")
		return false
	end
	MyMod=StaticFindObject("/Game/Mods/HUDAdjustX/ModActor.Default__ModActor_C") -- Default object is enought here
	if not MyMod:IsValid() then
		print(ModName.."Can't Find BP mod,Abort")
		return false
	end
	WidgetLib=StaticFindObject("/Script/UMG.Default__WidgetLayoutLibrary")

	prefix=BattleMainUI:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree." -- BattleMainUI object name
	
	hpbar=StaticFindObject(prefix.."PlayerStCon")
	fpbar=StaticFindObject(prefix.."BI_PlayerStickLevel")
	fpbar2=StaticFindObject(prefix.."BI_DaShengStickLevel")--大圣套棍势
	--StaticFindObject(prefix.."BI_DaShengStickLevel"):SetVisibility(0)
	--StaticFindObject(prefix.."BI_Trans"):SetVisibility(0)
	skillshortcut=StaticFindObject(prefix.."BI_ShortcutSkill")
	itemshortcut=StaticFindObject(prefix.."BI_ShortcutItem")
	treasureshortcut=StaticFindObject(prefix.."BI_Treasure")
	rzdshortcut=StaticFindObject(prefix.."BI_RZDSkill")

	--变身后UI
	SetCanvasSlotPosition(StaticFindObject(prefix.."BI_Trans.WidgetTree.ImgHPBar"),config.hpbar_x,config.hpbar_y+200)
	SetCanvasSlotPosition(StaticFindObject(prefix.."BI_Trans.WidgetTree.HPBar"),config.hpbar_x-110-16,config.hpbar_y+200-3)
	SetCanvasSlotPosition(StaticFindObject(prefix.."BI_Trans.WidgetTree.EnergyBar"),config.hpbar_x-110-14,config.hpbar_y+200+43)--神力条,即变身持续时间
	SetCanvasSlotPosition(StaticFindObject(prefix.."BI_Trans.WidgetTree.BI_ShortcutSkill"),config.skill_x,config.skill_y)
	
	--变身相关 BI_TransStyle_14_C 寅虎 13石双双
		--[[transStyle=FindFirstOf("BI_TransStyle_14_C")
	transprefix=transStyle:GetFullName():gsub("([^ ]*) (.*)","%2")..".WidgetTree."

	SetCanvasSlotPosition(StaticFindObject(transprefix.."ImgRing"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."UIFX_Ring"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."UIFX_Ring_2"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."ImgIcon"),-500+24,-500+55)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."ImgProgBar"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."ProgMainImg"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."UFIX_Sweep"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."UFIX_Warn"),-500,-500)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."BI_TransProg"),-500+24,-500+50)
	SetCanvasSlotPosition(StaticFindObject(transprefix.."UINS_Spark"),-500+72,-500+50)

	StaticFindObject(transprefix.."StyleCon")["Slots"]:ForEach(function(index,elem)
		--print(tostring(index))
		--print(tostring(elem:get():GetFullName()))
		MyMod:SetPosition(elem:get(),config.fpbar_x+400,config.fpbar_y+80)
	end)]]--
	--StaticFindObject(transprefix.."RingCon")["Slots"]:ForEach(function(index,elem)
	--	print(tostring(index))
	--	print(tostring(elem:get():GetFullName()))
	--	MyMod:SetPosition(elem:get(),-0+24,-0+55)
	--end)
	
	--BI_BloodBarList_C /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147472232.WidgetTree.BUI_BloodBarList_C_2147471273.WidgetTree.BI_BloodBarList
	bossHPlist=FindAllOf("BI_BloodBarList_C")
	bossHP=nil
	for _,widget in pairs(bossHPlist) do
		if widget:GetFullName():find("GameInstance") then
			bossHP=widget
			break
		end
	end
	--x=StaticFindObject(prefix.."BI_RZDSkill")

	SetCanvasSlotPosition(hpbar,config.hpbar_x,config.hpbar_y)
	SetCanvasSlotPosition(fpbar,config.fpbar_x,config.fpbar_y)
	SetCanvasSlotPosition(fpbar2,config.fpbar_x,config.fpbar_y)
	--SetCanvasSlotPosition(fpbar3,config.fpbar_x,config.fpbar_y,true) --BI_trans控件的对齐方式不同

	SetCanvasSlotPosition(skillshortcut,config.skill_x,config.skill_y)
	SetCanvasSlotPosition(itemshortcut,config.item_x,config.item_y)
	SetCanvasSlotPosition(treasureshortcut,config.treasure_x,config.treasure_y)
	SetCanvasSlotPosition(rzdshortcut,config.rzd_x,config.rzd_y)	
	SetCanvasSlotPosition(bossHP,config.bosshp_x,config.bosshp_y)
	--SetCanvasSlotPosition(x,300,300)
	print(ModName.."Init Done")
	return true
end

SetParam("Init on Load")