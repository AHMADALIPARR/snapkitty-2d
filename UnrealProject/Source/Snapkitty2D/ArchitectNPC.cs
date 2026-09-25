// The Architect — the suited-man NPC who briefs the kitten at checkpoints.
// UnrealSharp C#.

using UnrealSharp.Attributes;
using UnrealSharp.Engine;
using UnrealSharp.Paper2D;

namespace Snapkitty2D;

[UClass]
public class AArchitectNPC : AActor
{
    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Dialogue")]
    public string[] BriefingLines { get; set; } =
    [
        "Greetings, Architect. All systems nominal. Bifrost chain operational.",
        "Seal every event cube to keep ledger integrity at 100%.",
        "The chain remembers. Sovereignty is non-negotiable.",
    ];

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Dialogue")]
    public float TalkRadius { get; set; } = 220.0f;

    [UProperty(PropertyFlags.BlueprintReadWrite | PropertyFlags.EditAnywhere, Category = "Visual")]
    public UPaperFlipbook? IdleFlipbook { get; set; }

    private int lineIndex;
    private UPaperFlipbookComponent? sprite;

    public override void BeginPlay()
    {
        base.BeginPlay();
        sprite = GetComponentByClass<UPaperFlipbookComponent>();
        sprite?.SetFlipbook(IdleFlipbook);
    }

    // Called by the player when pressing Interact inside TalkRadius.
    [UFunction(FunctionFlags.BlueprintCallable, Category = "Dialogue")]
    public string GetNextLine()
    {
        if (BriefingLines.Length == 0) return "...";
        var line = BriefingLines[lineIndex % BriefingLines.Length];
        lineIndex++;
        return line;
    }

    [UFunction(FunctionFlags.BlueprintCallable | FunctionFlags.BlueprintPure, Category = "Dialogue")]
    public bool IsPlayerInRange(AActor player)
    {
        return GetDistanceTo(player) <= TalkRadius;
    }
}
