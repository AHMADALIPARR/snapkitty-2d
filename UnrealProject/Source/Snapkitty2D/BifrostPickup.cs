// Bifrost event cube — the collectible. Sealing one = +1 ledger event.
// UnrealSharp C#.

using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.Components;

namespace Snapkitty2D;

[UClass]
public class ABifrostPickup : AActor
{
    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Bifrost")]
    public int EventId { get; set; } = 1243;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Bifrost")]
    public float BobAmplitude { get; set; } = 12.0f;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Bifrost")]
    public float BobSpeed { get; set; } = 2.5f;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Bifrost")]
    public float SpinSpeed { get; set; } = 90.0f;

    private FVector baseLocation;
    private float age;
    private bool sealed_;

    public ABifrostPickup()
    {
        PrimaryActorTick.bCanEverTick = true;
    }

    public override void BeginPlay()
    {
        base.BeginPlay();
        baseLocation = K2_GetActorLocation();
        var box = GetComponentByClass<UBoxComponent>();
        if (box is not null)
            box.OnComponentBeginOverlap += OnOverlap;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        if (sealed_) return;
        age += deltaTime;
        var loc = baseLocation;
        loc.Z += (float)Math.Sin(age * BobSpeed) * BobAmplitude;
        K2_SetActorLocation(loc, false);
        var rot = K2_GetActorRotation();
        rot.Yaw += SpinSpeed * deltaTime;
        K2_SetActorRotation(rot, false);
    }

    private void OnOverlap(UPrimitiveComponent overlappedComponent, AActor otherActor,
        UPrimitiveComponent otherComp, int otherBodyIndex, bool fromSweep, FHitResult sweepResult)
    {
        if (sealed_) return;
        if (otherActor is ASnapkittyPlayer player)
        {
            sealed_ = true;
            var gm = UGameplayStatics.GetGameMode(this) as ASnapkittyGameMode;
            gm?.RegisterSealedEvent(EventId);
            player.OnEventSealed(gm?.SealedEvents ?? 0);
            K2_DestroyActor(this);
        }
    }
}
