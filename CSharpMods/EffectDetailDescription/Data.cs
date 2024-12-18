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
using EffectDetailDescription;

namespace EffectDetailDescription;
public static class Data
{
    public enum LanguageIndex : int
    {
        SimpleCN = 0,
        English = 1,
        Max = 2,
    }
    //2.4 4.8. 7.5 9.6 13.2
    public static string PlaceHolder = "__PlaceHolder__";

    public static Desc LightAttackPrefix = ["轻击", "Light Attack"];
    public static Desc HeavyAttackPrefix = ["重击", "Heavy Attack"];
    public static Desc SpellAttackPrefix = ["法术", "Spell"];
    public static Desc EnterAttackPrefix = ["进入变身", "Enter"];
    public static Desc ExitAttackPrefix = ["退出变身", "Exit"];

    //枪只有四段
    public static Desc LightAttackDesc =
    [
        "轻棍1~5段动作值: {AR:1080101}/{AR:1080201}/{AR:1080301}+{AR:1080302}/{AR:1080401}*4+{AR:1080402}/{AR:1080501}\n轻棍1~5段(枪):{AR:1070101}/{AR:1070201}+{AR:1070202}/{AR:1070301}*5/{AR:1070401}\n跳跃轻棍:{AR:1060101}",
        "Light Attack MotionValue:  {AR:1080101}/{AR:1080201}/{AR:1080301}+{AR:1080302}/{AR:1080401}*4+{AR:1080402}/{AR:1080501}\nLight Attack(Spear):{AR:1070101}/{AR:1070201}+{AR:1070202}/{AR:1070301}*5/{AR:1070401}\nLight Attack(Jump):{AR:1060101}"
    ];
    public static Desc HeavyAttackDesc =
    [
        "Lv0~4劈棍重击动作值:{AR:1021001}/{AR:1021101}/{AR:1021201}/{AR:1021301}/{AR:1021401};\n" +
        "立棍:{AR:1086001}/{AR:1086101}/{AR:1086201}/{AR:1086301}/{AR:1086401};" +
        "\n戳棍:{AR:1088001}/{AR:1088101}/{AR:1088201}/{AR:1088301}/{AR:1088401};\n" +
        "跳跃重击:{AR:1061001}/{AR:1061101}/{AR:1061201}/{AR:1061301}/{AR:1061401};\n" +
        "Lv5重击:x2*2+12\n",

        "Lv0~4 Smash heavy attack Motion Value:{AR:1021001}/{AR:1021101}/{AR:1021201}/{AR:1021301}/{AR:1021401};\n" +
        "Pillar:{AR:1086001}/{AR:1086101}/{AR:1086201}/{AR:1086301}/{AR:1086401};" +
        "\nThrust:{AR:1088001}/{AR:1088101}/{AR:1088201}/{AR:1088301}/{AR:1088401};\n" +
        "Jump:{AR:1061001}/{AR:1061101}/{AR:1061201}/{AR:1061301}/{AR:1061401};\n" +
        "Lv5:2*2+12\n"
    ];
    //ItemId->effectdesc
    public static DescDict ItemEqDesc = new DescDict
    {
        //法宝主动 VITreasureDetail::OnEquipIdChange
        {19001, ["40秒内+600火抗，每秒获得5棍势", "+600 FireResist,+5 Focus/s for 40s"] },
        {19002, ["20秒内+50%伤害减免", "+50% DamageReduction for 20s"] },
    };
    //精魂id->(skill id->hit数的map)
    //-1=*N
    public static Dictionary<int, List<KeyValuePair<int, int>>> SpiritActiveSkillId = new Dictionary<int, List<KeyValuePair<int, int>>>
    {
        {8011, [new KeyValuePair<int, int>(10380101, -1)] }, //"广谋"
        {8013, [new KeyValuePair<int, int>(1011361, 1)] }, //"幽魂"
        {8014, [new KeyValuePair<int, int>(1011401, 7)] }, //"鼠司空"
        {8015, [new KeyValuePair<int, int>(1011501, 1)] }, //"百目真人"
        {8017, [new KeyValuePair<int, int>(1011761, 1)] }, //"虎伥"
        {8022, [new KeyValuePair<int, int>(1012261, 2)] }, //"无量蝠"
        {8024, [new KeyValuePair<int, int>(1012401, 4), new KeyValuePair<int, int>(1012402, 1)] }, //"不净"
        {8025, [new KeyValuePair<int, int>(1012501, 1), new KeyValuePair<int, int>(1012502, -1)] }, //"不白
        //{8026,new List<KeyValuePair<int, int>>{ new KeyValuePair<int,int>( 1012601, 1 ),new KeyValuePair<int,int>(1012602,2 ),{ } }}, //"不能"
        {8027, [new KeyValuePair<int, int>(1012701, 1)] }, //"虫总兵"
        {8028, [new KeyValuePair<int, int>(43010501, -1), new KeyValuePair<int, int>(43010601, -1)] }, //"儡蜱士",小蜘蛛平A和自爆伤害，平A有43010301/43010401/43010501三种
        //{8029,new List<KeyValuePair<int, int>>{ new KeyValuePair<int,int>( , -1 ) }}, //"青冉冉"
        {8030, [new KeyValuePair<int, int>(1013001, 1), new KeyValuePair<int, int>(1013002, 1)] }, //"蝎太子"
        {8031, [new KeyValuePair<int, int>(1013101, -1)] }, //"百足虫"
        {8032, [new KeyValuePair<int, int>(1013201, 2), new KeyValuePair<int, int>(1013261, 1)] }, //"兴烘掀·掀烘兴"
        {8033, [new KeyValuePair<int, int>(1013301, 1), new KeyValuePair<int, int>(1013302, 1)] }, //"石父"
        {8034, [new KeyValuePair<int, int>(1013401, 1)] }, //"地罗刹"
        {8035, [new KeyValuePair<int, int>(1013503, 1), new KeyValuePair<int, int>(1013501, 1)] }, //"鳖宝",1013503在前
        //{8036,new List<KeyValuePair<int, int>>{ new KeyValuePair<int,int>( , -1 ) }}, //"燧统领"
        {8037,
            [
                new KeyValuePair<int, int>(1013701, 4), new KeyValuePair<int, int>(1013702, 1),
                new KeyValuePair<int, int>(1013703, 1)
            ]
        }, //"燧先锋"
        {8038, [new KeyValuePair<int, int>(1013803, -1)] }, //"琴螂仙"
        {8039, [new KeyValuePair<int, int>(40220101, -1)] }, //"火灵元母"
        {8040,
            [
                new KeyValuePair<int, int>(50060401, 1), new KeyValuePair<int, int>(50060402, 1),
                new KeyValuePair<int, int>(50060403, 1), new KeyValuePair<int, int>(50060404, 1)
            ]
        }, //"老人参精"
        {8041, [new KeyValuePair<int, int>(1014104, 1)] }, //"蘑女",还有若干无伤害的孢子
        {8042, [new KeyValuePair<int, int>(1014201, -1)] }, //"菇男"
        
        {8061, [new KeyValuePair<int, int>(1016101, -1)] }, //"狼刺客"
        {8062, [new KeyValuePair<int, int>(1016201, 1), new KeyValuePair<int, int>(1016263, 1)] }, //"疯虎"
        {8063, [new KeyValuePair<int, int>(1016301, 1),new KeyValuePair<int, int>(1016302, 2)] }, //"沙二郎"
        {8064, [new KeyValuePair<int, int>(1016401, 1), new KeyValuePair<int, int>(1016402, 1)] }, //"鼠禁卫"
        {8065, [
                new KeyValuePair<int, int>(1016502, 1), new KeyValuePair<int, int>(1016501, 7),
                new KeyValuePair<int, int>(1016502, 1)
            ]
        }, //"骨悚然"，起手吼和收尾砸都是1016502
        {8066, [new KeyValuePair<int, int>(1016601, 2)] }, //"狸侍长"
        {8067, [new KeyValuePair<int, int>(1016761, 2)] }, //"疾蝠"
        {8068, [new KeyValuePair<int, int>(1016801, 1), new KeyValuePair<int, int>(1016802, 1)] }, //"石双双" 
        //
        {8069, [new KeyValuePair<int, int>(1016901, 11)] }, //"鼠弩手"
        {8070, [new KeyValuePair<int, int>(1017001, 3), new KeyValuePair<int, int>(1017002, 1)] }, //"地狼"
        //{8071,new List<KeyValuePair<int, int>>{ new KeyValuePair<int,int>( , -1 ) }}, //"隼居士",有ar但没伤害
        {8072, [new KeyValuePair<int, int>(1017261, 1)] }, //"赤发鬼"
        {8073, [new KeyValuePair<int, int>(1017301, 2), new KeyValuePair<int, int>(1017302, 1)] }, //"戒刀僧"
        {8074, [new KeyValuePair<int, int>(1017402, 1)] }, //"泥塑金刚"new KeyValuePair<int,int>( 1017401, 4 )没有伤害
        {8075, [new KeyValuePair<int, int>(1017501, 1)] }, //"夜叉奴"
        {8076, [new KeyValuePair<int, int>(1017601, 1), new KeyValuePair<int, int>(1017602, 2)] }, //"巡山鬼"
        //{8077,new List<KeyValuePair<int, int>>{ new KeyValuePair<int,int>( , -1 ) }}, //"鸦香客"
        {8078, [new KeyValuePair<int, int>(1017801, 2)] }, //"虫校尉"
        {8079, [new KeyValuePair<int, int>(1017961, 1)] }, //"蜻蜓精"
        {8081, [new KeyValuePair<int, int>(1018101, 4)] },//"傀蜱士"
        {8083, [new KeyValuePair<int, int>(1018301, 1), new KeyValuePair<int, int>(1018302, 1)] }, //"蛇捕头"
        {8084, [new KeyValuePair<int, int>(1018401, -1)] }, //"蛇司药"
        {8085, [new KeyValuePair<int, int>(1018502, 6)] }, //"幽灯鬼"
        {8086, [new KeyValuePair<int, int>(1018602, 1)] }, //"黑脸鬼"
        {8087, [new KeyValuePair<int, int>(1018701, 3), new KeyValuePair<int, int>(1018702, 1)] }, //"牯都督"
        {8088, [new KeyValuePair<int, int>(1018802, 2), new KeyValuePair<int, int>(1018803, 3)] }, //"雾里云·云里雾"
        {8092, [new KeyValuePair<int, int>(1019201, -1)] }, //"九叶灵芝精"
    };
    public static ConstDescDict EAbnormalStateTypeDesc = new ConstDescDict
            {
                { (int)EAbnormalStateType.Abnormal_Burn, ["{0}%为火属性", "{0}% is Fire Damage"] },
                { (int)EAbnormalStateType.Abnormal_Poison, ["{0}%为毒属性", "{0}% is Poison Damage"] },
                { (int)EAbnormalStateType.Abnormal_Freeze, ["{0}%为冰属性", "{0}% is Ice Damage"] },
                { (int)EAbnormalStateType.Abnormal_Thunder, ["{0}%为雷属性", "{0}% is Thunder Damage"] },
                { (int)EAbnormalStateType.Abnormal_Yang, ["{0}%为阳属性", "{0}% is Yang Damage"] },
                { (int)EAbnormalStateType.Abnormal_Yin, ["{0}%为阴属性", "{0}% is Yin Damage"] },
            };
    public static ConstDescDict EAbnormalStateTypeDescShort = new ConstDescDict
    {
        { (int)EAbnormalStateType.Abnormal_Burn, ["{0}%火", "{0}% Fire"] },
        { (int)EAbnormalStateType.Abnormal_Poison, ["{0}%毒", "{0}% Poison"] },
        { (int)EAbnormalStateType.Abnormal_Freeze, ["{0}%冰", "{0}% Ice"] },
        { (int)EAbnormalStateType.Abnormal_Thunder, ["{0}%雷", "{0}% Thunder"] },
        { (int)EAbnormalStateType.Abnormal_Yang, ["{0}%阳", "{0}% Yang"] },
        { (int)EAbnormalStateType.Abnormal_Yin, ["{0}%阴", "{0}% Yin"] },
    };

