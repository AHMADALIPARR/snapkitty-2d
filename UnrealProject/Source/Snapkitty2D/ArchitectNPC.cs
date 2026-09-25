// SPDX-License-Identifier: AGPL-3.0-or-later
//
// SNAPKITTY: Bifrost Runner
// Copyright (C) 2026 SnapKitty Collective
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

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
