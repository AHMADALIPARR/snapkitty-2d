// SNAP-OS styled HUD: event counter, integrity bar, dialogue box.
// UnrealSharp C# — bind the UPROPERTYs to UMG widgets in the editor.

using UnrealSharp.Attributes;
using UnrealSharp.Engine;

namespace Snapkitty2D;

[UClass]
public class ASnapkittyHUD : AHUD
{
    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "HUD")]
    public int DisplayedSealed { get; set; }

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "HUD")]
    public int DisplayedTotal { get; set; } = 8;

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "HUD")]
    public float IntegrityPercent { get; set; } = 100.0f;

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "HUD")]
    public string DialogueText { get; set; } = "";

    [UProperty(PropertyFlags.BlueprintReadWrite, Category = "HUD")]
    public bool DialogueVisible { get; set; }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "HUD")]
    public void ShowDialogue(string text, float duration = 4.0f)
    {
        DialogueText = text;
        DialogueVisible = true;
        var timer = new FTimerHandle();
        var manager = GetWorldTimerManager();
        manager.SetTimer(ref timer, this, nameof(HideDialogue), duration, false);
    }

    [UFunction]
    public void HideDialogue()
    {
        DialogueVisible = false;
    }

    [UFunction(FunctionFlags.BlueprintCallable, Category = "HUD")]
    public void RefreshFromGameMode()
    {
        if (UGameplayStatics.GetGameMode(this) is ASnapkittyGameMode gm)
        {
            DisplayedSealed = gm.SealedEvents;
            DisplayedTotal = gm.EventsToWin;
            IntegrityPercent = 100.0f; // the chain never lies
        }
    }
}
