# Unity Project Context

<!-- unity-onboarding:generated:start -->

## Project summary

- Repository root: the folder containing `Assets/`, `Packages/`, `ProjectSettings/`.
- Analyzed: 2026-10-30; base commit `6d8cb8c43350da64015f795d60561e85161d4e09`.
- Scope: repository inspection for portfolio documentation. No Unity assets changed.

## Environment and structure

| Area | Finding | Evidence |
| --- | --- | --- |
| Editor | Confirmed: Unity 6000.0.33f1, revision 433b0a79340b | `ProjectSettings/ProjectVersion.txt` |
| Rendering | URP 17.0.3 dependency; GraphicsSettings assigns a render-pipeline asset | `Packages/manifest.json`, `ProjectSettings/GraphicsSettings.asset` |
| Input | Confirmed: both backends enabled; legacy movement and Input System mouse | `ProjectSettings/ProjectSettings.asset` (`activeInputHandler: 2`), `PlayerMovement.cs`, `PlayerLook.cs` |
| UI | uGUI 2.0.0; TextMeshPro fields in game scripts | manifest, `GameClock.cs`, `PizzaManager.cs` |
| Other packages | AI Navigation 2.0.5, Timeline 1.8.7; no gameplay networking established | manifest and package lock |
| Game code | `Assets/Scripts/Player/`, `Assets/Scripts/SaveSystem.cs` | repository tree and sampled source |
| Scenes | MainMenu, Gameplay, Options, MenuGame enabled in that order | `ProjectSettings/EditorBuildSettings.asset` |
| Tests | Unity Test Framework 1.4.5; one clock rollover test | manifest, `Assets/Tests/EditMode/GameClockTests.cs` |
| Targets | Unknown: no release build or device validation performed | inspection scope |

## Assemblies and architecture

No `.asmdef` or `.asmref` files are committed. Scripts therefore use Unity's
default assembly handling; a separate EditMode test assembly is not configured.

Confirmed component-oriented MonoBehaviour design, with direct Inspector
references, `FindObjectOfType` lookups and a static
`DeliverySpawner.DeliveryZoneSpawned` event for the compass.
`SaveSystem` is a persistent singleton and uses PlayerPrefs plus reflection.

Sampled scripts use global classes, public Inspector fields, braces on separate
lines and a mixture of direct calls, Unity callbacks and coroutines.
These observations are descriptive, not a new coding standard.

## Startup flow

`MainMenu` is the first enabled build scene. `MainMenu.StartGame()` loads
`Gameplay`; options and back-menu methods load named scenes.
`MainMenuManager` additionally contains an in-scene camera transition.
Actual button wiring and scene reference correctness are unverified.

## Validation and tools

- Existing test: midnight/day rollover for `GameClock.Advance30Minutes()`.
- Test discovery is unverified because there is no test assembly definition.
- No Unity compilation, build, Play Mode or profiler validation was performed.
- No Unity MCP package was found in the manifest/lock; no connected Unity
  tool was available in this session.
- The exact recorded editor was unavailable locally; newer installed editors
  were not used to upgrade or import the project.
- No repository CI workflow was found.

## Constraints and unknowns

Preserve scene/prefab GUIDs, imported assets and the recorded Unity version.
Do not infer original art authorship from imported asset folders.
Inspector references, tags, layers and delivery-location arrays are required.
Input behaviour, save references after scene changes and full delivery flow
need editor testing. No multiplayer implementation was established from the
sampled gameplay code; a Multiplayer Center package alone is not evidence of it.

## Main sources inspected

`ProjectSettings/{ProjectVersion.txt,EditorBuildSettings.asset,GraphicsSettings.asset,ProjectSettings.asset}`,
`Packages/{manifest.json,packages-lock.json}`,
`Assets/Scripts/Player/{MainMenu,MainMenuManager,PlayerMovement,PlayerLook,PizzaManager,PizzaPickupHandler,DeliveryPointManager,DeliverySpawner,DeliveryCompass,GameClock}.cs`,
`Assets/Scripts/SaveSystem.cs`, `Assets/Tests/EditMode/GameClockTests.cs`,
repository tree and public commit history.

<!-- unity-onboarding:generated:end -->