    public static DescDict ItemEffectDesc = new DescDict
    {
        {-2, ["满级时,{0}", "LvMax:{0}"] },
        {-1, ["变身期间根据等级{0};", "{0} by Lv when active."] },
        //消耗品,注意1118等是配方，2118等是丹药
        //鼻根器+10%持续2
        {2215, ["300秒","300s"] },//九转还魂丹
        {2218, [] },//朝元膏/Essence Decoction
        {2221, [] },//益气膏/Tonifying Decoction
        {2224, [] },//倍力丸/Amplification Pellets
        {2227, [] },//伏虎丸/Tiger Subduing Pellets
        {2230, [] },//延寿膏/Longevity Decoction
        {2234, ["75秒", "75s"] },//坚骨药/Fortifying Medicament
        {2236, ["回复40%", "Recover 40%"] },//镜中丹/Mirage Pill
        {2245, [] },//龙光倍力丸/Loong Aura Amplification Pellets
        {2247, [] },//避凶药/Evil Repelling Medicament
        
        {2251, [] },//加味参势丸/Enhanced Ginseng Pellets
        {2252, [] },//参势丸/Ginseng Pellets
        {2253, [] },//聚珍伏虎丸/Enhanced Tiger Subduing Pellets
        {2254, ["75秒内+10%攻击","+10% ATK in 75s"] },//宝鳞丸
        
        {2204, [] },//温里散/Body-Warming Powder
        {2205, [] },//度瘴散/Antimiasma Powder
        {2206, [] },//神霄散/Shock-Quelling Powder
        {2207, ["120秒", "120s.Only affects sprinting/jumping/rolling."] },//轻身散/Body-Fleeting Powder
        {2208, [] },//清凉散/Body-Cooling Powder
        {2213, ["75秒", "75s"] },//登仙散/Ascension Powder
        //{,new Desc{ "120秒", "130s"}},//九转还魂丹/Tonifying Decoction


        {18007, ["15秒内+15抗性", "+15 for 15s"] },//湘妃葫芦/Xiang River Goddess Gourd
        {18011, ["每30秒回满葫芦", "Refill each 30s"] },//青田葫芦
        {18012, ["20秒内+15攻击", "+15 ATK for 20s"] },//五鬼葫芦/Plaguebane Gourd
        {18014, ["20秒内+20攻击", "+20 ATK for 20s"] },//妙仙葫芦/Immortal Blessing Gourd
        //2804 DmgAdditionBase 30
        {18015, ["30秒内+30%伤害加成", "+30% Damage for 30s"] },//乾坤彩葫芦/Multi-Glazed Gourd
        {18017, ["15秒内+30气力", "+30 Stamina for 15s"] },//燃葫芦/Firey Gourd

        {2001, ["实际回复28%+15", "Actually 28%+15"] },//椰子酒/Coconut Wine
        {2002, ["实际回复32%+25", "Actually 32%+25"] },//椰子酒·三年陈/3-Year-Old Coconut Wine
        {2003, ["实际回复36%+33", "Actually 36%+33"] },//椰子酒·五年陈/5-Year-Old Coconut Wine
        {2004, ["实际回复39%+40", "Actually 39%+40"] },//椰子酒·十年陈/10-Year-Old Coconut Wine
        {2005, ["实际回复42%+46", "Actually 42%+46"] },//椰子酒·十八年陈/18-Year-Old Coconut Wine
        {2006, ["实际回复45%+51", "Actually 45%+51"] },//椰子酒·三十年陈/30-Year-Old Coconut Wine
        {2007, ["实际回复48%+55", "Actually 48%+55"] },//猴儿酿/Monkey Brew
        {2021, ["实际回复也是55%", "Indeed 55%"] },//玉液/Jade Dew

        {2009, ["15秒内+30%移动速度", "+30% for 15s"] },//蓝桥风月/Bluebridge Romance--Buff-Talent 2816 写的是15%/15%/15% 实测 约+30%/+30%/约+30% ,why???
        {2011, ["低于20%血量时恢复量24%->60%", "24%->60% under 20% HP"] },//无忧醑/Worryfree Brew
        {2012, ["6秒内+12%攻击", "+12 ATK for 6s"] },//龙膏/Loong Balm
        {2019, ["+20", "+20"] },//琼浆/Jade Essence
        //BuffDesc-Item 92023
        {2022, ["+75.0", "+75.0"] },//松醪/Pinebrew
        {2023, ["+15.0元气;+20法宝能量", "+15.0 Vigor-Qi;+20.0 Vessel-Qi"] },//九霞清醑/Sunset of the Nine Skies
        {2024, ["15秒内+35%耐力消耗,+6%重击动作值,+13%切手技动作值", "+35% Stamina Cost,+6% Heavy-Attack MotionValue,+13% Varied Combo MotionValue in 15s"] },//苦酒 92027 92027+92028

        {2303, [] },//龟泪/Turtle Tear
        {2304, ["10秒内+2秒持续", "+2s Duration for 10s"] },//困龙须/Stranded Loong's Whisker--92304 Passive 192
        {2305, ["10秒内+25%持续时间", "25% Duration for 10s"] },//灵台药苗/Moutain Lingtai Seedlings --92305 Passive 193
        //Talent 2100 AtkMul 20% 时间10秒，是10秒内施放隐身还是10秒内打出破隐?
        {2306, ["10秒内？+20%攻击", "+20% ATK for 10s?"] },//十二重楼胶/Breath of Fire
        {2307, ["5秒内共回复6%生命上限(1%*6)", "heal total 6% MaxHP(1%*6) in next 5s"] },//瑶池莲子/Celestial Lotus Seeds
        {2308, [] },//不老藤/Undying Vine
        {2309, [] },//虎舍利/Tiger Relic--92306
        {2310, [] },//梭罗琼芽/Laurel Buds
        {2311, [] },//甜雪/Sweet Ice
        {2312, [] },//霹雳角/Thunderbolt Horn
        {2314, ["低于20%血量时额外回复+12%生命上限", "+20% MaxHP when under 20% HP"] },//紫纹缃核/Purple-Veined Peach Pit
        {2315, ["15%？", "15%？"] },//蜂山石髓/Bee Mountain Stone
        {2316, [] },//铁弹/Iron Pellet
        {2317, ["15秒内踉跄并增加50%气力消耗;定身法-2秒，安神法-5秒，聚形散气-3秒，铜头铁臂-2秒，毫毛-8秒", 
                "Stagger and +50% Stamina Consume for 15s;Immobilize -2s,Ring of Fire -5s,Cloud Step -3s,Rock Solid-2s,A Pluck of Many -8s"] },//瞌睡虫蜕/Slumbering Beetle Husk
        {2318, ["10秒", "10s"] },//铜丸/Copper Pill
        {2319, ["15秒内+5秒持续", "+5s Duration for 15s"] },//血杞子/Goji Shoots --92319? Passive 198
        {2320, ["15秒内+0.066秒无敌，0-3段翻滚基础时间0.4/0.433/0.466/0.5秒", "+0.066s immue duration for 15s; 0-3 level roll base immue duration:0.4/0.433/0.466/0.5"] },//清虚道果/Fruit of Dao--92320 Passive 199 10105-10109
        {2321, [] },//火焰丹头/Flame Mediator
        {2322, [] },//双冠血/Double-Combed Rooster Blood
        {2323, [] },//胆中珠/Gall Gem
        {2324, ["11秒内共回复12%生命上限(1%*12)", "total 12% MaxHP(1%*12) in next 11s"] },//蕙性兰/Graceful Orchid，11秒持续间隔1秒
        {2325, [] },//嫩玉藕/Tender Jade Lotus
        {2326, [] },//铁骨银参/Steel Ginseng
        //Item 92317 92327 92328
        {2327, [] },//青山骨/Goat Skull
        {2328, ["法力低于生命上限(不是法力上限)的一半时,+10法宝能量,+8精魄能量","+10 Vessel-Qi and +8 Vigor-Qi when MP is lower than 50% MaxHP(not MaxMP)"] },//九秋菊
        {2329, [] },//肉角

        //精魂主动技能倍率
        {8011, ["", ""] }, //"广谋"
        {8012,["造成{AR:1026212}{AREle:1026212}*N/{AR:1026213}{AREle:1026213}*N/{AR:1026214}{AREle:1026214}*N/{AR:1026215}{AREle:1026215}*N/{AR:1026216}{AREle:1026216}*N伤害",
            "Deal {AR:1026212}{AREle:1026212}*N/{AR:1026213}{AREle:1026213}*N/{AR:1026214}{AREle:1026214}*N/{AR:1026215}{AREle:1026215}*N/{AR:1026216}{AREle:1026216}*N Damage"]},//波波浪浪，有多种
        {8013, ["", ""] }, //"幽魂"
        {8014, ["", ""] }, //"鼠司空"
        {8015, ["满级时，60秒内+35%攻击;", "LvMax:+35%ATK for 60s;"] }, //"百目真人"
        {8017, ["", ""] }, //"虎伥"
        {8022, ["", ""] }, //无量蝠
        {8024, ["", ""] }, //"不净"
        {8025, ["", ""] }, //"不白"
        {8026,
            [
                "轻棍0~4段派生分别造成x0.916*1/x0.916*2/x0.916*2/x0.916*2+x1.1/x0.916+x2.8416伤害;",
                "x0.916*1/x0.916*2/x0.916*2/x0.916*2+x1.1/x0.916+x2.8416 After Light Attack Lv0~4;"
            ]
        }, //"不能",根据轻棍段数不同
        {8027, ["", ""] }, //"虫总兵"
        {8028, ["", ""] }, //"儡蜱士"
        //{8029,new Desc{ "", ""}}, //"青冉冉"
        {8030, ["", ""] }, //"蝎太子"
        {8031, ["", ""] }, //"百足虫"
        {8032, ["", ""] }, //"兴烘掀·掀烘兴"
        {8033, ["", ""] }, //"石父"
        {8034, ["", ""] }, //"地罗刹"
        {8035, ["", ""] }, //"鳖宝"
        //{8036,new Desc{ "",""}}, //"燧统领"，无攻击
        {8037, ["", ""] }, //"燧先锋"
        {8038, ["", ""] }, //"琴螂仙"
        {8039, ["", ""] }, //"火灵元母"
        {8040, ["", ""] }, //"老人参精"
        {8041, ["", ""] }, //"蘑女"
        {8042, ["", ""] }, //"菇男"
        
        {8061, ["", ""] }, //"狼刺客"
        {8062, ["", ""] }, //"疯虎"
        {8063, ["", ""] }, //"沙二郎"
        {8064, ["", ""] }, //"鼠禁卫"
        {8065, ["", ""] }, //"骨悚然"
        {8066, ["", ""] }, //"狸侍长"
        {8067, ["", ""] }, //"疾蝠"
        {8068, ["", ""] }, //"石双双" 
        {8069, ["", ""] }, //"鼠弩手"
        {8070, ["", ""] }, //"地狼"
        //{8071,new Desc{ "", ""}}, //"隼居士"
        {8072, ["", ""] }, //"赤发鬼"
        {8073, ["", ""] }, //"戒刀僧"
        {8074, ["", ""] }, //"泥塑金刚"
        {8075, ["", ""] }, //"夜叉奴"
        {8076, ["", ""] }, //"巡山鬼"
        //{8077,new Desc{ "", ""}}, //"鸦香客"
        {8078, ["", ""] }, //"虫校尉"
        {8079, ["", ""] }, //"蜻蜓精"
        {8081, ["", ""] }, //"傀蛛士"
        {8083, ["", ""] }, //"蛇捕头"
        {8084, ["", ""] }, //"蛇司药"
        {8085, ["", ""] }, //"幽灯鬼"
        {8086, ["", ""] }, //"黑脸鬼"
        {8087, ["", ""] }, //"牯都督"
        {8088, ["", ""] }, //"雾里云·云里雾"
        {8092, ["", ""] }, //"九叶灵芝精"
        
    };
    public static Dictionary<int,List<ComboDesc>> TransformationItemBriefComboDesc=new Dictionary<int, List<ComboDesc>>
    {
        {5001,[new ComboDesc{
                Name=LightAttackPrefix,
                SkillEffectIdList = [
                    [[1201101]],
                    [[1201201]],
                    [[1201301]],
                    [[1201401],[1201411]],
                ] },
            new ComboDesc
            {
                Name = HeavyAttackPrefix,
                SkillEffectIdList = [
                    [[1204201]]
                ] },
            new ComboDesc{
                Name= HeavyAttackPrefix,
                Prefix = ["翻滚","Roll "],
                SkillEffectIdList=[[[1204301]]
                ]},
            new ComboDesc{
                Name= HeavyAttackPrefix,
                Prefix=["侧滚","Side-Roll "],
                SkillEffectIdList=[[[1204501,-1]]
                ]},
            new ComboDesc{
                Name= HeavyAttackPrefix,
                Prefix=["后滚","Back-Roll "],
                SkillEffectIdList=[[[1204401]]
                ]},
        ]},
        {5004,[new ComboDesc{
                Name=LightAttackPrefix,
                SkillEffectIdList = [
                    [[1301101]],
                    [[1301201]],
                    [[1301301]]
                ] },
            new ComboDesc
            {
                Name = HeavyAttackPrefix,
                SkillEffectIdList = [
                    [[1303205,-1],[1303201]]
                ] },
            new ComboDesc{
                Name= SpellAttackPrefix,
                SkillEffectIdList=[[[1307101]]
                ]},
            new ComboDesc{
                Name= ExitAttackPrefix,
                SkillEffectIdList=[[[1300904]]
                ]}
        ]},
        {5014,[
            new ComboDesc{
                Name= EnterAttackPrefix,
                SkillEffectIdList=[[[1401101]]
                ]},
            new ComboDesc{
                Name=LightAttackPrefix,
                SkillEffectIdList = [
                    [[1402101]],
                    [[1402201]],
                    [[1402301]],
                    [[1402101],[1402401]],
                    [[1402501]],
                ] },
            new ComboDesc
            {
                Name = HeavyAttackPrefix,
                SkillEffectIdList = [
                    [[1403101]],
                    [[1403101],[1403201]],
                    [[1403301]],
                    [[1403401]]
                ] },
            new ComboDesc{
                Name= SpellAttackPrefix,
                SkillEffectIdList=[[[1407101,4]]
                ]},
            new ComboDesc{
                Name= LightAttackPrefix,
                Prefix=["强化","Enchanced "],
                SkillEffectIdList=[[[1412101],[1412201],[1412301,2],[1412401,2]]
                ]},
        ]},
        {5006,[new ComboDesc{
                Name=LightAttackPrefix,
                SkillEffectIdList = [
                    [[1501101]],
                    [[1501201]],
                    [[1501201],[1501301]]
                ] },
            new ComboDesc
            {
                Name = HeavyAttackPrefix,
                SkillEffectIdList = [
                    [[1503131,-1]]
                ] },
            new ComboDesc{
                Name= ExitAttackPrefix,
                SkillEffectIdList=[[[1500901]]
                ]},
        ]},
        {5016,[
            new ComboDesc{
            Name=LightAttackPrefix,
            SkillEffectIdList = [
                [[1601101]],
                [[1601201]],
                [[1601301]],
                [[1601401]],
            ] },
            new ComboDesc
            {
                Name = HeavyAttackPrefix,
                SkillEffectIdList = [
                [[1603101,2]],
                [[1603201,2]]
            ] },
            new ComboDesc{
                Name= SpellAttackPrefix,
                SkillEffectIdList=[[[1607101,4]]
                ]},
            new ComboDesc{
                Name= LightAttackPrefix,
                Prefix=["冻土","Frozen Soil "],
                SkillEffectIdList=[[[1602101,-1]]
                ]},
            new ComboDesc{
                Name= HeavyAttackPrefix,
                Prefix=["冻土","Frozen Soil "],
                SkillEffectIdList=[[[1604101]]
                ]},
            ]},
        {5017,[
                new ComboDescEn()
                {
                    SkillEffectIdList = [[[ 1703404]],[[ 1703404]]]
                },
                new ComboDescL()
                {
                    SkillEffectIdList =[[[1701101]],[[1701201]],[[1701301]],[[1701401],[1702001]] ]
                },
                new ComboDescH()
                {
                    SkillEffectIdList =[[[1703101]],[[1703201]],[[1703301]],[[1703404,3],[1704001],[1703401]] ]
                },
                new ComboDescS()
                {
                    SkillEffectIdList =[[[1707101]]]
                },
                new ComboDescL()
                {
                    Prefix = ["强化","Enchanced "],
                    SkillEffectIdList =[[[1701101],[1702001]],[[1701201]],[[1701301],[1702001]] ]
                },
                new ComboDescS()
                {
                    Prefix = ["强化","Enchanced "],
                    SkillEffectIdList =[[[1707201],[1707204],[1704001]]]
                },
                new ComboDescH()
                {
                    Prefix = ["强化","Enchanced "],
                    SkillEffectIdList =[[[1703101],[1704001]],[[1703201],[1704001]],[[1703301],[1704001]],[[1703404,3],[1704001,3],[1703401],[1704001]] ]
                },
            ]
        },
        {
            5018,[
                new ComboDescEx
                {
                    SkillEffectIdList =[[[1800901]]]
                },
                new ComboDescL
                {
                    SkillEffectIdList =[[[1801101]],[[1801201]],[[1801301]],[[1801101],[1801401]]]
                },
                new ComboDescH
                {
                    Prefix = ["(对敌人)","(To Enemy)"],
                    SkillEffectIdList =[[[1803101]]]
                },
                new ComboDesc
                {
                Name = ["引爆1~4只虫卵","Lv1~4 Explosion"],
                SkillEffectIdList =[[[18401,1,1]],[[18402,1,1]],[[18403,1,1]],[[18404,1,1]]]
                }
            ]
        },
        {5019,[
            new ComboDescEn()
            {
                SkillEffectIdList = [[[1901101]]]
            },
            new ComboDescL()
            {
                SkillEffectIdList = [
                    [[1901101]],
                    [[1901201]],
                    [[1901301]],
                    [[1901401]]
                ]
            },
            new ComboDesc()
            {
                Name = ["切手技","Varied Combo"],
                SkillEffectIdList = [
                    [[1901501],[1901504]]
                ]
            },
            new ComboDescH()
            {
                SkillEffectIdList = [
                    [[1903901]],
                ]
            },
            new ComboDescH()
            {
                Prefix = ["1豆","Focus Lv.1"],
                SkillEffectIdList = [
                    [[1903101]],
                    [[1903201]],
                    [[1903301]],
                ]
            },
            new ComboDescH()
            {
            Prefix = ["2豆","Focus Lv.2"],
            SkillEffectIdList = [
                [[1903601]],
                [[1903701]],
                [[1903801]],
            ]
            },
            new ComboDescH()
            {
            Prefix = ["3豆","Focus Lv.3"],
            SkillEffectIdList = [
                [[1904101]],
                [[1904201]],
                [[1904301]],
            ]
            }
        ]},
        {5008,[
            new ComboDescL()
            {
                SkillEffectIdList = [
                    [[2301101]],
                    [[2301201]],
                    [[2301301]],
                    [[2301401]],
                ]
            },
            new ComboDescH()
            {
                SkillEffectIdList = [
                    [[2303101]],
                    [[2303201]],
                    [[2303301]],
                    [[2303401]],
                ]
            },
            new ComboDescL()
            {
                Prefix = ["强化","Enchanced"],
                SkillEffectIdList = [
                    [[2302101],[2302002]],
                    [[2302201],[2302002]],
                    [[2302301],[2302002]],
                    [[2302401],[2302002]],
                ],
            },
            new ComboDescH()
            {
            Prefix = ["强化","Enchanced"],
            SkillEffectIdList = [
                [[2304101],[2302002]],
            ],
            },
            new ComboDesc()
            {
            Prefix = ["雷阵","Thunder Area"],
            SkillEffectIdList = [
                [[2302002,-1]],
            ],
            }
        ]},
        {5024,[
            new ComboDescL()
            {
                SkillEffectIdList = [
                    [[2401101]],
                    [[2401201]],
                    [[2401301]],
                    [[2401401]],
                    [[2401501]],
                ]
            },
            new ComboDesc()
            {
              Name = ["远距离轻攻击第一段","First Light Attack from far distance"],
              SkillEffectIdList  = [[[2401001]]]
            },
            new ComboDescH()
            {
                Name = ["0~4级重击","Lv0~4"],
                SkillEffectIdList = [
                    [[2403901]],
                    [[2403101]],
                    [[2403201,2]],
                    [[2403301,2]],
                    [[2403401]],
                ]
            },
            new ComboDesc()
            {
                Name = ["任意轻攻击后重攻击","Heavy Attack After Any Light Attack"],
                SkillEffectIdList = [
                    [[2401601]],
                    [[2401801]],
                ]
            },            
            new ComboDescL()
            {
                Prefix = ["火","Fire"],
                SkillEffectIdList = [
                    [[2401172]],
                    [[2401272]],
                    [[2401372]],
                    [[2401472]],
                    [[2401572]],
                ]
            },            
            new ComboDescL()
            {
                Prefix = ["冰","Ice"],
                SkillEffectIdList = [
                    [[2401171]],
                    [[2401271]],
                    [[2401371]],
                    [[2401471]],
                    [[2401571]],
                ]
            },
            new ComboDescH()
            {
                Prefix = ["0~4级火","Lv0~4 Fire"],
                SkillEffectIdList = [
                    [[2403972]],
                    [[2403172]],
                    [[2403272,2]],
                    [[2403372,2]],
                    [[2403472]],
                ]
            },
            new ComboDescH()
            {
                Prefix = ["0~4级冰","Lv0~4Ice"],
                SkillEffectIdList = [
                    [[2403971]],
                    [[2403171]],
                    [[2403271,2]],
                    [[2403371,2]],
                    [[2403471]],
                ]
            },
        ]}
        
    };

