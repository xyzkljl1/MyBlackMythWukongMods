
using b1;
using UnrealEngine.Engine;
using UnrealEngine.Runtime;
using GSE.GSSdk;
using GSE;
using b1.Protobuf.DataAPI;
using ResB1;
using b1.Protobuf.GSProtobufRuntimeAPI;
using EffectDetailDescription;
#nullable enable
namespace EffectDetailDescription
{
    public static class MyUtils
    {
        private static UWorld? world;

        public static UWorld? GetWorld()
        {
            if (world == null)
            {
                UObjectRef uobjectRef = GCHelper.FindRef(FGlobals.GWorld);
                world = uobjectRef?.Managed as UWorld;
            }
            return world;
        }

        public static APawn GetControlledPawn()
        {
            return UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld()).GetControlledPawn();
        }

        public static BGUPlayerCharacterCS GetBGUPlayerCharacterCS()
        {
            return (GetControlledPawn() as BGUPlayerCharacterCS)!;
        }

        public static BGP_PlayerControllerB1 GetPlayerController()
        {
            return (BGP_PlayerControllerB1)UGSE_EngineFuncLib.GetFirstLocalPlayerController(GetWorld());
        }

        public static BUS_GSEventCollection GetBUS_GSEventCollection()
        {
            return BUS_EventCollectionCS.Get(GetControlledPawn());
        }

    }
}
