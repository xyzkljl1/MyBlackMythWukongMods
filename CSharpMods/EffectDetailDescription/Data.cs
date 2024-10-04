using b1;
using BtlB1;
using BtlShare;
using OssB1;
using ResB1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EffectDetailDescription
{
    static public class Data
    {
        public enum LanguageIndex : int
        {
            SimpleCN = 0,
            English = 1,
            Max = 2,
        }
        public static string PlaceHolder = "__PlaceHolder__";
        //ItemId->effectdesc
        public static DescDict ItemEqDesc = new DescDict
        {
            //法宝主动 VITreasureDetail::OnEquipIdChange
            {19001,new Desc{ "40秒内+600火抗，每秒获得5棍势", "+600 FireResist,+5 Focus/s in 40s"}},
            {19002,new Desc{ "20秒内+50%伤害减免", "+50% DamageReduction in 20s"}},
        };

        public static DescDict ItemDesc = new DescDict
        {
            //消耗品,注意1118等是配方，2118等是丹药
            //鼻根器+10%持续2
            {2218,new Desc{ }},//朝元膏/Essence Decoction
            {2221,new Desc{ }},//益气膏/Tonifying Decoction
            {2224,new Desc{ }},//倍力丸/Amplification Pellets
            {2227,new Desc{ }},//伏虎丸/Tiger Subduing Pellets
            {2230,new Desc{ }},//延寿膏/Longevity Decoction
            {2234,new Desc{ "75秒", "75s"}},//坚骨药/Fortifying Medicament
            {2236,new Desc{ "回复40%", "Recover 40%"}},//镜中丹/Mirage Pill
            {2245,new Desc{ }},//龙光倍力丸/Loong Aura Amplification Pellets
            {2247,new Desc{ }},//避凶药/Evil Repelling Medicament
            
            {2251,new Desc{ }},//加味参势丸/Enhanced Ginseng Pellets
            {2252,new Desc{ }},//参势丸/Ginseng Pellets
            {2253,new Desc{ }},//聚珍伏虎丸/Enhanced Tiger Subduing Pellets
            
            {2204,new Desc{ }},//温里散/Body-Warming Powder
            {2205,new Desc{ }},//度瘴散/Antimiasma Powder
            {2206,new Desc{ }},//神霄散/Shock-Quelling Powder
            {2207,new Desc{ "120秒", "+60 in 130s"}},//轻身散/Body-Fleeting Powder
            {2208,new Desc{ }},//清凉散/Body-Cooling Powder
            {2213,new Desc{ "75秒", "75s"}},//登仙散/Ascension Powder
            //{,new Desc{ "120秒", "130s"}},//九转还魂丹/Tonifying Decoction


            {18007,new Desc{ "15秒内+15抗性", "+15 in 15s"}},//湘妃葫芦/Xiang River Goddess Gourd
            {18011,new Desc{ "每30秒回满葫芦", "Refill each 30s"}},//青田葫芦
            {18012,new Desc{ "20秒内+15攻击", "+15 ATK in 20s"}},//五鬼葫芦/Plaguebane Gourd
            {18014,new Desc{ "20秒内+20攻击", "+20 ATK in 20s"}},//妙仙葫芦/Immortal Blessing Gourd
            //2804 DmgAdditionBase 30
            {18015,new Desc{ "30秒内+30%伤害加成", "+30% Damage in 30s"}},//乾坤彩葫芦/Multi-Glazed Gourd
            {18017,new Desc{ "15秒内+30气力", "+30 Stamina in 15s"}},//燃葫芦/Firey Gourd

            {2001,new Desc{ "实际回复28%+15", "Actually 28%+15"}},//椰子酒/Coconut Wine
            {2002,new Desc{ "实际回复32%+25", "Actually 32%+25"}},//椰子酒·三年陈/3-Year-Old Coconut Wine
            {2003,new Desc{ "实际回复36%+33", "Actually 36%+33"}},//椰子酒·五年陈/5-Year-Old Coconut Wine
            {2004,new Desc{ "实际回复39%+40", "Actually 39%+40"}},//椰子酒·十年陈/10-Year-Old Coconut Wine
            {2005,new Desc{ "实际回复42%+46", "Actually 42%+46"}},//椰子酒·十八年陈/18-Year-Old Coconut Wine
            {2006,new Desc{ "实际回复45%+51", "Actually 45%+51"}},//椰子酒·三十年陈/30-Year-Old Coconut Wine
            {2007,new Desc{ "实际回复48%+55", "Actually 48%+55"}},//猴儿酿/Monkey Brew
            {2021,new Desc{ "实际回复确实是55%", "Indeed 55%"}},//玉液/Jade Dew

            {2009,new Desc{ "15秒内+30%移动速度", "+30% in 15s"}},//蓝桥风月/Bluebridge Romance--Buff-Talent 2816 写的是15%/15%/15% 实测 约+30%/+30%/约+30% ,why???
            {2011,new Desc{ "低于20%血量时恢复量24%->60%", "24%->60% under 20% HP"}},//无忧醑/Worryfree Brew
            {2012,new Desc{ "6秒内+12%攻击", "+12 ATK in 6s"}},//龙膏/Loong Balm
            {2019,new Desc{ "+20", "+20"}},//琼浆/Jade Essence
            //BuffDesc-Item 92023
            {2022,new Desc{ "+75.0", "+75.0"}},//松醪/Pinebrew
            {2023,new Desc{ "+15.0元气;+20法宝能量", "+15.0 Qi;+20.0 Treasure Energy"}},//九霞清醑/Sunset of the Nine Skies
    
            {2303,new Desc{ }},//龟泪/Turtle Tear
            {2304,new Desc{ "10秒内+2秒持续", "+2s Duration in 10s"}},//困龙须/Stranded Loong's Whisker--92304 Passive 192
            {2305,new Desc{ "10秒内+25%持续时间", "25% Duration in 10s"}},//灵台药苗/Moutain Lingtai Seedlings --92305 Passive 193
            //Talent 2100 AtkMul 20% 时间10秒，是10秒内施放隐身还是10秒内打出破隐?
            {2306,new Desc{ "10秒内？+20%攻击", "+20% ATK in 10s?"}},//十二重楼胶/Breath of Fire
            {2307,new Desc{ "5秒内共回复6%上限(1%*6)", "total 6% MaxHP(1%*6) in 5s"}},//瑶池莲子/Celestial Lotus Seeds
            {2308,new Desc{ }},//不老藤/Undying Vine
            {2309,new Desc{ }},//虎舍利/Tiger Relic--92306
            {2310,new Desc{ }},//梭罗琼芽/Laurel Buds
            {2311,new Desc{ }},//甜雪/Sweet Ice
            {2312,new Desc{ }},//霹雳角/Thunderbolt Horn
            {2314,new Desc{ "低于20%血量时额外回复+12%生命上限", "+20% MaxHP when under 20% HP"}},//紫纹缃核/Purple-Veined Peach Pit
            {2315,new Desc{ "15%？", "15%？"}},//蜂山石髓/Bee Mountain Stone
            {2316,new Desc{ }},//铁弹/Iron Pellet
            {2317,new Desc{ "15秒内踉跄并增加50%气力消耗;定身法-2秒，安神法-5秒，聚形散气-3秒，铜头铁臂-2秒，毫毛-8秒", "Stagger and +50% Stamina Consume in 15s;Immobilize -2s,Ring of Fire -5s,Cloud Step -3s,Rock Solid-2s,A Pluck of Many -8s"}},//瞌睡虫蜕/Slumbering Beetle Husk
            {2318,new Desc{ "10秒", "10s"}},//铜丸/Copper Pill
            {2319,new Desc{ "15秒内+5秒持续", "+5s Duration in 15s"}},//血杞子/Goji Shoots --92319? Passive 198
            {2320,new Desc{ "15秒内+0.066秒无敌，0-3段翻滚基础时间0.4/0.433/0.466/0.5秒", "+0.066s in 15s; 0-3 level roll base immue time:0.4/0.433/0.466/0.5"}},//清虚道果/Fruit of Dao--92320 Passive 199 10105-10109
            {2321,new Desc{ }},//火焰丹头/Flame Mediator
            {2322,new Desc{ }},//双冠血/Double-Combed Rooster Blood
            {2323,new Desc{ }},//胆中珠/Gall Gem
            {2324,new Desc{ "11秒内共回复12%生命上限(1%*12)", "total 12% MaxHP(1%*12) in 11s"}},//蕙性兰/Graceful Orchid，11秒持续间隔1秒
            {2325,new Desc{ }},//嫩玉藕/Tender Jade Lotus
            {2326,new Desc{ }},//铁骨银参/Steel Ginseng
            //Item 92317 92327 92328
            {2327,new Desc{ }},//青山骨/Goat Skull

        };
        //\["(.*?)"\]=(".*?"),[ ]*\["(.*?)"\]=(".*?"),
        //{,new Desc{ $2, $4}},//$1/$3
        //珍玩装备
        public static DescDict EquipDesc = new DescDict
        {
            //首饰
            {16001,new Desc{ }},//细磁茶盂/Fine China Tea Bowl
            {16002,new Desc{ }},//猫睛宝串/Cat Eye Beads
            {16003,new Desc{ }},//玛瑙罐/Agate Jar
            {16004,new Desc{ }},//虎头牌/Tiger Tally
            {16005,new Desc{ "身法-1秒/奇术-3秒/毫毛-6s", "-1s/3s/6s by type"}},//砗磲佩/Tridacna Pendant
            {16006,new Desc{ }},//金花玉萼/Goldflora Hairpin
            {16007,new Desc{ "+30", "+30"}},//琉璃舍利瓶/Glazed Reliquary,效果对应3个passive分别为123级，各回30/60/90
            //695.5
            {16008,new Desc{ }},//风铎/Wind Chime --1006008 buff-desc 96008 写的是7%实测也是7%
            {16009,new Desc{ }},//雷榍/Thunderstone
            {16010,new Desc{ }},//耐雪枝/Frostsprout Twig
            {16011,new Desc{ }},//白狐毫/Snow Fox Brush
            {16012,new Desc{ "100次?", "100 times?"}},//未来珠/Maitreya's Orb--buff-item 96010-96012
            {16013,new Desc{ }},//金色鲤/Golden Carp
            {16014,new Desc{ "+1%,有日金乌时+3%", "+1%, +3% if have Gold Sun"}},//月玉兔/Jade Moon Rabbit
            {16015,new Desc{ }},//三清令/Tablet of the Three Supreme
            {16016,new Desc{ "180秒内+60生命上限,+40法力/气力上限", "+60 MaxHP,+40 MaxMP/MaxSt in 180s"}},//定颜珠/Preservation Orb ，有三个buff，每个分别增加一种属性的上限并回复等量值，自动生成的太长了，
            {16017,new Desc{ "+1%,有月玉兔时+3%", "+1%, +3% if have Jade Moon"}},//日金乌/Gold Sun Crow
            {16018,new Desc{ }},//错金银带钩/Cuo Jin-Yin Belt Hook
            {16019,new Desc{ }},//金钮/Gold Button
            {16020,new Desc{ }},//阳燧珠/Flame Orb
            {16021,new Desc{ }},//水火篮/Daoist's Basket of Fire and Water
            {16022,new Desc{ }},//辟水珠/Waterward Orb
            {16023,new Desc{ "1-4段棍势上限由100/210/330/480变为90/200/320/470", "Lv.1-4 Max focus point:100/210/330/480 -> 90/200/320/470"}},//琥珀念珠/Amber Prayer Beads，有多个passiv
            {16024,new Desc{}},//仙箓/Celestial Registry Tablet
            {16025,new Desc{}},//虎筋绦子/Tiger Tendon Belt
            //16026仙胞石片
            {16027,new Desc{ }},//摩尼珠/Mani Bead
            {16028,new Desc{ }},//博山炉/Boshan Censer
            {16029,new Desc{ }},//不求人/Back Scratcher
            {16030,new Desc{ "", ""}},//吉祥灯/Auspicious Lantern
            {16031,new Desc{ "+32防御,每次造成10%天命人攻击力的伤害", "+32 DEF.Deal x0.1 player attack Damage per Hit"}},//金棕衣/Gold Spikeplate
            {16032,new Desc{ }},//兽与佛/Beast Buddha
            {16033,new Desc{ }},//铜佛坠/Bronze Buddha Pendant
            {16034,new Desc{ }},//雷火印/Thunderflame Seal
            {16035,new Desc{ "约0.1/s", "about 0.1/s"}},//君子牌/Virtuous Bamboo Engraving
            {16036,new Desc{ }},//卵中骨/Spine in the Sack
            {16037,new Desc{ }},//白贝腰链/White Seashell Waist Chain

            //装备独门妙用
            //散件
	        {17001,new Desc{ "高于1%血量时每秒-3HP,额外回复+15%生命上限", "-3HP/s when over 1% HP.+15% MaxHP Recover"}},//地灵伞盖/Earth Spirit Cap
            {17002,new Desc{ "饮酒后15秒内+10攻击,20秒后-20攻击", "+10 ATK in 15s;-20 ATK after 20s"}},//长嘴脸/Snout Mask
            {17003,new Desc{ "+2%", "+2%"}},//鳖宝头骨/Skull of Turtle Treasure
            {17004,new Desc{ }},//山珍蓑衣/Ginseng Cape
            //蜢虫头907005 97005、97015 185、186
            {17005,new Desc{ "+15%/20% 动作倍率", "+15%/20% SkillEffect"}},//长须头面/Locust Antennae Mask
            //Talent 2702
            {17006,new Desc{ }},//白脸子/Grey Wolf Mask
            {17008,new Desc{ "阳:+20%伤害减免。阴:20%暴击-30%伤害减免", "Yang:+20% DamageReduction. Yin:+20% Crit,-30% DamageReduction"}},//阴阳法衣/Yin-Yang Daoist Robe
            //Talent 2713/2714
            {17010,new Desc{ "300秒内+40生命/法力上限", "+40 MaxHP/MP in 300s"}},//南海念珠/Guanyin's Prayer Beads ,类似定颜珠，自动生成的太长了
            {17011,new Desc{ }},//金刚护臂/Vajra Armguard
            {10701,new Desc{ }},//厌火夜叉面/Yaksha Mask of Outrage
            {11601,new Desc{ "+10棍势，强硬时+30", "+10 Focus.+30 when Tenacity"}},//大力王面/Bull King's Mask
            {10302,new Desc{ "低于半血时每秒+1.5%生命上限(水中+2%)", "+1.5%MaxHP/s under 50% MaxHP;+2% MaxHP/s in water"}},//锦鳞战袍/Serpentscale Battlerobe，一个buff具有两个效果：回血，施加另一个处于水中回血的buff
            {11001,new Desc{ "+100棍势", "+100"}},//金身怒目面/Golden Mask of Fury，2111buff触发施加2112，2112没有效果，2113的激活条件是具有2112
            //没有11902，从11912开始
            {11912,new Desc{ "+15", "+15"}},//昆蚑毒敌甲/Venomous Sting Insect Armor
            {11803,new Desc{ "3秒内+15%攻击", "+15% ATK in 3s"}},//玄铁硬手/Iron-Tough Gauntlets 把IronBodyBuff覆盖为2171
            {10603,new Desc{ "每消耗一段棍势8秒内+6%暴击", "+6% in 8s for each focus level"}},//赭黄臂甲/Ochre Armguard
            {11304,new Desc{ "10秒", "10s"}},//不净泥足/Non-Pure Greaves
            {11204,new Desc{ "+10", "+10"}},//藏风护腿/Galeguard Greaves
            //Talent 2142
            {11402,new Desc{ }},//羽士戗金甲/Centipede Qiang-Jin Armor
            //Talent 2098
            {10904,new Desc{ "+15%攻击", "+15%ATK"}},//乌金行缠/Ebongold Gaiters //升满级的10934关联到buff2099/2097不知道有什么效果；2091~2099全是，太复杂了

            {15012,new Desc{ "5秒内获得共127.5棍势(2.5+25*5)", "Gain 127.5(2.5+25*5) Focus in 5s"}},//狼牙棒/Spikeshaft Staff
            {15007,new Desc{ "每段棍势+5HP(中毒敌人+40)", "+5 HP(40 for Poisoned enemy) for each focus level"}},//昆棍·百眼/Visionary Centipede Staff
            {15013,new Desc{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍·蛛仙/Spider Celestial Staff
            //{,new Desc{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍/Chitin Staff
            {15018,new Desc{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍·通天/Adept Spine-Shooting Fuban Staff
            {15016,new Desc{ "每点防御+0.15攻击", "+0.15ATK per DEF"}},//混铁棍/Dark Iron Staff
            {15015,new Desc{ "+20% 动作倍率", "+20% SkillEffect"}},//飞龙宝杖/Golden Loong Staff//15015 145
            {15101,new Desc{ "+15% 动作倍率", "+15% SkillEffect"}},//三尖两刃枪/Tri-Point Double-Edged Spear//15015 145
        };
        //天赋
        public static DescDict TalentDisplayDesc = new DescDict
        {
            //根基-气力
            //7.5/6/5.25
            {100101,new Desc{ "Lv1: -20%/Lv2: -30%,基础 7.5/s","Lv1: -20%/Lv2: -30%,BaseCost 7.5/s"}},//体健
            {100102,new Desc{ "+50%气力回复","+50% Stamina Recover"}},//绵绵
            {100104,new Desc{ "-30%,基础消耗 30","-30%,BaseCost 30",}},//身轻
            {100105,new Desc{ "+15%/lv",}},//调息
            {100106,new Desc{ "-2.5/lv,基础消耗 20","-2.5/lv,BaseCost 20"}},//猿捷
            {100108,new Desc{ "+10/lv,基础 40","+10/lv,Base 40"}},//走险
            {100109,new Desc{ "15%,持续2秒","15% in 2s"}},//绝念
            //{,new Desc{ "",}},//任翻腾//第一段翻滚由SkillDesc 10301变为10305

            //武艺
            {100201,new Desc{ "+100/lv,基础800/900/1000", "+100/lv,Base 800/900/1000"}},//直取/Switft Engage//Passive 13/14 SkillCtrlDesc 10701,10798-10801
            {100202,new Desc{ "18->23 per Hit",}},//接力
            //10508 46/47 SkillEffectDamageExpand
            //{,new Desc{ "Lv1-2:+100/150", "Lv1-2:+100/150"}},//断筋/TODO
            {100205,new Desc{ "30%",}},//筋节
            //10506 PassiveSkillDesc 16写的是Mul 50%,实测约+18%，回复量会浮动，约7.5~8，点完变成9上下
            {100209,new Desc{ "约+18%", "about +18%"}},//化吉/Silver Lining
            {100210,new Desc{ "-30%, 基础26/秒(移步48/秒)","-30%, BaseCost 26/s(48/s when moving)"}},//应手
            {100211,new Desc{ "4->6 per Hit, 不影响原地棍花(3 per Hit)","4->6 per Hit. Not affect Staff Spin(3 per Hit)"}},//得心

            //体段修行
            {100301,new Desc{ "+10/lv",}},//五脏坚固
            {100302,new Desc{ "+10/lv",}},//气海充盈
            //TalentSDesc 100303 PassiveSkillDesc 20-25
            {100303,new Desc{ "-5%持续时间/lv", "-5% Duration/lv"}},//灾苦消减/Bane Mitigation
            {100304,new Desc{ "+1%/lv",}},//怒相厚积
            {100305,new Desc{ "+10/lv",}},//皮肉粗糙
            {100306,new Desc{ "+10/lv",}},//法性贯通
            {100307,new Desc{ "+2.5气力回复/lv","+2.5 Stamina Recover/lv"}},//吐纳绵长
            {100308,new Desc{ "+2攻击力/lv","+2 ATK/lv"}},//攻势澎湃
            {100309,new Desc{ "+3/lv",}},//四灾忍耐
            {100310,new Desc{ "+4%/lv",}},//威能凶猛
            //棍法
            {100502,new Desc{ "回复+2%/3%/4%(Lv1/Lv2/Lv3) 最大生命","Heal +2%/3%/4%(Lv1/Lv2/Lv3) MaxHP"}},//壮怀
            //55/65/90/115/115 n
            //43/50/67.5/85 Lv3
            //29/32.5/41.2/50 Lv3+r
            //35/40/52.5/65 r
            //100504 Passive 36-38里写的是-10%,但和实测对每种风格都是减固定值
            //劈棍原地重击消耗10+55/65/90/115/115，立棍10+40/65/75/100/100，戳棍47.5/57.5/82.5/107.5，跳劈(三种棍势一样)50/75/100/125/125,
            //3级天赋劈棍43/50/67.5/85，立棍28/50/53/70，戳棍35.5/42.5/60/77.5
            //不影响跳劈,不影响蓄力起手(三种风格起手消耗10/10/15)
            {100504,new Desc{ "每级令0-4级重棍-4/-5/-7.5/-10/-10气力消耗，有身轻体快(根器)时效果减半，0-4级重棍基础消耗:劈棍55/65/90/115/115,立棍40/65/75/100/100,戳棍47.5/57.5/82.5/107.5;不影响蓄力起手式和跳跃重击", "Each level reduce 0-4 Focus-Level heavy-attack cost: -4/-5/-7.5/-10/-10. Only have 50% effect when having Nimble Body(body relic).Base cost of each focus-level:Smash 55/65/90/115/115,Pillar 40/65/75/100/100,The other 47.5/57.5/82.5/107.5;Not affect charging and jump heavy attack cost"}},//熟谙
            //10506 PassiveSkillDesc 39-41
            {100506,new Desc{ "+4%/lv 动作倍率", "+4%/lv SkillEffect"}},//通变/Versatility
            {100507,new Desc{ "+5%/lv,基础55点棍势/秒","+5%/lv,Base 55 Focus/s"}},//精壮
            {100602,new Desc{ "Lv1:+20%/Lv2:+30%",}},//克刚
            //10603 51/52 SkillEffectFloat
            {100603,new Desc{ "Lv1-2:+5%/8% 动作倍率", "Lv1-2:+5%/8% SkillEffect"}},//压溃/Smashing Force
            {100610,new Desc{ "+100",}},//抖擞
            {100611,new Desc{ "根据棍势+5%/10%/15%/20% 攻击", "+5%/10%/15%/20% ATK by focus level"}},//乘胜追击/Vantage Point
            //10702 53/54
            {100702,new Desc{ "-20%/lv", "-20%/lv"}},//铁树/Steel Pillar
            {100706,new Desc{ "20%",}},//拂痒

            {100802,new Desc{ "Lv1-2:回复30/50", "Lv1-2:Recover 30/50"}},//借力/Borrowed Strength//Passive 13/14 SkillCtrlDesc 10701,10798-10801
            {100804,new Desc{ "9秒内+1% 每层","+1% per stack in 9s"}},//骋势
            //When I complained modding Wukong is so hard a month ago,someone told me "any game can be made mod friendly".That's damn right.
            //1048~1050

            //奇术
            //FUStBuffDesc-Talent 1025 TalentSDesc 10901 PassiveSkillDesc 57/58，降低对手的伤害减免，buff基础数值是0，通过passiveskill修改buffeffect
            {100901,new Desc{ "Lv1-2:+10%/15%敌人承受伤害", "Lv1-2:+10%/15% Enemy Damage Taken"}},//脆断/Crash
            //BuffDesc-Talent 1027
            {100902,new Desc{ "+60%持续;+30%敌人承受伤害", "+60% duration;+30% Enemy Damage Taken"}},//瞬机/Evanescence
            {100903,new Desc{ "+15MP",}},//不破不立
            {100904,new Desc{ "50MP->80MP,+30%持续时间", "50MP->80MP, +30% Duration"}},//假借须臾/Time Bargain //100904 10904,10914,10924 Passive 61-63
            {100905,new Desc{ "每层-2%敌人定身抗性,基础8秒","-2% Enemy Immobilize Resist per stack, Base 8s"}},//凝滞//10905 Passive 64/65 Buff 1064 写的是DingshenDefAdditionBase +2,但是Passive里只改了叠加上限没改数值，怀疑是百分比加成，每层+2%
            {101301,new Desc{ "+2/4/5秒，基础20.5秒","+2/4/5s，Base 20.5s"}},//圆明
            {101302,new Desc{ "20/s->23/s",}},//烈烈
            {101303,new Desc{ "Lv1:+10/Lv2:+15 每跳","Lv1:+10/Lv2:+15 per Tick"}},//弥坚
            {101304,new Desc{ "60MP->90MP, 离开后持续5秒","60MP->90MP. 5s Duration after leaving."}},//无挂无碍
            {101305,new Desc{ "+30% -> 40+30%",}},//昂扬
            {101306,new Desc{ "+20%",}},//归根
            {101502,new Desc{ "-10%/lv",}},//不休
            {101503,new Desc{ "每级+0.02攻击力/1法力，基础0.12攻击力/1法力","Each level +0.02 ATK per MP, Base 0.12ATK per MP"}},//智力高强
            {101504,new Desc{ "0.03%暴击/1气力","0.03% Crit Chance per Stamina"}},//放手一搏
            {101505,new Desc{ "20%生命上限","20% MaxHP"}},//舍得

            //身法
            //1059,10/15/15? walk/run/dash?
            {101001,new Desc{ "徐行/奔跑/冲刺速度+10%/15%/15%", "+10%/15%/15% Walk/Run/Sprint speed"}},//纵横/Gallop
            {101002,new Desc{ "+2秒/lv,基础10秒", "+2s/lv,Base10s"}},//养气/Converging Clouds
            //Talent 2101 AtkMul 2000
            {101004,new Desc{ "30MP->40MP;+20% 攻击", "30MP->40MP;+20% ATK"}},//破影一击/Absolute Strike
            {101005,new Desc{ "Lv1:+3%/Lv2:+4% 每层","Lv1:+3%/Lv2:+4% per stack"}},//洞察
            //Talent 11006 Passive 74/75
            {101006,new Desc{ "+15%/lv", "+15%/lv"}},//捣虚/Ruse
            
            {101105,new Desc{ "15s->13s",}},//频频//11105 85 -2s
            {101106,new Desc{ "+15%,持续15秒","+15% in 15s"}},//巩固
            {101102,new Desc{ "+75",}},//知机
            {101103,new Desc{ "Lv1-2:+10MP/15MP;-0.2/0.3秒,基础1秒", "Lv1-2:+10MP/15MP;-0.2s/0.3s,Base 1s"}},//厉无咎/Bold Venture //101103 Passive 77-84

            //毫毛
            //11201 Passive 88/89
            {101201,new Desc{ "+2秒/lv,基础约25秒", "+2s/lv,Base about 25s"}},//存身/Longstrand
            {101202,new Desc{ "每层+0.4s?", "+0.4s per stack?"}},//合契/Synergy//101201 Passive 89 1001101写的+4，实测最多约+4秒？
            {101203,new Desc{ "20MP/lv",}},//毛吞大海
            {101205,new Desc{ "+10%", "+10%"}},//同寿/Grey Hair//11205 96
            //Talent 1082
            {101206,new Desc{ "+15% 攻击", "+15% ATK"}},//仗势/Tyranny of Numbers
            {101207,new Desc{ "积攒效率为本体的20%","20% efficiency comparing to player"}},//玄同
            {101208,new Desc{ "+15%/lv", "+15%/lv"}},//浇油/Insult to Injury//11208 97/98 1083buff基础15%,1/2级天赋+0/+15%
            {101401,new Desc{ "-10秒", "-10s"}},//回阳/Glorious Return//buff 1111
            {101403,new Desc{ "回复20%法力上限", "Recover 20% MaxMP"}},//不增不减/Spirited Return

            {101404,new Desc{ "20%", "20%"}},//去来空/Cycle Breaker//101404 112
            //变化
            {301001,new Desc{ "+3.3%/lv",}},//暗藏神妙
            {301002,new Desc{ "-1.67%/lv,基础 1.25/s","-1.67%/lv,Base 1.25/s"}},//保养精神
            {301003,new Desc{ "+2%/lv,基础 0.5/s","+2%/lv,Base 0.5/s"}},//存神炼气
            {301004,new Desc{ "+2/lv",}},//虚相凶猛
            {301005,new Desc{ "+15/lv",}},//炼实返虚
            {301006,new Desc{ "+2%神力上限/lv,基础 40%","Gain +2% Max Transformation Energy/lv,Base 40%"}},//截伪续真
            
            {301101,new Desc{ "Lv1-3:+10%/15%/20%攻击力", "Lv1-3:+10%/15%/20%ATK"}},//步月
            {301102,new Desc{ "+15/lv",}},//磊磊
            //TalentSDesc 301103 PassiveSkillDesc 232  233 实际是-1.5 3.5
            {301103,new Desc{ "-10%",}},//剪尾
            {301104,new Desc{ "Lv1->6:+4/6/8/10/12/14",}},//爆躁
            {301105,new Desc{ "Lv1->6:+4/6/8/10/12/14",}},//霏霏
            {301106,new Desc{ "60秒内+1 攻击力/每层", "+1 per stack in 60s"}},//红眼/Red Eyes
            {301107,new Desc{ "Lv1->6:+4/6/8/10/12/14",}},//恶秽
            {301109,new Desc{ "Lv1->6:+4/6/8/10/12/14",}},//奔霄
            //TalentSDesc 301108 PassiveSkillDesc 236 BuffDesc-lz
            {301108,new Desc{ "+0.5秒->0.8秒", "+0.5s->0.8s"}},//一闪/Lightning Flash
            {301110,new Desc{ "+12",}},//不坏身

            //根器
   	        {200101,new Desc{ "轻棍1-5段全中获得的棍势由15/18/17/28/40变为15/18/26/35/43", "Light Attack 1-5 focus point if all hit :15/18/17/28/40 -> 15/18/26/35/43"}},//见机强攻/Opportune Watcher
            //buff talent 1053
            {200102,new Desc{ "-12.5秒", "-12.5s"}},//眼乖手疾/Eagle Eye
            {200103,new Desc{ "+15%", "+15%"}},//慧眼圆睁/Keen Insight

            {200201,new Desc{ "+0.066秒,基础0.4(劈棍)/0.366(戳棍)/0.3(不明)/0.5(不明)秒", "+0.066s,Base 0.4(Smash)/0.366(Pillar)/0.3(Unknown)/0.5(Unknown) second"}},//耳听八方/All Ears//20201 Passive 121 287,293,114,10110
            {200202,new Desc{ "-0.1秒，基础1秒", "-0.1s,Base 1s"}},//如撞金钟/Sound as A Bell // Ｐａｓｓｉｖｅ 122-125 Buff-lz 228 buff-talent 1069
            {200203,new Desc{ "5秒内+10%攻击", "+10% ATK in 5s"}},//耳畔风响/Whistling Wind

            {200301,new Desc{ "15秒内+10%伤害加成", "+10% Damage in 15s"}},//气味相投/Lingering Aroma //buff 2107
            //{,new Desc{ "+12", "+12"}},//阳燧珠/In One Breath -
            {200303,new Desc{ "+0.066秒,0-3段翻滚基础无敌时间0.4/0.433/0.466/0.5秒", "+0.066s,0-3 level roll base time 0.4/0.433/0.466/0.5s"}},//屏气敛息/Hold Breath//-20303 Passive  127 buff-lz  10105-10109

            {200404,new Desc{ "+10%丹药持续时间", "+10% Duration"}},//舌尝思/Envious Tongue
            {200401,new Desc{ "5秒内+15%", "+15% in 5s"}},//丹满力足/Refreshing Taste
            //{,new Desc{ "+12", "+12"}},//阳燧珠/Spread the Word
            {200403,new Desc{ "每个增加+4%生命上限的回复量", "+4% MaxHP recover each"}},//遍尝百草/Tongue of A Connoisseur

            {200501,new Desc{ "0-4级棍势气力消耗-20/25/37.5/60,令熟谙天赋效果减半", "0-4 Focus-Level heavy-attack cost :-20/25/37.5/60. Reduce Instinct(talent) effect by half"}},//身轻体快/Nimble Body
            {200502,new Desc{ "+60", "+60"}},//福寿长臻/Everlasting Vitality
            {200503,new Desc{ "+15", "+15"}},//灾愆不侵/Divine Safeguard

            {200601,new Desc{ "+5%/10% 动作倍率", "+5%/10% SkillEffect"}},//万相归真/Elegance in Simplicity
            {200603,new Desc{ "30秒内+30%伤害加成", "+30% Damage in 30s"}},//不生不灭/Unbegotten, Undying
        };
        //套装效果
        public static DescDict SuitInfoDesc = new DescDict
        {
            //SuitInfo和RedQualityInfo都是通过AttrEffectID或TalentID生效

            //Talent 2037->90301 ?,鳞棍2034->15003?
            {900311,new Desc{ "x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"}},//浪里白条/Wave-Rider
            //没找到对应passive
            //{900321,new Desc{ "x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"}},//浪里白条/Wave-Rider
            //650->702，+满级百足734.5
            //Talent 2041 - 2044 写的是0 / 8 / 10,实测是0 / 8 %/ 约10 %
            {900411,new Desc{ "+8%/10%奔跑/冲刺速度", "+8%/10% Run/Sprint speed."}},//日行千里/Swift Pilgrim
            {900412,new Desc{ "每层+10%攻击,持续2秒", "+10% ATK per stack in 2s"}},//日行千里/Swift Pilgrim
            {900421,new Desc{ "每秒+12棍势", "+12 Focus/s"}},//日行千里/Swift Pilgrim

            {900511,new Desc{ "每个天赋+24防御", "+24 DEF per Relic Talent"}},//心灵福至/Fortune's Favor
            //Talent 2063
            {900611,new Desc{ "20%减伤", "20% Damage Reduction"}},//走石飞砂/Raging Sandstorm
            {900711,new Desc{ "+20% 动作倍率", "+20% SkillEffect"}},//离火入魔/Outrage
            {705,new Desc{ "+25%伤害 -30%伤害减免", "+25% Damage.-30% DamageReduction"}},//离火入魔/Outrage
            {900811,new Desc{ "+10赋雷攻击", "+10 Thunder ATK"}},//龙血玄黄/Thunder Veins
            {900821,new Desc{ "+10赋雷攻击", "+10 Thunder ATK"}},//龙血玄黄/Thunder Veins
            {901011,new Desc{ "20秒内+15%攻击", "+15% ATK in 20s"}},//借假修真/Gilded Radiance
            {901012,new Desc{ "暴击+3元气,\n击杀+5元气", "+3/+5 Qi when Crit/Kill"}},//借假修真/Gilded Radiance
            //96005 / 96006 实测每次减少0.75~1秒冷却不定？？非传奇和传奇没有区别？？
            {901211,new Desc{ "+15棍势", "+15 Focus"}},//举步生风/Gale Guardian
            {901212,new Desc{ "+-0.75~1秒冷却?", "-0.75s~1s CD？"}},//举步生风/Gale Guardian
            //TalentSDesc 91221 91222 91223,但是Passive里只有91221
            {901221,new Desc{ "-0.1s无敌时间，不会额外减少冷却", "-0.1s Immune Duration.Won't reduce more CD"}},//举步生风/Gale Guardian
            
            //90711 Passive 167
            //Talent 2135 - 0.005，实测变身还原 + 1.5每秒 ；2137 - 0.00375, 实测约1.12每秒，结合 - 0.005推测应为1.125 / s
            {901311,new Desc{ "+20%伤害减免，结束变身时获得12秒黑泥，化身还原后获得6秒黑泥", "+20% DamageReduction.Gain Mud in 12s upon quiting tranformation;Gain Mud in 6s upon quiting vigor."}},//泥塑金装/From Mud to Lotus
            {901312,new Desc{ "翻滚回复约0.3神力,结束变身后12秒内+1.5/s神力回复，化身还原后4秒(not 6)内+1.125/s神力回复", "About +0.3 Might upon roll. +1.5/s Might Recover for 12s upon quiting tranformation.+1.125/s Might Recover in 4s(not 6s) upon quiting vigor."}},//泥塑金装/From Mud to Lotus
            {901411,new Desc{ "x0.8毒伤(和抗性效果乘算)", "x0.8 Poison Damage(Multi with Poison Resist effect)"}},//花下死/Poison Ward
            {901412,new Desc{ "+20%攻击", "+20% ATK"}},//花下死/Poison Ward
            //独角仙套 91912 Passive 185
            {901511,new Desc{ "+10%灵蕴", "+10% Will"}},//锱铢必较/Every Bit Counts
            {901611,new Desc{ "5秒内+10%防御", "+10% DEF in 5s"}},//百折不挠/Unyielding Resolve
            {901811,new Desc{ "+50棍势", "+50 Focus"}},//铜心铁胆/Iron Will
            {901812,new Desc{ "-5秒冷却", "-5s CD"}},//铜心铁胆/Iron Will
            {901911,new Desc{ "+100棍势", "+100 focus"}},//毒魔狠怪/Fuban Strength
            {901912,new Desc{ "+20%持续时间", "+20% Duration"}},//毒魔狠怪/Fuban Strength
            {902011,new Desc{ "10秒内+8%暴击", "+8% Crit in 10s"}},//试比天高/Heaven's Equal
            {902012,new Desc{ "-1秒冷却 per Hit", "-1s CD per Hit"}},//试比天高/Heaven's Equal


            {900921,new Desc{ "+20法力消耗;假身持续时间不变,但不会因破隐而消失", "+20 MP Cost"}},//乘风乱舞/Dance of the Black Wind
            //Talent 2181 青铜套内部叫黑铁
            {901712,new Desc{ "-15秒冷却", "-15s CD"}},//炼魔荡怪/Evil Crasher
        };
        //精魂(RZD)被动 VIPassiveDesc.OnChangeItemId
        public static DescDict SpiritDesc = new DescDict
        {
            {-1,new Desc{ "1/4/6级时,{0}", "{0} at Lv1/4/6"}},
            //注意和珍玩不同，自动生成的需要设成"",而不是null
            {8011,new Desc{ "", ""}}, //"广谋"
            {8012,new Desc{ "[-6%]/[8%]/[10%]", "[-6%]/[8%]/[10%]"}},//波波浪浪/波里个浪
            {8013,new Desc{ "", ""}}, //"幽魂"
            {8014,new Desc{ "", ""}}, //"鼠司空"
            {8015,new Desc{ "", ""}}, //"百目真人"
            {8017,new Desc{ "", ""}}, //"虎伥"
            {8020,new Desc{ "", ""}}, //"不空"
            //8022 ["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
            {8024,new Desc{ "", ""}}, //"不净"
            {8025,new Desc{ "", ""}}, //"不白"
            {8026,new Desc{ "", ""}}, //"不能"
            {8027,new Desc{ "",""}}, //"虫总兵"
            {8028,new Desc{ "[-10]/[15]/[20]消耗", "[-10]/[15]/[20] MP Cost"}}, //"儡蜱士"
            {8029,new Desc{ "", ""}}, //"青冉冉"
            {8030,new Desc{ "", ""}}, //"蝎太子"
            //取平地普通移动再停止后最后一个数字，该数字是稳定的,观察PlayerLocomotion.MaxSpeed:[650]/[663]/[669]/682.5
            {8031,new Desc{ "", ""}}, //"百足虫"
            {8032,new Desc{ "",""}}, //"兴烘掀·掀烘兴"
            {8033,new Desc{ "",""}}, //"石父"
            {8034,new Desc{ "", ""}}, //"地罗刹"
            {8035,new Desc{ "", ""}}, //"鳖宝"--303184/33484 Passive 292/272
            {8036,new Desc{ "",""}}, //"燧统领"
            {8037,new Desc{ "",""}}, //"燧先锋"
            {8038,new Desc{ "", ""}}, //"琴螂仙"
            {8039,new Desc{ "", ""}}, //"火灵元母"
            {8040,new Desc{ "", ""}}, //"老人参精" --303157 [33657]/[33457]/[33157] 244/264/
            {8041,new Desc{ "", ""}}, //"蘑女"
            {8042,new Desc{ "", ""}}, //"菇男"
            
            {8061,new Desc{ "", ""}}, //"狼刺客"
            {8062,new Desc{ "", ""}}, //"疯虎"
            {8063,new Desc{ "", ""}}, //"沙二郎"
            {8064,new Desc{ "", ""}}, //"鼠禁卫"
            {8065,new Desc{ "", ""}}, //"骨悚然"
            {8066,new Desc{ "", ""}}, //"狸侍长"
            {8067,new Desc{ "", ""}}, //"疾蝠"
            {8068,new Desc{ "[-6%]/[8%]/[10%]气力消耗,[+10%]/[12%]/[15%] 跳跃轻击动作倍率", "[-6%]/[8%]/[10%] Stamina Cost,[+10%]/[12%]/[15%] SkillEffect"}}, //"石双双" --33681/33581 [247]/[267]/[287]
            //--26/[24.5]/[24]/[23.5]
            {8069,new Desc{ "减少每秒消耗[-1.5]/[2]/[2.5]，基础消耗26/s(移步48/s)", "[-1.5]/[2]/[2.5] cost per second(Base Cost:26/s.Base Cost When moving:48/s)"}}, //"鼠弩手"
            {8070,new Desc{ "", ""}}, //"地狼"
            {8071,new Desc{ "", ""}}, //"隼居士"
            {8072,new Desc{ "", ""}}, //"赤发鬼"[--33687]/[33487]/[33187] [295]/[275]/[255]
            {8073,new Desc{ "", ""}}, //"戒刀僧"
            {8074,new Desc{ "", ""}}, //"泥塑金刚"
            {8075,new Desc{ "", ""}}, //"夜叉奴"
            {8077,new Desc{ "", ""}}, //"鸦香客"
            {8078,new Desc{ "", ""}}, //"虫校尉"
            {8079,new Desc{ "", ""}}, //"蜻蜓精"
            {8081,new Desc{ "", ""}}, //"傀蛛士"
            {8083,new Desc{ "", ""}}, //"蛇捕头"
            {8084,new Desc{ "", ""}}, //"蛇司药"
            {8085,new Desc{ "", ""}}, //"幽灯鬼"
            {8086,new Desc{ "", ""}}, //"黑脸鬼"
            {8087,new Desc{ "", ""}}, //"牯都督"
            {8088,new Desc{ "[-6]/[-8]/[-10]", "[-6]/[8]/[10]"}}, //"雾里云·云里雾"
            {8092,new Desc{ "[-6]/[8]/[10]点消耗", "[-6]/[8]/[10]"}}, //"九叶灵芝精"
            //{8076,new Desc{ "", ""}}, //"巡山鬼"--303686 [254]/[274]/[294] [1061410]/[1061430]/[1061460]?    
        };
        public static DescDict FabaoAttrDesc = new DescDict
        {
            //法宝被动(携带效果,EqDesc AttrDesc) VITreasureDesc::OnChangeItemId
            {19001,new Desc{ }},
            {19002,new Desc{ } },
            {19004,new Desc{ }},
            {19005,new Desc{ }},
        };
        //精魂消耗,PreFillDict时填充
        public static DescDict SpiritCost = new DescDict
        {
        };        

        public readonly static ConstDescDict EBGUAttrFloatDictConst = new ConstDescDict
        {
            {(int)EBGUAttrFloat.HpMax,new Desc{ "{0}生命上限","{0} MaxHP" } },
            {(int)EBGUAttrFloat.MpMax,new Desc{ "{0}法力上限","{0} MaxHP" } },
            //{(int)EBGUAttrFloat.B1StunMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.StaminaDepletedLimit,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.StaminaMax,new Desc{ "{0}气力上限","{0} MaxStamina" } },
            //{(int)EBGUAttrFloat.SkillSuperArmorMax,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.TransEnergyMax,new Desc{ "{0}神力上限","{0} MaxMight" } },
            //{(int)EBGUAttrFloat.EnergyMinConsume,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.EnergyConsumeSpeed,new Desc{ "{0}神力消耗速度","{0} Might Consume Speed" } },
            {(int)EBGUAttrFloat.EnergyIncreaseSpeed,new Desc{ "{0}神力回复","{0} Might Recover" } },
            //{(int)EBGUAttrFloat.SpecialEnergyMax,new Desc{ "{0}","{0}" } },//用途不明
            {(int)EBGUAttrFloat.FabaoEnergyMax,new Desc{ "{0}法宝能量上限","{0} Max Vessel Energy" } },
            {(int)EBGUAttrFloat.VigorEnergyMax,new Desc{ "{0}精魂能量上限","{0} Max Vigor Energy" } },
            //{(int)EBGUAttrFloat.BlockCollapseArmorMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.FreezeAbnormalAccMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnAbnormalAccMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonAbnormalAccMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderAbnormalAccMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BlindSlotMax,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.BloodBottomNumMax,new Desc{ "{0}葫芦存储上限","{0} Max Hulu Num" } },
            {(int)EBGUAttrFloat.PelevelMax,new Desc{ "{0}棍势等级上限","{0} Max FocusLevel" } },
            //{(int)EBGUAttrFloat.ShieldMax,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.PevalueMax,new Desc{ "{0}棍势上限","{0} Max Focus" } },
            //{(int)EBGUAttrFloat.YinAbnormalAccMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAbnormalAccMax,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.HpMaxMul,new Desc{ "{0}%生命上限","{0}% MaxHP" } },
            {(int)EBGUAttrFloat.MpMaxMul,new Desc{ "{0}%法力上限","{0}% MaxMP" } },
            {(int)EBGUAttrFloat.AtkMul,new Desc{ "{0}%攻击","{0}% ATK" } },
            {(int)EBGUAttrFloat.DefMul,new Desc{ "{0}%防御","{0}% DEF" } },
            //{(int)EBGUAttrFloat.B1StunMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.StaminaDepletedLimitMul,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.StaminaMaxMul,new Desc{ "{0}%气力上限","{0}% MaxStamina" } },
            {(int)EBGUAttrFloat.StaminaRecoverMul,new Desc{ "{0}%气力回复","{0}% Stamina Recover" } },
            //{(int)EBGUAttrFloat.KptturnSpeedMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.FreezeAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YinAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.StaminaCostMultiperMul,new Desc{ "{0}% 气力消耗倍率加成","{0}% Additional Stamina Cost Multiper" } },
            {(int)EBGUAttrFloat.TransEnergyMaxMul,new Desc{ "{0}%神力上限","{0}% Max Might" } },
            //{(int)EBGUAttrFloat.EnergyMinConsumeMul,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.EnergyConsumeSpeedMul,new Desc{ "{0}%神力消耗速度","{0}% Might Consume Speed" } },
            {(int)EBGUAttrFloat.EnergyIncreaseSpeedMul,new Desc{ "{0}%神力回复","{0}% Might Recover" } },
            {(int)EBGUAttrFloat.HpMaxBase,new Desc{ "{0}生命上限","{0} MaxHP" } },
            {(int)EBGUAttrFloat.MpMaxBase,new Desc{ "{0}法力上限","{0} MaxMP" } },
            {(int)EBGUAttrFloat.AtkBase,new Desc{ "{0}攻击","{0} ATK" } },
            {(int)EBGUAttrFloat.DefBase,new Desc{ "{0}防御","{0} DEF" } },
            //{(int)EBGUAttrFloat.B1StunMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.StaminaDepletedLimitBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.StaminaMaxBase,new Desc{ "{0}气力上限","{0} MaxStamina" } },
            {(int)EBGUAttrFloat.StaminaRecoverBase,new Desc{ "{0}气力回复","{0} Stamina Recover" } },
            //{(int)EBGUAttrFloat.SkillSuperArmorMaxBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.CritRateBase,new Desc{ "{0}%暴击率","{0}% Crit Rate" } },
            {(int)EBGUAttrFloat.CritMultiplierBase,new Desc{ "{0}%暴伤","{0}% Crit Damage" } },
            {(int)EBGUAttrFloat.TenacityBase,new Desc{ "{0}霸体","{0} Tenacity" } },
            //{(int)EBGUAttrFloat.KptturnSpeedBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.EarPlugBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritRateDefBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritDmgMulDefBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.DmgAdditionBase,new Desc{ "{0}%伤害加成","{0}% Damage Bonus" } },
            {(int)EBGUAttrFloat.DmgDefBase,new Desc{ "{0}%减伤","{0}% Damage Bonus" } },
            //{(int)EBGUAttrFloat.BlockCollapseArmorMaxBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.DingshenDefAdditionBase,new Desc{ "{0}定身抗性","{0} Immobilize Resist" } },
            //{(int)EBGUAttrFloat.FreezeAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.FreezeAtkBase,new Desc{ "{0}赋冰攻击","{0} Chill ATK" } },
            {(int)EBGUAttrFloat.BurnAtkBase,new Desc{ "{0}赋火攻击","{0} Burn ATK" } },
            {(int)EBGUAttrFloat.PoisonAtkBase,new Desc{ "{0}赋毒攻击","{0} Poison ATK" } },
            {(int)EBGUAttrFloat.ThunderAtkBase,new Desc{ "{0}赋雷攻击","{0} Shock ATK" } },
            {(int)EBGUAttrFloat.FreezeDefBase,new Desc{ "{0}冰抗","{0} Chill Resist" } },
            {(int)EBGUAttrFloat.BurnDefBase,new Desc{ "{0}火抗","{0} Burn Resist" } },
            {(int)EBGUAttrFloat.PoisonDefBase,new Desc{ "{0}毒抗","{0} Poison Resist" } },
            {(int)EBGUAttrFloat.ThunderDefBase,new Desc{ "{0}雷抗","{0} Shock Resist" } },
            {(int)EBGUAttrFloat.BloodBottomNumMaxBase,new Desc{ "{0}葫芦存储上限", "{0} Max Hulu Num" } },
            {(int)EBGUAttrFloat.PelevelMaxBase,new Desc{ "{0}棍势等级上限","{0} Max Focus Level" } },
            //{(int)EBGUAttrFloat.ShieldMaxBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.PevalueMaxBase,new Desc{ "{0}棍势上限","{0} Max Focus" } },
            //{(int)EBGUAttrFloat.YinAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YinAtkBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAtkBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YinDefBase,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangDefBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.StaminaCostMultiperBase,new Desc{ "{0}%气力消耗倍率","{0}% Stamina Cost MultiperBase" } },
            {(int)EBGUAttrFloat.TransEnergyMaxBase,new Desc{ "{0}神力上限","{0} Max Might" } },
            //{(int)EBGUAttrFloat.EnergyMinConsumeBase,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.EnergyConsumeSpeedBase,new Desc{ "{0}神力消耗速度","{0} Might Consueme Speed" } },
            {(int)EBGUAttrFloat.EnergyIncreaseSpeedBase,new Desc{ "{0}神力回复","{0} Might Recover" } },
            {(int)EBGUAttrFloat.CommDropAddition,new Desc{ "{0}%掉落加成","{0}% Drop Rate" } },
            {(int)EBGUAttrFloat.ExpDropAddition,new Desc{ "{0}%道行(经验)","{0}% Exp" } },
            {(int)EBGUAttrFloat.SpiritDropAddition,new Desc{ "{0}%灵蕴","{0}% Money" } },

            {(int)EBGUAttrFloat.Hp,new Desc{ "{0}生命","{0}HP" } },
            {(int)EBGUAttrFloat.Mp,new Desc{ "{0}法力","{0}MP" } },
            //{(int)EBGUAttrFloat.Atk,new Desc{ "{0}攻击","{0}ATK" } },
            //{(int)EBGUAttrFloat.Def,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.B1Stun,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.Stamina,new Desc{ "{0}气力","{0}Stamina" } },
            //{(int)EBGUAttrFloat.StaminaRecover,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.SkillSuperArmor,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritRate,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritMultiplier,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.Tenacity,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.KptturnSpeed,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.EarPlug,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritRateDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.CritDmgMulDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.DmgAddition,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.DmgDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BlockCollapseArmor,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.DingshenDefAddition,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.FreezeAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BlindSlot,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.FreezeAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.FreezeDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BurnDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.PoisonDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.ThunderDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.BloodBottomNum,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.Pelevel,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.CurEnergy,new Desc{ "{0}神力","{0} Might" } },
            //{(int)EBGUAttrFloat.SpecialEnergy,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.Shield,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.Pevalue,new Desc{ "{0}棍势","{0} Focus" } },
            //{(int)EBGUAttrFloat.YinAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAbnormalAcc,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YinAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangAtk,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YinDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.YangDef,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.StaminaCostMultiper,new Desc{ "{0}","{0}" } },
            {(int)EBGUAttrFloat.FabaoEnergy,new Desc{ "{0}法宝能量","{0} Vessel Energy" } },
            {(int)EBGUAttrFloat.VigorEnergy,new Desc{ "{0}精魂能量","{0} Vigor Energy" } },
            //{(int)EBGUAttrFloat.AttrFloatMax,new Desc{ "{0}","{0}" } },
            //{(int)EBGUAttrFloat.EnumMax,new Desc{ "{0}","{0}" } },
        };
        public readonly static ConstDescDict ValOpDictConst = new ConstDescDict()
        {
            {(int)EValOp.Mul,new Desc{ "{0}%","{0}%" } },
            {(int)EValOp.Add,new Desc{ "{0}" ,"{0}" } },
            {(int)EValOp.OverrideBase,new Desc{"变为{0}","Set to {0}"} },
        };
        public readonly static ConstDescDict BuffAndSkillEffectTypeDictConst = new ConstDescDict() {
            {(int)EBuffAndSkillEffectType.AddAttr,new Desc{ "{0}","{0}" } },
            {(int)EBuffAndSkillEffectType.RecoverAttr,new Desc{ "回复{0}","Recover {0}" } },
            {(int)EBuffAndSkillEffectType.ChangeMoveSpeed,new Desc{ "{0}{1}速度","{0}{1}Speed" } },
        };
        public readonly static ConstDescDict BuffEffectTriggerTypeDictConst = new ConstDescDict() {
            {(int)EBuffEffectTriggerType.BeAttacked,new Desc{ "{0} per Hit","{0} per Hit" } },
            {(int)EBuffEffectTriggerType.OnSkillDamage,new Desc{ "{0} per Hit","{0} per Hit" } },
            {(int)EBuffEffectTriggerType.Time,new Desc{ "<Interval>每{1}秒</Interval>{0}", "{0} <Interval>per {1} sec</Interval>" } },
            //generation是一次性获得，不需要时间
            {(int)EBuffEffectTriggerType.Generation,new Desc{ "{0}", "{0}" } },
        };
        public readonly static ConstDescDict EAttrCostTypeDictConst = new ConstDescDict() {
            {(int)EAttrCostType.VigorEnergy,new Desc{ "{0}元气<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Qi <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.Hp,new Desc{ "{0}生命<Positive>获得</Positive><Negative>消耗</Negative>", "{0} HP <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.Mp,new Desc{ "{0}法力<Positive>获得</Positive><Negative>消耗</Negative>", "{0} MP <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.Stamina,new Desc{ "{0}气力<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Stamina <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.FabaoEnergy,new Desc{ "{0}法宝能量<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Fabao Energy <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.TransEnergy,new Desc{ "{0}神力<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Might <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.PotentialEnergy,new Desc{ "{0}棍势<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Focus <Positive>Gain</Positive><Negative>Cost</Negative>" } },
            {(int)EAttrCostType.BloodBottleNum,new Desc{ "{0}酒<Positive>获得</Positive><Negative>消耗</Negative>", "{0} Hulu <Positive>Gain</Positive><Negative>Cost</Negative>" } },
        };
        public readonly static Desc DefaultDesc = new Desc() { "{0}", "{0}" };
        public readonly static Desc EmptyDesc = new Desc() { "", "" };
        public readonly static Desc DurationFormat = new Desc(){"<Duration>{1}秒内</Duration>{0}","{0}<Duration> in {1}s</Duration>"};
        public readonly static Desc GreaterFormat = new Desc() { "<Target>目标</Target>{0}高于{1}时{2}", "{2} when<Target> target</Target> {0} over {1}" };
        public readonly static Desc LesserFormat = new Desc() { "<Target>目标</Target>{0}不高于{1}时{2}", "{2} when<Target> target</Target> {0} under {1}" };
        public readonly static Desc FullFormat = new Desc() { "<Target>目标</Target>满{0}时{1}", "{1} when<Target> target</Target> full {0}" };
        public readonly static ConstDescDict SpeedNameDictConst = new ConstDescDict() {
            {0b1,new Desc{ "徐行","Walk" } },
            {0b10,new Desc{ "奔跑","Run" } },
            {0b11,new Desc{ "徐行和奔跑","Walk and Run" } },
            {0b100,new Desc{ "冲刺", "Run" } },
            {0b101,new Desc{ "冲刺和徐行","Walk and Sprint" } },
            {0b110,new Desc{ "奔跑和冲刺","Run and Sprint" } },
            {0b111,new Desc{ "移动","Move" } },
        };
    }
}
