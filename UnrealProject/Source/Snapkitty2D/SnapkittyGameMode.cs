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
