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
