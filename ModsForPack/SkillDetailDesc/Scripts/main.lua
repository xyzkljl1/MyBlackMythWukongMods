local ModName="[SkillDetailDesc] "
--author:xyzkljl1
--需要以utf8保存以识别中文
--local config = require("skilldetaildesc-config")
--108+
local Dictionary={
	--根基-气力
	["调息"]="(+15%/lv)",
	["猿捷"]="(-2.5/lv,基础消耗 20)",
	["体健"]="(-15%/lv,基础消耗 7.5/s)",
	["身轻"]="(-30%,基础消耗 30)",
	["绝念"]="(15%,持续2秒)",
	["走险"]="(+10/lv,基础 40)",
	["志心"]="(+10/lv,基础 40)",
	["绵绵"]="(+50%气力回复)",

	--武艺
	["接力"]="(18->23 per Hit)",
	["化吉"]="(7.5->8.7)",
	["筋节"]="(30%)",
	["应手"]="(22.5/秒->14.5/秒)",
	["得心"]="(4->6 per Hit)",
	--体段修行
	["吐纳绵长"]="(+2.5气力回复/lv)",
	["五脏坚固"]="(+10/lv)",
	["气海充盈"]="(+10/lv)",
	--["灾苦消减"]="(？/lv)",
	["怒相厚积"]="(+1%/lv)",
	["皮肉粗糙"]="(+10/lv)",
	["法性贯通"]="(+10/lv)",
	["攻势澎湃"]="(+2攻击力/lv)",
	["四灾忍耐"]="(+3/lv)",
	["威能凶猛"]="(+4%/lv)",
	--棍法
	["壮怀"]="(回复+2%/3%/4%(Lv1/Lv2/Lv3) 最大生命)",
	["熟谙"]="(-4%/lv)",
	["克刚"]="(Lv1:+20%/Lv2:+30%)",
	["抖擞"]="(+100)",
	["骋势"]="(+1% 每层)",
	["拂痒"]="(20%)",

	["精壮"]="(+5%/lv,基础55点棍势/秒)",
	--奇术
	["不破不立"]="(+15MP)",
	["假借须臾"]="(50MP->80MP,+2秒持续)",
	["烈烈"]="(20/s->23/s)",
	["昂扬"]="(+30% -> 40+30%)",
	["弥坚"]="(Lv1:+10/Lv2:+15 每跳)",
	["归根"]="(+20%)",
	["无挂无碍"]="(60MP->90MP, 5秒持续)",

	["舍得"]="(20%生命上限)",
	["不休"]="(-10%/lv)",
	["智力高强"]="(每级+0.02攻击力/1法力，基础0.12攻击力/1法力)",
	["放手一搏"]="(0.03%暴击/1气力)",
	["凝滞"]="(+0.16秒/lv，基础8秒)",
	["瞬机"]="(+60%持续)",	
	["圆明"]="(+2/4/5秒，基础20.5秒)",

	--身法
	["洞察"]="(Lv1:+3%/Lv2:+4% 每层)",
	["破影一击"]="(30MP->40MP)",
	["频频"]="(15s->13s)",
	["巩固"]="(+15%,持续15秒)",
	["知机"]="(+75)",
	["厉无咎"]="(Lv1:10MP/Lv2:15MP)",
	["养气"]="(+2秒/lv,基础10秒)",

	--毫毛
	["玄同"]="(积攒效率为本体的20%)",
	["毛吞大海"]="(20MP/lv)",
	--变化
	["虚相凶猛"]="(+2/lv)",
	["炼实返虚"]="(+15/lv)",
	["磊磊"]="(+15/lv)",
	["剪尾"]="(-10%)",
	["红眼"]="(+1 每层)",

	["恶秽"]="(Lv1->6:+4/6/8/10/12/14)",
	["霏霏"]="(Lv1->6:+4/6/8/10/12/14)",
	["爆躁"]="(Lv1->6:+4/6/8/10/12/14)",


	["暗藏神妙"]="(+3.3%/lv)",
	["截伪续真"]="(+2%神力上限/lv,基础 40%)",
	["存神炼气"]="(+0.0125/lv,基础 0.625)",
	["保养精神"]="(-0.0208/lv,基础 1.25)",


	--英文
	["Focused Breath"]="(+15% Stamina Recover/lv)",
	["Simian Agility"]="(-2.5/lv,Base Cost 20)",
	["Endurance"]="(-15%/lv,Base Cost 3.6)",
	["Featherlight"]="(-30%,Base Cost 30)",
	["Ephemeral Shadow"]="(15% in 2s)",
	["Bold Move"]="(+10/lv,Base Value 40)",
	["Vigilant Heart"]="(+10/lv,Base Value 40)",
	["Everlasting Vigor"]="(+50% Stamina Recover)",
	--武艺
	["Effortless Finisher"]="(18->23 per Hit)",
	["Silver Lining"]="(7.5->8.7)",

	["Sturdy"]="(30%)",
	["Graceful Whirl"]="(22.5/s->14.5/s)",
	["Focused Spinning"]="(4->6 per Hit)",
	--体段修行
	["Deep Breath"]="(+2.5 Stamina Recover/lv)",
	["Robust Constitution"]="(+10/lv)",
	["Rampant Vigor"]="(+10/lv)",
	--["灾苦消减"]="(？/lv)",
	["Wrathful Escalation"]="(+1%/lv)",
	["Rough Skin"]="(+10/lv)",
	["Spiritual Awakening"]="(+10/lv)",
	["Surging Momentum"]="(+2/lv)",
	["Four Banes Endurance"]="(+3/lv)",
	["Wrathful Might"]="(+4%/lv)",
	--棍法
	["Exhilaration"]="(+2%/3%/4%(Lv1/Lv2/Lv3) MaxHP)",
	["Instinct"]="(-4%/lv)",
	["Ironbound Resolve"]="(Lv1:+20%/Lv2:+30%)",
	["Invigoration"]="(+100)",
	["Force Cascade"]="(+1% per stack)",
	["Gale's Blessing"]="(20%)",
	["Quick Hand"]="(+5%/lv,Base 55 focus point/s)",
	--奇术
	["Spirit Shards"]="(+15MP)",
	["Time Bargain"]="(50MP->80MP, +2s)",
	["Raging Flames"]="(20/s->23/s)",
	["Burning Zeal"]="(+30% -> 40+30%)",
	["Consolidation"]="(Lv1:+10/Lv2:+15 per tick)",
	["Flame's Embrace"]="(+20%)",
	["Boundless Blessings"]="(60MP->90MP, 5s)",

	["Foregone, Foregained"]="(20%MaxHP)",
	["Light Heart"]="(-10%/lv)",
	["Smart Move"]="(每级+0.02攻击力/1法力，基础0.12攻击力/1法力)",
	["All or Nothing"]="(0.03%暴击/1气力)",
	
	["Stagnation"]="(+0.16s/lv, Base 8s)",
	["Evanescence"]="(+60% duration)",	
	["Flaring Dharma"]="(+2/4/5s，Base 20.5s)",
	--身法
	["Concealed Observation"]="(Lv1:+3%/Lv2:+4% per stack)",
	["Absolute Strike"]="(30MP->40MP)",
	["Rock Mastery"]="(15s->13s)",
	["Ironclad"]="(+15% for 15s)",
	["Nick of Time"]="(+75)",
	["Bold Venture"]="(Lv1:10MP/Lv2:15MP)",
	["Ruse"]="(+2s/lv,Base10s)",

	--毫毛
	["Harmony"]="(20% efficiency comparing to player)",
	["Multitude"]="(20MP/lv)",
	--变化
	["Ferocious Form"]="(+2/lv)",
	["Enduring Form"]="(+15/lv)",
	["Steadfast"]="(+15/lv)",
	["Cold Edge"]="(-10%)",
	["Hidden Might"]="(+3.3%/lv)",
	["Red Eyes"]="(+1 per stack)",
	["Rage Burst"]="(Lv1->6:+4/6/8/10/12/14)",
	["Filthy Malice"]="(Lv1->6:+4/6/8/10/12/14)",
	["Snow Veil"]="(Lv1->6:+4/6/8/10/12/14)",

	["Might Reserve"]="(+2%MaxEnergy/lv,Base 40%)",
	["Might Fortification"]="(+0.0125/lv,Base: 0.625)",
	["Evergreen"]="(-0.0208/lv,Base 1.25)",
}