    public static DescDict ItemBriefDesc = new DescDict
    {
        //变身,briefDesc+Desc
        {5001,["神力上限150,恢复速度0.833/s,变身期间-28冰抗,+45火抗,-4%伤害加成,+20%减伤,基础生命上限变为1.2倍",
            "150 Max Might.0.833/s Might Recover;-28 IceResist,+45 FireResist,-4% DamageBonus,+20% DamageReduction,Base MaxHP x1.2",
        ]},//狼
        {5004,["神力上限100,恢复速度0.625/s,变身期间+999全抗,+50%减伤,基础生命上限变为1.5倍",
            "100 Max Might.0.625/s Might Recover;+999 AllResist,+50% DamageReduction,Base MaxHP x1.5",
        ]},//石
        {5014,["神力上限125,恢复速度0.625/s;变身期间-28火抗,-5.71%伤害加成,+33.3%减伤,基础生命上限变为1.5倍",
            "125 Max Might.0.625/s Might Recover;-28 FireResist,-5.71% DamageBonus,+33.3% DamageReduction,Base MaxHP x1.5"]},//虎
        {5006,["神力上限150,恢复速度0.833/s;变身期间+28冰抗,-999火抗,-7.69%伤害加成,+19.6%减伤,基础生命上限变为1.2倍",
                "150 Max Might.0.833/s Might Recover;+28 IceResist,-999 FireResist,-7.69% DamageBonus,+19.6% DamageReduction,Base MaxHP x1.2"]},//鼠
        {5016,["神力上限150,恢复速度0.75/s;变身期间-28火抗,-9雷抗,+999冰抗,-13.3%伤害加成,+20%减伤,基础生命上限变为1.2倍",
                "150 Max Might.0.75/s Might Recover;-28 FireResist,-9 ThunderResist,+999 IceResist,-13.3% DamageBonus,+20% DamageReduction,Base MaxHP x1.2"] },//海上僧
        {5017,["神力上限100,恢复速度0.833/s;变身期间-28火抗,-9雷抗,-4%伤害加成,+20%减伤,基础生命上限变为1.2倍",
                "100 Max Might.0.833/s Might Recover;-28 FireResist,-9 ThunderResist,-4% DamageBonus,+20% DamageReduction,Base MaxHP x1.2"]},//马猴
        {5018,["神力上限100,恢复速度0.5/s;变身期间-9火抗,-9冰抗,+999毒抗,-8.57%伤害加成,+20%减伤,基础生命上限不变",
                "100 Max Might.0.5/s Might Recover;-9 FireResist,-9 IceResist,+999 PoisonResist, -8.57% DamageBonus,+20% DamageReduction,Base MaxHP x1.0"]},//虫
        {5019,["神力上限100,恢复速度0.4167/s;变身期间+10冰抗,+10火抗,+999雷抗,-15.56%伤害加成,+20%减伤,基础生命上限变为1.2倍",
                "100 Max Might.0.4167/s Might Recover;+10 IceResist,+10 FireResist,+999 ThunderResist,-15.56% DamageBonus,+20% DamageReduction,Base MaxHP x1.2"]},//龙
        {5008,["神力上限125,恢复速度0.625/s;变身期间+10冰抗,+45雷抗,-12.5%伤害加成,+19.6%减伤,基础生命上限变为1.2倍",
                "125 Max Might.0.625/s Might Recover;+10 IceResist,+45 ThunderResist,-12.5% DamageBonus,+19.6% DamageReduction,Base MaxHP x1.2"]},//马
        {5024,["神力上限150,恢复速度0.5/s;变身期间+999毒抗,+999雷抗,-16%伤害加成,+20%减伤,基础生命上限变为1.5倍",
            "150 Max Might.0.5/s Might Recover;+999 PoisonResist,+999 IceResist,-16% DamageBonus,+20% DamageReduction,Base MaxHP x1.2"]},//石猴
    };
    //\["(.*?)"\]=(".*?"),[ ]*\["(.*?)"\]=(".*?"),
    //{,new Desc{ $2, $4}},//$1/$3
    //珍玩装备
    public static DescDict EquipDesc = new DescDict
    {
        //首饰
        {16001, [] },//细磁茶盂/Fine China Tea Bowl
        {16002, [] },//猫睛宝串/Cat Eye Beads
        {16003, [] },//玛瑙罐/Agate Jar
        {16004, [] },//虎头牌/Tiger Tally
        {16005, ["身法-1秒/奇术-3秒/毫毛-6s", "-1s/3s/6s by type"] },//砗磲佩/Tridacna Pendant
        {16006, [] },//金花玉萼/Goldflora Hairpin
        {16007, ["+30", "+30"] },//琉璃舍利瓶/Glazed Reliquary,效果对应3个passive分别为123级，各回30/60/90
        //695.5
        {16008, [] },//风铎/Wind Chime --1006008 buff-desc 96008 写的是7%实测也是7%
        {16009, [] },//雷榍/Thunderstone
        {16010, [] },//耐雪枝/Frostsprout Twig
        {16011, [] },//白狐毫/Snow Fox Brush
        {16012, ["100次?", "100 times?"] },//未来珠/Maitreya's Orb--buff-item 96010-96012
        {16013, [] },//金色鲤/Golden Carp
        {16014, ["+1%,有日金乌时+3%", "+1%, +3% if have Gold Sun"] },//月玉兔/Jade Moon Rabbit
        {16015, [] },//三清令/Tablet of the Three Supreme
        {16016, ["180秒内+60生命上限,+40法力/气力上限", "+60 MaxHP,+40 MaxMP/MaxSt for 180s"] },//定颜珠/Preservation Orb ，有三个buff，每个分别增加一种属性的上限并回复等量值，自动生成的太长了，
        {16017, ["+1%,有月玉兔时+3%", "+1%, +3% if have Jade Moon"] },//日金乌/Gold Sun Crow
        {16018, [] },//错金银带钩/Cuo Jin-Yin Belt Hook
        {16019, [] },//金钮/Gold Button
        {16020, [] },//阳燧珠/Flame Orb
        {16021, [] },//水火篮/Daoist's Basket of Fire and Water
        {16022, [] },//辟水珠/Waterward Orb
        {16023, ["1-5段棍势上限由100/210/330/480/880变为90/200/320/470/870", "Lv.1-5 Max Focus:100/210/330/480/880 -> 90/200/320/470/870"] },//琥珀念珠/Amber Prayer Beads，有多个passiv
        {16024, [] },//仙箓/Celestial Registry Tablet
        {16025, [] },//虎筋绦子/Tiger Tendon Belt
        //16026仙胞石片
        {16027, [] },//摩尼珠/Mani Bead
        {16028, [] },//博山炉/Boshan Censer
        {16029, [] },//不求人/Back Scratcher
        {16030, ["", ""] },//吉祥灯/Auspicious Lantern
        {16031, ["+32防御,每次受击时玩家对敌人造成动作值为x0.1的伤害", "+32 DEF.Deal x0.1 Damage(based on player's ATK) per Hit"] },//金棕衣/Gold Spikeplate
        {16032, [] },//兽与佛/Beast Buddha
        {16033, [] },//铜佛坠/Bronze Buddha Pendant
        {16034, [] },//雷火印/Thunderflame Seal
        {16035, ["0.1/s", "0.1/s"] },//君子牌/Virtuous Bamboo Engraving
        {16036, [] },//卵中骨/Spine in the Sack
        {16037, [] },//白贝腰链/White Seashell Waist Chain
        {16038, [] },//石虎节
        //{16039, [] },//满堂红
        {16040, ["+100%持续时间(基础60s),+15%攻击","+100% Duration (Base 60s).+15%ATK"] },//神铁碎片

        //装备独门妙用
        //散件
	    {17001, ["高于1%血量时每秒-3HP,额外回复+15%生命上限", "-3HP/s when over 1% HP.+15% MaxHP Recover"] },//地灵伞盖/Earth Spirit Cap
        {17002, ["饮酒后15秒内+10攻击,20秒后-20攻击", "+10 ATK for 15s;-20 ATK after 20s"] },//长嘴脸/Snout Mask
        {17003, ["+2%", "+2%"] },//鳖宝头骨/Skull of Turtle Treasure
        {17004, [] },//山珍蓑衣/Ginseng Cape
        //蜢虫头907005 97005、97015 185、186
        {17005, ["+15%/20% 动作值", "+15%/20% MotionValue"] },//长须头面/Locust Antennae Mask
        //Talent 2702
        {17006, [] },//白脸子/Grey Wolf Mask
        {17008, ["阳:+20%伤害减免。阴:20%暴击-30%伤害减免", "Yang:+20% DamageReduction. Yin:+20% Crit,-30% DamageReduction"] },//阴阳法衣/Yin-Yang Daoist Robe
        //Talent 2713/2714
        {17010, ["300秒内+40生命/法力上限", "+40 MaxHP/MP for 300s"] },//南海念珠/Guanyin's Prayer Beads ,类似定颜珠，自动生成的太长了
        {17011, [] },//金刚护臂/Vajra Armguard
        {10701, [] },//厌火夜叉面/Yaksha Mask of Outrage
        {11601, ["+10棍势，强硬时+30", "+10 Focus.+30 when Tenacity"] },//大力王面/Bull King's Mask
        {10302, ["低于半血时每秒+1.5%生命上限(水中+2%)", "+1.5%MaxHP/s under 50% MaxHP;+2% MaxHP/s in water"] },//锦鳞战袍/Serpentscale Battlerobe，一个buff具有两个效果：回血，施加另一个处于水中回血的buff
        {11001, ["+100棍势", "+100"] },//金身怒目面/Golden Mask of Fury，2111buff触发施加2112，2112没有效果，2113的激活条件是具有2112
        //没有11902，从11912开始
        {11912, ["+15", "+15"] },//昆蚑毒敌甲/Venomous Sting Insect Armor
        {11803, ["3秒内+15%攻击", "+15% ATK for 3s"] },//玄铁硬手/Iron-Tough Gauntlets 把IronBodyBuff覆盖为2171
        {10603, ["每消耗一段棍势8秒内+6%暴击", "+6% for 8s for each focus level"] },//赭黄臂甲/Ochre Armguard
        {11304, ["10秒", "10s"] },//不净泥足/Non-Pure Greaves
        {11204, ["+10", "+10"] },//藏风护腿/Galeguard Greaves
        //Talent 2142
        {11402, [] },//羽士戗金甲/Centipede Qiang-Jin Armor
        //Talent 2098
        {10904, ["+15%攻击", "+15%ATK"] },//乌金行缠/Ebongold Gaiters //升满级的10934关联到buff2099/2097不知道有什么效果；2091~2099全是，太复杂了
        {12101, ["每段轻棍附加0.5动作值的伤害", "An additional damage of 0.5 MotionValue for each light attack"] },//三山冠
        {12201, ["", ""] },//彩金狮子冠

        {15012, ["5秒内获得共127.5棍势(2.5+25*5)", "Gain 127.5(2.5+25*5) Focus for 5s"] },//狼牙棒/Spikeshaft Staff
        {15007, ["每段棍势+5HP(中毒敌人+40)", "+5 HP(40 for Poisoned enemy) for each focus level"] },//昆棍·百眼/Visionary Centipede Staff
        {15013, ["每段棍势+5HP", "+5 HP for each focus level"] },//昆棍·蛛仙/Spider Celestial Staff
        //{,new Desc{ "每段棍势+5HP", "+5 HP for each focus level"}},//昆棍/Chitin Staff
        {15018, ["每段棍势:+5HP并造成x1.0伤害", "+5 HP and deal x1.0 Damage for each focus level"] },//昆棍·通天/Adept Spine-Shooting Fuban Staff
        {15016, ["每点防御+0.15攻击", "+0.15ATK per DEF"] },//混铁棍/Dark Iron Staff
        {15015, ["+20% 动作值", "+20% MotionValue"] },//飞龙宝杖/Golden Loong Staff//15015 145
        {15101,
            [
                "Lv0~4重击动作值+20%/20%/20%/20%/10%;搅阵/进尺动作值+15%;飞剑造成{AR:1099916} per Hit伤害",
                "Lv0~4 Heavy Attack MotionValue:+20%/20%/20%/20%/10%;Deal {AR:1099916} Damage per Hit"
            ]
        },//三尖两刃枪/Tri-Point Double-Edged Spear//15015 145
        {15017,
            [
                "每次命中附加一段x0.1或x0.3或x1.0的伤害，其中50%为雷属性",
                "An addition x0.1/x0.3/x1.0 Damage per hit.50% is Thunder Damage."
            ]
        },//天龙棍
        {15102,
            [
                "Lv0~4重击动作值+20%/20%/20%/20%/10%;搅阵/进尺动作值+15%;",
                "Lv0~4 Heavy Attack MotionValue:+20%/20%/20%/20%/10%;Whirling/Forceful Thrust MotionValue:+15%"
            ]
        },//楮白枪
    };
    //天赋
    public static DescDict TalentDisplayDesc = new DescDict
    {
        //根基-气力
        //7.5/6/5.25
        {100101, ["Lv1: -20%/Lv2: -30%,基础 7.5/s", "Lv1: -20%/Lv2: -30%,BaseCost 7.5/s"] },//体健
        {100102, ["+50%气力回复", "+50% Stamina Recover"] },//绵绵
        {100104, ["-30%,基础消耗 30", "-30%,BaseCost 30"] },//身轻
        {100105, ["+15%/lv"] },//调息
        {100106, ["-2.5/lv,基础消耗 20", "-2.5/lv,BaseCost 20"] },//猿捷
        {100108, ["+10/lv,基础 40", "+10/lv,Base 40"] },//走险
        {100109, ["15%,持续2秒", "15% for 2s"] },//绝念
        {100110, ["动作值:{AR:1031102}", "MotionValue:{AR:1031102}"] },//梦幻泡影
        //{,new Desc{ "",}},//任翻腾//第一段翻滚由SkillDesc 10301变为10305

        //武艺
        {100201, ["+100/lv,基础800/900/1000;", "+100/lv,Base 800/900/1000;"] + LightAttackDesc},//直取/Switft Engage//Passive 13/14 SkillCtrlDesc 10701,10798-10801
        {100202, ["18->23 per Hit"] },//接力
        {100203, ["动作值{AR:1080101}->{AR:1080001}", "MotionValue:{AR:1080101}->{AR:1080001}"] },//冲霄
        {100204, ["动作值:{AR:1080505}", "MotionValue:{AR:1080505}"] },//捣蒜打
        //10508 46/47 SkillEffectDamageExpand
        //{,new Desc{ "Lv1-2:+100/150", "Lv1-2:+100/150"}},//断筋/TODO
        {100205, ["30%"] },//筋节
        {100206, ["动作值:{AR:1080501}->{AR:1075501};不影响枪", "MotionValue:{AR:1080501}->{AR:1075501};Not affect spear"]
        },//气息不绝
        //10506 PassiveSkillDesc 16写的是Mul 50%,实测约+18%，回复量会浮动，约7.5~8，点完变成9上下
        {100207, ["棍花动作值:{AR:1081701};棍花移步:{AR:1082301}", "MotionValue:{AR:1081701};Moving:{AR:1082301}"] },//棍花移步
        {100208, ["动作值:{AR:1085201}", "MotionValue:{AR:1085201}"] },//方圆径寸
        {100209, ["约+18%", "about +18%"] },//化吉/Silver Lining
        {100210, ["-30%, 基础26/秒，棍花移步基础48/秒", "-30%. BaseCost 26/s(48/s when moving)"] },//应手
        {100211, ["4->6 per Hit, 不影响原地棍花(3 per Hit)", "4->6 per Hit. Not affect Staff Spin(3 per Hit)"] },//得心

        //体段修行
        {100301, ["+10/lv,变身后保留20%", "+10/lv.Remain 20% during transformation"] },//五脏坚固
        {100302, ["+10/lv,变身不保留", "+10/lv.No effect during transformation"] },//气海充盈
        //TalentSDesc 100303 PassiveSkillDesc 20-25
        {100303, ["-5%持续时间/lv", "-5% Duration/lv"] },//灾苦消减/Bane Mitigation
        {100304, ["+1%/lv,变身不保留", "+1%/lv,No effect during transformation"] },//怒相厚积
        {100305, ["+10/lv,变身不保留", "+10/lv.No effect during transformation"] },//皮肉粗糙
        {100306, ["+10/lv"] },//法性贯通
        {100307, ["+2.5气力回复/lv", "+2.5 Stamina Recover/lv"] },//吐纳绵长
        {100308, ["+2攻击力/lv,变身不保留", "+2 ATK/lv.No effect during transformation"] },//攻势澎湃
        {100309, ["+3/lv,变身不保留", "+3/lv.No effect during transformation"] },//四灾忍耐
        {100310, ["+4%/lv,变身不保留", "+4%/lv.No effect during transformation"] },//威能凶猛
        //棍法
        {100501,HeavyAttackDesc+LightAttackDesc},//二段棍势
        {100502, ["回复+2%/3%/4%(Lv1/Lv2/Lv3) 最大生命", "Heal +2%/3%/4%(Lv1/Lv2/Lv3) MaxHP"] },//壮怀
        {100503,HeavyAttackDesc+LightAttackDesc},//三段棍势
        {100505,HeavyAttackDesc+LightAttackDesc},//四段棍势
        //55/65/90/115/115 n
        //43/50/67.5/85 Lv3
        //29/32.5/41.2/50 Lv3+r
        //35/40/52.5/65 r
        //100504 Passive 36-38里写的是-10%,但和实测对每种风格都是减固定值
        //劈棍原地重击消耗10+55/65/90/115/115，立棍10+40/65/75/100/100，戳棍47.5/57.5/82.5/107.5，跳劈(三种棍势一样)50/75/100/125/125,
        //3级天赋劈棍43/50/67.5/85，立棍28/50/53/70，戳棍35.5/42.5/60/77.5
        //不影响跳劈,不影响蓄力起手(三种风格起手消耗10/10/15)
        //基础重击消耗是40/50/75/100,每种风格在其上附加固定气力消耗，附加的气力消耗不受天赋和根器影响， 劈棍15/15/15/15/15，立棍附加0/15/0/0/0,戳棍附加7.5/7.5/7.5/7.5
        {100504, [
                "-10%/lv基础气力消耗,0-4级重棍基础消耗:40/50/75/100/100;和身轻体快(根器)乘算，不影响劈/立/戳的额外气力消耗，不影响蓄力和跳跃重击,劈棍:额外+15消耗,立棍:仅Lv1额外15消耗,戳棍:额外+7.5消耗",
                "-10%/lv heavy-attack Base Stamina Cost.Multi with Nimble Body(Relic) effect.Not affect addition cost of Smash/Pillar/Thrust.Not affect charge/jump-heavy-attack. Base cost of 0~4 focus-level:40/50/75/100/100. Smash:additional +15 Cost.Pillar:(Lv1 only) additional +15 Cost.Thrust:additional +7.5 Cost."
            ]
        },//熟谙
        //10506 PassiveSkillDesc 39-41
        {100506, ["+4%/lv 动作值", "+4%/lv MotionValue"] },//通变/Versatility
        {100507, ["+5%/lv,基础每秒55(劈)/35(立)/40(戳)点棍势", "+5%/lv,Base 55(Smash)/35(Pillar)/40(Thrust) Focus per sec"] },//精壮
        {100508,
            [
                "Lv1~2:额外造成目标当前生命值1%/1.5%的真实伤害;此伤害无视防御和减伤,与消耗棍势数量无关",
                "Lv1~2: Deal extra true damage which equals to 1%/1.5% of target current HP.Ignore damage reduction&Defense.Not related to consumed Focus."
            ]
        },//精壮
        {100509,
            [
                "识破:{AR:1070501}->{AR:1070601};斩棍:{AR:1070701}->{AR:1070801};江海翻:{AR:1072402}->{AR:1072502};进尺:{AR:1071401}->{AR:1071501}",
                "Resolute Strike: {AR:1070501}->{AR:1070601};Skyfall Strike:{AR:1070701}->{AR:1070801};Churning Gale:{AR:1072402}->{AR:1072502};Forceful Thrust:{AR:1071401}->{AR:1071501}"
            ]
        },
        {100602, ["Lv1:+20%/Lv2:+30%"] },//克刚
        //10603 51/52 SkillEffectFloat
        {100603, ["Lv1-2:+5%/8% 动作值", "Lv1-2:+5%/8% MotionValue"] },//压溃/Smashing Force
        {100607,
            [
                "动作值:{AR:1070501},无敌时间0.5s,GP判定窗口0.4s",
                "MotionValue:{AR:1070501}.Immue Duration 0.5s.GP-Success Window 0.4s"
            ]
        },//识破
        {100609, ["动作值:{AR:1070701}", "MotionValue:{AR:1070701}"] },//斩棍势
        {100610, ["+100"] },//抖擞
        {100611, ["每段棍势+5%攻击", "+5% ATK per Focus Level"] },//乘胜追击/Vantage Point
        //10702 53/54
        {100701,
            [
                "Lv0~4重击动作值:{AR:1086001}/{AR:1086101}/{AR:1086201}/{AR:1086301}/{AR:1086401}",
                "Lv0~4 heavy attack Motion Value:{AR:1086001}/{AR:1086101}/{AR:1086201}/{AR:1086301}/{AR:1086401}"
            ]
        },//立棍
        {100702, ["-20%/lv", "-20%/lv"] },//铁树/Steel Pillar
        {100705, ["动作值:{AR:1072102},无敌时间0.3s", "MotionValue:{AR:1072102}.Immue Duration 0.3s"] },//风云转
        {100706, ["20%"] },//拂痒
        {100707, ["动作值:{AR:1072402}", "MotionValue:{AR:1072402}"] },//江海翻
        {100708,
            [
                "获得等于0.35*棍势值的攻击力百分比加成;例:4段棍势(480)时获得168%攻击力加成",
                "Gain (0.35*Focus)% Atk.(Eg,gain +168% atk when 480 Focus)"
            ]
        },//天地倾，对AtkMul施加id为5参数为-15的FixFunction修正，see RunByBuffApply GetFixFunctionDesc;Fix function 5 为根据Pevalue的35计算

        {100801,
            [
                "Lv0~4重击动作值:{AR:1088001}/{AR:1088101}/{AR:1088201}/{AR:1088301}/{AR:1088401}",
                "Lv0~4 heavy attack Motion Value:{AR:1088001}/{AR:1088101}/{AR:1088201}/{AR:1088301}/{AR:1088401}"
            ]
        },//戳棍
        {100802, ["Lv1-2:回复30/50", "Lv1-2:Recover 30/50"] },//借力/Borrowed Strength//Passive 13/14 SkillCtrlDesc 10701,10798-10801
        {100803, ["动作值:{AR:1071102}", "MotionValue:{AR:1071102}"] },//搅阵
        {100804, ["9秒内+1% 每层", "+1% per stack for 9s"] },//骋势
        {100807,
            [
                "退寸无敌时间0.5s,进尺动作值:{AR:1071401}",
                "Tactical Retreat Immue Duration 0.5s.Forceful Thrust MotionValue:{AR:1071401}"
            ]
        },//退寸进尺
        {100809,
            [
                "退寸获得0.366秒GP判定窗口，GP成功时获得等同识破成功的效果,包含0.5s基础无敌时间",
                "Grants Tactical Retreat a 0.366s GP-Success-Window.Trigger all See Through effects when GP successfully,including 0.5s Immue."
            ]
        },//赌胜，enable buff 293,使得戳棍gp成功触发288、1007、2026、96025、1046(此消彼长)、295，其中288 enable 272(劈棍gp成功获得0.5s无敌)
        {100810, ["15s"] },//此消彼长 buff 1047 setsimplestate 40
        //1048~1050

        //奇术
        //FUStBuffDesc-Talent 1025 TalentSDesc 10901 PassiveSkillDesc 57/58，降低对手的伤害减免，buff基础数值是0，通过passiveskill修改buffeffect
        {100901, ["Lv1-2:+10%/15%敌人承受伤害", "Lv1-2:+10%/15% Enemy Damage Taken"] },//脆断/Crash
        //BuffDesc-Talent 1027
        {100902, ["+60%持续;+30%敌人承受伤害", "+60% duration;+30% Enemy Damage Taken"] },//瞬机/Evanescence
        {100903, ["+15MP"] },//不破不立
        {100904, ["50MP->80MP,+30%持续时间", "50MP->80MP, +30% Duration"] },//假借须臾/Time Bargain //100904 10904,10914,10924 Passive 61-63
        {100905, ["每层-2%敌人定身抗性,基础8秒", "-2% Enemy Immobilize Resist per stack, Base 8s"] },//凝滞//10905 Passive 64/65 Buff 1064 写的是DingshenDefAdditionBase +2,但是Passive里只改了叠加上限没改数值，怀疑是百分比加成，每层+2%
        {101301, ["+2/4/5秒，基础20.5秒", "+2/4/5s，Base 20.5s"] },//圆明
        {101302, ["20/s->23/s"] },//烈烈
        {101303, ["Lv1:+10/Lv2:+15 每秒", "Lv1:+10/Lv2:+15 per sec"] },//弥坚
        {101304, ["60MP->90MP, 离开后持续5秒", "60MP->90MP. 5s Duration after leaving."] },//无挂无碍
        {101305, ["+30% -> 40+30%"] },//昂扬
        {101306, ["+20%"] },//归根
        {101502, ["-10%/lv"] },//不休
        {101503, ["每级+0.02攻击力/1法力，基础0.12攻击力/1法力", "Each level +0.02 ATK per MP, Base 0.12ATK per MP"] },//智力高强
        {101504, ["0.03%暴击/1气力", "0.03% Crit Chance per Stamina"] },//放手一搏
        {101505, ["20%生命上限", "20% MaxHP"] },//舍得

        //身法
        //1059,10/15/15? walk/run/dash?
        {101001, ["徐行/奔跑/冲刺速度+10%/15%/15%", "+10%/15%/15% Walk/Run/Sprint speed"] },//纵横/Gallop
        {101002, ["+2秒/lv,基础10秒", "+2s/lv,Base10s"] },//养气/Converging Clouds
        //Talent 2101 AtkMul 2000
        {101004,
            [
                "施放消耗30MP->40MP;+20% 攻击;不影响维持消耗(基础3MP/秒)",
                "Cast Cost:30MP->40MP;+20% ATK;Not Affect Cost Over Time(3MP/s)"
            ]
        },//破影一击/Absolute Strike
        {101005, ["Lv1:+3%/Lv2:+4% 每层", "Lv1:+3%/Lv2:+4% per stack"] },//洞察
        //Talent 11006 Passive 74/75
        {101006, ["+15%/lv", "+15%/lv"] },//捣虚/Ruse
        {101007, ["动作值:x1", "MotionValue:x1"] },//舍身 1055,是个buff?
        
        {101104, ["{AR:1080101}/{AR:1070101} -> {AR:1050201}"] },//先发
        {101105, ["15s->13s"] },//频频//11105 85 -2s
        {101106, ["+15%,持续15秒", "+15% for 15s"] },//巩固
        {101102, ["+75"] },//知机
        {101103, ["Lv1-2:+10MP/15MP;-0.2/0.3秒,基础1秒", "Lv1-2:+10MP/15MP;-0.2s/0.3s,Base 1s"] },//厉无咎/Bold Venture //101103 Passive 77-84

        //毫毛
        //11201 Passive 88/89
        {101201, ["+2秒/lv,基础约25秒", "+2s/lv,Base about 25s"] },//存身/Longstrand
        {101202, ["每层+0.4s?", "+0.4s per stack?"] },//合契/Synergy//101201 Passive 89 1001101写的+4，实测最多约+4秒？
        {101203, ["20MP/lv"] },//毛吞大海
        {101205, ["+10%", "+10%"] },//同寿/Grey Hair//11205 96
        //Talent 1082
        {101206, ["+15% 攻击", "+15% ATK"] },//仗势/Tyranny of Numbers
        {101207, ["积攒效率为本体的20%", "20% efficiency comparing to player"] },//玄同
        {101208, ["+15%/lv", "+15%/lv"] },//浇油/Insult to Injury//11208 97/98 1083buff基础15%,1/2级天赋+0/+15%
        {101401, ["-10秒", "-10s"] },//回阳/Glorious Return//buff 1111
        {101403, ["回复20%法力上限", "Recover 20% MaxMP"] },//不增不减/Spirited Return

        {101404, ["20%", "20%"] },//去来空/Cycle Breaker//101404 112
        //变化
        {301001, ["+3.3%/lv"] },//暗藏神妙
        {301002, ["-1.67%/lv,基础 1.25/s", "-1.67%/lv,Base 1.25/s"] },//保养精神
        {301003, ["+2%/lv", "+2%/lv"] },//存神炼气
        {301004, ["+2/lv"] },//虚相凶猛
        {301005, ["+15/lv"] },//炼实返虚
        {301006, ["+2%神力上限/lv,基础 40%", "Gain +2% Max Might/lv,Base 40%"] },//截伪续真
        
        //-28冰+45火抗
        {301101, ["Lv1-3:+10%/15%/20%攻击力", "Lv1-3:+10%/15%/20%ATK"] },//步月
        {301102, ["+15/lv"] },//磊磊
        //TalentSDesc 301103 PassiveSkillDesc 232  233 实际是-1.5 3.5
        {301103, ["-10%"] },//剪尾
        {301104, ["Lv1->6:+4/6/8/10/12/14"] },//爆躁
        {301105, ["Lv1->6:+4/6/8/10/12/14"] },//霏霏
        {301106, ["60秒内+1 攻击力/每层", "+1 ATK for 60s per stack"] },//红眼/Red Eyes
        {301107, ["Lv1->6:+4/6/8/10/12/14"] },//恶秽
        {301109, ["Lv1->6:+4/6/8/10/12/14"] },//奔霄
        {301108, ["+0.5秒->0.8秒", "+0.5s->0.8s"] },//一闪/Lightning Flash Passive 31108; Add buff 272 duration 300
        {301110, ["+12"] },//不坏身

        //根器
   	    {200101,
            [
                "轻棍1-5段全中获得的棍势由15/18/17/28/40变为15/18/26/35/43",
                "Light Attack 1-5 focus point if all hit :15/18/17/28/40 -> 15/18/26/35/43"
            ]
        },//见机强攻/Opportune Watcher
        //buff talent 1053
        {200102, ["-12.5秒", "-12.5s"] },//眼乖手疾/Eagle Eye
        {200103, ["+15%", "+15%"] },//慧眼圆睁/Keen Insight

        //20201 Passive 20201 Add buff 287(识破判定),293(退寸判定),114(识破和退寸无敌),10110(风云转无敌) duration 66
        //287,0.4s是个判定buff，成功施加288(慢放并触发19998被动，即令1070501的EffectParamsInt[1]++(21->22)，实测不增加动作值,应该是异常等级+1)、1007(天赋-GP成功接连招标记)、2026(狼牙棒)、96025(虎筋绦子)
        //293,0.366s也是判定buff，成功施加288、1007、2026、96025、1046(此消彼长)、295(戳棍gp，用途不明),需要有天赋100809赌胜，即赌胜实际效果是让退寸也能如同识破一样触发各种gp类无敌和buff
        //114,0.5s和10110,0.3s是setsimplestate 116 17 57 59 99的无敌buff,116= EBGUSimpleState.CommonDamageImmue
        //272,0.5s同样是setsimplestate 116 17 57 59 99的无敌buff，识破/退寸后固定获得，但是需要有buff 288才会生效，即劈棍、戳棍GP成功获得0.5s无敌；288时间为1s，所以不改288时此无敌效果不会超过1s
        {200201,
            [
                "+0.066秒无敌时间和GP窗口,基础无敌时间:0.5(识破)/0.3(风云转)/0.5(赌胜退寸)秒,基础GP窗口:0.4(识破)/0.366(赌胜退寸)秒",
                "+0.066s Immune Duration and GP-Success Window.Base Immue Duration: 0.5(Resolute)/0.3(Sweeping Gale)/0.5(Tactical Retreat) sec. Base GP-Success Window: 0.4(Resolute)/0.366(Tactical Retreat) sec"
            ]
        },//耳听八方/All Ears
        {200202, ["-0.1秒，基础1秒", "-0.1s,Base 1s"] },//如撞金钟/Sound as A Bell // Ｐａｓｓｉｖｅ 122-125 Buff-lz 228 buff-talent 1069
        {200203, ["5秒内+10%攻击", "+10% ATK for 5s"] },//耳畔风响/Whistling Wind

        {200301, ["15秒内+10%伤害加成", "+10% Damage for 15s"] },//气味相投/Lingering Aroma //buff 2107
        //{,new Desc{ "+12", "+12"}},//阳燧珠/In One Breath -
        {200303,
            ["+0.066秒,0-3段翻滚基础无敌时间0.4/0.433/0.466/0.5秒", "+0.066s,0-3 level roll base time 0.4/0.433/0.466/0.5s"]
        },//屏气敛息/Hold Breath//-20303 Passive  127 buff-lz  10105-10109

        {200404, ["+10%丹药持续时间", "+10% Duration"] },//舌尝思/Envious Tongue
        {200401, ["5秒内+15%", "+15% for 5s"] },//丹满力足/Refreshing Taste
        //{,new Desc{ "+12", "+12"}},//阳燧珠/Spread the Word
        {200403, ["每个增加+4%生命上限的回复量", "+4% MaxHP recover each"] },//遍尝百草/Tongue of A Connoisseur

        //劈棍原地重击消耗10+55/65/90/115/115，立棍10+40/65/75/100/100，戳棍47.5/57.5/82.5/107.5，跳劈(三种棍势一样)50/75/100/125/125,
        //根器效果是气力消耗-50，基础重击消耗是40/50/75/100,每种风格在其上附加固定气力消耗，附加的气力消耗不受天赋和根器
        //实际劈棍附加15/15/15/15/15，立棍附加0/15/0/0/0,戳棍附加7.5/7.5/7.5/7.5
        {200501,
            [
                "重击基础气力消耗减半，0~4级基础消耗40/50/75/100/100，和熟谙(天赋)乘算，不影响劈/立/戳附加的气力消耗，不影响重击起手式/蓄力/跳跃重击消耗",
                "-50% heavy-attack Base Cost.Lv0~4 Base Cost:40/50/75/100/100.Multi with Instinct(talent) effect.Not affect the additional Stamina Cost of Smash/Pillar/Thrust.Not affect cost of charging or jump-heavy-attack."
            ]
        },//身轻体快/Nimble Body
        {200502, ["+60", "+60"] },//福寿长臻/Everlasting Vitality
        {200503, ["+15", "+15"] },//灾愆不侵/Divine Safeguard

        {200601,
            [
                "1~4段+10%动作值，5段+5%动作值，不影响跳跃轻击",
                "1~5 Light Attack:+10/10/10/10/5 MotionValue.Not affect Jump light attack."
            ]
        },//万相归真/Elegance in Simplicity
        {200603, ["30秒内+30%伤害加成", "+30% Damage for 30s"] },//不生不灭/Unbegotten, Undying
    };
    //套装效果
    public static DescDict SuitInfoDesc = new DescDict
    {
        //SuitInfo和RedQualityInfo都是通过AttrEffectID或TalentID生效

        //Talent 2037->90301 ?,鳞棍2034->15003?
        {900311, ["x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"] },//浪里白条/Wave-Rider
        //没找到对应passive
        //{900321,new Desc{ "x0.5气力消耗(和天赋效果乘算)", "x0.5 Stamina Cost(Multi with talent effect)"}},//浪里白条/Wave-Rider
        //650->702，+满级百足734.5
        //Talent 2041 - 2044 写的是0 / 8 / 10,实测是0 / 8 %/ 约10 %
        {900411, ["+8%/10%奔跑/冲刺速度", "+8%/10% Run/Sprint speed."] },//日行千里/Swift Pilgrim
        {900412, ["每层+10%攻击,持续2秒", "+10% ATK per stack for 2s"] },//日行千里/Swift Pilgrim
        {900421, ["每秒+12棍势", "+12 Focus/s"] },//日行千里/Swift Pilgrim

        {900511, ["每个天赋+24防御", "+24 DEF per Relic Talent"] },//心灵福至/Fortune's Favor
        //Talent 2063
        {900611, ["20%减伤", "20% Damage Reduction"] },//走石飞砂/Raging Sandstorm
        {900711, ["+20% 动作值", "+20% MotionValue"] },//离火入魔/Outrage
        {705, ["+25%伤害 -30%伤害减免", "+25% Damage.-30% DamageReduction"] },//离火入魔/Outrage
        {900811, ["+10赋雷攻击", "+10 Thunder ATK"] },//龙血玄黄/Thunder Veins
        {900821, ["+10赋雷攻击", "+10 Thunder ATK"] },//龙血玄黄/Thunder Veins
        {901011, ["20秒内+15%攻击", "+15% ATK for 20s"] },//借假修真/Gilded Radiance
        {901012, ["暴击+3元气,\n击杀+5元气", "+3/+5 Qi when Crit/Kill"] },//借假修真/Gilded Radiance
        //96005 / 96006 实测每次减少0.75~1秒冷却不定？？非传奇和传奇没有区别？？
        {901211, ["+15棍势", "+15 Focus"] },//举步生风/Gale Guardian
        {901212, ["+-0.75~1秒冷却?", "-0.75s~1s CD？"] },//举步生风/Gale Guardian
        //TalentSDesc 91221 91222 91223,但是Passive里只有91221
        {901221, ["-0.1s无敌时间，不会额外减少冷却", "-0.1s Immune Duration.Won't reduce more CD"] },//举步生风/Gale Guardian
        
        //90711 Passive 167
        //Talent 2135 - 0.005，实测变身还原 + 1.5每秒 ；2137 - 0.00375, 实测约1.12每秒，结合 - 0.005推测应为1.125 / s
        {901311,
            [
                "+20%伤害减免，结束变身时获得12秒黑泥，化身还原后获得6秒黑泥",
                "+20% DamageReduction.Gain Mud for 12s upon quiting tranformation;Gain Mud for 6s upon quiting vigor."
            ]
        },//泥塑金装/From Mud to Lotus
        {901312,
            [
                "翻滚回复约0.3神力,结束变身后12秒内+1.5/s神力回复，化身还原后4秒(not 6)内+1.125/s神力回复",
                "About +0.3 Might upon roll. +1.5/s Might Recover for 12s upon quiting tranformation.+1.125/s Might Recover for 4s(not 6s) upon quiting vigor."
            ]
        },//泥塑金装/From Mud to Lotus
        {901411, ["x0.8毒伤(和抗性效果乘算)", "x0.8 Poison Damage(Multi with Poison Resist effect)"] },//花下死/Poison Ward
        {901412, ["+20%攻击", "+20% ATK"] },//花下死/Poison Ward
        //独角仙套 91912 Passive 185
        {901511, ["+10%灵蕴", "+10% Will"] },//锱铢必较/Every Bit Counts
        {901611, ["5秒内+10%防御", "+10% DEF for 5s"] },//百折不挠/Unyielding Resolve
        {901612, ["", "Grants Tenacity OVER half HP"] },//百折不挠/Unyielding Resolve
        {901811, ["+50棍势", "+50 Focus"] },//铜心铁胆/Iron Will
        {901812, ["-5秒冷却", "-5s CD"] },//铜心铁胆/Iron Will
        {901911, ["+100棍势", "+100 focus"] },//毒魔狠怪/Fuban Strength
        {901912, ["+20%持续时间", "+20% Duration"] },//毒魔狠怪/Fuban Strength
        {902011, ["10秒内+8%暴击", "+8% Crit for 10s"] },//试比天高/Heaven's Equal
        {902012, ["-1秒冷却 per Hit", "-1s CD per Hit"] },//试比天高/Heaven's Equal


        {900921, ["+20法力消耗;假身持续时间不变,但不会因破隐而消失", "+20 MP Cost"] },//乘风乱舞/Dance of the Black Wind
        //Talent 2181 青铜套内部叫黑铁
        {901712, ["-15秒冷却", "-15s CD"] },//炼魔荡怪/Evil Crasher
        {902211,["-30%"]},//四时吉庆,92211
        //{902311,[]},//烟花,意义不明
    };
    //精魂(RZD)被动 VIPassiveDesc.OnChangeItemId
    public static DescDict SpiritDesc = new DescDict
    {
        {-1, ["1/4/6级时,{0}", "{0} at Lv1/4/6"] },
        //注意和珍玩不同，自动生成的需要设成"",而不是null
        {8011, ["", ""] }, //"广谋"
        {8012, ["[-6%]/[8%]/[10%]", "[-6%]/[8%]/[10%]"] },//波波浪浪/波里个浪
        {8013, ["", ""] }, //"幽魂"
        {8014, ["", ""] }, //"鼠司空"
        {8015, ["", ""] }, //"百目真人"
        {8017, ["", ""] }, //"虎伥"
        {8020, ["", ""] }, //"不空"
        //8022 ["无量蝠"]=nil,["Apramāṇa Bat"]=nil,
        {8024, ["", ""] }, //"不净"
        {8025, ["", ""] }, //"不白"
        {8026, ["", ""] }, //"不能"
        {8027, ["", ""] }, //"虫总兵"
        {8028, ["[-10]/[15]/[20]消耗", "[-10]/[15]/[20] MP Cost"] }, //"儡蜱士"
        {8029, ["", ""] }, //"青冉冉"
        {8030, ["", ""] }, //"蝎太子"
        //取平地普通移动再停止后最后一个数字，该数字是稳定的,观察PlayerLocomotion.MaxSpeed:[650]/[663]/[669]/682.5
        {8031, ["", ""] }, //"百足虫"
        {8032, ["", ""] }, //"兴烘掀·掀烘兴"
        {8033, ["", ""] }, //"石父"
        {8034, ["", ""] }, //"地罗刹"
        {8035, ["", ""] }, //"鳖宝"--303184/33484 Passive 292/272，1061301,1061401,1021301,1021401,1086301,1086401,1088301,1088401
        {8036, ["", ""] }, //"燧统领"
        {8037, ["", ""] }, //"燧先锋"
        {8038, ["", ""] }, //"琴螂仙"
        {8039, ["", ""] }, //"火灵元母"
        {8040, ["", ""] }, //"老人参精" --303157 [33657]/[33457]/[33157] 244/264/
        {8041, ["", ""] }, //"蘑女"
        {8042, ["", ""] }, //"菇男"
        
        {8061, ["", ""] }, //"狼刺客"
        {8062, ["", ""] }, //"疯虎"
        {8063, ["", ""] }, //"沙二郎"
        {8064, ["", ""] }, //"鼠禁卫"
        {8065, ["", ""] }, //"骨悚然"
        {8066, ["", ""] }, //"狸侍长"
        {8067, ["", ""] }, //"疾蝠"
        {8068,
            [
                "[-6%]/[8%]/[10%]气力消耗,[+10%]/[12%]/[15%] 跳跃轻击动作值",
                "[-6%]/[8%]/[10%] Stamina Cost,[+10%]/[12%]/[15%] MotionValue"
            ]
        }, //"石双双" --33681/33581 [247]/[267]/[287]
        //--26/[24.5]/[24]/[23.5]
        {8069,
            [
                "减少每秒消耗[-1.5]/[2]/[2.5]，基础消耗26/s(移步48/s)",
                "[-1.5]/[2]/[2.5] cost per second(Base Cost:26/s.Base Cost When moving:48/s)"
            ]
        }, //"鼠弩手"
        {8070, ["", ""] }, //"地狼"
        {8071, ["", ""] }, //"隼居士"
        {8072, ["", ""] }, //"赤发鬼"[--33687]/[33487]/[33187] [295]/[275]/[255]
        {8073, ["", ""] }, //"戒刀僧"
        {8074, ["", ""] }, //"泥塑金刚"
        {8075, ["", ""] }, //"夜叉奴"
        {8077, ["", ""] }, //"鸦香客"
        {8078, ["", ""] }, //"虫校尉"
        {8079, ["", ""] }, //"蜻蜓精"
        {8081, ["", ""] }, //"傀蛛士"
        {8083, ["", ""] }, //"蛇捕头"
        {8084, ["", ""] }, //"蛇司药"
        {8085, ["", ""] }, //"幽灯鬼"
        {8086, ["", ""] }, //"黑脸鬼"
        {8087, ["", ""] }, //"牯都督"
        {8088, ["[-6]/[-8]/[-10]", "[-6]/[8]/[10]"] }, //"雾里云·云里雾"
        {8092, ["[-6]/[8]/[10]点消耗", "[-6]/[8]/[10]"] }, //"九叶灵芝精"
        {8076, ["+[2]%/[2.5]%/[3]%", "+[2]%/[2.5]%/[3]%"] }, //"巡山鬼"--303686 [254]/[274]/[294] [1061410]/[1061430]/[1061460]?    Hook IsAttackCrit实测，加算
    };
    public static DescDict FabaoAttrDesc = new DescDict
    {
        //法宝被动(携带效果,EqDesc AttrDesc) VITreasureDesc::OnChangeItemId
        {19001, [] },
        {19002, [] },
        {19004, [] },
        {19005, [] },
    };
    //精魂消耗,PreFillDict时填充
    public static DescDict SpiritCost = new DescDict
    {
    };        

