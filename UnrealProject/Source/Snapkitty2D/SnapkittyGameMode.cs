// Game mode: tracks sealed Bifrost events and win condition.
// UnrealSharp C#.

using UnrealSharp.Attributes;
using UnrealSharp.Engine;

namespace Snapkitty2D;

[UClass]
public class ASnapkittyGameMode : AGameModeBase
{
    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Bifrost")]
    public int SealedEvents { get; private set; }

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Bifrost")]
    public int EventsToWin { get; set; } = 8;

    [UProperty(PropertyFlags.BlueprintReadOnly, Category = "Bifrost")]
    public bool ChainComplete { get; private set; }

    public void RegisterSealedEvent(int eventId)
    {
        SealedEvents++;
        NotifyEventSealed(SealedEvents);
    }

    // BlueprintImplementableEvent — wire the victory fanfare / UI in Blueprint.
    [UFunction(FunctionFlags.BlueprintImplementableEvent, Category = "Bifrost")]
    public void NotifyEventSealed(int totalSealed) { }

    [UFunction(FunctionFlags.BlueprintImplementableEvent, Category = "Bifrost")]
    public void NotifyChainComplete() { }

    public override void BeginPlay()
    {
        base.BeginPlay();
        SealedEvents = 0;
        ChainComplete = false;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        if (!ChainComplete && SealedEvents >= EventsToWin)
        {
            ChainComplete = true;
            NotifyChainComplete();
        }
    }
}
