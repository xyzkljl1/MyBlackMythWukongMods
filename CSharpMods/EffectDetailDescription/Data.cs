using BtlShare;
using OssB1;
using ResB1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using DescDict = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>>;
namespace EffectDetailDescription
{
    static public class Data
    {
        public enum LanguageIndex:int
        {
            SimpleCN=0,
            English=1
        }
        //ItemId->effectdesc
        public static DescDict ItemEqDesc = new DescDict
        {
            //法宝主动 VITreasureDetail::OnEquipIdChange
            {19001,new List<string>{ "40秒内+600火抗，每秒获得5棍势", "+600 FireResist,+5 Focus/s in 40s"}},
            {19002,new List<string>{ "20秒内+50%伤害减免", "+50% DamageReduction in 20s"}},
        };
        
        public static DescDict ItemDesc = new DescDict
        {
            //消耗品,注意1118等是配方，2118等是丹药
            //鼻根器+10%持续2
            {2218,new List<string>{ "120秒内+60", "+60 in 130s"}},//朝元膏/Essence Decoction
            {2221,new List<string>{ "120秒内+60", "+60 in 130s"}},//益气膏/Tonifying Decoction
            {2224,new List<string>{ "75秒内+10%", "+10% in 75s"}},//倍力丸/Amplification Pellets
            {2227,new List<string>{ "75秒内+8攻击", "+8 ATK in 75s"}},//伏虎丸/Tiger Subduing Pellets
            {2230,new List<string>{ "120秒内+60", "+60 in 130s"}},//延寿膏/Longevity Decoction
            {2234,new List<string>{ "75秒", "75s"}},//坚骨药/Fortifying Medicament
            {2236,new List<string>{ "+60", "+60"}},//镜中丹/Mirage Pill
            {2245,new List<string>{ "75秒内+15攻击15%暴击20%暴伤", "+15 ATK/15% Crit/20% CritDamage in 75s"}},//龙光倍力丸/Loong Aura Amplification Pellets
            {2247,new List<string>{ "75秒内+10%", "+10% in 75s"}},//避凶药/Evil Repelling Medicament
            
            {2251,new List<string>{ "+480", "+480"}},//加味参势丸/Enhanced Ginseng Pellets
            {2252,new List<string>{ "30秒内+10/秒", "+10/s in 30s"}},//参势丸/Ginseng Pellets
            {2253,new List<string>{ "75秒内+15攻击", "+15 ATK in 75s"}},//聚珍伏虎丸/Enhanced Tiger Subduing Pellets
            
            {2204,new List<string>{ "75秒内+30", "+30 in 75s"}},//温里散/Body-Warming Powder
            {2205,new List<string>{ "75秒内+30", "+30 in 75s"}},//度瘴散/Antimiasma Powder
            {2206,new List<string>{ "75秒内+30", "+30 in 75s"}},//神霄散/Shock-Quelling Powder
            {2207,new List<string>{ "120秒", "+60 in 130s"}},//轻身散/Body-Fleeting Powder
            {2208,new List<string>{ "75秒内+30", "+30 in 75s"}},//清凉散/Body-Cooling Powder
            {2213,new List<string>{ "75秒", "75s"}},//登仙散/Ascension Powder
            //{,new List<string>{ "120秒", "130s"}},//九转还魂丹/Tonifying Decoction


            {18007,new List<string>{ "15秒内+15抗性", "+15 in 15s"}},//湘妃葫芦/Xiang River Goddess Gourd
            {18011,new List<string>{ "每30秒回满葫芦", "Refill each 30s"}},//青田葫芦
            {18012,new List<string>{ "20秒内+15攻击", "+15 ATK in 20s"}},//五鬼葫芦/Plaguebane Gourd
            {18014,new List<string>{ "20秒内+20攻击", "+20 ATK in 20s"}},//妙仙葫芦/Immortal Blessing Gourd
            //2804 DmgAdditionBase 30
            {18015,new List<string>{ "30秒内+30%伤害加成", "+30% Damage in 30s"}},//乾坤彩葫芦/Multi-Glazed Gourd
            {18017,new List<string>{ "15秒内+30气力", "+30 Stamina in 15s"}},//燃葫芦/Firey Gourd

            {2001,new List<string>{ "实际回复28%+15", "Actually 28%+15"}},//椰子酒/Coconut Wine
            {2002,new List<string>{ "实际回复32%+25", "Actually 32%+25"}},//椰子酒·三年陈/3-Year-Old Coconut Wine
            {2003,new List<string>{ "实际回复36%+33", "Actually 36%+33"}},//椰子酒·五年陈/5-Year-Old Coconut Wine
            {2004,new List<string>{ "实际回复39%+40", "Actually 39%+40"}},//椰子酒·十年陈/10-Year-Old Coconut Wine
            {2005,new List<string>{ "实际回复42%+46", "Actually 42%+46"}},//椰子酒·十八年陈/18-Year-Old Coconut Wine
            {2006,new List<string>{ "实际回复45%+51", "Actually 45%+51"}},//椰子酒·三十年陈/30-Year-Old Coconut Wine
            {2007,new List<string>{ "实际回复48%+55", "Actually 48%+55"}},//猴儿酿/Monkey Brew
            {2021,new List<string>{ "实际回复确实是55%", "Indeed 55%"}},//玉液/Jade Dew

            {2009,new List<string>{ "15秒内+30%移动速度", "+30% in 15s"}},//蓝桥风月/Bluebridge Romance--Buff-Talent 2816 写的是15%/15%/15% 实测 约+30%/+30%/约+30% ,why???
            {2011,new List<string>{ "低于20%血量时恢复量24%->60%", "24%->60% under 20% HP"}},//无忧醑/Worryfree Brew
            {2012,new List<string>{ "6秒内+12%攻击", "+12 ATK in 6s"}},//龙膏/Loong Balm
            {2019,new List<string>{ "+20", "+20"}},//琼浆/Jade Essence
            //BuffDesc-Item 92023
            {2022,new List<string>{ "+75.0", "+75.0"}},//松醪/Pinebrew
            {2023,new List<string>{ "+15.0元气;+20法宝能量", "+15.0 Qi;+20.0 Treasure Energy"}},//九霞清醑/Sunset of the Nine Skies
    
            {2303,new List<string>{ "满血时+20法力", "+20 when 100% HP"}},//龟泪/Turtle Tear
            {2304,new List<string>{ "10秒内+2秒持续", "+2s Duration in 10s"}},//困龙须/Stranded Loong's Whisker--92304 Passive 192
            {2305,new List<string>{ "10秒内+25%持续时间", "25% Duration in 10s"}},//灵台药苗/Moutain Lingtai Seedlings --92305 Passive 193
            //Talent 2100 AtkMul 20% 时间10秒，是10秒内施放隐身还是10秒内打出破隐?
            {2306,new List<string>{ "10秒内？+20%攻击", "+20% ATK in 10s?"}},//十二重楼胶/Breath of Fire
            {2307,new List<string>{ "5秒内共回复6%上限(1%*6)", "total 6% MaxHP(1%*6) in 5s"}},//瑶池莲子/Celestial Lotus Seeds
            {2308,new List<string>{ "回复33.33%气力上限,15秒内+50%气力回复", "Recover 33.33% MaxSt.+50% Stamina Recover in 15s"}},//不老藤/Undying Vine
            {2309,new List<string>{ "15秒内+5%", "+5% in 15s"}},//虎舍利/Tiger Relic--92306
            {2310,new List<string>{ "15秒内+10% ", "+10% in 15s"}},//梭罗琼芽/Laurel Buds
            {2311,new List<string>{ "15秒内+15", "+15 in 15s"}},//甜雪/Sweet Ice
            {2312,new List<string>{ "15秒内+15", "+15 in 15s"}},//霹雳角/Thunderbolt Horn
            {2314,new List<string>{ "低于20%血量时额外回复+12%生命上限", "+20% MaxHP when under 20% HP"}},//紫纹缃核/Purple-Veined Peach Pit
            {2315,new List<string>{ "15%？", "15%？"}},//蜂山石髓/Bee Mountain Stone
            {2316,new List<string>{ "30%减伤", "30% Damage Reduction"}},//铁弹/Iron Pellet
            {2317,new List<string>{ "15秒内踉跄;定身法-2秒，安神法-5秒，聚形散气-3秒，铜头铁臂-2秒，毫毛-8秒", "Stagger in 15s;Immobilize -2s,Ring of Fire -5s,Cloud Step -3s,Rock Solid-2s,A Pluck of Many -8s"}},//瞌睡虫蜕/Slumbering Beetle Husk
            {2318,new List<string>{ "10秒", "10s"}},//铜丸/Copper Pill
            {2319,new List<string>{ "15秒内+5秒持续", "+5s Duration in 15s"}},//血杞子/Goji Shoots --92319? Passive 198
            {2320,new List<string>{ "15秒内+0.066秒无敌，0-3段翻滚基础时间0.4/0.433/0.466/0.5秒", "+0.066s in 15s; 0-3 level roll base immue time:0.4/0.433/0.466/0.5"}},//清虚道果/Fruit of Dao--92320 Passive 199 10105-10109
            {2321,new List<string>{ "15秒内+15", "+15 in 15s"}},//火焰丹头/Flame Mediator
            {2322,new List<string>{ "15秒内+5%暴击,+12%移动速度", "+5% CritRate/+12% Speed in 15s"}},//双冠血/Double-Combed Rooster Blood
            {2323,new List<string>{ "15秒内+15", "+15 in 15s"}},//胆中珠/Gall Gem
            {2324,new List<string>{ "11秒内共回复12%生命上限(1%*12)", "total 12% MaxHP(1%*12) in 11s"}},//蕙性兰/Graceful Orchid
            {2325,new List<string>{ "15秒内+5%防御", "+5% in 15s"}},//嫩玉藕/Tender Jade Lotus
            {2326,new List<string>{ "+30", "+30"}},//铁骨银参/Steel Ginseng
            //Item 92317 92327 92328
            {2327,new List<string>{ "30秒内+30生命上限", "+30 in 30s"}},//青山骨/Goat Skull

        };
        //\["(.*?)"\]=(".*?"),[ ]*\["(.*?)"\]=(".*?"),
        //{,new List<string>{ $2, $4}},//$1/$3
        //珍玩装备
        public static DescDict EquipDesc = new DescDict
        {
            //首饰
            {16001,new List<string>{ "+8", "+8"}},//细磁茶盂/Fine China Tea Bowl
            {16002,new List<string>{ "+3%", "+3%"}},//猫睛宝串/Cat Eye Beads
            {16003,new List<string>{ "+14", "+14"}},//玛瑙罐/Agate Jar
            {16004,new List<string>{ "6秒内+8%攻击", "+8% ATK in 6s"}},//虎头牌/Tiger Tally
            {16005,new List<string>{ "身法-1秒/奇术-3秒/毫毛-6s", "-1s/3s/6s by type"}},//砗磲佩/Tridacna Pendant
            {16006,new List<string>{ "+10%", "+10%"}},//金花玉萼/Goldflora Hairpin
            {16007,new List<string>{ "+45", "+45"}},//琉璃舍利瓶/Glazed Reliquary
            //695.5
            {16008,new List<string>{ "+7% 移动速度", "+7% move speed"}},//风铎/Wind Chime --1006008 buff-desc 96008 写的是7%实测也是7%
            {16009,new List<string>{ "+12", "+12"}},//雷榍/Thunderstone
            {16010,new List<string>{ "+12", "+12"}},//耐雪枝/Frostsprout Twig
            {16011,new List<string>{ "-20%神力消耗速度", "-20% Energy consume speed"}},//白狐毫/Snow Fox Brush
            {16012,new List<string>{ "100次?", "100 times?"}},//未来珠/Maitreya's Orb--buff-item 96010-96012
            {16013,new List<string>{ "+2%", "+2%"}},//金色鲤/Golden Carp
            {16014,new List<string>{ "+1%,有日金乌时+3%", "+1%, +3% if have Gold Sun"}},//月玉兔/Jade Moon Rabbit
            {16015,new List<string>{ "生命不足一半时+10%", "+10% under 50% HP"}},//三清令/Tablet of the Three Supreme
            {16016,new List<string>{ "180秒内+60生命上限,+40法力/气力上限", "+60 MaxHP,+40 MaxMP/MaxSt in 180s"}},//定颜珠/Preservation Orb
            {16017,new List<string>{ "+1%,有月玉兔时+3%", "+1%, +3% if have Jade Moon"}},//日金乌/Gold Sun Crow
            {16018,new List<string>{ "+12", "+12"}},//错金银带钩/Cuo Jin-Yin Belt Hook
            {16019,new List<string>{ "满血时+15攻击", "+15 when full HP"}},//金钮/Gold Button
            {16020,new List<string>{ "+12", "+12"}},//阳燧珠/Flame Orb
            {16021,new List<string>{ "+10", "+10"}},//水火篮/Daoist's Basket of Fire and Water
            {16022,new List<string>{ "+8%防御", "+8% DEF"}},//辟水珠/Waterward Orb
            {16023,new List<string>{ "1-4段棍势上限由100/210/330/480变为90/200/320/470", "Lv.1-4 Max focus point:100/210/330/480 -> 90/200/320/470"}},//琥珀念珠/Amber Prayer Beads
            {16024,new List<string>{ "+10%", "+10%"}},//仙箓/Celestial Registry Tablet
            {16025,new List<string>{ "6秒内+12%攻击", "+12% ATK in 6s"}},//虎筋绦子/Tiger Tendon Belt
            //16026仙胞石片
            {16027,new List<string>{ "15秒内+8%", "+8% in 15s"}},//摩尼珠/Mani Bead
            {16028,new List<string>{ "+15%", "+15%"}},//博山炉/Boshan Censer
            {16029,new List<string>{ "+30", "+30"}},//不求人/Back Scratcher
            {16030,new List<string>{ "20秒内-80生命上限，+10%攻击", "-80 MaxHP,+10% ATK in 20s"}},//吉祥灯/Auspicious Lantern
            {16031,new List<string>{ "+32防御,每次造成10%天命人攻击力的伤害", "+32 DEF.Deal x0.1 player attack Damage per Hit"}},//金棕衣/Gold Spikeplate
            {16032,new List<string>{ "+9%", "+9%"}},//兽与佛/Beast Buddha
            {16033,new List<string>{ "+5", "+5"}},//铜佛坠/Bronze Buddha Pendant
            {16034,new List<string>{ "+30", "+30"}},//雷火印/Thunderflame Seal
            {16035,new List<string>{ "约0.1/s", "about 0.1/s"}},//君子牌/Virtuous Bamboo Engraving
            {16036,new List<string>{ "+10", "+10"}},//卵中骨/Spine in the Sack
            {16037,new List<string>{ "+5", "+5"}},//白贝腰链/White Seashell Waist Chain

            //装备独门妙用
            //散件
	        {17001,new List<string>{ "高于1%血量时每秒-3HP,额外回复+15%生命上限", "-3HP/s when over 1% HP.+15% MaxHP Recover"}},//地灵伞盖/Earth Spirit Cap
            {17002,new List<string>{ "饮酒后15秒内+10攻击,20秒后-20攻击", "+10 ATK in 15s;-20 ATK after 20s"}},//长嘴脸/Snout Mask
            {17003,new List<string>{ "+2%", "+2%"}},//鳖宝头骨/Skull of Turtle Treasure
            {17004,new List<string>{ "15秒内+40", "+40 in 15s"}},//山珍蓑衣/Ginseng Cape
            //蜢虫头907005 97005、97015 185、186
            {17005,new List<string>{ "+15%/20% 动作倍率", "+15%/20% SkillEffect"}},//长须头面/Locust Antennae Mask
            //Talent 2702
            {17006,new List<string>{ "+15%攻击", "+15%ATK"}},//白脸子/Grey Wolf Mask
            {17008,new List<string>{ "阳:+20%伤害减免。阴:20%暴击-30%伤害减免", "Yang:+20% DamageReduction. Yin:+20% Crit,-30% DamageReduction"}},//阴阳法衣/Yin-Yang Daoist Robe
            //Talent 2713/2714
            {17010,new List<string>{ "300秒内+40生命/法力上限", "+40 MaxHP/MP in 300s"}},//南海念珠/Guanyin's Prayer Beads
            {17011,new List<string>{ "-10%", "-10%"}},//金刚护臂/Vajra Armguard
            
            //套装
            {10701,new List<string>{ "低于半血时+15攻击", "+15ATK under 50% HP"}},//厌火夜叉面/Yaksha Mask of Outrage
            {11601,new List<string>{ "+10棍势，强硬时+30", "+10 Focus.+30 when Tenacity"}},//大力王面/Bull King's Mask
            {10302,new List<string>{ "低于半血时每秒+1.5%生命上限(水中+2%)", "+1.5%MaxHP/s under 50% MaxHP;+2% MaxHP/s in water"}},//锦鳞战袍/Serpentscale Battlerobe
            {11001,new List<string>{ "+100棍势", "+100"}},//金身怒目面/Golden Mask of Fury
            //实际没有11902，从11912开始
            {11902,new List<string>{ "+15", "+15"}},//昆蚑毒敌甲/Venomous Sting Insect Armor
            {11803,new List<string>{ "3秒内+15%攻击", "+15% ATK in 3s"}},//玄铁硬手/Iron-Tough Gauntlets
            {10603,new List<string>{ "每消耗一段棍势8秒内+6%暴击", "+6% in 8s for each focus level"}},//赭黄臂甲/Ochre Armguard
            {11304,new List<string>{ "10秒", "10s"}},//不净泥足/Non-Pure Greaves
            {11204,new List<string>{ "+10", "+10"}},//藏风护腿/Galeguard Greaves
            //Talent 2142
            {11402,new List<string>{ "20秒内+10%攻击", "+10%ATK in 20s"}},//羽士戗金甲/Centipede Qiang-Jin Armor
            //Talent 2098
            {10904,new List<string>{ "+15%攻击", "+15%ATK"}},//乌金行缠/Ebongold Gaiters

            {15012,new List<string>{ "5秒内获得共127.5棍势(2.5+25*5)", "Gain 127.5(2.5+25*5) Focus in 5s"}},//狼牙棒/Spikeshaft Staff
            {15007,new List<string>{ "每段棍势+5HP(中毒敌人+40)", "+5 HP(40 for Poisoned enemy) for each focus level"}},//昆棍·百眼/Visionary Centipede Staff
            {15013,new List<string>{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍·蛛仙/Spider Celestial Staff
            //{,new List<string>{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍/Chitin Staff
            {15018,new List<string>{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍·通天/Adept Spine-Shooting Fuban Staff
            {15016,new List<string>{ "每点防御+0.15攻击", "+0.15ATK per DEF"}},//混铁棍/Dark Iron Staff
            {15015,new List<string>{ "+20% 动作倍率", "+20% SkillEffect"}},//飞龙宝杖/Golden Loong Staff//15015 145
            {15101,new List<string>{ "+15% 动作倍率", "+15% SkillEffect"}},//三尖两刃枪/Tri-Point Double-Edged Spear//15015 145
        };
        //天赋
        public static DescDict TalentDisplayDesc = new DescDict
        {
            //根基-气力
            //7.5/6/5.25
            {100101,new List<string>{ "Lv1: -20%/Lv2: -30%,基础 7.5/s","Lv1: -20%/Lv2: -30%,BaseCost 7.5/s"}},//体健
            {100102,new List<string>{ "+50%气力回复","+50% Stamina Recover"}},//绵绵
            {100104,new List<string>{ "-30%,基础消耗 30","-30%,BaseCost 30",}},//身轻
            {100105,new List<string>{ "+15%/lv",}},//调息
            {100106,new List<string>{ "-2.5/lv,基础消耗 20","-2.5/lv,BaseCost 20"}},//猿捷
            {100108,new List<string>{ "+10/lv,基础 40","+10/lv,Base 40"}},//走险
            {100109,new List<string>{ "15%,持续2秒","15% in 2s"}},//绝念
            //{,new List<string>{ "",}},//任翻腾//第一段翻滚由SkillDesc 10301变为10305

            //武艺
            {100201,new List<string>{ "+100/lv,基础800/900/1000", "+100/lv,Base 800/900/1000"}},//直取/Switft Engage//Passive 13/14 SkillCtrlDesc 10701,10798-10801
            {100202,new List<string>{ "18->23 per Hit",}},//接力
            //10508 46/47 SkillEffectDamageExpand
            //{,new List<string>{ "Lv1-2:+100/150", "Lv1-2:+100/150"}},//断筋/TODO
            {100205,new List<string>{ "30%",}},//筋节
            //10506 PassiveSkillDesc 16写的是Mul 50%,实测约+18%，回复量会浮动，约7.5~8，点完变成9上下
            {100209,new List<string>{ "约+18%", "about +18%"}},//化吉/Silver Lining
            {100210,new List<string>{ "-30%, 基础26/秒(移步48/秒)","-30%, BaseCost 26/s(48/s when moving)"}},//应手
            {100211,new List<string>{ "4->6 per Hit, 不影响原地棍花(3 per Hit)","4->6 per Hit. Not affect Staff Spin(3 per Hit)"}},//得心

            //体段修行
            {100301,new List<string>{ "+10/lv",}},//五脏坚固
            {100302,new List<string>{ "+10/lv",}},//气海充盈
            //TalentSDesc 100303 PassiveSkillDesc 20-25
            {100303,new List<string>{ "-5%持续时间/lv", "-5% Duration/lv"}},//灾苦消减/Bane Mitigation
            {100304,new List<string>{ "+1%/lv",}},//怒相厚积
            {100305,new List<string>{ "+10/lv",}},//皮肉粗糙
            {100306,new List<string>{ "+10/lv",}},//法性贯通
            {100307,new List<string>{ "+2.5气力回复/lv","+2.5 Stamina Recover/lv"}},//吐纳绵长
            {100308,new List<string>{ "+2攻击力/lv","+2 ATK/lv"}},//攻势澎湃
            {100309,new List<string>{ "+3/lv",}},//四灾忍耐
            {100310,new List<string>{ "+4%/lv",}},//威能凶猛
            //棍法
            {100502,new List<string>{ "回复+2%/3%/4%(Lv1/Lv2/Lv3) 最大生命","Heal +2%/3%/4%(Lv1/Lv2/Lv3) MaxHP"}},//壮怀
            //55/65/90/115/115 n
            //43/50/67.5/85 Lv3
            //29/32.5/41.2/50 Lv3+r
            //35/40/52.5/65 r
            //100504 Passive 36-38里写的是-10%,但和实测对每种风格都是减固定值
            //劈棍原地重击消耗10+55/65/90/115/115，立棍10+40/65/75/100/100，戳棍47.5/57.5/82.5/107.5，跳劈(三种棍势一样)50/75/100/125/125,
            //3级天赋劈棍43/50/67.5/85，立棍28/50/53/70，戳棍35.5/42.5/60/77.5
            //不影响跳劈,不影响蓄力起手(三种风格起手消耗10/10/15)
            {100504,new List<string>{ "每级令0-4级重棍-4/-5/-7.5/-10/-10气力消耗，有身轻体快(根器)时效果减半，0-4级重棍基础消耗:劈棍55/65/90/115/115,立棍40/65/75/100/100,戳棍47.5/57.5/82.5/107.5;不影响蓄力起手式和跳跃重击", "Each level reduce 0-4 Focus-Level heavy-attack cost: -4/-5/-7.5/-10/-10. Only have 50% effect when having Nimble Body(body relic).Base cost of each focus-level:Smash 55/65/90/115/115,Pillar 40/65/75/100/100,The other 47.5/57.5/82.5/107.5;Not affect charging and jump heavy attack cost"}},//熟谙
            //10506 PassiveSkillDesc 39-41
            {100506,new List<string>{ "+4%/lv 动作倍率", "+4%/lv SkillEffect"}},//通变/Versatility
            {100507,new List<string>{ "+5%/lv,基础55点棍势/秒","+5%/lv,Base 55 Focus/s"}},//精壮
            {100602,new List<string>{ "Lv1:+20%/Lv2:+30%",}},//克刚
            //10603 51/52 SkillEffectFloat
            {100603,new List<string>{ "Lv1-2:+5%/8% 动作倍率", "Lv1-2:+5%/8% SkillEffect"}},//压溃/Smashing Force
            {100610,new List<string>{ "+100",}},//抖擞
            {100611,new List<string>{ "根据棍势+5%/10%/15%/20% 攻击", "+5%/10%/15%/20% ATK by focus level"}},//乘胜追击/Vantage Point
            //10702 53/54
            {100702,new List<string>{ "-20%/lv", "-20%/lv"}},//铁树/Steel Pillar
            {100706,new List<string>{ "20%",}},//拂痒

            {100802,new List<string>{ "Lv1-2:回复30/50", "Lv1-2:Recover 30/50"}},//借力/Borrowed Strength//Passive 13/14 SkillCtrlDesc 10701,10798-10801
            {100804,new List<string>{ "9秒内+1% 每层","+1% per stack in 9s"}},//骋势
            //When I complained modding Wukong is so hard a month ago,someone told me "any game can be made mod friendly".That's damn right.
            //1048~1050

            //奇术
            //FUStBuffDesc-Talent 1025 TalentSDesc 10901 PassiveSkillDesc 57/58，降低对手的伤害减免，buff基础数值是0，通过passiveskill修改buffeffect
            {100901,new List<string>{ "Lv1-2:+10%/15%敌人承受伤害", "Lv1-2:+10%/15% Enemy Damage Taken"}},//脆断/Crash
            //BuffDesc-Talent 1027
            {100902,new List<string>{ "+60%持续;+30%敌人承受伤害", "+60% duration;+30% Enemy Damage Taken"}},//瞬机/Evanescence
            {100903,new List<string>{ "+15MP",}},//不破不立
            {100904,new List<string>{ "50MP->80MP,+30%持续时间", "50MP->80MP, +30% Duration"}},//假借须臾/Time Bargain //100904 10904,10914,10924 Passive 61-63
            {100905,new List<string>{ "每层-2%敌人定身抗性,基础8秒","-2% Enemy Immobilize Resist per stack, Base 8s"}},//凝滞//10905 Passive 64/65 Buff 1064 写的是DingshenDefAdditionBase +2,但是Passive里只改了叠加上限没改数值，怀疑是百分比加成，每层+2%
            {101301,new List<string>{ "+2/4/5秒，基础20.5秒","+2/4/5s，Base 20.5s"}},//圆明
            {101302,new List<string>{ "20/s->23/s",}},//烈烈
            {101303,new List<string>{ "Lv1:+10/Lv2:+15 每跳","Lv1:+10/Lv2:+15 per Tick"}},//弥坚
            {101304,new List<string>{ "60MP->90MP, 离开后持续5秒","60MP->90MP. 5s Duration after leaving."}},//无挂无碍
            {101305,new List<string>{ "+30% -> 40+30%",}},//昂扬
            {101306,new List<string>{ "+20%",}},//归根
            {101502,new List<string>{ "-10%/lv",}},//不休
            {101503,new List<string>{ "每级+0.02攻击力/1法力，基础0.12攻击力/1法力","Each level +0.02 ATK per MP, Base 0.12ATK per MP"}},//智力高强
            {101504,new List<string>{ "0.03%暴击/1气力","0.03% Crit Chance per Stamina"}},//放手一搏
            {101505,new List<string>{ "20%生命上限","20% MaxHP"}},//舍得

            //身法
            //1059,10/15/15? walk/run/dash?
            {101001,new List<string>{ "徐行/奔跑/冲刺速度+10%/15%/15%", "+10%/15%/15% Walk/Run/Sprint speed"}},//纵横/Gallop
            {101002,new List<string>{ "+2秒/lv,基础10秒", "+2s/lv,Base10s"}},//养气/Converging Clouds
            //Talent 2101 AtkMul 2000
            {101004,new List<string>{ "30MP->40MP;+20% 攻击", "30MP->40MP;+20% ATK"}},//破影一击/Absolute Strike
            {101005,new List<string>{ "Lv1:+3%/Lv2:+4% 每层","Lv1:+3%/Lv2:+4% per stack"}},//洞察
            //Talent 11006 Passive 74/75
            {101006,new List<string>{ "+15%/lv", "+15%/lv"}},//捣虚/Ruse
            
            {101105,new List<string>{ "15s->13s",}},//频频//11105 85 -2s
            {101106,new List<string>{ "+15%,持续15秒","+15% in 15s"}},//巩固
            {101102,new List<string>{ "+75",}},//知机
            {101103,new List<string>{ "Lv1-2:+10MP/15MP;-0.2/0.3秒,基础1秒", "Lv1-2:+10MP/15MP;-0.2s/0.3s,Base 1s"}},//厉无咎/Bold Venture //101103 Passive 77-84

            //毫毛
            //11201 Passive 88/89
            {101201,new List<string>{ "+2秒/lv,基础约25秒", "+2s/lv,Base about 25s"}},//存身/Longstrand
            {101202,new List<string>{ "每层+0.4s?", "+0.4s per stack?"}},//合契/Synergy//101201 Passive 89 1001101写的+4，实测最多约+4秒？
            {101203,new List<string>{ "20MP/lv",}},//毛吞大海
            {101205,new List<string>{ "+10%", "+10%"}},//同寿/Grey Hair//11205 96
            //Talent 1082
            {101206,new List<string>{ "+15% 攻击", "+15% ATK"}},//仗势/Tyranny of Numbers
            {101207,new List<string>{ "积攒效率为本体的20%","20% efficiency comparing to player"}},//玄同
            {101208,new List<string>{ "+15%/lv", "+15%/lv"}},//浇油/Insult to Injury//11208 97/98 1083buff基础15%,1/2级天赋+0/+15%
            {101401,new List<string>{ "-10秒", "-10s"}},//回阳/Glorious Return//buff 1111
            {101403,new List<string>{ "回复20%法力上限", "Recover 20% MaxMP"}},//不增不减/Spirited Return

            {101404,new List<string>{ "20%", "20%"}},//去来空/Cycle Breaker//101404 112
            //变化
            {301001,new List<string>{ "+3.3%/lv",}},//暗藏神妙
            {301002,new List<string>{ "-1.67%/lv,基础 1.25/s","-1.67%/lv,Base 1.25/s"}},//保养精神
            {301003,new List<string>{ "+2%/lv,基础 0.5/s","+2%/lv,Base 0.5/s"}},//存神炼气
            {301004,new List<string>{ "+2/lv",}},//虚相凶猛
            {301005,new List<string>{ "+15/lv",}},//炼实返虚
            {301006,new List<string>{ "+2%神力上限/lv,基础 40%","Gain +2% Max Transformation Energy/lv,Base 40%"}},//截伪续真
            
            {301101,new List<string>{ "Lv1-3:+10%/15%/20%攻击力", "Lv1-3:+10%/15%/20%ATK"}},//步月
            {301102,new List<string>{ "+15/lv",}},//磊磊
            //TalentSDesc 301103 PassiveSkillDesc 232  233 实际是-1.5 3.5
            {301103,new List<string>{ "-10%",}},//剪尾
            {301104,new List<string>{ "Lv1->6:+4/6/8/10/12/14",}},//爆躁
            {301105,new List<string>{ "Lv1->6:+4/6/8/10/12/14",}},//霏霏
            {301106,new List<string>{ "60秒内+1 攻击力/每层", "+1 per stack in 60s"}},//红眼/Red Eyes
            {301107,new List<string>{ "Lv1->6:+4/6/8/10/12/14",}},//恶秽
            {301109,new List<string>{ "Lv1->6:+4/6/8/10/12/14",}},//奔霄
            //TalentSDesc 301108 PassiveSkillDesc 236 BuffDesc-lz
            {301108,new List<string>{ "+0.5秒->0.8秒", "+0.5s->0.8s"}},//一闪/Lightning Flash
            {301110,new List<string>{ "+12",}},//不坏身

            //根器
   	        {200101,new List<string>{ "轻棍1-5段全中获得的棍势由15/18/17/28/40变为15/18/26/35/43", "Light Attack 1-5 focus point if all hit :15/18/17/28/40 -> 15/18/26/35/43"}},//见机强攻/Opportune Watcher
            //buff talent 1053
            {200102,new List<string>{ "-12.5秒", "-12.5s"}},//眼乖手疾/Eagle Eye
            {200103,new List<string>{ "+15%", "+15%"}},//慧眼圆睁/Keen Insight

            {200201,new List<string>{ "+0.066秒,基础0.4(劈棍)/0.366(戳棍)/0.3(不明)/0.5(不明)秒", "+0.066s,Base 0.4(Smash)/0.366(Pillar)/0.3(Unknown)/0.5(Unknown) second"}},//耳听八方/All Ears//20201 Passive 121 287,293,114,10110
            {200202,new List<string>{ "-0.1秒，基础1秒", "-0.1s,Base 1s"}},//如撞金钟/Sound as A Bell // Ｐａｓｓｉｖｅ 122-125 Buff-lz 228 buff-talent 1069
            {200203,new List<string>{ "5秒内+10%攻击", "+10% ATK in 5s"}},//耳畔风响/Whistling Wind

            {200301,new List<string>{ "15秒内+10%伤害加成", "+10% Damage in 15s"}},//气味相投/Lingering Aroma //buff 2107
            //{,new List<string>{ "+12", "+12"}},//阳燧珠/In One Breath -
            {200303,new List<string>{ "+0.066秒,0-3段翻滚基础无敌时间0.4/0.433/0.466/0.5秒", "+0.066s,0-3 level roll base time 0.4/0.433/0.466/0.5s"}},//屏气敛息/Hold Breath//-20303 Passive  127 buff-lz  10105-10109

            {200404,new List<string>{ "+10%丹药持续时间", "+10% Duration"}},//舌尝思/Envious Tongue
            {200401,new List<string>{ "5秒内+15%", "+15% in 5s"}},//丹满力足/Refreshing Taste
            //{,new List<string>{ "+12", "+12"}},//阳燧珠/Spread the Word
            {200403,new List<string>{ "每个增加+4%生命上限的回复量", "+4% MaxHP recover each"}},//遍尝百草/Tongue of A Connoisseur

            {200501,new List<string>{ "0-4级棍势气力消耗-20/25/37.5/60,令熟谙天赋效果减半", "0-4 Focus-Level heavy-attack cost :-20/25/37.5/60. Reduce Instinct(talent) effect by half"}},//身轻体快/Nimble Body
            {200502,new List<string>{ "+60", "+60"}},//福寿长臻/Everlasting Vitality
            {200503,new List<string>{ "+15", "+15"}},//灾愆不侵/Divine Safeguard

            {200601,new List<string>{ "+5%/10% 动作倍率", "+5%/10% SkillEffect"}},//万相归真/Elegance in Simplicity
            {200603,new List<string>{ "30秒内+30%伤害加成", "+30% Damage in 30s"}},//不生不灭/Unbegotten, Undying
        };
        //套装效果
        public static DescDict SuitInfoDesc = new DescDict
        {
            //SuitInfo和RedQualityInfo都是通过AttrEffectID或TalentID生效

            //Talent 2037->90301 ?,鳞棍2034->15003?
            {900311,new List<string>{ "x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"}},//浪里白条/Wave-Rider
            //没找到对应passive
            //{900321,new List<string>{ "x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"}},//浪里白条/Wave-Rider
            //650->702，+满级百足734.5
            //Talent 2041 - 2044 写的是0 / 8 / 10,实测是0 / 8 %/ 约10 %
            {900411,new List<string>{ "+8%/10%奔跑/冲刺速度", "+8%/10% Run/Sprint speed."}},//日行千里/Swift Pilgrim
            {900412,new List<string>{ "每层+10%攻击,持续2秒", "+10% ATK per stack in 2s"}},//日行千里/Swift Pilgrim
            {900421,new List<string>{ "每秒+12棍势", "+12 Focus/s"}},//日行千里/Swift Pilgrim

            {900511,new List<string>{ "每个天赋+24防御", "+24 DEF per Relic Talent"}},//心灵福至/Fortune's Favor
            //Talent 2063
            {900611,new List<string>{ "20%减伤", "20% Damage Reduction"}},//走石飞砂/Raging Sandstorm
            {900711,new List<string>{ "+20% 动作倍率", "+20% SkillEffect"}},//离火入魔/Outrage
            {705,new List<string>{ "+25%伤害 -30%伤害减免", "+25% Damage.-30% DamageReduction"}},//离火入魔/Outrage
            {900811,new List<string>{ "+10赋雷攻击", "+10 Thunder ATK"}},//龙血玄黄/Thunder Veins
            {900821,new List<string>{ "+10赋雷攻击", "+10 Thunder ATK"}},//龙血玄黄/Thunder Veins
            {901011,new List<string>{ "20秒内+15%攻击", "+15% ATK in 20s"}},//借假修真/Gilded Radiance
            {901012,new List<string>{ "暴击+3元气,\n击杀+5元气", "+3/+5 Qi when Crit/Kill"}},//借假修真/Gilded Radiance
            //96005 / 96006 实测每次减少0.75~1秒冷却不定？？非传奇和传奇没有区别？？
            {901211,new List<string>{ "+15棍势", "+15 Focus"}},//举步生风/Gale Guardian
            {901212,new List<string>{ "+-0.75~1秒冷却?", "-0.75s~1s CD？"}},//举步生风/Gale Guardian
            //TalentSDesc 91221 91222 91223,但是Passive里只有91221
            {901221,new List<string>{ "-0.1s无敌时间，不会额外减少冷却", "-0.1s Immune Duration.Won't reduce more CD"}},//举步生风/Gale Guardian
            
            //90711 Passive 167
            //Talent 2135 - 0.005，实测变身还原 + 1.5每秒 ；2137 - 0.00375, 实测约1.12每秒，结合 - 0.005推测应为1.125 / s
            {901311,new List<string>{ "+20%伤害减免，结束变身时获得12秒黑泥，化身还原后获得6秒黑泥", "+20% DamageReduction.Gain Mud in 12s upon quiting tranformation;Gain Mud in 6s upon quiting vigor."}},//泥塑金装/From Mud to Lotus
            {901312,new List<string>{ "翻滚回复约0.3神力,结束变身后12秒内+1.5/s神力回复，化身还原后4秒(not 6)内+1.125/s神力回复", "About +0.3 Might upon roll. +1.5/s Might Recover for 12s upon quiting tranformation.+1.125/s Might Recover in 4s(not 6s) upon quiting vigor."}},//泥塑金装/From Mud to Lotus
            {901411,new List<string>{ "x0.8毒伤(和抗性效果乘算)", "x0.8 Poison Damage(Multi with Poison Resist effect)"}},//花下死/Poison Ward
            {901412,new List<string>{ "+20%攻击", "+20% ATK"}},//花下死/Poison Ward
            //独角仙套 91912 Passive 185
            {901511,new List<string>{ "+10%灵蕴", "+10% Will"}},//锱铢必较/Every Bit Counts
            {901611,new List<string>{ "5秒内+10%防御", "+10% DEF in 5s"}},//百折不挠/Unyielding Resolve
            {901811,new List<string>{ "+50棍势", "+50 Focus"}},//铜心铁胆/Iron Will
            {901812,new List<string>{ "-5秒冷却", "-5s CD"}},//铜心铁胆/Iron Will
            {901911,new List<string>{ "+100棍势", "+100 focus"}},//毒魔狠怪/Fuban Strength
            {901912,new List<string>{ "+20%持续时间", "+20% Duration"}},//毒魔狠怪/Fuban Strength
            {902011,new List<string>{ "10秒内+8%暴击", "+8% Crit in 10s"}},//试比天高/Heaven's Equal
            {902012,new List<string>{ "-1秒冷却 per Hit", "-1s CD per Hit"}},//试比天高/Heaven's Equal


            {900921,new List<string>{ "+20法力消耗;假身持续时间不变,但不会因破隐而消失", "+20 MP Cost"}},//乘风乱舞/Dance of the Black Wind
            //Talent 2181 青铜套内部叫黑铁
            {901712,new List<string>{ "-15秒冷却", "-15s CD"}},//炼魔荡怪/Evil Crasher
        };
        //精魂(RZD)被动 VIPassiveDesc.OnChangeItemId
        public static DescDict SpiritDesc = new DescDict
        {
            {8011,new List<string>{ "根据等级[+6]/[+8]/[+10]", "[+6]/[+8]/[+10] by Lv"}}, //"广谋"
            {8012,new List<string>{ "根据等级[-6%]/[8%]/[10%]", "[-6%]/[8%]/[10%] by Lv"}},
            {8013,new List<string>{ "根据等级[+20]/[24]/[30]", "[+20]/[24]/[30] by Lv"}}, //"幽魂"
            {8014,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"鼠司空"
            {8015,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"百目真人"
            {8017,new List<string>{ "根据等级[+6%]/[8%]/[10%]", "[+6%]/[8%]/[10%] by Lv"}}, //"虎伥"
            {8020,new List<string>{ "根据等级[+20]/[24]/[30] per Hit", "[+20]/[24]/[30] per Hit by Lv"}}, //"不空"
            //8022 ["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
            {8024,new List<string>{ "根据等级10秒内[+6%]/[+8%]/[+10%]减伤", "[+6%]/[+8%]/[+10%] DamageReduction in 10s by Lv"}}, //"不净"
            {8025,new List<string>{ "根据等级[+6]/[+8]/[+10]", "[+6]/[+8]/[+10] by Lv"}}, //"不白"
            {8026,new List<string>{ "根据等级[+2%]/[2.5%]/[3%]暴击,[+4%]/[5%]/[6%]爆伤,[+5]/[7]/[10]攻击力,[-50]/[75]/[100]法力上限", "[+2%]/[2.5%]/[3%] CritRate,[+4%]/[5%]/[6%] CritDamage,[+5]/[7]/[10] ATK,[-50]/[75]/[100] MaxMP by Lv"}}, //"不能"
            {8027,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"虫总兵"
            {8028,new List<string>{ "根据等级[-10]/[15]/[20]消耗", "[-10]/[15]/[20] MP Cost by Lv"}}, //"儡蜱士"
            {8029,new List<string>{ "根据等级每6秒回复[1.5%]/[1.75%]/[2%]生命上限", "[1.5%]/[1.75%]/[2%] MaxHP per 6 seconds by Lv"}}, //"青冉冉"
            {8030,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"蝎太子"
            //取平地普通移动再停止后最后一个数字，该数字是稳定的,观察PlayerLocomotion.MaxSpeed:[650]/[663]/[669]/682.5
            {8031,new List<string>{ "根据等级[+2%]/[3%]/[5%],基础跑步速度650", "[+2%]/[3%]/[5%] by Lv. Base Run Speed 650"}}, //"百足虫"
            {8032,new List<string>{ "根据等级[+30]/[36]/[45]防御,[-5]/[7]/[10]攻击", "[+30]/[36]/[45] DEF,[-5]/[7]/[10] ATK by Lv"}}, //"兴烘掀·掀烘兴"
            {8033,new List<string>{ "根据等级[+1.5%]/[2%]/[2.5%]暴击,[+3%]/[4%]/[5%]暴伤", "[+1.5%]/[2%]/[2.5%] CritChance,[+3%]/[4%]/[5%] CritDamage by Lv"}}, //"石父"
            {8034,new List<string>{ "根据等级10秒内[+6%]/[8%]/[10%]", "[+6%]/[8%]/[10%] in 10s by Lv"}}, //"地罗刹"
            {8035,new List<string>{ "根据等级[+10%]/[12%]/[15%] 动作倍率", "[+10%]/[12%]/[15%] SkillEffect by Lv"}}, //"鳖宝"--303184/33484 Passive 292/272
            {8036,new List<string>{ "根据等级[+5]/[7]/[10]", "[+5]/[7]/[10] by Lv"}}, //"燧统领"
            {8037,new List<string>{ "根据等级[+6]/[+8]/[+10]", "[+6]/[+8]/[+10] by Lv"}}, //"燧先锋"
            {8038,new List<string>{ "根据等级[+4%]/[+5%]/[+6%]", "[+4%]/[+5%]/[+6%] by Lv"}}, //"琴螂仙"
            {8039,new List<string>{ "根据等级[-6%]/[8%]/[10%]", "[-6%]/[-8%]/[-10%] by Lv"}}, //"火灵元母"
            {8040,new List<string>{ "根据等级[+6%]/[8%]/[10%]", "[+6%]/[8%]/[10%] by Lv"}}, //"老人参精" --303157 [33657]/[33457]/[33157] 244/264/
            {8041,new List<string>{ "根据等级[+20]/[24]/[30]生命,[-10]/[12]/[15]气力", "[+20]/[24]/[30] MaxHP，[-10]/[12]/[15] MaxStamina by Lv"}}, //"蘑女"
            {8042,new List<string>{ "根据等级[6%]/[8%]/[10%]", "[6%]/[8%]/[10%] by Lv"}}, //"菇男"
            
            {8061,new List<string>{ "根据等级[+3%]/[4%]/[5%]", "[+3%]/[4%]/[5%] by Lv"}}, //"狼刺客"
            {8062,new List<string>{ "根据等级[+7]/[10]/[15]攻击,[-75]/[100]/[150]生命上限", "[+7]/[10]/[15]ATK,[-75]/[100]/[150]MaxHP By Lv"}}, //"疯虎"
            {8063,new List<string>{ "生命值低于25%时根据等级[+10]/[15]/[20]攻击", "[+10]/[15]/[20] ATK under 25% MaxHP by Lv"}}, //"沙二郎"
            {8064,new List<string>{ "根据等级[+6]/[8]/[10]", "[+6]/[8]/[10] by Lv"}}, //"鼠禁卫"
            {8065,new List<string>{ "根据等级[+1%]/[2%]/[3%]", "[+1%]/[2%]/[3%] by Lv"}}, //"骨悚然"
            {8066,new List<string>{ "根据等级[+5]/[7]/[10]", "[+5]/[7]/[10] by Lv"}}, //"狸侍长"
            {8067,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"疾蝠"
            {8068,new List<string>{ "根据等级[-6%]/[8%]/[10%]气力消耗,[+10%]/[12%]/[15%] 跳跃轻击动作倍率", "[-6%]/[8%]/[10%] Stamina Cost,[+10%]/[12%]/[15%] SkillEffect by Lv"}}, //"石双双" --33681/33581 [247]/[267]/[287]
            //--26/[24.5]/[24]/[23.5]
            {8069,new List<string>{ "根据等级减少每秒消耗[-1.5]/[2]/[2.5]，基础消耗26/s(移步48/s)", "[-1.5]/[2]/[2.5] per second cost by Lv. Base Cost 26/s(48/s when moving)"}}, //"鼠弩手"
            {8070,new List<string>{ "根据等级[+2]/[2.4]/[3] per Hit", "[+2]/[2.4]/[3] per Hit by Lv"}}, //"地狼"
            {8071,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"隼居士"
            {8072,new List<string>{ "根据等级[+10%]/[12%]/[15%] 动作倍率", "[+10%]/[12%]/[15%] SkillEffect"}}, //"赤发鬼"[--33687]/[33487]/[33187] [295]/[275]/[255]
            {8073,new List<string>{ "根据等级10秒内[+10]/[15]/[20]攻击", "[+10]/[15]/[20] ATK in 10s by Lv"}}, //"戒刀僧"
            {8074,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"泥塑金刚"
            {8075,new List<string>{ "根据等级[+10]/[12]/[15] per Hit", "[+10]/[12]/[15] per Hit by Lv"}}, //"夜叉奴"
            {8077,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"鸦香客"
            {8078,new List<string>{ "根据等级[+10]/[12]/[15]", "[+10]/[12]/[15] by Lv"}}, //"虫校尉"
            {8079,new List<string>{ "根据等级时[+3%]/[4%]/[5]元气获得,[-40]/[50]/[60]生命,[-20]/[25]/[30]法力,[-20]/[25]/[30]气力", "[+3%]/[4%]/[5%] Qi Gain,[-40]/[50]/[60] MaxHP,[-20]/[25]/[30] MaxMP,[-20]/[25]/[30] MaxSt by Lv"}}, //"蜻蜓精"
            {8081,new List<string>{ "根据等级[-6%]/[8%]/[10%]神力消耗速度", "[-6%]/[8%]/[10%] Energy Cost Speed by Lv"}}, //"傀蛛士"
            {8083,new List<string>{ "根据等级[+5%]/[10%]/[25%]攻击，[-20%]/[40%]/[100%]防御", "[+5%]/[10%]/[25%] ATK,[-20%]/[40%]/[100%] DEF at Lv1"}}, //"蛇捕头"
            {8084,new List<string>{ "根据等级回复[6%]/[8%]/[10%]生命上限", "[6%]/[8%]/[10%] MaxHP by Lv"}}, //"蛇司药"
            {8085,new List<string>{ "根据等级[+6%]/[8%]/[10%]", "[+6%]/[8%]/[10%] by Lv"}}, //"幽灯鬼"
            {8086,new List<string>{ "根据等级[+6%]/[8%]/[10%]", "[+6%]/[8%]/[10%] by Lv"}}, //"黑脸鬼"
            {8087,new List<string>{ "根据等级[+10]/[12]/[15] ", "[+10]/[12]/[15] by Lv"}}, //"牯都督"
            {8088,new List<string>{ "根据等级[-6]/[-8]/[-10]", "[-6]/[8]/[10] by Lv"}}, //"雾里云·云里雾"
            {8092,new List<string>{ "根据等级[-6]/[8]/[10]点消耗", "[-6]/[8]/[10] by Lv"}}, //"九叶灵芝精"
            //8076 ["巡山鬼"]=nil,    ["Mountain Patroller"]=nil,--303686 [254]/[274]/[294] [1061410]/[1061430]/[1061460]?                     
        };
        public static DescDict FabaoAttrDesc = new DescDict
        {
            //法宝被动(携带效果,EqDesc AttrDesc) VITreasureDesc::OnChangeItemId
            {19001,new List<string>{ "+30"}},
            {19002,new List<string>{ "+2%"} },
            {19004,new List<string>{ "+2%暴击+6%暴伤", "+2%Crit +6% CritDamage"}},
            {19005,new List<string>{ "+10"}},
        };
        //精魂消耗,PreFillDict时填充
        public static DescDict SpiritCost = new DescDict
        {
        };

    }
}