    public static readonly ConstDescDict EBGUAttrFloatDictConst = new ConstDescDict
    {
        {(int)EBGUAttrFloat.HpMax, ["{0}生命上限", "{0} MaxHP"] },
        {(int)EBGUAttrFloat.MpMax, ["{0}法力上限", "{0} MaxHP"] },
        //{(int)EBGUAttrFloat.B1StunMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.StaminaDepletedLimit,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.StaminaMax, ["{0}气力上限", "{0} MaxStamina"] },
        //{(int)EBGUAttrFloat.SkillSuperArmorMax,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.TransEnergyMax, ["{0}神力上限", "{0} MaxMight"] },
        //{(int)EBGUAttrFloat.EnergyMinConsume,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.EnergyConsumeSpeed, ["{0}神力消耗速度", "{0} Might Consume Speed"] },
        {(int)EBGUAttrFloat.EnergyIncreaseSpeed, ["{0}神力回复", "{0} Might Recover"] },
        //{(int)EBGUAttrFloat.SpecialEnergyMax,new Desc{ "{0}","{0}" } },//用途不明
        {(int)EBGUAttrFloat.FabaoEnergyMax, ["{0}法宝能量上限", "{0} Max Vessel-Qi"] },
        {(int)EBGUAttrFloat.VigorEnergyMax, ["{0}精魂能量上限", "{0} Max Vigor-Qi"] },
        //{(int)EBGUAttrFloat.BlockCollapseArmorMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.FreezeAbnormalAccMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.BurnAbnormalAccMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.PoisonAbnormalAccMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.ThunderAbnormalAccMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.BlindSlotMax,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.BloodBottomNumMax, ["{0}葫芦存储上限", "{0} Max Hulu Num"] },
        {(int)EBGUAttrFloat.PelevelMax, ["{0}棍势等级上限", "{0} Max FocusLevel"] },
        //{(int)EBGUAttrFloat.ShieldMax,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.PevalueMax, ["{0}棍势上限", "{0} Max Focus"] },
        //{(int)EBGUAttrFloat.YinAbnormalAccMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAbnormalAccMax,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.HpMaxMul, ["{0}%生命上限", "{0}% MaxHP"] },
        {(int)EBGUAttrFloat.MpMaxMul, ["{0}%法力上限", "{0}% MaxMP"] },
        {(int)EBGUAttrFloat.AtkMul, ["{0}%攻击", "{0}% ATK"] },
        {(int)EBGUAttrFloat.DefMul, ["{0}%防御", "{0}% DEF"] },
        //{(int)EBGUAttrFloat.B1StunMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.StaminaDepletedLimitMul,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.StaminaMaxMul, ["{0}%气力上限", "{0}% MaxStamina"] },
        {(int)EBGUAttrFloat.StaminaRecoverMul, ["{0}%气力回复", "{0}% Stamina Recover"] },
        //{(int)EBGUAttrFloat.KptturnSpeedMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.FreezeAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.BurnAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.PoisonAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.ThunderAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YinAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAbnormalAccMaxMul,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.StaminaCostMultiperMul, ["{0}% 气力消耗倍率加成", "{0}% Additional Stamina Cost Multiper"] },
        {(int)EBGUAttrFloat.TransEnergyMaxMul, ["{0}%神力上限", "{0}% Max Might"] },
        //{(int)EBGUAttrFloat.EnergyMinConsumeMul,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.EnergyConsumeSpeedMul, ["{0}%神力消耗速度", "{0}% Might Consume Speed"] },
        {(int)EBGUAttrFloat.EnergyIncreaseSpeedMul, ["{0}%神力回复", "{0}% Might Recover"] },
        {(int)EBGUAttrFloat.HpMaxBase, ["{0}生命上限", "{0} MaxHP"] },
        {(int)EBGUAttrFloat.MpMaxBase, ["{0}法力上限", "{0} MaxMP"] },
        {(int)EBGUAttrFloat.AtkBase, ["{0}攻击", "{0} ATK"] },
        {(int)EBGUAttrFloat.DefBase, ["{0}防御", "{0} DEF"] },
        //{(int)EBGUAttrFloat.B1StunMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.StaminaDepletedLimitBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.StaminaMaxBase, ["{0}气力上限", "{0} MaxStamina"] },
        {(int)EBGUAttrFloat.StaminaRecoverBase, ["{0}气力回复", "{0} Stamina Recover"] },
        //{(int)EBGUAttrFloat.SkillSuperArmorMaxBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.CritRateBase, ["{0}%暴击率", "{0}% Crit Rate"] },
        {(int)EBGUAttrFloat.CritMultiplierBase, ["{0}%暴伤", "{0}% Crit Damage"] },
        {(int)EBGUAttrFloat.TenacityBase, ["{0}霸体", "{0} Tenacity"] },
        //{(int)EBGUAttrFloat.KptturnSpeedBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.EarPlugBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.CritRateDefBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.CritDmgMulDefBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.DmgAdditionBase, ["{0}%伤害加成", "{0}% Damage Bonus"] },
        {(int)EBGUAttrFloat.DmgDefBase, ["{0}%减伤", "{0}% Damage Reduction"] },
        //{(int)EBGUAttrFloat.BlockCollapseArmorMaxBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.DingshenDefAdditionBase, ["{0}定身抗性", "{0} Immobilize Resist"] },
        //{(int)EBGUAttrFloat.FreezeAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.BurnAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.PoisonAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.ThunderAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.FreezeAtkBase, ["{0}赋冰攻击", "{0} Chill ATK"] },
        {(int)EBGUAttrFloat.BurnAtkBase, ["{0}赋火攻击", "{0} Burn ATK"] },
        {(int)EBGUAttrFloat.PoisonAtkBase, ["{0}赋毒攻击", "{0} Poison ATK"] },
        {(int)EBGUAttrFloat.ThunderAtkBase, ["{0}赋雷攻击", "{0} Shock ATK"] },
        {(int)EBGUAttrFloat.FreezeDefBase, ["{0}冰抗", "{0} Chill Resist"] },
        {(int)EBGUAttrFloat.BurnDefBase, ["{0}火抗", "{0} Burn Resist"] },
        {(int)EBGUAttrFloat.PoisonDefBase, ["{0}毒抗", "{0} Poison Resist"] },
        {(int)EBGUAttrFloat.ThunderDefBase, ["{0}雷抗", "{0} Shock Resist"] },
        {(int)EBGUAttrFloat.BloodBottomNumMaxBase, ["{0}葫芦存储上限", "{0} Max Hulu Num"] },
        {(int)EBGUAttrFloat.PelevelMaxBase, ["{0}棍势等级上限", "{0} Max Focus Level"] },
        //{(int)EBGUAttrFloat.ShieldMaxBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.PevalueMaxBase, ["{0}棍势上限", "{0} Max Focus"] },
        //{(int)EBGUAttrFloat.YinAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAbnormalAccMaxBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YinAtkBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAtkBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YinDefBase,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangDefBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.StaminaCostMultiperBase, ["{0}%气力消耗倍率", "{0}% Stamina Cost MultiperBase"] },
        {(int)EBGUAttrFloat.TransEnergyMaxBase, ["{0}神力上限", "{0} Max Might"] },
        //{(int)EBGUAttrFloat.EnergyMinConsumeBase,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.EnergyConsumeSpeedBase, ["{0}神力消耗速度", "{0} Might Consueme Speed"] },
        {(int)EBGUAttrFloat.EnergyIncreaseSpeedBase, ["{0}神力回复", "{0} Might Recover"] },
        {(int)EBGUAttrFloat.CommDropAddition, ["{0}%掉落加成", "{0}% Drop Rate"] },
        {(int)EBGUAttrFloat.ExpDropAddition, ["{0}%道行(经验)", "{0}% Exp"] },
        {(int)EBGUAttrFloat.SpiritDropAddition, ["{0}%灵蕴", "{0}% Money"] },

        {(int)EBGUAttrFloat.Hp, ["{0}生命", "{0}HP"] },
        {(int)EBGUAttrFloat.Mp, ["{0}法力", "{0}MP"] },
        //{(int)EBGUAttrFloat.Atk,new Desc{ "{0}攻击","{0}ATK" } },
        //{(int)EBGUAttrFloat.Def,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.B1Stun,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.Stamina, ["{0}气力", "{0}Stamina"] },
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
        {(int)EBGUAttrFloat.CurEnergy, ["{0}神力", "{0} Might"] },
        //{(int)EBGUAttrFloat.SpecialEnergy,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.Shield,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.Pevalue, ["{0}棍势", "{0} Focus"] },
        //{(int)EBGUAttrFloat.YinAbnormalAcc,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAbnormalAcc,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YinAtk,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangAtk,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YinDef,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.YangDef,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.StaminaCostMultiper,new Desc{ "{0}","{0}" } },
        {(int)EBGUAttrFloat.FabaoEnergy, ["{0}法宝能量", "{0} Vessel-Qi"] },
        {(int)EBGUAttrFloat.VigorEnergy, ["{0}精魂能量", "{0} Vigor-Qi"] },
        //{(int)EBGUAttrFloat.AttrFloatMax,new Desc{ "{0}","{0}" } },
        //{(int)EBGUAttrFloat.EnumMax,new Desc{ "{0}","{0}" } },
    };
    public static readonly ConstDescDict ValOpDictConst = new ConstDescDict()
    {
        {(int)EValOp.Mul, ["{0}%", "{0}%"] },
        {(int)EValOp.Add, ["{0}", "{0}"] },
        {(int)EValOp.OverrideBase, ["变为{0}", "Set to {0}"] },
    };
    public static readonly ConstDescDict BuffAndSkillEffectTypeDictConst = new ConstDescDict() {
        {(int)EBuffAndSkillEffectType.AddAttr, ["{0}", "{0}"] },
        {(int)EBuffAndSkillEffectType.RecoverAttr, ["回复{0}", "Recover {0}"] },
        {(int)EBuffAndSkillEffectType.ChangeMoveSpeed, ["{0}{1}速度", "{0}{1}Speed"] },
        {(int)EBuffAndSkillEffectType.ActiveExtLifeSavingHair, ["{0}秒", "{0}s"] },
    };
    public static readonly ConstDescDict BuffEffectTriggerTypeDictConst = new ConstDescDict() {
        {(int)EBuffEffectTriggerType.BeAttacked, ["{0} per Hit", "{0} per Hit"] },
        {(int)EBuffEffectTriggerType.OnSkillDamage, ["{0} per Hit", "{0} per Hit"] },
        {(int)EBuffEffectTriggerType.Time, ["<Interval>每{1}秒</Interval>{0}", "{0} <Interval>per {1} sec</Interval>"]
        },
        //generation是一次性获得，不需要时间
        {(int)EBuffEffectTriggerType.Generation, ["{0}", "{0}"] },
    };
    public static readonly ConstDescDict EAttrCostTypeDictConst = new ConstDescDict() {
        {(int)EAttrCostType.VigorEnergy,
            [
                "{0}元气<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Qi <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.Hp,
            [
                "{0}生命<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} HP <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.Mp,
            [
                "{0}法力<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} MP <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.Stamina,
            [
                "{0}气力<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Stamina <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.FabaoEnergy,
            [
                "{0}法宝能量<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Vessel-Qi <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.TransEnergy,
            [
                "{0}神力<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Might <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.PotentialEnergy,
            [
                "{0}棍势<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Focus <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
        {(int)EAttrCostType.BloodBottleNum,
            [
                "{0}酒<Positive>获得</Positive><Negative>消耗</Negative>",
                "{0} Hulu <Positive>Gain</Positive><Negative>Cost</Negative>"
            ]
        },
    };
    public static readonly Desc ThenConnection = [",再", ".Then "];
    public static readonly Desc MotionValueFormat =
        ["造成<HitCount>{2}次</HitCount>x{0}{1}伤害", "Deal x{0}{1} Damage<HitCount> {2} times</HitCount>"];
    public static readonly Desc DurationFormat =
        ["<Duration>{1}秒内</Duration>{0}", "{0}<Duration> for {1}s</Duration>"];
    public static readonly Desc GreaterFormat =
        ["<Target>目标</Target>{0}高于{1}时{2}", "{2} when<Target> target</Target> {0} over {1}"];
    public static readonly Desc LesserFormat =
        ["<Target>目标</Target>{0}不高于{1}时{2}", "{2} when<Target> target</Target> {0} under {1}"];
    public static readonly Desc FullFormat =
        ["<Target>目标</Target>满{0}时{1}", "{1} when<Target> target</Target> full {0}"];

    public static readonly ConstDescDict SpeedNameDictConst = new ConstDescDict() {
        {0b1, ["徐行", "Walk"] },
        {0b10, ["奔跑", "Run"] },
        {0b11, ["徐行和奔跑", "Walk and Run"] },
        {0b100, ["冲刺", "Run"] },
        {0b101, ["冲刺和徐行", "Walk and Sprint"] },
        {0b110, ["奔跑和冲刺", "Run and Sprint"] },
        {0b111, ["移动", "Move"] },
    };
}
