local ModName="[LargeDamageNumber] "
--author:xyzkljl1
local config = require("largedamagenumber-config")
RegisterHook("/Script/Engine.PlayerController:ClientRestart", function(Context)
	SetParam("Init on ClientRestart")
end)

function SetParam(message)
	print(ModName..message)

	conf=StaticFindObject("/Game/00Main/Design/UIConfig/DA_DamageNumConfig.DA_DamageNumConfig")
	if not conf:IsValid() then
		print(ModName.."Game not ready,Abort")
		return
	end
	print(tostring(config.min))
	print(tostring(config.max))
	print(tostring(config.directionx))
	print(tostring(config.directiony))
	print(tostring(config.directionrandom))
	conf["DefaultDir"]["X"]=config.directionx
	conf["DefaultDir"]["Y"]=config.directiony
	conf["DirRandomParam"]=directionrandom
	conf["AmplitudeMin"]=config.min
	conf["AmplitudeMax"]=config.max
	print(ModName.."Init Done")
end
SetParam("Init on Load")