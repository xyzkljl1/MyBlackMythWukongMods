local ModName="[SkillDetailDesc] "

--author:xyzkljl1
--需要以utf8保存以识别中文
--local config = require("skilldetaildesc-config")
--108+

local Dictionary={
	--根基-气力
	["调息"]="+15%/lv",
	["猿捷"]="-2.5/lv,基础消耗 20",
	--7.5/6/5.25
	["体健"]="Lv1: -20%/Lv2: -30%,基础 7.5/s",
	["身轻"]="-30%,基础消耗 30",
	["绝念"]="15%,持续2秒",
	["走险"]="+10/lv,基础 40",
	["志心"]="+10/lv,基础 40",
	["绵绵"]="+50%气力回复",
	--["任翻腾"]="",--第一段翻滚由SkillDesc 10301变为10305
	["直取"]="+100/lv,基础800/900/1000",["Switft Engage"]="+100/lv,Base 800/900/1000",--Passive 13/14 SkillCtrlDesc 10701,10798-10801

	--武艺
	["接力"]="18->23 per Hit",
	--10506 PassiveSkillDesc 16写的是Mul 50%,实测约+18%，回复量会浮动，约7.5~8，点完变成9上下
	["化吉"]="约+18%",["Silver Lining"]="about +18%",
	--10508 46/47 SkillEffectDamageExpand
	--["断筋"]="Lv1-2:+100/150",["TODO"]="Lv1-2:+100/150",
	["筋节"]="30%",
	["应手"]="-30%, 基础26/秒(移步48/秒)",
	--10506 PassiveSkillDesc 39-41
	["通变"]="+4%/lv 动作倍率",["Versatility"]="+4%/lv SkillEffect",
	--10603 51/52 SkillEffectFloat
	["压溃"]="Lv1-2:+5%/8% 动作倍率",["Smashing Force"]="Lv1-2:+5%/8% SkillEffect",
	--10702 53/54
	["铁树"]="-20%/lv",["Steel Pillar"]="-20%/lv",
	["得心"]="4->6 per Hit, 不影响原地棍花(3 per Hit)",
	--体段修行
	["吐纳绵长"]="+2.5气力回复/lv",
	["五脏坚固"]="+10/lv",
	["气海充盈"]="+10/lv",
	--TalentSDesc 100303 PassiveSkillDesc 20-25
	["灾苦消减"]="-5%持续时间/lv",["Bane Mitigation"]="-5% Duration/lv",
	["怒相厚积"]="+1%/lv",
	["皮肉粗糙"]="+10/lv",
	["法性贯通"]="+10/lv",
	["攻势澎湃"]="+2攻击力/lv",
	["四灾忍耐"]="+3/lv",
	["威能凶猛"]="+4%/lv",
	--棍法
	["壮怀"]="回复+2%/3%/4%Lv1/Lv2/Lv3 最大生命",
	--55/65/90/115/115 n
	--43/50/67.5/85 Lv3
	--29/32.5/41.2/50 Lv3+r
	--35/40/52.5/65 r
	--100504 Passive 36-38里写的是-10%,但和实测对每种风格都是减固定值
	--劈棍原地重击消耗10+55/65/90/115/115，立棍10+40/65/75/100/100，戳棍47.5/57.5/82.5/107.5，跳劈(三种棍势一样)50/75/100/125/125, 
	--3级天赋劈棍43/50/67.5/85，立棍28/50/53/70，戳棍35.5/42.5/60/77.5
	--不影响跳劈,不影响蓄力起手(三种风格起手消耗10/10/15)
	["熟谙"]="每级令0-4级重棍-4/-5/-7.5/-10/-10气力消耗，有身轻体快(根器)时效果减半，0-4级重棍基础消耗:劈棍55/65/90/115/115,立棍40/65/75/100/100,戳棍47.5/57.5/82.5/107.5;不影响蓄力起手式和跳跃重击",
	["Instinct"]="Each level reduce 0-4 Focus-Level heavy-attack cost: -4/-5/-7.5/-10/-10. Only have 50% effect when having Nimble Body(body relic).Base cost of each focus-level:Smash 55/65/90/115/115,Pillar 40/65/75/100/100,The other 47.5/57.5/82.5/107.5;Not affect charging and jump heavy attack cost",

	["克刚"]="Lv1:+20%/Lv2:+30%",
	["抖擞"]="+100",
	["骋势"]="9秒内+1% 每层",
	["拂痒"]="20%",
	["精壮"]="+5%/lv,基础55点棍势/秒",
	["借力"]="Lv1-2:回复30/50",["Borrowed Strength"]="Lv1-2:Recover 30/50",--Passive 13/14 SkillCtrlDesc 10701,10798-10801

	--1048~1050
	["乘胜追击"]="根据棍势+5%/10%/15%/20% 攻击",["Vantage Point"]="+5%/10%/15%/20% ATK by focus level",

	--奇术
	["不破不立"]="+15MP",
	["假借须臾"]="50MP->80MP,+30%持续时间",	["Time Bargain"]="50MP->80MP, +30% Duration", --100904 10904,10914,10924 Passive 61-63
	["烈烈"]="20/s->23/s",
	["昂扬"]="+30% -> 40+30%",
	["弥坚"]="Lv1:+10/Lv2:+15 每跳",
	["归根"]="+20%",
	["无挂无碍"]="60MP->90MP, 5秒持续",

	["舍得"]="20%生命上限",
	["不休"]="-10%/lv",
	["智力高强"]="每级+0.02攻击力/1法力，基础0.12攻击力/1法力",
	["放手一搏"]="0.03%暴击/1气力",
	["凝滞"]="每层-2%敌人定身抗性,基础8秒",--10905 Passive 64/65 Buff 1064 写的是DingshenDefAdditionBase +2,但是Passive里只改了叠加上限没改数值，怀疑是百分比加成，每层+2%
	["Stagnation"]="-2% Enemy Immobilize Resist per stack, Base 8s",

	--BuffDesc-Talent 1027
	["瞬机"]="+60%持续;+30%敌人承受伤害",		["Evanescence"]="+60% duration;+30% Enemy Damage Taken",	
	["圆明"]="+2/4/5秒，基础20.5秒",

	--FUStBuffDesc-Talent 1025 TalentSDesc 10901 PassiveSkillDesc 57/58，降低对手的伤害减免，buff基础数值是0，通过passiveskill修改buffeffect
	["脆断"]="Lv1-2:+10%/15%敌人承受伤害",["Crash"]="Lv1-2:+10%/15% Enemy Damage Taken",

	--身法
	["洞察"]="Lv1:+3%/Lv2:+4% 每层",
	--Talent 2101 AtkMul 2000
	["破影一击"]="30MP->40MP;+20% 攻击",	["Absolute Strike"]="30MP->40MP;+20% ATK",

	["频频"]="15s->13s",--11105 85 -2s
	["巩固"]="+15%,持续15秒",
	["知机"]="+75",
	["厉无咎"]="Lv1-2:+10MP/15MP;-0.2/0.3秒,基础1秒",	["Bold Venture"]="Lv1-2:+10MP/15MP;-0.2s/0.3s,Base 1s", --101103 Passive 77-84
	["养气"]="+2秒/lv,基础10秒",	["Converging Clouds"]="+2s/lv,Base10s",
	--Talent 11006 Passive 74/75
	["捣虚"]="+15%/lv",["Ruse"]="+15%/lv",

	--1059,10/15/15? walk/run/dash?
	["纵横"]="徐行/奔跑/冲刺速度+10%/15%/15%",["Gallop"]="+10%/15%/15% Walk/Run/Sprint speed",
	--毫毛
	["玄同"]="积攒效率为本体的20%",
	["毛吞大海"]="20MP/lv",
	--11201 Passive 88/89
	["存身"]="+2秒/lv,基础约25秒",["Longstrand"]="+2s/lv,Base about 25s",
	["同寿"]="+10%",["Grey Hair"]="+10%",--11205 96
	["浇油"]="+15%/lv",["Insult to Injury"]="+15%/lv",--11208 97/98 1083buff基础15%,1/2级天赋+0/+15%
	["回阳"]="-10秒",["Glorious Return"]="-10s",--buff 1111
	["不增不减"]="回复20%法力上限",["Spirited Return"]="Recover 20% MaxMP",
	["合契"]="每层+0.4s?",["Synergy"]="+0.4s per stack?",--101201 Passive 89 1001101写的+4，实测最多约+4秒？
	--Talent 1082
	["仗势"]="+15% 攻击",["Tyranny of Numbers"]="+15% ATK",
	["去来空"]="20%",["Cycle Breaker"]="20%",--101404 112
	--变化
	["虚相凶猛"]="+2/lv",
	["炼实返虚"]="+15/lv",
	["磊磊"]="+15/lv",

	--TalentSDesc 301103 PassiveSkillDesc 232  233 实际是-1.5 3.5 
	["剪尾"]="-10%",
	["红眼"]="60秒内+1 攻击力/每层",	["Red Eyes"]="+1 per stack in 60s",
	["恶秽"]="Lv1->6:+4/6/8/10/12/14",
	["霏霏"]="Lv1->6:+4/6/8/10/12/14",
	["爆躁"]="Lv1->6:+4/6/8/10/12/14",
	["暗藏神妙"]="+3.3%/lv",
	["截伪续真"]="+2%神力上限/lv,基础 40%",
	["存神炼气"]="+2%/lv,基础 0.5/s",
	["保养精神"]="-1.67%/lv,基础 1.25/s",

	["步月"]="Lv1-3:+10%/15%/20%攻击力",
	["奔霄"]="Lv1->6:+4/6/8/10/12/14",
	["不坏身"]="+12",
	
	--TalentSDesc 301108 PassiveSkillDesc 236 BuffDesc-lz
	["一闪"]="+0.5秒->0.8秒",["Lightning Flash"]="+0.5s->0.8s",

	--英文 
	["Focused Breath"]="+15% Stamina Recover/lv",
	["Simian Agility"]="-2.5/lv,Base Cost 20",
	["Endurance"]="Lv1: -20%/Lv2: -30%, Base Cost 7.5/s",
	["Featherlight"]="-30%, Base Cost 30",
	["Ephemeral Shadow"]="15% in 2s",
	["Bold Move"]="+10/lv, Base Value 40",
	["Vigilant Heart"]="+10/lv, Base Value 40",
	["Everlasting Vigor"]="+50% Stamina Recover",
	--武艺
	["Effortless Finisher"]="18->23 per Hit",
	

	["Sturdy"]="30%",
	["Graceful Whirl"]="-30%, Base Cost 26/s(48/s when moving)",
	["Focused Spinning"]="4->6 per Hit",
	--体段修行
	["Deep Breath"]="+2.5 Stamina Recover/lv",
	["Robust Constitution"]="+10/lv",
	["Rampant Vigor"]="+10/lv",
	--["灾苦消减"]="？/lv",
	["Wrathful Escalation"]="+1%/lv",
	["Rough Skin"]="+10/lv",
	["Spiritual Awakening"]="+10/lv",
	["Surging Momentum"]="+2/lv",
	["Four Banes Endurance"]="+3/lv",
	["Wrathful Might"]="+4%/lv",
	--棍法
	["Exhilaration"]="+2%/3%/4%Lv1/Lv2/Lv3 MaxHP",
	["Ironbound Resolve"]="Lv1:+20%/Lv2:+30%",
	["Invigoration"]="+100",
	["Force Cascade"]="+1% per stack in 9s",
	["Gale's Blessing"]="20%",
	["Quick Hand"]="+5%/lv,Base 55 focus point/s",
	--奇术
	["Spirit Shards"]="+15MP",

	["Raging Flames"]="20/s->23/s",
	["Burning Zeal"]="+30% -> 40+30%",
	["Consolidation"]="Lv1:+10/Lv2:+15 per tick",
	["Flame's Embrace"]="+20%",
	["Boundless Blessings"]="60MP->90MP, 5s",

	["Foregone, Foregained"]="20%MaxHP",
	["Light Heart"]="-10%/lv",
	["Smart Move"]="+0.02 ATK per MP/lv，Base +0.12 ATK per MP",
	["All or Nothing"]="0.03% Crit per Stamina",
	

	["Flaring Dharma"]="+2/4/5s，Base 20.5s",
	--身法
	["Concealed Observation"]="Lv1:+3%/Lv2:+4% per stack",
	["Rock Mastery"]="15s->13s",
	["Ironclad"]="+15% for 15s",
	["Nick of Time"]="+75",



	--毫毛
	["Harmony"]="20% efficiency comparing to player",
	["Multitude"]="20MP/lv",
	--变化
	["Ferocious Form"]="+2/lv",
	["Enduring Form"]="+15/lv",
	["Steadfast"]="+15/lv",
	["Cold Edge"]="-10%",
	["Hidden Might"]="+3.3%/lv",
	["Rage Burst"]="Lv1->6:+4/6/8/10/12/14",
	["Filthy Malice"]="Lv1->6:+4/6/8/10/12/14",
	["Snow Veil"]="Lv1->6:+4/6/8/10/12/14",
	["Might Reserve"]="+2%MaxEnergy/lv,Base 40%",
	["Might Fortification"]="+2%/lv,Base 0.5/s",
	["Evergreen"]="-1.67%/lv,Base 1.25/s",

	["Moon Roam"]="Lv1-3:+10%/15%/20%攻击力",
	["Bolt Runner"]="Lv1->6:+4/6/8/10/12/14",
	["Indestructibility"]="+50%",
}

