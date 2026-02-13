# Tomorrow Checklist (Ryan)

This is the minimal set of steps needed on your PC Unity editor to keep the vertical slice moving while Jarvis pushes code changes.

## A) Daily update loop (5 minutes)
1) Pull latest code:
```powershell
cd "$HOME\AshFallFrontier-repo"
git pull
```
2) Open Unity project.
3) Open `Assets/Scenes/SampleScene.unity`.
4) Press Play and sanity-check:
- WASD movement + mouse orbit
- Space dodge drains stamina
- Shift block
- LMB/RMB attacks
- Enemy melee grunt chases + telegraphs + damages player
- HUD bars update

If anything breaks, screenshot Console + Inspector for the object involved.

## B) One-time scene wiring (until we automate more)
### 1) Ensure scene has ground + NavMesh
- Ground plane exists and is Static.
- `NavMeshSurface` component on Ground.
- Bake NavMesh (see AI Navigation workflow).

### 2) Ensure Player is tagged
- Player GameObject Tag = `Player`

### 3) Ensure Player components & refs
Player should have:
- CharacterController
- PlayerMotor (cam ref set)
- Health
- Combatant (health ref set)
- PlayerCombat (combatant/controller/hitbox refs set)
- Child: `MeleeHitbox` with `MeleeHitbox` component (hit mask includes Enemy)

Main Camera:
- ThirdPersonCamera (target=Player)

UIRoot:
- HudAutoBuilder (player assigned to Player Combatant)

### 4) Enemy setup
MeleeGrunt capsule:
- NavMeshAgent
- Health
- Combatant
- EnemyMeleeGrunt

## C) When loot pickup lands (upcoming)
You’ll be asked to:
- Add a `LootPickup` prefab to the scene (or confirm runtime spawn works)
- Confirm prompt shows "Press E to pick up"
- Confirm inventory count increases

## D) When quests/crafting land (later)
You’ll be asked to:
- Place a few Quest NPC markers
- Place 3 gather nodes
- Place crafting station trigger

## E) Reporting format (copy/paste)
- Pulled commit: <hash>
- Playtest result: PASS/FAIL
- Issue:
  - What I expected:
  - What happened:
  - Console errors (first 5 lines):
  - Screenshot(s):
