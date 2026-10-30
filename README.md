# Delivery The Good

A Unity 3D pizza-delivery prototype. Pick up a pizza, find its delivery zone,
and deliver it before it gets cold. The project also explores player energy,
an in-game clock, menus and local saving.

## Stack

- **Unity 6000.0.33f1**, as recorded in `ProjectSettings/ProjectVersion.txt`.
- C#, MonoBehaviour components, CharacterController and 3D physics triggers.
- Universal Render Pipeline **17.0.3**; uGUI / TextMeshPro.
- Input System **1.11.2**, alongside legacy Input Manager calls. Project
  settings enable both input backends.
- PlayerPrefs for selected save data; Unity Test Framework **1.4.5**.

## Open and run

1. Clone or download the repository.
2. In Unity Hub, add the repository root (the folder containing `Assets`,
   `Packages` and `ProjectSettings`).
3. Open it with **6000.0.33f1** and allow packages and assets to import.
4. Open `Assets/Scenes/MainMenu.unity`, then enter Play Mode and start the game.
5. For a direct gameplay inspection, open `Assets/Scenes/Gameplay.unity`.

The committed build scene order is `MainMenu`, `Gameplay`, `Options`,
`MenuGame`. Use the committed editor version for the first run; opening in
another version can upgrade serialized project files.

### Controls visible in the scripts

| Input | Action |
| --- | --- |
| Horizontal / Vertical axes (normally WASD / arrows) | Move |
| Mouse | Look |
| Left Shift | Sprint |
| Jump button (normally Space) | Jump |
| Left Ctrl / C | Crouch handling |
| E while looking at a pizza within range | Pick up |

Delivery occurs through a `DeliveryZone` trigger. The C-key toggle shares logic
with the held-Control path, so its final behaviour should be checked in Play Mode.

## Systems to inspect

- `Assets/Scripts/Player/PizzaManager.cs`: delivery countdown, reward and pizza state.
- `DeliverySpawner.cs`: random destination selection and a delivery-zone event.
- `DeliveryCompass.cs`: UI arrow following the current destination.
- `PlayerMovement.cs` / `PlayerLook.cs`: character movement, energy and camera.
- `GameClock.cs`: 30-minute game-time ticks, day rollover and time reset.
- `Assets/Scripts/SaveSystem.cs`: money, energy and clock values in PlayerPrefs.
- `Assets/Scenes/`: menu, gameplay and options scenes.
- `Assets/Tests/EditMode/`: the existing clock rollover test.

## Development and attribution

The public [commit history](https://github.com/DrKrisz/Delivery-The-good/commits/main/)
records scene and gameplay updates under DrKrisz. It also includes Codex
branches for features and fixes, including the
[clock test](https://github.com/DrKrisz/Delivery-The-good/pull/2),
[delivery timer](https://github.com/DrKrisz/Delivery-The-good/pull/6),
[save system](https://github.com/DrKrisz/Delivery-The-good/pull/7) and
[compass](https://github.com/DrKrisz/Delivery-The-good/pull/8).
These are part of an AI-assisted development workflow; the history alone does
not establish which lines were written manually.

The repository also includes imported city/vehicle assets and TextMeshPro
examples. Their presence is not a claim of original art authorship.

## Validation and current limits

Open Unity's Test Runner, choose EditMode, and check whether
`GameClockTests.Advance30MinutesTwice_UpdatesHourAndDay` is discovered. It checks
crossing midnight from Monday 23:30 to Tuesday 00:30.

There is no committed test assembly definition, so discovery/compilation of
this test still needs checking in the recorded editor version. This
documentation update was checked against repository source; it does not
claim a successful Unity build or Play Mode run.

Before showing the prototype, check menu-to-gameplay navigation, pizza pickup,
successful and expired deliveries, and saving/reloading.

Known implementation constraints:

- Inspector references, tags, layers and non-empty delivery-location arrays are required.
- Saving uses PlayerPrefs and reflection into component fields; it stores only
  selected values, not a complete world snapshot.
- Scene reloads and persistent `SaveSystem` references need gameplay testing.
- This is a prototype, with no verified release build or platform compatibility matrix.

See [project context](Docs/AI/UnityProjectContext.md) for the source-based environment notes.