local DictionarySpirit={
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
	["波里个浪"]="211.8",["波波浪浪"]="211.8",--升满级后改名
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

	["牯都督"]="211.8",["Bull Governor"]="211.8",
	["火灵元母"]="211.8",["Mother of Flamlings"]="211.8",
	["燧先锋"]="205.2",["Flint Vanguard"]="205.2",
	["琴螂仙"]="205.2",["Elder Amourworm"]="205.2",
	["九叶灵芝精"]="103.8",["Nine-Capped Lingzhi Guai"]="103.8",
	["兴烘掀·掀烘兴"]="211.8",["Top Takes Bottom, Bottom Takes Top"]="211.8",
	["雾里云·云里雾"]="211.8",["Misty Cloud, Cloudy Mist"]="211.8",
	["地罗刹"]="103.8",["Earth Rakshasa"]="103.8",
	["鳖宝"]="103.8",["Turtle Treasure"]="103.8",
	["燧统领"]="103.8",["Flint Chief"]="103.8",
	["黑脸鬼"]="64.9",["Charface"]="64.9",
	["石父"]="103.8",["Father of Stones"]="103.8",
	["沙二郎"]="211.8",["Second Rat Prince"]="211.8",

	["蘑女"]="211.8",["Fungiwoman"]="211.8",
	["老人参精"]="205.2",["Old Ginseng Guai"]="205.2",
	["儡蜱士"]="205.2",["Puppet Tick"]="205.2",
	["傀蛛士"]="64.9",["Puppet Spider"]="64.9",
	["幽灯鬼"]="103.8",["Lantern Holder"]="103.8",
	["戒刀僧"]="103.8",["Blade Monk"]="103.8",
	["狼刺客"]="64.9",["Wolf Assassin"]="64.9",


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
	["Baw-Li-Guhh-Lang"]="211.8",["Baw-Baw-Lang-Lang"]="211.8",
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
--升级减少消耗的百分比不同
--211.8/195.4/170.8/129.7
--103.8/92.7
for k,v in pairs(DictionarySpirit) do
	if v=="211.8" then
		v="211.8/195.4/170.8/129.7"
	elseif v=="205.2" then
		v="205.2/197/184.7/164.2"
	elseif v=="103.8" then
		v="103.8/92.7/79.8/64.9"
	elseif v=="64.9" then
		v="64.9/59/51.9/43.2"
	elseif v=="47.2" then
		v="47.2/43.2/38.4/32.4"

	elseif v=="22.6" then
		v="22.6/20.9/18.9/16.2"
	elseif v=="10.4" then
		v="10.4/9.7/8.8/7.6"
	end
	DictionarySpirit[k]=v
end


local DictionarySpiritPassive={
	["幽魂"]="根据等级+20/24/30",	["Wandering Wight"]="+20/24/30 by Lv",
	["广谋"]="根据等级+6/+8/+10",	["Guangmou"]="+6/+8/+10 by Lv",
	["虫总兵"]="根据等级+10/12/15",	["Commander Beetle"]="+10/12/15 by Lv",
	["百足虫"]=nil,["Centipede Guai"]=nil,
	["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
	["不空"]="根据等级+20/24/30 per Hit",["Non-Void"]="+20/24/30 per Hit by Lv",
	["不净"]="根据等级10秒内+6%/+8%/+10%减伤",["Non-Pure"]="+6%/+8%/+10% DamageReduction in 10s by Lv",
	["不白"]="根据等级+6/+8/+10",["Non-White"]="+6/+8/+10 by Lv",
	["不能"]="根据等级+2%/2.5%/3%暴击,+4%/5%/6%爆伤,+5/7/10攻击力,-50/75/100法力上限",["Non-Able"]="+2%/2.5%/3% CritRate,+4%/5%/6% CritDamage,+5/7/10 ATK,-50/75/100 MaxMP by Lv",

	["疯虎"]="根据等级+7/10/15攻击,-75/100/150生命上限",	["Mad Tiger"]="+7/10/15ATK,-75/100/150MaxHP By Lv",
	["百目真人"]="根据等级+10/12/15",	["Gore-Eye Daoist"]="+10/12/15 by Lv",
	["虎伥"]="根据等级+6%/8%/10%",	["Tiger's Acolyte"]="+6%/8%/10% by Lv",
	["地狼"]="根据等级+2/2.4/3 per Hit",["Earth Wolf"]="+2/2.4/3 per Hit by Lv",
	["波里个浪"]="根据等级-6%/8%/10%",["波波浪浪"]="根据等级-6%/8%/10%",["Baw-Li-Guhh-Lang"]="-6%/8%/10% by Lv",["Baw-Baw-Lang-Lang"]="-6%/8%/10% by Lv",
	["蛇司药"]="根据等级回复6%/8%/10%生命上限",["Snake Herbalist"]="6%/8%/10% MaxHP by Lv",
	["青冉冉"]="根据等级每6秒回复1.5%/1.75%/2%生命上限",	["Verdant Glow"]="1.5%/1.75%/2% MaxHP per 6 seconds by Lv",
	["蛇捕头"]="根据等级+5%/10%/25%攻击，-20%/40%/100%防御",	["Snake Sheriff"]="+5%/10%/25% ATK,-20%/40%/100% DEF at Lv1",
	--1.5 1.56
	["蜻蜓精"]="根据等级时+3%/4%/5元气获得,-40/50/60生命,-20/25/30法力,-20/25/30气力",	["Dragonfly Guai"]="+3%/4%/5% Qi Gain,-40/50/60 MaxHP,-20/25/30 MaxMP,-20/25/30 MaxSt by Lv",
	["虫校尉"]="根据等级+10/12/15",	["Beetle Captain"]="+10/12/15 by Lv",
	["蝎太子"]="根据等级+10/12/15",["Scorpion Prince"]="+10/12/15 by Lv",
	["泥塑金刚"]="根据等级+10/12/15",	["Clay Vajra"]="+10/12/15 by Lv",
	["赤发鬼"]="根据等级+10%/12%/15% 动作倍率",	["Red-Haired Yaksha"]="+10%/12%/15% SkillEffect",--33687/33487/33187 295/275/255
	["鸦香客"]="根据等级+10/12/15",	["Crow Diviner"]="+10/12/15 by Lv",
	["隼居士"]="根据等级+10/12/15",	["Falcon Hermit"]="+10/12/15 by Lv",
	["夜叉奴"]="根据等级+10/12/15 per Hit",	["Enslaved Yaksha"]="+10/12/15 per Hit by Lv",
	["菇男"]="根据等级6%/8%/10%",["Fungiman"]="6%/8%/10% by Lv",
	["巡山鬼"]=nil,	["Mountain Patroller"]=nil,--303686 254/274/294 1061410/1061430/1061460?
	["鼠司空"]="根据等级+10/12/15",	["Rat Governor"]="+10/12/15 by Lv",
	["骨悚然"]="根据等级+1%/2%/3%",	["Spearbone"]="+1%/2%/3% by Lv",
	--7.5/7.07/6.74  4.725
	["石双双"]="根据等级-6%/8%/10%气力消耗,+10%/12%/15% 跳跃轻击动作倍率",	["Poisestone"]="-6%/8%/10% Stamina Cost,+10%/12%/15% SkillEffect by Lv", --33681/33581 247/267/287
	["鼠禁卫"]="根据等级+6/8/10",["Rat Imperial Guard"]="+6/8/10 by Lv",
	["狸侍长"]="根据等级+5/7/10",["Civet Sergeant"]="+5/7/10 by Lv",
	["疾蝠"]="根据等级+10/12/15",["Swift Bat"]="+10/12/15 by Lv",
	--26->24.5/24/23.5
	["鼠弩手"]="根据等级减少每秒消耗-1.5/2/2.5，基础消耗26/s(移步48/s)",["Rat Archer"]="-1.5/2/2.5 per second cost by Lv. Base Cost 26/s(48/s when moving)",


	["牯都督"]="根据等级+10/12/15 ",["Bull Governor"]="+10/12/15 by Lv",
	--15/35/50/90/120, 14/33/47/85/113 14/32/46/83/110 ,14/32/45/81/108
	["火灵元母"]="根据等级-6%/8%/10%",["Mother of Flamlings"]="-6%/-8%/-10% by Lv",
	["燧先锋"]="根据等级+6/+8/+10",["Flint Vanguard"]="+6/+8/+10 by Lv",
	["琴螂仙"]="根据等级+4%/+5%/+6%",["Elder Amourworm"]="+4%/+5%/+6% by Lv",
	--50/60 44/54 42/52
	["九叶灵芝精"]="根据等级-6/8/10点消耗",["Nine-Capped Lingzhi Guai"]="-6/8/10 by Lv",
	["兴烘掀·掀烘兴"]="根据等级+30/36/45防御,-5/7/10攻击",["Top Takes Bottom, Bottom Takes Top"]="+30/36/45 DEF,-5/7/10 ATK by Lv",
	["雾里云·云里雾"]="根据等级-6/-8/-10",["Misty Cloud, Cloudy Mist"]="-6/8/10 by Lv",
	["地罗刹"]="根据等级10秒内+6%/8%/10%",["Earth Rakshasa"]="+6%/8%/10% in 10s by Lv",
	["鳖宝"]="根据等级+10%/12%/15% 动作倍率",["Turtle Treasure"]="+10%/12%/15% SkillEffect by Lv",--303184/33484 Passive 292/272
	["燧统领"]="根据等级+5/7/10",["Flint Chief"]="+5/7/10 by Lv",
	["黑脸鬼"]="根据等级+6%/8%/10%",["Charface"]="+6%/8%/10% by Lv",
	["石父"]="根据等级+1.5%/2%/2.5%暴击,+3%/4%/5%暴伤",["Father of Stones"]="+1.5%/2%/2.5% CritChance,+3%/4%/5% CritDamage by Lv",
	["沙二郎"]="生命值低于25%时根据等级+10/15/20攻击",["Second Rat Prince"]="+10/15/20 ATK under 25% MaxHP by Lv",
	--取平地普通移动再停止后最后一个数字，该数字是稳定的,观察PlayerLocomotion.MaxSpeed:650/663/669/682.5
	["百足虫"]="根据等级+2%/3%/5%,基础650",["Centipede Guai"]="+2%/3%/5% by Lv.Base 650",
	
	["蘑女"]="根据等级+20/24/30生命,-10/12/15气力",["Fungiwoman"]="+20/24/30生命，-10/12/15气力",
	["老人参精"]="根据等级+6%/8%/10%",["Old Ginseng Guai"]="+6%/8%/10% by Lv", --303157 33657/33457/33157 244/264/
	["儡蜱士"]="根据等级-10/15/20消耗",["Puppet Tick"]="-10/15/20 MP Cost by Lv",
	["傀蛛士"]="根据等级-6%/8%/10%神力消耗速度",["Puppet Spider"]="-6%/8%/10% Energy Cost Speed by Lv",
	["幽灯鬼"]="根据等级+6%/8%/10%",["Lantern Holder"]="+6%/8%/10% by Lv",
	["戒刀僧"]="根据等级10秒内+10/15/20攻击",["Blade Monk"]="+10/15/20 ATK in 10s by Lv",
	["狼刺客"]="根据等级+3%/4%/5%",["Wolf Assassin"]="+3%/4%/5% by Lv",
}

--酒、葫芦、泡酒物
local DictionaryGourd={
	["妙仙葫芦"]="20秒内+20攻击",			["Immortal Blessing Gourd"]="+20 ATK in 20s",
	["湘妃葫芦"]="15秒内+15抗性",			["Xiang River Goddess Gourd"]="+15 in 15s",
	["五鬼葫芦"]="20秒内+15攻击",			["Plaguebane Gourd"]="+15 ATK in 20s",
	["燃葫芦"]="15秒内+30气力",				["Firey Gourd"]="+30 Stamina in 15s",
	--2804 DmgAdditionBase 30
	["乾坤彩葫芦"]="30秒内+30%伤害加成",				["Multi-Glazed Gourd"]="+30% Damage in 30s",

	["椰子酒"]="实际回复28%+15",	["Coconut Wine"]="Actually 28%+15",
	["椰子酒·三年陈"]="实际回复32%+25",	["3-Year-Old Coconut Wine"]="Actually 32%+25",
	["椰子酒·五年陈"]="实际回复36%+33",	["5-Year-Old Coconut Wine"]="Actually 36%+33",
	["椰子酒·十年陈"]="实际回复39%+40",	["10-Year-Old Coconut Wine"]="Actually 39%+40",
	["椰子酒·十八年陈"]="实际回复42%+46",	["18-Year-Old Coconut Wine"]="Actually 42%+46",
	["椰子酒·三十年陈"]="实际回复45%+51",	["30-Year-Old Coconut Wine"]="Actually 45%+51",
	["猴儿酿"]="实际回复48%+55",	["Monkey Brew"]="Actually 48%+55",
	["玉液"]="的确是55%",	["Jade Dew"]="Indeed 55%",
	["蓝桥风月"]="15秒内+30%移动速度",	["Bluebridge Romance"]="+30% in 15s",--Buff-Talent 2816 写的是15%/15%/15% 实测 约+30%/+30%/约+30% ,why???

	["琼浆"]="+20",							["Jade Essence"]="+20",
	["无忧醑"]="低于20%血量时恢复量24%->60%",	["Worryfree Brew"]="24%->60% under 20% HP",
	--BuffDesc-Item 92023
	["九霞清醑"]="+15.0元气;+20法宝能量",					["Sunset of the Nine Skies"]="+15.0 Qi;+20.0 Treasure Energy",
	["松醪"]="+75.0",						["Pinebrew"]="+75.0",

	["龙膏"]="6秒内+12%攻击",				["Loong Balm"]="+12 ATK in 6s",
	["龟泪"]="满血时+20法力",				["Turtle Tear"]="+20 when 100% HP",
	["瑶池莲子"]="5秒内共回复6%上限(1%*6)",		["Celestial Lotus Seeds"]="total 6% MaxHP(1%*6) in 5s",
	["虎舍利"]="15秒内+5%",					["Tiger Relic"]="+5% in 15s",--92306
	["梭罗琼芽"]="15秒内+10% ",				["Laurel Buds"]="+10% in 15s",
	["铁弹"]="30%减伤",						["Iron Pellet"]="30% Damage Reduction",
	["紫纹缃核"]="低于20%血量时额外回复+12%生命上限",	["Purple-Veined Peach Pit"]="+20% MaxHP when under 20% HP",
	["双冠血"]="15秒内+5%暴击,+12%移动速度",				["Double-Combed Rooster Blood"]="+5% CritRate/+12% Speed in 15s",
	["嫩玉藕"]="15秒内+5%防御",				["Tender Jade Lotus"]="+5% in 15s",
	["铁骨银参"]="+30",						["Steel Ginseng"]="+30",
	["胆中珠"]="15秒内+15",					["Gall Gem"]="+15 in 15s",
	["霹雳角"]="15秒内+15",					["Thunderbolt Horn"]="+15 in 15s",
	["甜雪"]="15秒内+15",					["Sweet Ice"]="+15 in 15s",
	--Item 92317 92327 92328
	["瞌睡虫蜕"]="15秒内踉跄;定身法-2秒，安神法-5秒，聚形散气-3秒，铜头铁臂-2秒，毫毛-8秒",					["Slumbering Beetle Husk"]="Stagger in 15s;Immobilize -2s,Ring of Fire -5s,Cloud Step -3s,Rock Solid-2s,A Pluck of Many -8s",
	["清虚道果"]="15秒内+0.066秒无敌，0-3段翻滚基础时间0.4/0.433/0.466/0.5秒",					["Fruit of Dao"]="+0.066s in 15s; 0-3 level roll base immue time:0.4/0.433/0.466/0.5",--92320 Passive 199 10105-10109
	--Talent 2100 AtkMul 20% 时间10秒，是10秒内施放隐身还是10秒内打出破隐?
	["十二重楼胶"]="10秒内？+20%攻击",					["Breath of Fire"]="+20% ATK in 10s?",
	["蜂山石髓"]="15%？",					["Bee Mountain Stone"]="15%？",
	["灵台药苗"]="10秒内+25%持续时间",					["Moutain Lingtai Seedlings"]="25% Duration in 10s", --92305 Passive 193 
	["铜丸"]="10秒",					["Copper Pill"]="10s",

	["血杞子"]="15秒内+5秒持续",						["Goji Shoots"]="+5s Duration in 15s", --92319? Passive 198
	["困龙须"]="10秒内+2秒持续",						["Stranded Loong's Whisker"]="+2s Duration in 10s",--92304 Passive 192
	["不老藤"]="回复33.33%气力上限,15秒内+50%气力回复",	["Undying Vine"]="Recover 33.33% MaxSt.+50% Stamina Recover in 15s",
	["蕙性兰"]="11秒内共回复12%生命上限(1%*12)",	["Graceful Orchid"]="total 12% MaxHP(1%*12) in 11s",
	["青山骨"]="30秒内+30生命上限",			["Goat Skull"]="+30 in 30s",
	["火焰丹头"]="15秒内+15",				["Flame Mediator"]="+15 in 15s",
}

--珍玩，消耗品
local DictionaryItem={
	--Item 96035 0.1?不是加在属性上，而是获得一个buff
	["君子牌"]="about 0.1/s",	["Virtuous Bamboo Engraving"]="about 0.1/s", 
	["白狐毫"]="-20%神力消耗速度",	["Snow Fox Brush"]="-20% Energy consume speed",
	["耐雪枝"]="+12",	["Frostsprout Twig"]="+12",
	["错金银带钩"]="+12",	["Cuo Jin-Yin Belt Hook"]="+12",
	["猫睛宝串"]="+3%",	["Cat Eye Beads"]="+3%",
	["金花玉萼"]="+10%",	["Goldflora Hairpin"]="+10%",
	["仙箓"]="+10%",	["Celestial Registry Tablet"]="+10%",
	["金色鲤"]="+2%",	["Golden Carp"]="+2%",
	["金棕衣"]="+32防御,每次造成10%天命人攻击力的伤害",	["Gold Spikeplate"]="+32 DEF.Deal x0.1 player attack Damage per Hit",

	["月玉兔"]="+1%,有日金乌时+3%",["Jade Moon Rabbit"]="+1%, +3% if have Gold Sun",
	["日金乌"]="+1%,有月玉兔时+3%",	["Gold Sun Crow"]="+1%, +3% if have Jade Moon",
	["水火篮"]="+10",	["Daoist's Basket of Fire and Water"]="+10",
	["辟水珠"]="+8%防御",	["Waterward Orb"]="+8% DEF",
	["琥珀念珠"]="1-4段棍势上限由100/210/330/480变为90/200/320/470",	["Amber Prayer Beads"]="Lv.1-4 Max focus point:100/210/330/480 -> 90/200/320/470",
	["博山炉"]="+15%",	["Boshan Censer"]="+15%",
	["不求人"]="+30",	["Back Scratcher"]="+30",
	["兽与佛"]="+9%",	["Beast Buddha"]="+9%",
	["铜佛坠"]="+5",	["Bronze Buddha Pendant"]="+5",
	["卵中骨"]="+10",	["Spine in the Sack"]="+10",
	["白贝腰链"]="+5",	["White Seashell Waist Chain"]="+5",
	["细磁茶盂"]="+8",	["Fine China Tea Bowl"]="+8",
	["虎头牌"]="6秒内+8%攻击",	["Tiger Tally"]="+8% ATK in 6s",
	["砗磲佩"]="身法-1秒/奇术-3秒/毫毛-6s",	["Tridacna Pendant"]="-1s/3s/6s by type",
	["琉璃舍利瓶"]="+45",	["Glazed Reliquary"]="+45",
	["三清令"]="生命不足一半时+10%",	["Tablet of the Three Supreme"]="+10% under 50% HP",
	["金钮"]="满血时+15攻击",	["Gold Button"]="+15 when full HP",
	["阳燧珠"]="+12",	["Flame Orb"]="+12",
	["摩尼珠"]="15秒内+8%",	["Mani Bead"]="+8% in 15s",

	["玛瑙罐"]="+14",	["Agate Jar"]="+14",
	["雷榍"]="+12",	["Thunderstone"]="+12",
	["雷火印"]="+30",	["Thunderflame Seal"]="+30",
	["吉祥灯"]="20秒内-80生命上限，+10%攻击",	["Auspicious Lantern"]="-80 MaxHP,+10% ATK in 20s",
	["定颜珠"]="180秒内+60生命上限,+40法力/气力上限",	["Preservation Orb"]="+60 MaxHP,+40 MaxMP/MaxSt in 180s",
	["虎筋绦子"]="6秒内+12%攻击",	["Tiger Tendon Belt"]="+12% ATK in 6s",
	["未来珠"]="100次?",	["Maitreya's Orb"]="100 times?",--buff-item 96010-96012

	--695.5
	["风铎"]="+7% 移动速度",	["Wind Chime"]="+7% move speed", --1006008 buff-desc 96008 写的是7%实测也是7%
	--消耗品
	--鼻根器+10%持续2
	["朝元膏"]="120秒内+60",	["Essence Decoction"]="+60 in 130s",
	["益气膏"]="120秒内+60",	["Tonifying Decoction"]="+60 in 130s",
	["延寿膏"]="120秒内+60",	["Longevity Decoction"]="+60 in 130s",

	["登仙散"]="75秒",	["Ascension Powder"]="75s",
	["龙光倍力丸"]="75秒内+15攻击15%暴击20%暴伤",	["Loong Aura Amplification Pellets"]="+15 ATK/15% Crit/20% CritDamage in 75s",
	["加味参势丸"]="+480",	["Enhanced Ginseng Pellets"]="+480",
	["聚珍伏虎丸"]="75秒内+15攻击",	["Enhanced Tiger Subduing Pellets"]="+15 ATK in 75s",
	["轻身散"]="120秒",	["Body-Fleeting Powder"]="+60 in 130s",

	["倍力丸"]="75秒内+10%",	["Amplification Pellets"]="+10% in 75s",
	["伏虎丸"]="75秒内+8攻击",	["Tiger Subduing Pellets"]="+8 ATK in 75s",
	["坚骨药"]="75秒",	["Fortifying Medicament"]="75s",
	["温里散"]="75秒内+30",	["Body-Warming Powder"]="+30 in 75s",
	["度瘴散"]="75秒内+30",	["Antimiasma Powder"]="+30 in 75s",
	["神霄散"]="75秒内+30",	["Shock-Quelling Powder"]="+30 in 75s",
	["清凉散"]="75秒内+30",	["Body-Cooling Powder"]="+30 in 75s",
	["避凶药"]="75秒内+10%",	["Evil Repelling Medicament"]="+10% in 75s",
	["镜中丹"]="+60",	["Mirage Pill"]="+60",
	--["九转还魂丹"]="120秒",	["Tonifying Decoction"]="130s",

	["参势丸"]="30秒内+10/秒",	["Ginseng Pellets"]="+10/s in 30s",
}

--套装效果
local DictionarySuit={
	["心灵福至"]="每个天赋+24防御",	["Fortune's Favor"]="+24 DEF per Relic Talent",
	--650->702，+满级百足734.5
	--Talent 2041-2044 写的是0/8/10,实测是0/8%/约10%
	["日行千里"]="+8%/10%奔跑/冲刺速度;\n每层+10%攻击,持续2秒;\n[*]:每秒+12棍势",	["Swift Pilgrim"]="+8%/10% Run/Sprint speed.\n+10% ATK per stack in 2s\n[*]:+12 Focus/s"	,
	--96005/96006 实测每次减少0.75~1秒冷却不定？？非传奇和传奇没有区别？？
	["举步生风"]="+15棍势,-0.75~1秒冷却?\n[*]:不会额外减少冷却",			["Gale Guardian"]="+15 Focus,-0.75s~1s CD？\n[*]:Won't reduce more CD" ,
	["花下死"]="x0.8毒伤(和抗性效果乘算),+20%攻击",["Poison Ward"]="x0.8 Poison Damage(Multi with \nPoison Resist effect).+20% ATK"	,
	["试比天高"]="10秒内+8%暴击;-1秒冷却 per Hit",	["Heaven's Equal"]="+8% Crit in 10s;-1s CD per Hit"	,
	["毒魔狠怪"]="+100棍势，+20%持续时间",["Fuban Strength"]="+100 focus.+20% Duration"	,--独角仙套 91912 Passive 185
	["锱铢必较"]="+10%灵蕴",		["Every Bit Counts"]="+10% Will"	,
	--Talent 2037 ->90301?,鳞棍2034->15003?
	["浪里白条"]="x0.5气力消耗(和天赋效果乘算)",["Wave-Rider"]="x0.5 Stamina Cost(Multi with talent effect)",
	["龙血玄黄"]="+10赋雷攻击\n[*]:+10赋雷攻击",		["Thunder Veins"]="+10 Thunder ATK\n[*]:+10 Thunder ATK",	
	["借假修真"]="20秒内+15%攻击,暴击+3元气,\n击杀+5元气",["Gilded Radiance"]="+15% ATK in 20s. +3/+5 Qi when Crit/Kill",	
	["百折不挠"]="5秒内+10%防御",	["Unyielding Resolve"]="+10% DEF in 5s",	
	["离火入魔"]="[2]+20% 动作倍率 \n[4]+25%伤害 -30%伤害减免",["Outrage"]="[2]+20% SkillEffect \n[4]+25% Damage.-30% DamageReduction",	--90711 Passive 167
	--Talent 2135 -0.005，实测变身还原+1.5每秒 ；2137 -0.00375,实测约1.12每秒，结合-0.005推测应为1.125/s
	["泥塑金装"]="[2]黑泥期间+20%伤害减免，翻滚回复约0.3神力;\n[4]结束变身时获得12秒黑泥，期间+1.5/s神力回复;\n化身还原后获得6秒黑泥，前4秒内+1.125/s神力回复",["From Mud to Lotus"]="[2]+20% DamageReduction;Gain about 0.3 Might upon roll;\n[4] Gain Mud and +1.5/s Might Recover in 12s upon quiting\n tranformation;Gain Mud in 6s and +1.125/s Might\n Recover in 4s upon quiting vigor.",	
	["铜心铁胆"]="+50棍势;-5秒冷却",			["Iron Will"]="+50 Focus;-5s CD",	
	["乘风乱舞"]="[*]:+20法力消耗;假身持续时间\n不变,但不会因破隐而消失",			["Dance of the Black Wind"]="[*]:+20 MP Cost",	
	--Talent 2063
	["走石飞砂"]="20%减伤",			["Raging Sandstorm"]="20% Damage Reduction",	

	--Talent 2181 青铜套内部叫黑铁
	["炼魔荡怪"]="-15秒冷却",["Evil Crasher"]="-15s CD",
}
--独门妙用
local DictionaryEquip={
	["大力王面"]="+10棍势，强硬时+30",		["Bull King's Mask"]="+10 Focus.+30 when Tenacity",	
	["锦鳞战袍"]="低于半血时每秒+1.5%生命上限(水中+2%)",["Serpentscale Battlerobe"]="+1.5%MaxHP/s under 50% MaxHP;+2% MaxHP/s in water",	
	["厌火夜叉面"]="低于半血时+15攻击",		["Yaksha Mask of Outrage"]="+15ATK under 50% HP",	
	["金身怒目面"]="+100棍势",				["Golden Mask of Fury"]="+100",
	["长嘴脸"]="饮酒后15秒内+10攻击,20秒后-20攻击",["Snout Mask"]="+10 ATK in 15s;-20 ATK after 20s",
	["鳖宝头骨"]="+2%",						["Skull of Turtle Treasure"]="+2%",
	["地灵伞盖"]="高于1%血量时每秒-3HP,额外回复+15%生命上限",["Earth Spirit Cap"]="-3HP/s when over 1% HP.+15% MaxHP Recover",
	["昆蚑毒敌甲"]="+15",					["Venomous Sting Insect Armor"]="+15",
	["阴阳法衣"]="阳:+20%伤害减免。阴:20%暴击-30%伤害减免",["Yin-Yang Daoist Robe"]="Yang:+20% DamageReduction. Yin:+20% Crit,-30% DamageReduction",
	["山珍蓑衣"]="15秒内+30",				["Ginseng Cape"]="+30 in 15s",
	["玄铁硬手"]="3秒内+15%攻击",			["Iron-Tough Gauntlets"]="+15% ATK in 3s",
	["金刚护臂"]="-10%",					["Vajra Armguard"]="-10%",
	["赭黄臂甲"]="每消耗一段棍势8秒内+6%暴击",["Ochre Armguard"]="+6% in 8s for each focus level",
	["不净泥足"]="10秒",					["Non-Pure Greaves"]="10s",
	["藏风护腿"]="+10",						["Galeguard Greaves"]="+10",
	--Talent 2713/2714
	["南海念珠"]="300秒内+40生命/法力上限",		["Guanyin's Prayer Beads"]="+40 MaxHP/MP in 300s",
	--Talent 2142
	["羽士戗金甲"]="20秒内+10%攻击",		["Centipede Qiang-Jin Armor"]="+10%ATK in 20s",
	--Talent 2098
	["乌金行缠"]="+15%攻击",		["Ebongold Gaiters"]="+15%ATK",
	--Talent 2702
	["白脸子"]="+15%攻击",		["Grey Wolf Mask"]="+15%ATK",
	--蜢虫头907005 97005、97015 185、186
	["长须头面"]="+15%/20% 动作倍率",		["Locust Antennae Mask"]="+15%/20% SkillEffect",

	["狼牙棒"]="5秒内获得共127.5棍势(2.5+25*5)",		["Spikeshaft Staff"]="Gain 127.5(2.5+25*5) Focus in 5s",
	["昆棍·百眼"]="每段棍势+5HP(中毒敌人+40)",		["Visionary Centipede Staff"]="+5 HP(40 for Poisoned enemy) for each focus level",
	["昆棍·蛛仙"]="每段棍势+5HP",		["Spider Celestial Staff"]="+5 HP for each focus level",
	["昆棍"]="每段棍势+5HP",		["Chitin Staff"]="+5 HP for each focus level",
	["昆棍·通天"]="每段棍势+5HP",		["Adept Spine-Shooting Fuban Staff"]="+5 HP for each focus level",
	["混铁棍"]="每点防御+0.15攻击",		["Dark Iron Staff"]="+0.15ATK per DEF",

	["飞龙宝杖"]="+20% 动作倍率",		["Golden Loong Staff"]="+20% SkillEffect",--15015 145
	["三尖两刃枪"]="+15% 动作倍率",		["Tri-Point Double-Edged Spear"]="+15% SkillEffect",--15015 145
}
--[[ 
--Fuck chinese characters,Fuck encoding
local x="见机强攻"
local y="‌见机强攻"
print("-- Group1:")
print(tostring(x==y))--false
local x="‌见机强攻"
local y="‌见机强攻"
print("-- Group2:")
print(tostring(x==y))--true
]]--

local DictionaryRelic={
	["见机强攻"]="轻棍1-5段全中获得的棍势由15/18/17/28/40变为15/18/26/35/43",	["Opportune Watcher"]="Light Attack 1-5 focus point if all hit :15/18/17/28/40 -> 15/18/26/35/43",
	--buff talent 1053
	["眼乖手疾"]="-12.5秒",	["Eagle Eye"]="-12.5s",
	["慧眼圆睁"]="+15%",	["Keen Insight"]="+15%",
	
	["耳听八方"]="+0.066秒,基础0.4(劈棍)/0.366(戳棍)/0.3(不明)/0.5(不明)秒",	["All Ears"]="+0.066s,Base 0.4(Smash)/0.366(Pillar)/0.3(Unknown)/0.5(Unknown) second",--20201 Passive 121 287,293,114,10110
	["如撞金钟"]="-0.1秒，基础1秒",	["Sound as A Bell"]="-0.1s,Base 1s", -- Ｐａｓｓｉｖｅ　122-125　Buff-lz 228 buff-talent 1069
	["耳畔风响"]="5秒内+10%攻击",	["Whistling Wind"]="+10% ATK in 5s",

	["气味相投"]="15秒内+10%伤害加成",	["Lingering Aroma"]="+10% Damage in 15s", --buff 2107
	--["阳燧珠"]="+12",	["In One Breath"]="+12", -
	["屏气敛息"]="+0.066秒,0-3段翻滚基础无敌时间0.4/0.433/0.466/0.5秒",	["Hold Breath"]="+0.066s,0-3 level roll base time 0.4/0.433/0.466/0.5s",---20303 Passive  127 buff-lz  10105-10109 
	
	["舌尝思"]="+10%丹药持续时间",	["Envious Tongue"]="+10% Duration",
	["丹满力足"]="5秒内+15%",	["Refreshing Taste"]="+15% in 5s",
	--["阳燧珠"]="+12",	["Spread the Word"]="+12",
	["遍尝百草"]="每个增加+4%生命上限的回复量",	["Tongue of A Connoisseur"]="+4% MaxHP recover each",

	["身轻体快"]="0-4级棍势气力消耗-20/25/37.5/60,令熟谙天赋效果减半",	["Nimble Body"]="0-4 Focus-Level heavy-attack cost :-20/25/37.5/60. Reduce Instinct(talent) effect by half",
	["福寿长臻"]="+60",	["Everlasting Vitality"]="+60",
	["灾愆不侵"]="+15",	["Divine Safeguard"]="+15",

	["万相归真"]="+5%/10% 动作倍率",	["Elegance in Simplicity"]="+5%/10% SkillEffect",
	["不生不灭"]="30秒内+30%伤害加成",	["Unbegotten, Undying"]="+30% Damage in 30s",

	--法宝激活效果
	["辟火罩"]="40秒内+600火抗，每秒获得5棍势",["Fireproof Mantle"]="+600 FireResist,+5 Focus/s in 40s",
	["定风珠"]="20秒内+50%伤害减免",["Wind Tamer"]="+50% DamageReduction in 20s",
}
local DictionaryTreasurePassive={
	["辟火罩"]="+30",["Fireproof Mantle"]="+30",
	["定风珠"]="+2%",["Wind Tamer"]="+2%",
	["绣花针"]="+2%暴击+6%暴伤",["Weaver's Needle"]="+2%Crit +6% CritDamage",
	["芭蕉扇"]="+10",["Plantain Fan"]="+10",
}

--Merge All dic to Dictionary
local DictionaryMerge={}
local DictionaryMergeSecondary={}
for k,v in pairs(Dictionary) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionaryGourd) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionaryItem) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionaryRelic) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionarySuit) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionaryEquip) do
	DictionaryMerge[k]=v
