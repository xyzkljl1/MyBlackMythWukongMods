local ModName="[GameSpeedAdjust] "
--author:xyzkljl1
local config = require("gamespeedadjust-config")
local speed=config.defaultspeed
function SetParam(message)
	cm=FindFirstOf("CheatManager")
	if not cm:IsValid() then
		print(ModName.."Can't find CheatManager")
		return
	end
	cm:Slomo(speed)
	print(ModName.."Set Game Speed="..tostring(speed))
end
SetParam("Init on Load")

NotifyOnNewObject("/Script/Engine.CheatManager",function()
	SetParam("Init on CheatManager Created")
end)
RegisterKeyBind(Key.F1,function()
	if speed>=0.15 then
		speed=speed-0.1
		SetParam("Change Speed")
	end
end)
RegisterKeyBind(Key.F2,function()
	speed=speed+0.1
	SetParam("Change Speed")
end)