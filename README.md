# Ashfall Frontier (Vertical Slice)

Third-person action-RPG vertical slice inspired by **New World** (action combat + gathering/crafting), **WoW** (questing + dungeon roles), and **Diablo** (loot chase + affixes).

## Status
This repo currently contains spec + docs. Next step is to initialize the Unity project in this repo.

## Engine
- **Unity 6 LTS** (PC/Windows)
- Single-player through M3; co-op planned in M4+.

## How to run (once Unity project is initialized)
1. Install Unity Hub + Unity 6 LTS.
2. Open this folder as a Unity project.
3. Open scene: `Assets/_Project/World/Scenes/Boot.unity` (to be added).
4. Press Play.

## Controls (planned)
- Move: WASD
- Camera: Mouse
- Light attack: LMB
- Heavy attack: RMB
- Dodge: Space
- Block: Shift (or RMB hold depending on final mapping)
- Abilities: 1/2/3
- Interact: E
- Inventory: I
- Talents: K

## Docs
- `VerticalSliceSpec.md`
- `Roadmap.md`
- `TestingChecklist.md`

## Milestones
- **M1:** combat + zone spine + basic loot + inventory + save/load
- **M2:** crafting + quests + world events + full zone POIs + 4 enemy types
- **M3:** dungeon + boss + end chest
- **M4:** co-op + polish + build pipeline
