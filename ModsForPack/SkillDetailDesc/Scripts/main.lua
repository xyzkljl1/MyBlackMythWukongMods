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

	--武艺
	["接力"]="18->23 per Hit",
	["化吉"]="7.5->8.7",
	["筋节"]="30%",
	["应手"]="-30%, 基础26/秒(移步48/秒)",
	["得心"]="4->6 per Hit, 不影响原地棍花(3 per Hit)",
	--体段修行
	["吐纳绵长"]="+2.5气力回复/lv",
	["五脏坚固"]="+10/lv",
	["气海充盈"]="+10/lv",
	--["灾苦消减"]="？/lv",
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
	["熟谙"]="每级令0-4级重棍气力消耗-4/-5/-7.5/-10/-10，有身根器的减耗效果时该天赋效果减半，0-4棍势等级基础消耗55/65/90/115/115",
	["克刚"]="Lv1:+20%/Lv2:+30%",
	["抖擞"]="+100",
	["骋势"]="+1% 每层",
	["拂痒"]="20%",

	["精壮"]="+5%/lv,基础55点棍势/秒",
	--奇术
	["不破不立"]="+15MP",
	["假借须臾"]="50MP->80MP,+2秒持续",
	["烈烈"]="20/s->23/s",
	["昂扬"]="+30% -> 40+30%",
	["弥坚"]="Lv1:+10/Lv2:+15 每跳",
	["归根"]="+20%",
	["无挂无碍"]="60MP->90MP, 5秒持续",

	["舍得"]="20%生命上限",
	["不休"]="-10%/lv",
	["智力高强"]="每级+0.02攻击力/1法力，基础0.12攻击力/1法力",
	["放手一搏"]="0.03%暴击/1气力",
	["凝滞"]="+0.16秒/lv，基础8秒",
	["瞬机"]="+60%持续",	
	["圆明"]="+2/4/5秒，基础20.5秒",

	--身法
	["洞察"]="Lv1:+3%/Lv2:+4% 每层",
	["破影一击"]="30MP->40MP",
	["频频"]="15s->13s",
	["巩固"]="+15%,持续15秒",
	["知机"]="+75",
	["厉无咎"]="Lv1:10MP/Lv2:15MP",
	["养气"]="+2秒/lv,基础10秒",

	--毫毛
	["玄同"]="积攒效率为本体的20%",
	["毛吞大海"]="20MP/lv",
	--变化
	["虚相凶猛"]="+2/lv",
	["炼实返虚"]="+15/lv",
	["磊磊"]="+15/lv",
	["剪尾"]="-10%",
	["红眼"]="+1 每层",
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
	["Silver Lining"]="7.5->8.7",

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
	["Instinct"]="Each level let 0-4 Focus-Level heavy-attack cost: -4/-5/-7.5/-10/-10. Only have 50% effect when having Nimble Body(body relic).Base cost of each focus-level:55/65/90/115/115",
	["Ironbound Resolve"]="Lv1:+20%/Lv2:+30%",
	["Invigoration"]="+100",
	["Force Cascade"]="+1% per stack",
	["Gale's Blessing"]="20%",
	["Quick Hand"]="+5%/lv,Base 55 focus point/s",
	--奇术
	["Spirit Shards"]="+15MP",
	["Time Bargain"]="50MP->80MP, +2s",
	["Raging Flames"]="20/s->23/s",
	["Burning Zeal"]="+30% -> 40+30%",
	["Consolidation"]="Lv1:+10/Lv2:+15 per tick",
	["Flame's Embrace"]="+20%",
	["Boundless Blessings"]="60MP->90MP, 5s",

	["Foregone, Foregained"]="20%MaxHP",
	["Light Heart"]="-10%/lv",
	["Smart Move"]="每级+0.02攻击力/1法力，基础0.12攻击力/1法力",
	["All or Nothing"]="0.03%暴击/1气力",
	
	["Stagnation"]="+0.16s/lv, Base 8s",
	["Evanescence"]="+60% duration",	
	["Flaring Dharma"]="+2/4/5s，Base 20.5s",
	--身法
	["Concealed Observation"]="Lv1:+3%/Lv2:+4% per stack",
	["Absolute Strike"]="30MP->40MP",
	["Rock Mastery"]="15s->13s",
	["Ironclad"]="+15% for 15s",
	["Nick of Time"]="+75",
	["Bold Venture"]="Lv1:10MP/Lv2:15MP",
	["Ruse"]="+2s/lv,Base10s",

	--毫毛
	["Harmony"]="20% efficiency comparing to player",
	["Multitude"]="20MP/lv",
	--变化
	["Ferocious Form"]="+2/lv",
	["Enduring Form"]="+15/lv",
	["Steadfast"]="+15/lv",
	["Cold Edge"]="-10%",
	["Hidden Might"]="+3.3%/lv",
	["Red Eyes"]="+1 per stack",
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
local DictionarySpiritPassive={
	["幽魂"]="根据等级+18/24/30",	["Wandering Wight"]="+18/24/30 by Lv",
	["广谋"]="根据等级+6/+8/+10",	["Guangmou"]="+6/+8/+10 by Lv",
	["虫总兵"]="根据等级+10/12/15",	["Commander Beetle"]="+10/12/15 by Lv",
	["百足虫"]=nil,["Centipede Guai"]=nil,
	["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
	["不空"]="根据等级+18/24/30 per Hit",["Non-Void"]="+18/24/30 per Hit by Lv",
	["不净"]="根据等级10秒内+6%/+8%/+10%减伤",["Non-Pure"]="+6%/+8%/+10% DamageReduction in 10s by Lv",
	["不白"]="根据等级+6/+8/+10",["Non-White"]="+6/+8/+10 by Lv",
	["不能"]="根据等级+2%/2.5%/3%暴击,+4%/5%/6%爆伤,+5/7/10攻击力,-50/75/100法力上限",["Non-Able"]="+2%/2.5%/3% CritRate,+4%/5%/6% CritDamage,+5/7/10 ATK,-50/75/100 MaxMP by Lv",

	["疯虎"]="根据等级+7/10/15攻击,-75/100/150生命上限",	["Mad Tiger"]="+7/10/15ATK,-75/100/150MaxHP By Lv",
	["百目真人"]="根据等级+10/12/15",	["Gore-Eye Daoist"]="+10/12/15 by Lv",
	["虎伥"]="根据等级+6%/8%/10%",	["Tiger's Acolyte"]="+6%/8%/10% by Lv",
	["地狼"]="根据等级+2/2.4/3 per Hit",["Earth Wolf"]="+2/2.4/3 per Hit by Lv",
	["波里个浪"]="根据等级-6%/8%/10%",["Baw-Li-Guhh-Lang"]="-6%/8%/10% by Lv",
	["蛇司药"]="根据等级回复6%/8%/10%生命上限",["Snake Herbalist"]="6%/8%/10% MaxHP by Lv",
	["青冉冉"]="根据等级每6秒回复1.5%/1.75%/2%生命上限",	["Verdant Glow"]="1.5%/1.75%/2% MaxHP per 6 seconds by Lv",
	["蛇捕头"]="根据等级+5%/10%/25%攻击，-20%/40%/100%防御",	["Snake Sheriff"]="+5%/10%/25% ATK,-20%/40%/100% DEF at Lv1",
	--1.5 1.56
	["蜻蜓精"]="根据等级时+3%/4%/5元气获得,-40/50/60生命,-20/25/30法力,-20/25/30气力",	["Dragonfly Guai"]="+3%/4%/5% Qi Gain,-40/50/60 MaxHP,-20/25/30 MaxMP,-20/25/30 MaxSt by Lv",
	["虫校尉"]="根据等级+10/12/15",	["Beetle Captain"]="+10/12/15 by Lv",
	["蝎太子"]="根据等级+10/12/15",["Scorpion Prince"]="+10/12/15 by Lv",
	["泥塑金刚"]="根据等级+10/12/15",	["Clay Vajra"]="+10/12/15 by Lv",
	["赤发鬼"]=nil,	["Red-Haired Yaksha"]=nil,
	["鸦香客"]="根据等级+10/12/15",	["Crow Diviner"]="+10/12/15 by Lv",
	["隼居士"]="根据等级+10/12/15",	["Falcon Hermit"]="+10/12/15 by Lv",
	["夜叉奴"]="根据等级+10/12/15 per Hit",	["Enslaved Yaksha"]="+10/12/15 per Hit by Lv",
	["菇男"]="根据等级6%/8%/10%",["Fungiman"]="6%/8%/10% by Lv",
	["巡山鬼"]=nil,	["Mountain Patroller"]=nil,
	["鼠司空"]="根据等级+10/12/15",	["Rat Governor"]="+10/12/15 by Lv",
	["骨悚然"]="根据等级+1%/2%/3%",	["Spearbone"]="+1%/2%/3% by Lv",
	--7.5/7.07/6.74  4.725
	["石双双"]="根据等级-6%/8%/10%气力消耗",	["Poisestone"]="-6%/8%/10% Stamina Cost by Lv",
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
	--["鳖宝"]="根据等级+5/7/10",["Turtle Treasure"]="+5/7/10 by Lv",
	["燧统领"]="根据等级+5/7/10",["Flint Chief"]="+5/7/10 by Lv",
	["黑脸鬼"]="根据等级+6%/8%/10%",["Charface"]="+6%/8%/10% by Lv",
	["石父"]="根据等级+1.5%/2%/2.5%暴击,+3%/4%/5%暴伤",["Father of Stones"]="+1.5%/2%/2.5% CritChance,+3%/4%/5% CritDamage by Lv",
	["沙二郎"]="生命值低于25%时根据等级+10/15/20攻击",["Second Rat Prince"]="+10/15/20 ATK under 25% MaxHP by Lv",
	--取平地普通移动再停止后最后一个数字，该数字是稳定的,观察PlayerLocomotion.MaxSpeed:650/663/669/682.5
	["百足虫"]="+2%/3%/5%,基础650",["Centipede Guai"]="+2%/3%/5% by Lv.Base 650",

}

--酒、葫芦、泡酒物
local DictionaryGourd={
	["妙仙葫芦"]="20秒内+20攻击",			["Immortal Blessing Gourd"]="+20 ATK in 20s",
	["湘妃葫芦"]="15秒内+15抗性",			["Xiang River Goddess Gourd"]="+15 in 15s",
	["五鬼葫芦"]="20秒内+15攻击",			["Plaguebane Gourd"]="+15 ATK in 20s",
	["燃葫芦"]="15秒内+30气力",				["Firey Gourd"]="+30 Stamina in 15s",

	["椰子酒"]="实际回复28%+15",	["Coconut Wine"]="Actually 28%+15",
	["椰子酒·三年陈"]="实际回复32%+25",	["3-Year-Old Coconut Wine"]="Actually 32%+25",
	["椰子酒·五年陈"]="实际回复36%+33",	["5-Year-Old Coconut Wine"]="Actually 36%+33",
	["椰子酒·十年陈"]="实际回复39%+40",	["10-Year-Old Coconut Wine"]="Actually 39%+40",
	["椰子酒·十八年陈"]="实际回复42%+46",	["18-Year-Old Coconut Wine"]="Actually 42%+46",
	["椰子酒·三十年陈"]="实际回复45%+51",	["30-Year-Old Coconut Wine"]="Actually 45%+51",
	["猴儿酿"]="实际回复48%+55",	["Monkey Brew"]="Actually 48%+55",
	["猴儿酿"]="的确是55%",	["Jade Dew"]="Indeed 55%",

	["琼浆"]="+20",							["Jade Essence"]="+20",
	["无忧醑"]="低于20%血量时恢复量24%->60%",	["Worryfree Brew"]="24%->60% under 20% HP",
	["九霞清醑"]="+15.0",					["Sunset of the Nine Skies"]="+15.0",
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

	["血杞子"]="+5秒",						["Goji Shoots"]="+5s",
	["困龙须"]="+2秒",						["Stranded Loong's Whisker"]="+2s",
	["不老藤"]="回复1/3气力上限,15秒内+50%气力回复",	["Undying Vine"]="Recover 1/3 MaxSt.+50% Stamina Recover in 15s",
	["蕙性兰"]="11秒内共回复12%生命上限(1%*12)",	["Graceful Orchid"]="total 12% MaxHP(1%*12) in 11s",
	["青山骨"]="30秒内+30生命上限",			["Goat Skull"]="+30 in 30s",
	["火焰丹头"]="15秒内+15",				["Flame Mediator"]="+15 in 15s",
}

--珍玩，消耗品
local DictionaryItem={
	["君子牌"]="about 0.12/s",	["Virtuous Bamboo Engraving"]="about 0.12/s", -- 不是加在属性上，而是获得一个buff
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
	["细磁茶盂"]="+11",	["Fine China Tea Bowl"]="+11",
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

	--695.5
	["风铎"]="+7%",	["Wind Chime"]="+7%",
	--消耗品
	["朝元膏"]="130秒内+60",	["Essence Decoction"]="+60 in 130s",
	["益气膏"]="130秒内+60",	["Tonifying Decoction"]="+60 in 130s",
	["延寿膏"]="130秒内+60",	["Longevity Decoction"]="+60 in 130s",

	["登仙散"]="80秒",	["Ascension Powder"]="80s",
	["龙光倍力丸"]="80秒内+15攻击15%暴击20%暴伤",	["Loong Aura Amplification Pellets"]="+15 ATK/15% Crit/20% CritDamage in 80s",
	["加味参势丸"]="+480",	["Enhanced Ginseng Pellets"]="+480",
	["聚珍伏虎丸"]="80秒内+15攻击",	["Enhanced Tiger Subduing Pellets"]="+15 ATK in 80s",
	["轻身散"]="130秒",	["Body-Fleeting Powder"]="+60 in 130s",

	["倍力丸"]="80秒内+10%",	["Amplification Pellets"]="+10% in 80s",
	["伏虎丸"]="80秒内+8攻击",	["Tiger Subduing Pellets"]="+8 ATK in 80s",
	["坚骨药"]="80秒",	["Fortifying Medicament"]="80s",
	["温里散"]="80秒内+30",	["Body-Warming Powder"]="+30 in 80s",
	["度瘴散"]="80秒内+30",	["Antimiasma Powder"]="+30 in 80s",
	["神霄散"]="80秒内+30",	["Shock-Quelling Powder"]="+30 in 80s",
	["清凉散"]="80秒内+30",	["Body-Cooling Powder"]="+30 in 80s",
	["避凶药"]="80秒内+10%",	["Evil Repelling Medicament"]="+10% in 80s",
	["镜中丹"]="+60",	["Mirage Pill"]="+60",
	--["九转还魂丹"]="130秒",	["Tonifying Decoction"]="130s",

	["参势丸"]="33秒内+10/秒",	["Ginseng Pellets"]="+10/s in 33s",
}

--套装效果
local DictionarySuit={
	["心灵福至"]="每个天赋+24防御",	["Fortune's Favor"]="+24 DEF per Relic Talent",
	--650->702，+满级百足734.5
	["日行千里"]="+8%移动速度，每层+10%攻击\n[*]:每秒+12棍势",	["Swift Pilgrim"]="+8% Speed.+10% ATK per stack\n[*]:+12 Focus/s"	,
	["举步生风"]="+15棍势",			["Gale Guardian"]="+15 Focus" ,
	["花下死"]="x0.8毒伤(和抗性效果乘算),+20%攻击",["Poison Ward"]="x0.8 Poison Damage(Multi with \nPoison Resist effect).+20% ATK"	,
	["试比天高"]="10秒内+8%暴击",	["Heaven's Equal"]="+8% Crit in 10s"	,
	["毒魔狠怪"]="+100棍势，+10%持续时间",["Fuban Strength"]="+100 focus.+10% Duration"	,
	["锱铢必较"]="+10%灵蕴",		["Every Bit Counts"]="+10% Will"	,
	["浪里白条"]="x0.5气力消耗(和天赋效果乘算)",["Wave-Rider"]="x0.5 Stamina Cost(Multi with talent effect)",
	["龙血玄黄"]="+10赋雷攻击\n[*]:+10赋雷攻击",		["Thunder Veins"]="+10 Thunder ATK\n[*]:+10 Thunder ATK",	
	["借假修真"]="20秒内+15%攻击,暴击+3元气,\n击杀+5元气",["Gilded Radiance"]="+15% ATK in 20s. +3/+5 Qi when Crit/Kill",	
	["百折不挠"]="5秒内+10%防御",	["Unyielding Resolve"]="+10% DEF in 5s",	
	["离火入魔"]="[4]+25%伤害 -30%伤害减免",["Outrage"]="[4]+25% Damage.-30% DamageReduction",	
	["泥塑金装"]="变身时及变身结束后12秒内+20%伤害减免,\n约+1.5/s神力回复,翻滚回复约0.3神力",["From Mud to Lotus"]="+20% DamageReduction in 12s.About\n +1.5/s Might recover.Gain about\n 0.3 Might when roll",	
	["铜心铁胆"]="+50棍势",			["Iron Will"]="+50 Focus",	
	["乘风乱舞"]="[*]:+20法力消耗;假身持续时间不变,但不会因破隐而消失",			["Dance of the Black Wind"]="[*]:+20 MP Cost",	
	--["炼魔荡怪"]="",["Pilgrim's Garb"]="",
}
--独门妙用
local DictionaryEquip={
	["大力王面"]="+10棍势，强硬时+30",		["Bull King's Mask"]="+10 Focus.+30 when Tenacity",	
	["锦鳞战袍"]="低于半血时每秒+1.5%生命上限(水中+2%)",["Serpentscale Battlerobe"]="+1.5%MaxHP/s under 50% MaxHP;+2% MaxHP/s in water",	
	["厌火夜叉面"]="低于半血时+15攻击",		["Yaksha Mask of Outrage"]="+15ATK under 50% HP",	
	["金身怒目面"]="+100棍势",				["Golden Mask of Fury"]="+100",
	["长嘴脸"]="饮酒后15秒内+30攻击，15~20秒+20攻击，20秒后-20攻击",["Snout Mask"]="+30 ATK in 15s;+20 ATK in 15~20s;-20 ATK after 20s",
	["鳖宝头骨"]="+2%",						["Skull of Turtle Treasure"]="+2%",
	["地灵伞盖"]="高于1%血量时每秒-3HP,额外回复+15%生命上限",["Earth Spirit Capped"]="-3HP/s when over 1% HP.+15% MaxHP Recover",
	["昆岐毒敌甲"]="+15",					["Venomous Sting Insect Armor"]="+15",
	["阴阳法衣"]="阳:+20%伤害减免。阴:20%暴击-30%伤害减免",["Yin-Yang Daoist Robe"]="Yang:+20% DamageReduction. Yin:+20% Crit,-30% DamageReduction",
	["山珍蓑衣"]="15秒内+30",				["Ginseng Cape"]="+30 in 15s",
	["玄硬铁手"]="3秒内+15%攻击",			["Iron-Tough Gauntlets"]="+15% ATK in 3s",
	["金刚护臂"]="-10%",					["Vajra Armguard"]="-10%",
	["赭黄臂甲"]="每消耗一段棍势8秒内+6%暴击",["Ochre Armguard"]="+6% in 8s for each focus level",
	["不净泥足"]="10秒",					["Non-Pure Greaves"]="10s",
	["藏风护腿"]="+10",						["Galeguard Greaves"]="+10",
	["南海念珠"]="+40生命/法力上限",		["Guanyin's Prayer Beads"]="+40 MaxHP/MP",
	["羽士戗金甲"]="20秒内+10%攻击",		["Centipede Qiang-Jin Armor"]="+10%ATK in 20s",

	["狼牙棒"]="5秒内获得共127.5棍势(2.5+25*5)",		["Spikeshaft Staff"]="Gain 127.5(2.5+25*5) Focus in 5s",
	["昆棍·百眼"]="每段棍势+5HP(中毒敌人+40)",		["Visionary Centipede Staff"]="+5 HP(40 for Poisoned enemy) for each focus level",
	["昆棍·蛛仙"]="每段棍势+5HP",		["Spider Celestial Staff"]="+5 HP for each focus level",
	["昆棍"]="每段棍势+5HP",		["Chitin Staff"]="+5 HP for each focus level",

	["昆棍·通天"]="每段棍势+5HP",		["Adept Spine-Shooting Fuban Staff"]="+5 HP for each focus level",
	["混铁棍"]="每点防御+0.15攻击",		["Dark Iron Staff"]="+0.15ATK per DEF",
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
	["见机强攻"]="轻棍1-5段全部命中的棍势由15/18/17/28/40变为15/18/26/35/43",	["Opportune Watcher"]="Light Attack 1-5 focus point if all hit :15/18/17/28/40 -> 15/18/26/35/43",
	--["眼乖手疾"]="+45",	["Eagle Eye"]="+45",
	["慧眼圆睁"]="+15%",	["Keen Insight"]="+15%",
	--["金钮"]="满血时+15攻击",	["All Ears"]="+15 when full HP",
	--["阳燧珠"]="+12",	["Sound as A Bell"]="+12",
	["耳畔风响"]="5秒内+10%攻击",	["Whistling Wind"]="+10% ATK in 5s",
	--见机强攻
	--["阳燧珠"]="+12",	["Lingering Aroma"]="+12",
	--["阳燧珠"]="+12",	["In One Breath"]="+12",
	--["阳燧珠"]="+12",	["Hold Breath"]="+12",
	
	["丹满力足"]="5秒内+15%",	["Refreshing Taste"]="+15% in 5s",
	--["阳燧珠"]="+12",	["Spread the Word"]="+12",
	["遍尝百草"]="每个增加+4%生命上限的回复量",	["Tongue of A Connoisseur"]="Each +4% MaxHP recover",

	["身轻体快"]="0-4级棍势气力消耗55/65/90/115/115 -> 35/40/52.5/65/65 令熟谙天赋效果减半",	["Nimble Body"]="0-4 Focus-Level heavy-attack cost: 55/65/90/115/115 -> 35/40/52.5/65/65 Reduce Instinct talent effect by half",
	["福寿长臻"]="+60",	["Everlasting Vitality"]="+60",
	["灾愆不侵"]="+15",	["Divine Safeguard"]="+15",
}

--Merge All dic to Dictionary
--DictionarySpirit/DictionarySpiritPassive不能合并，因为其中有相同key
local DictionaryMerge={}
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

local detailtextSpirit=nil
local detailtextSpiritPassive=nil
local detailtextMerge=nil


--目前看来，这些title一定在text之前设置。只有套装效果例外
--TODO: optimize
local MergeTitleList43={ -- Gourd/Wine/WineMat/Relic ....
["etTree.BI_HuluDetail.WidgetTree.TxtNameRuby"]=true,--装备界面 葫芦
["etTree.BI_WineDetail.WidgetTree.TxtNameRuby"]=true,--泡制界面 酒
["ree.BI_WineMatDetail.WidgetTree.TxtNameRuby"]=true,--泡制界面 泡酒物
["l.WidgetTree.BI_WineDesc.WidgetTree.TxtName"]=true,--装备界面 酒
["Tree.BI_GenqiDecs_1.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["Tree.BI_GenqiDecs_2.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["Tree.BI_GenqiDecs_3.WidgetTree.TxtGenqiRuby"]=true,--根器天赋
["ree.BI_JewelryDetail.WidgetTree.TxtNameRuby"]=true,--珍玩
["etTree.BI_ItemDetail.WidgetTree.TxtNameRuby"]=true,--行囊
["idgetTree.BI_SpellDetail.WidgetTree.TxtName"]=true,--天赋
["tTree.BI_EquipDetail.WidgetTree.TxtNameRuby"]=true,--装备名
}
local MergeDetailList43={
	["etTree.BI_HuluDetail.WidgetTree.TxtHuluDesc"]=true,--装备界面 葫芦效果
	["etTree.BI_WineDetail.WidgetTree.TxtWineDesc"]=true,--泡制界面 酒效果
	["e.BI_WineMatDetail.WidgetTree.TxtEffectDesc"]=true,--泡制界面 泡酒物效果
	["l.WidgetTree.BI_WineDesc.WidgetTree.TxtDesc"]=true,--装备界面 酒
	["ee.BI_GenqiDecs_1.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["ee.BI_GenqiDecs_2.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["ee.BI_GenqiDecs_3.WidgetTree.TxtGenqiTalent"]=true,--根器天赋
	["e.BI_JewelryDetail.WidgetTree.TxtEffectDesc"]=true,--珍玩
	["Tree.BI_SpellDetail.WidgetTree.TxtSpellDesc"]=true,--天赋
	["Tree.BI_ItemDetail.WidgetTree.TxtEffectDesc"]=true,--行囊
	["ree.BI_EquipDetail.WidgetTree.TxtLegendDesc"]=true,--装备独门妙用
	}

--local lastSuitDescWidget=nil
--local lastSuitDescWidgetText=nil
function HookTextBlockSetText(Context,InText)
	local name=Context:get():GetFullName()
	if type(name) ~= "string" then return end
	local name43=name:sub(-43)

	if detailtextSpirit~=nil and name43==".WidgetTree.BI_RZDDetail.WidgetTree.TxtCost" then--精魂能量消耗
		--print(tostring(InText:get():ToString()).."/"..tostring(name))
		InText:set(FText(InText:get():ToString().."("..detailtextSpirit.." at Lv1)"))
		detailtextSpirit=nil
		--InText:set(FText(detailtextSpirit or "Unknown"))

	elseif MergeTitleList43[name43]==true then --DictionaryMerge
		detailtextMerge=DictionaryMerge[InText:get():ToString()]

	elseif detailtextMerge~=nil and MergeDetailList43[name43]==true then --DictionaryMerge
		InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
		detailtextMerge=nil

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
	--elseif InText:get():ToString():find("牙") then
	--	print("C.."..tostring(name).." "..name43)
	--elseif InText:get():ToString():find("识破对手") then
	--	print("D.."..tostring(name).." "..name43)
		
	end
	--print(".."..tostring(InText:get():ToString()))
end
--["tTree.BI_EquipDetail.WidgetTree.TxtNameRuby"]=true,--装备名 Rich
--套装效果名not rich: /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147470897.WidgetTree.BUI_EquipMain_C_2147427437.WidgetTree.BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.TxtSuitTitle Tree.BI_EquipDetail.WidgetTree.TxtSuitTitle
--RichTextBlock       /Engine/Transient.GameEngine_2147482611:BGW_GameInstance_B1_2147482576.BUI_B1_Root_V2_C_2147470897.WidgetTree.BUI_EquipMain_C_2147427437.WidgetTree.BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.BI_SuitDesc.WidgetTree.BI_SuitDesc_C_2147422605.WidgetTree.TxtSuitDesc
function HookRichTextBlockSetText(Context,InText)
	local name=Context:get():GetFullName()
	if type(name) ~= "string" then return end
	local name43=name:sub(-43)
	if name43=="getTree.BI_RZDDetail.WidgetTree.TxtNameRuby" then	--精魂名字
		detailtextSpirit=DictionarySpirit[InText:get():ToString()] --or "(None)"
		detailtextSpiritPassive=DictionarySpiritPassive[InText:get():ToString()]
		--print("!!"..tostring(InText:get():ToString()).."/"..tostring(detailtext))

	elseif name:sub(-21)==".WidgetTree.TxtEffect" then --精魂升级描述
		if InText:get():ToString()=="Reduces the Qi cost for the skill." then
			InText:set(FText("Reduces the Qi cost for the skill.(-8%/18%/39% at Lv 2/3/5)"))
		elseif InText:get():ToString()=="减少施展此技所需元气" then
			InText:set(FText("减少施展此技所需元气 (-8%/18%/39% at Lv 2/3/5)"))
		end
		--print(".."..tostring(InText:get():ToString()))

	elseif MergeTitleList43[name43]==true then	--DictionaryMerge中的名字
		detailtextMerge=DictionaryMerge[InText:get():ToString()]
		
	elseif detailtextSpiritPassive~=nil and name:sub(-73)==".WidgetTree.BI_RZDDetail.WidgetTree.TreasureEqDesc.WidgetTree.TxtSuitDesc" then	--精魂被动描述
		InText:set(FText(InText:get():ToString().."("..detailtextSpiritPassive..")"))
		detailtextSpiritPassive=nil

	elseif detailtextMerge~=nil then
		if MergeDetailList43[name43]==true then	--DictionaryMerge
			InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
			detailtextMerge=nil
		elseif name:find("WidgetTree.BI_MaterialListItem.WidgetTree.BI_MaterialListItem_V2_C") then--泡制界面泡酒物缩略效果
			InText:set(FText(InText:get():ToString().."("..detailtextMerge..")"))
			detailtextMerge=nil
		end
	--[[
	--.WidgetTree.BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.BI_SuitDesc.WidgetTree.BI_SuitDesc_C_2147459831.WidgetTree.TxtSuitDesc
	--换装界面套装效果，当前装备和装备比较时不同控件,每查看一个装备都会临时创建一个比较控件，退出菜单后清空
	--有多条效果时每条效果各有一个text，不做区分，改了一个之后清空detailtext
	--装备名和套装效果名都在套装效果之后set，坑爹
	--反过来搞，记录最后一个被赋值的textblock，设置title的时候修改该textblock;
	elseif name:sub(-11)=="TxtSuitDesc" and 
			(name:find("BI_EquipDetailCompare.WidgetTree.BI_EquipDetail.WidgetTree.BI_SuitDesc") 
				or name:find("BI_EquipDetail.WidgetTree.BI_EquipDetail.WidgetTree.BI_SuitDesc"))  then
			lastSuitDescWidget=Context:get()
			lastSuitDescWidgetText=InText:get():ToString()
			--print("Last Widet: ",tostring(lastSuitDescWidget:GetFullName()))
	]]--
	--5 127.5
	--elseif InText:get():ToString():find("狼牙棒") then
	--	print("A.."..tostring(name).." "..name43)
	--elseif InText:get():ToString():find("识破对手") then
	--	print("B.."..tostring(name).." "..name43)		
	end

	--print(".."..tostring(InText:get():ToString()))
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
		--hooked=true
		--print(ModName.."Hook")
--	end
--end)

--RegisterKeyBind(Key.F12,function()
--	RegisterHook("/Script/UMG.TextBlock:SetText",function()end)
--end)
--RegisterHook("/Script/UMG.TextBlock:SetText",function()end)
--Work()

