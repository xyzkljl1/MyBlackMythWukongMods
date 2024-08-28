local config = 
{
	freq=1.5, --refresh frequency(Hz),
--	fontsize=20,
--	color={1,1,1,1},
	show_hp=true,
	show_mp=true,
	show_st=true,
	show_fp=true,
	show_sp=true,
	show_tr=true,
	show_en=true,

	hp={870,1930}, --870,150 for HUD Adjust X default
	mp={870,1980}, --870,200 for HUD Adjust X default
	--mp={0,0}, --870,200 for HUD Adjust X default
	st={870,2030}, --870,250 for HUD Adjust X default
	fp={3450,1790},--focus point/棍势,1850,1640 for HUD Adjust X default
	tr={3100,1870}, --treasure/法宝,410,540 for HUD Adjust X default
	sp={2900,1870},--spirit/精魂,230,540 for HUD Adjust X default
	en={3550,1420},--energy/变身条,3500,1890 for HUD Adjust X default
}

return config