local DictionarySpirit={
	--211.8/195.4/170.8/129.7
	--103.8/92.7
	["幽魂"]="211.8",
	["广谋"]="211.8",
	["虫总兵"]="103.8",
	["百足虫"]="103.8",
	["无量蝠"]="211.8",
	["不空"]="10.4",
	["不净"]="103.8",
	["不白"]="103.8",
	["不能"]="22.6",
	["疯虎"]="211.8",
	["百目真人"]="205.2",
	["虎伥"]="103.8",
	["地狼"]="211.8",
	["波里个浪"]="211.8",
	["蛇司药"]="64.9",
	["青冉冉"]="211.8",
	["蛇捕头"]="47.2",
	["蜻蜓精"]="64.9",
	["虫校尉"]="64.9",
	["蝎太子"]="103.8",
	["泥塑金刚"]="64.9",
	["赤发鬼"]="103.8",
	["鸦香客"]="64.9",
	["隼居士"]="103.8",
	["夜叉奴"]="103.8",
	["菇男"]="211.8",
	["巡山鬼"]="64.9",
	["鼠司空"]="64.9",
	["骨悚然"]="103.8",
	["石双双"]="47.2",
	["鼠禁卫"]="64.9",
	["狸侍长"]="64.9",
	["疾蝠"]="64.9",
	["鼠弩手"]="103.8",

	["Wandering Wight"]="211.8",
	["Guangmou"]="211.8",
	["Commander Beetle"]="103.8",
	["Centipede Guai"]="103.8",
	["Apramāṇa Bat"]="211.8",
	["Non-Void"]="10.4",
	["Non-Pure"]="103.8",
	["Non-White"]="103.8",
	["Non-Able"]="22.6",
	["Mad Tiger"]="211.8",
	["Gore-Eye Daoist"]="205.2",
	["Tiger's Acolyte"]="103.8",
	["Earth Wolf"]="211.8",
	["Baw-Li-Guhh-Lang"]="211.8",
	["Snake Herbalist"]="64.9",
	["Verdant Glow"]="211.8",
	["Snake Sheriff"]="47.2",
	["Dragonfly Guai"]="64.9",
	["Beetle Captain"]="64.9",
	["Scorpion Prince"]="103.8",
	["Clay Vajra"]="64.9",
	["Red-Haired Yaksha"]="103.8",
	["Crow Diviner"]="64.9",
	["Falcon Hermit"]="103.8",
	["Enslaved Yaksha"]="103.8",
	["Fungiman"]="211.8",
	["Mountain Patroller"]="64.9",
	["Rat Governor"]="64.9",
	["Spearbone"]="103.8",
	["Poisestone"]="47.2",
	["Rat Imperial Guard"]="64.9",
	["Civet Sergeant"]="64.9",
	["Swift Bat"]="64.9",
	["Rat Archer"]="103.8",
}
local DictionarySpiritPassive={
	--211.8/195.4/170.8/129.7
	--103.8/92.7
	["幽魂"]="+24",	["Wandering Wight"]="+24",
	["广谋"]="+8/+8",	["Guangmou"]="+8/+8",
	["虫总兵"]="+10",	["Commander Beetle"]="+10",
	["百足虫"]=nil,["Centipede Guai"]=nil,
	["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
	["不空"]=nil,["Non-Void"]=nil,
	["不净"]="10秒内+6%",["Non-Pure"]="+6% in 6s",
	["不白"]="+6/+6",["Non-White"]="103.8",
	["不能"]="+2%暴击,+4%爆伤,+5攻击力,-50法力上限",["Non-Able"]="+2%CritRate,+4%CritDamage,+5ATK,-50MaxMP",
	["疯虎"]="+2攻击,-75生命上限",	["Mad Tiger"]="+2ATK,-75MaxHP",
	["百目真人"]="+10",	["Gore-Eye Daoist"]="+10",
	["虎伥"]="+6%",	["Tiger's Acolyte"]="+6%",
	["地狼"]="+2 per Hit",["Earth Wolf"]="+2 per Hit",
	["波里个浪"]="-6%",["Baw-Li-Guhh-Lang"]="-6%",
	["蛇司药"]="回复6%生命上限",["Snake Herbalist"]="6% MaxHP",
	["青冉冉"]="每5秒回复1.5%生命上限",	["Verdant Glow"]="1.5% MaxHP per 5 seconds",
	["蛇捕头"]="+5%攻击，-20%防御",	["Snake Sheriff"]="+5% ATK,-20% DEF",
	["蜻蜓精"]="+10%元气,-40生命,-20法力,-20气力",	["Dragonfly Guai"]="+10% Qi,-40 MaxHP,-20 MaxMP,-20 MaxSt",
	["虫校尉"]="+10",	["Beetle Captain"]="+10",
	["蝎太子"]="+10",["Scorpion Prince"]="+10",
	["泥塑金刚"]="+10",	["Clay Vajra"]="+10",
	["赤发鬼"]=nil,	["Red-Haired Yaksha"]=nil,
	["鸦香客"]="+10",	["Crow Diviner"]="+10",
	["隼居士"]="+10",	["Falcon Hermit"]="+10",
	["夜叉奴"]="+10 per Hit",	["Enslaved Yaksha"]="+10 per Hit",
	["菇男"]="+6%",["Fungiman"]="+6%",
	["巡山鬼"]=nil,	["Mountain Patroller"]=nil,
	["鼠司空"]="+10",	["Rat Governor"]="+10",
	["骨悚然"]="+1%",	["Spearbone"]="+1%",
	["石双双"]=nil,	["Poisestone"]=nil,
	["鼠禁卫"]="+6",["Rat Imperial Guard"]="+6",
	["狸侍长"]="+5",["Civet Sergeant"]="+5",
	["疾蝠"]="+10",["Swift Bat"]="+10",
	["鼠弩手"]="-18%",["Rat Archer"]="-18%",
}


--NotifyOnNewObject没有找到合适的对象
--[[
--o=StaticFindObject("/Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147476511.WidgetTree.BUI_LearnTalent_C_2147464698.WidgetTree.BI_SpellDetail")
--print(tostring(o:GetClass():GetFullName()))
function Test()
	MyMod=StaticFindObject("/Game/Mods/CustomHPBar/ModActor.Default__ModActor_C")
	--GSInputRichTextBlock /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147472234.WidgetTree.BUI_LearnTalent_C_2147463849.WidgetTree.BI_SpellDetail.WidgetTree.TxtSpellDesc
	lt=FindFirstOf("BUI_LearnTalent_C")
	prefix=lt:GetFullName():gsub("([^ ]*) (.*)","%2")
	textblock=StaticFindObject(prefix..".WidgetTree.BI_SpellDetail.WidgetTree.TxtSpellDesc")
	--GSScaleText /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147472234.WidgetTree.BUI_LearnTalent_C_2147463849.WidgetTree.BI_SpellDetail.WidgetTree.TxtName
	titleblock=StaticFindObject(prefix..".WidgetTree.BI_SpellDetail.WidgetTree.TxtName")
	title=tostring(titleblock:GetText():ToString())
	if Dictionary[title]~=nil then
		--print(tostring(title:GetText():ToString()))
		--print(tostring(title:GetText():ToString()=="1"))
		MyMod:SetRichBlock(textblock,Dictionary[title],true)
	else
		MyMod:SetRichBlock(textblock,tostring(textblock:GetFullName()),true)
	end
end
]]--
--BUI_TalentMain_C /Game/00Main/UI/BluePrintsV3/LearnSpell/BUI_TalentMain.Default__BUI_TalentMain_C

local detailtext=nil
local detailtextSpirit=nil
local detailtextSpiritPassive=nil


--目前看来，title一定在text之前设置
RegisterHook("/Script/UMG.TextBlock:SetText",function(Context,InText)
	local name=Context:get():GetFullName()
	if name:sub(-45)==".WidgetTree.BI_SpellDetail.WidgetTree.TxtName" then	--天赋名字
		title=InText:get():ToString()
		detailtext=Dictionary[title] --or "(None)"
		--print("--")
		--print(tostring(title))
		--print(tostring(Dictionary[title]))
	elseif detailtextSpirit~=nil and name:sub(-43)==".WidgetTree.BI_RZDDetail.WidgetTree.TxtCost" then--精魂能量消耗
		--print(tostring(InText:get():ToString()).."/"..tostring(name))
		InText:set(FText(InText:get():ToString().."("..detailtextSpirit.." at Lv1)"))
		detailtextSpirit=nil
		--InText:set(FText(detailtextSpirit or "Unknown"))
	--elseif InText:get():ToString():find("服用丹药的同时") then
	--	print(".."..tostring(name))
	end
end)

RegisterHook("/Script/UMG.RichTextBlock:SetText",function(Context,InText)
	local name=Context:get():GetFullName()
	if name:sub(-50)==".WidgetTree.BI_SpellDetail.WidgetTree.TxtSpellDesc" then--天赋名字
		--print(tostring(detailtext))
		if detailtext~=nil then
			InText:set(FText(InText:get():ToString()..detailtext))
		end
		detailtext=nil
	elseif name:sub(-47)==".WidgetTree.BI_RZDDetail.WidgetTree.TxtNameRuby" then--精魂名字
		detailtextSpirit=DictionarySpirit[InText:get():ToString()] --or "(None)"
		detailtextSpiritPassive=DictionarySpiritPassive[InText:get():ToString()]
		--print("!!"..tostring(InText:get():ToString()).."/"..tostring(detailtext))
	elseif name:sub(-21)==".WidgetTree.TxtEffect" then --精魂升级描述
		--print(".."..tostring(InText:get():ToString()))
		if InText:get():ToString()=="Reduces the Qi cost for the skill." then
			InText:set(FText("Reduces the Qi cost for the skill.(-8%/18%/39% at Lv 2/3/5)"))
		elseif InText:get():ToString()=="减少施展此技所需元气" then
			InText:set(FText("减少施展此技所需元气 (-8%/18%/39% at Lv 2/3/5)"))
		end
	elseif detailtextSpiritPassive~=nil and name:sub(-73)==".WidgetTree.BI_RZDDetail.WidgetTree.TreasureEqDesc.WidgetTree.TxtSuitDesc" then--精魂被动描述
		InText:set(FText(InText:get():ToString().."("..detailtextSpiritPassive..")"))
		detailtextSpiritPassive=nil
	--elseif InText:get():ToString():find("服用丹药的同时") then
	--	print(".."..tostring(name))
	end
end)

--Work()
