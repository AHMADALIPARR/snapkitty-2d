# SNAPKITTY: Bifrost Runner — Unreal 5 + C# (UnrealSharp)

A 2D side-scrolling platformer. You are the hoodie kitten. Seal Bifrost
event cubes, take briefings from the Architect, keep ledger integrity
at 100%.

## Open it

1. Unreal Engine 5.5+ with the **UnrealSharp** and **Paper2D** plugins enabled.
2. Open `Snapkitty2D.uproject`.
3. Import the art from `../art/` as Paper2D sprites:
   - `kitten.png` → player flipbooks (idle / run / jump)
   - `suited-man.png` → Architect NPC sprite
   - `civilization.jpg` → parallax background layer
   - `snap-os.jpg` → title-screen / HUD reference
4. Create Blueprints subclassing the C# classes in `Source/Snapkitty2D/`:
   - `ASnapkittyPlayer` — 2D character controller (plane-locked X/Z,
     run + jump, flipbook animation states)
   - `AArchitectNPC` — dialogue NPC with talk radius + briefing lines
   - `ABifrostPickup` — bobbing/spinning collectible event cube
   - `ASnapkittyGameMode` — sealed-event counter, win at `EventsToWin`
   - `ASnapkittyHUD` — SNAP-OS styled HUD: event counter, integrity
     bar, timed dialogue box
5. Build a level (`L_BifrostRunner`): ground collision, 8 pickups,
   2–3 Architect checkpoints, player start.

## Controls (Enhanced Input)

- `MoveAction` (A/D or arrows) — run
- `JumpAction` (Space) — jump
- Interact (E) near the Architect — next briefing line

## Web prototype

`../web/index.html` is a playable browser prototype of the same game —
same mechanics, same art — so the design is proven before engine work.
