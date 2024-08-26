#pragma once
#include "CoreMinimal.h"
//CROSS-MODULE INCLUDE V2: -ModuleName=CoreUObject -ObjectName=SoftClassPath -FallbackName=SoftClassPath
//CROSS-MODULE INCLUDE V2: -ModuleName=CoreUObject -ObjectName=SoftObjectPath -FallbackName=SoftObjectPath
//CROSS-MODULE INCLUDE V2: -ModuleName=Engine -ObjectName=BlueprintFunctionLibrary -FallbackName=BlueprintFunctionLibrary
#include "BGWAssetLoader.generated.h"

class UBGWAssetLoaderRequest;
class UObject;

UCLASS(Blueprintable)
class B1_API UBGWAssetLoader : public UBlueprintFunctionLibrary {
    GENERATED_BODY()
public:
    UBGWAssetLoader();
    UFUNCTION(BlueprintCallable)
    static void AsyncLoadClass(const FSoftClassPath& LoadPath, UBGWAssetLoaderRequest* request, int32 Priority){}

};