end
for k,v in pairs(DictionarySpirit) do
	DictionaryMerge[k]=v
end
--因为有重复key，放到secondary
for k,v in pairs(DictionaryTreasurePassive) do
	DictionaryMergeSecondary[k]=v
end
for k,v in pairs(DictionarySpiritPassive) do
	DictionaryMergeSecondary[k]=v
end


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

local detailtextMerge=nil
local detailtextMergeSecondary=nil

--目前看来，这些title一定在text之前设置。只有套装效果例外
local MergeTitleList43={ -- Gourd/Wine/WineMat/Relic ....
["etTree.BI_HuluDetail.WidgetTree.TxtNameRuby"]=true,--装备界面 葫芦
["etTree.BI_WineDetail.WidgetTree.TxtNameRuby"]=true,--泡制界面 酒
["ree.BI_WineMatDetail.WidgetTree.TxtNameRuby"]=true,--泡制界面 泡酒物
["l.WidgetTree.BI_WineDesc.WidgetTree.TxtName"]=true,--装备界面 酒
["Tree.BI_GenqiDecs_1.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["Tree.BI_GenqiDecs_2.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["Tree.BI_GenqiDecs_3.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["Tree.BI_GenqiDetail.WidgetTree.TxtGenqiName"]=true,--根器名
["ree.BI_JewelryDetail.WidgetTree.TxtNameRuby"]=true,--珍玩
["etTree.BI_ItemDetail.WidgetTree.TxtNameRuby"]=true,--行囊
["idgetTree.BI_SpellDetail.WidgetTree.TxtName"]=true,--天赋
["tTree.BI_EquipDetail.WidgetTree.TxtNameRuby"]=true,--装备名
["getTree.BI_RZDDetail.WidgetTree.TxtNameRuby"]=true,--精魂名
["ee.BI_TreasureDetail.WidgetTree.TxtNameRuby"]=true,--法宝名
}
local MergeDetailList43={
	["etTree.BI_HuluDetail.WidgetTree.TxtHuluDesc"]=true,--装备界面 葫芦效果
	["etTree.BI_WineDetail.WidgetTree.TxtWineDesc"]=true,--泡制界面 酒效果
	["e.BI_WineMatDetail.WidgetTree.TxtEffectDesc"]=true,--泡制界面 泡酒物效果
	["l.WidgetTree.BI_WineDesc.WidgetTree.TxtDesc"]=true,--装备界面 酒
	["ee.BI_GenqiDecs_1.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["ee.BI_GenqiDecs_2.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["ee.BI_GenqiDecs_3.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["ee.BI_GenqiDetail.WidgetTree.TxtGenqiBSkill"]=true,--根器主效果
	["e.BI_JewelryDetail.WidgetTree.TxtEffectDesc"]=true,--珍玩
	["Tree.BI_SpellDetail.WidgetTree.TxtSpellDesc"]=true,--天赋
	["Tree.BI_ItemDetail.WidgetTree.TxtEffectDesc"]=true,--行囊
	["ree.BI_EquipDetail.WidgetTree.TxtLegendDesc"]=true,--装备独门妙用
	[".WidgetTree.BI_RZDDetail.WidgetTree.TxtCost"]=true,--精魂能量消耗
	[".BI_TreasureDetail.WidgetTree.TxtActiveDesc"]=true,--法宝激活效果
	}
local MergeDetailList43Secondary={
	["tTree.TreasureEqDesc.WidgetTree.TxtSuitDesc"]=true,--精魂被动
	}

--local lastSuitDescWidget=nil
--local lastSuitDescWidgetText=nil
function HookTextBlockSetText(Context,InText)
	local name=Context:get():GetFullName()
	if type(name) ~= "string" then return end
	local name43=name:sub(-43)

	if MergeTitleList43[name43]==true then --DictionaryMerge
		local t=InText:get():ToString()
		detailtextMerge=DictionaryMerge[t]
		detailtextMergeSecondary=DictionaryMergeSecondary[t]

	elseif detailtextMerge~=nil and MergeDetailList43[name43]==true then --DictionaryMerge
		InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
		detailtextMerge=nil
	--elseif detailtextMergeSecondary~=nil and MergeDetailList43Secondary[name43]==true then --DictionaryMerge
	--	InText:set(FText(InText:get():ToString().."("..detailtextMergeSecondary..")"))
	--	detailtextMergeSecondary=nil

	elseif name:sub(-7)=="TxtName" then
		if name:find("WidgetTree.BI_HuluDetail.WidgetTree.BI_MatDesc.WidgetTree.BI_SoakDesc_C") --装备界面泡酒物名字
			or name:find("WidgetTree.BI_MaterialListItem.WidgetTree.BI_MaterialListItem_V2_C") then
			detailtextMerge=DictionaryMerge[InText:get():ToString()]
		end
	elseif detailtextMerge~=nil and name:sub(-7)=="TxtDesc" then --装备界面泡酒物效果
		if  name:find("WidgetTree.BI_HuluDetail.WidgetTree.BI_MatDesc.WidgetTree.BI_SoakDesc_C")
			or name:find("WidgetTree.BI_MaterialListItem.WidgetTree.BI_MaterialListItem_V2_C") then
			InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
			detailtextMerge=nil
		end

	elseif name43=="Tree.BI_EquipDetail.WidgetTree.TxtSuitTitle" then --套装名，套装效果在名字之前被Set
		local suitTitle=InText:get():ToString()
		local suitDesc=DictionarySuit[suitTitle]
		--if lastSuitDescWidget~=nil and lastSuitDescWidget:IsValid() and suitDesc~=nil then
		if suitDesc~=nil then
			--lastSuitDescWidget:SetText(FText(lastSuitDescWidgetText.."("..suitDesc..")")) --此处SetText又会触发一次hook并设置lastSuitDescWidget,要在SetText后清零
			--改单条套装效果文本无法控制出现在哪条上，改成直接放在套装名上，可以\n换行
			InText:set(FText(InText:get():ToString().."\n("..suitDesc..")"))
			--lastSuitDescWidget=nil
			--lastSuitDescWidgetText=nil
			--print("Set")
		end

	--elseif InText:get():ToString():find("辟火罩") then
	--	print("C.."..tostring(name).." "..name43)
	--elseif InText:get():ToString():find("较大增加") then
	--	print("D.."..tostring(name).." "..name43)		
	end
	--if InText:get():ToString():find("较大增加") then
	--	print("E.."..tostring(name).." "..name43)		
	--end
	--print(".."..tostring(InText:get():ToString()))
end
--["tTree.BI_EquipDetail.WidgetTree.TxtNameRuby"]=true,--装备名 Rich
--套装效果名not rich: /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147470897.WidgetTree.BUI_EquipMain_C_2147427437.WidgetTree.BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.TxtSuitTitle Tree.BI_EquipDetail.WidgetTree.TxtSuitTitle
--RichTextBlock       /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147470897.WidgetTree.BUI_EquipMain_C_2147427437.WidgetTree.BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.BI_SuitDesc.WidgetTree.BI_SuitDesc_C_2147422605.WidgetTree.TxtSuitDesc
function HookRichTextBlockSetText(Context,InText)
	local name=Context:get():GetFullName()
	if type(name) ~= "string" then return end
	local name43=name:sub(-43)

	if MergeTitleList43[name43]==true then	--DictionaryMerge/DictionaryMergeSecondary中的名字
		local t=InText:get():ToString()
		detailtextMerge=DictionaryMerge[t]
		detailtextMergeSecondary=DictionaryMergeSecondary[t]
		
	elseif detailtextMergeSecondary~=nil then
		if MergeDetailList43Secondary[name43]==true then --DictionaryMergeSecondary
			InText:set(FText(InText:get():ToString().."("..detailtextMergeSecondary..")"))
			detailtextMergeSecondary=nil
		elseif name:sub(-11)=="TxtSuitDesc" and name:find("BI_TreasureDetail.WidgetTree.TreasureEqDesc.WidgetTree.BI_TreasureEqDesc_C") then--法宝被动
			InText:set(FText(InText:get():ToString().."("..detailtextMergeSecondary..")"))
			detailtextMergeSecondary=nil
		end

	elseif detailtextMerge~=nil then
		if MergeDetailList43[name43]==true then	--DictionaryMerge
			InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
			detailtextMerge=nil
		elseif name:find("WidgetTree.BI_MaterialListItem.WidgetTree.BI_MaterialListItem_V2_C") then--泡制界面泡酒物缩略效果
			InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
			detailtextMerge=nil
		end
	--elseif InText:get():ToString():find("辟火罩") then
	--	print("A.."..tostring(name).." "..name43)
	--elseif InText:get():ToString():find("较大增加") then
	--	print("B.."..tostring(name).." "..name43)
	end
	--if InText:get():ToString():find("较大增加") then
	--	print("F.."..tostring(name).." "..name43)		
	--end
end

local hooked=false

--RegisterHook("/Script/Engine.PlayerController:ClientRestart", function()
--	if false and not hooked then
RegisterHook("/Script/UMG.TextBlock:SetText",function(Context,InText)
	--Can pcall prevent the mystery crash??
	success,res = pcall(HookTextBlockSetText,Context,InText)
	if not success then
		print(ModName.." Fuck "..res)
	end
end)
RegisterHook("/Script/UMG.RichTextBlock:SetText",function(Context,InText)
	success,res = pcall(HookRichTextBlockSetText,Context,InText)
	if not success then
		print(ModName.." Fuck "..res)
	end
end)
