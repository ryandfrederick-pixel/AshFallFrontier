# Testing Checklist (Vertical Slice)

## Combat
- [ ] WASD movement + camera feel stable.
- [ ] Light/heavy attack triggers reliably; no stuck states.
- [ ] Dodge consumes stamina and grants brief i-frames.
- [ ] Block reduces damage and drains stamina.
- [ ] Ability cooldowns + costs work; UI displays remaining CD.
- [ ] Enemy telegraphs show before damage window.
- [ ] Hit reactions/stagger occur; enemies don’t get double-hit incorrectly.

## Loot + Inventory
- [ ] Loot drops spawn and can be picked up.
- [ ] Rarity distribution matches expected weights.
- [ ] Affixes roll and apply to derived stats.
- [ ] Equip/unequip updates stats + visuals.
- [ ] Tooltip comparisons are correct.

## Quests
- [ ] Talk/kill/collect objectives increment correctly.
- [ ] Quest tracker updates instantly.
- [ ] Completing dungeon objective completes quest.
- [ ] Quest state persists after save/load.

## Crafting
- [ ] Gathering nodes can be harvested and respawn.
- [ ] Refining consumes raw resources and produces refined mats.
- [ ] Crafting consumes mats and produces items.

## World events
- [ ] Defend event: waves spawn; success/failure resolves; rewards granted.
- [ ] Escort event: escort follows path; enemies spawn; success/failure resolves.

## Persistence
- [ ] Save/load restores: level, XP, talents, inventory, equipment, quests.
- [ ] Save schema version handles basic migration.
