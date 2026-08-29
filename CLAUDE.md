# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```powershell
dotnet build SquareExpedition.sln                      # build everything
dotnet run --project SquareExpedition.Client           # launch the game (WinExe, MonoGame DesktopGL)
dotnet run --project SquareExpedition.Server           # stub console app
dotnet test SquareExpedition.Tests                     # all tests (NUnit)
dotnet test SquareExpedition.Tests --filter FullyQualifiedName~TerrainGeneratorServiceTests   # one class
dotnet test SquareExpedition.Tests --filter Name=GenerateTerrain                              # one test
```

Content pipeline: the Client csproj runs `dotnet tool restore` before Restore, but if the MGCB
step fails with `MSB3073 ... "dotnet" "mgcb" ... exited with code 1`, run
`dotnet tool restore` manually from `SquareExpedition.Client/` and rebuild. Content assets live in
`SquareExpedition.Client/Content/Content.mgcb` (currently empty); edit it with `dotnet mgcb-editor`.

MSBuild/test output on this machine is localized (Polish) — `Kompilacja powiodła się` = build
succeeded, `Liczba błędów` = error count, `Powodzenie!` = tests passed.

## Architecture

.NET 9, MonoGame 3.8.3 (DesktopGL). Project dependency direction:

```
Client ──► Application ──► Data ──► Common
Server ──► Application ──► ML (Microsoft.ML 5.0 preview, currently empty)
Tests   (standalone — references no production project yet)
```

`Common` and `ML` deliberately use flat root namespaces (`Common`, `ML`), not
`SquareExpedition.*` — the `RootNamespace` override is in their csproj.

### World generation pipeline

`GameCore.Initialize` → `WorldGeneratorService.GenerateNewWorld` → `AreaGeneratorService` →
`TerrainGeneratorService`. The split matters:

- **`AreaGeneratorService`** creates the *empty spatial grid*: `TerrainSize` is a total block
  count (Small = 10 000), `dimension = sqrt(size)`, and it emits a `Localization` for every
  `(x, y, z)` with `y` in `0..9` — so 10 vertical layers, ~1M slots for Small. Coordinates are
  centred by subtracting `dimension / 2` from x and z.
- **`TerrainGeneratorService`** then *fills* only the `y == 0` layer, attaching a `Block` to each
  `Localization` via `SetGameObject`.

`Localization` is the core abstraction: an immutable centre point plus at most one `GameObject`.
`SetGameObject` silently refuses (returns null, logs to console) when the slot is occupied or the
resident object is not editable — it does not throw, so callers must check the return value.

### Rendering model

`GameObject` extends MonoGame's `DrawableGameComponent` and uses a template-method pattern: setting
the `Localization` property triggers `UpdateLocalization()` (recompute the 8 `Corners`) then
`UpdateVertexPositionColor()` (build the `VertexBuffer`). `Block` overrides both to emit a cube as
6 four-vertex `TriangleStrip` runs, drawn as `DrawPrimitives(TriangleStrip, 4 * i, 2)`.

Consequences to keep in mind:

- Vertex buffer construction needs a live `GraphicsDevice`, so world generation must happen **after**
  `base.Initialize()` in `GameCore` — that is why the generation call sits at the end of `Initialize`.
- Objects render because `GameCore.Initialize` adds them to `Game.Components`; MonoGame drives their
  `Draw`, and each object re-applies its own copies of the projection/view/world matrices onto the
  shared `BasicEffect`. Those matrices are captured **by value** at construction, so camera
  movement updates `GameCore._viewMatrix` but not the already-constructed components.

### Services and coordinates

- `CameraService` and `ControllerService` are double-checked-lock singletons via
  `GetInstance(dependency)`. The dependency passed on the **first** call wins; later arguments are
  ignored. `Camera` itself is a plain nullable-Vector3 holder in `Data`.
- `ControllerService.HandleInput` maps WASD to camera translation (A/D are inverted relative to
  the sign of the delta) and Escape/GamePad-Back to exit, then rebuilds the view matrix by `ref`.
- Two `Vector3` types are in play: `Microsoft.Xna.Framework.Vector3` (used by `Localization`,
  `GameObject`, `Block`) and `System.Numerics.Vector3` (aliased at the top of `Area.cs` and
  `TerrainGeneratorService.cs`). MonoGame 3.8's implicit conversions make this compile — be
  explicit about which one you mean when adding code that crosses that boundary.
- `Area.GetLocalizationUsingCords` is a `SingleOrDefault` linear scan over the whole
  `Localizations` collection, called once per block during terrain generation. This is the
  dominant cost of world generation; replace with a dictionary keyed by coordinate if it needs to scale.

### Known incomplete areas

`Abstract/IAreaGeneratorService` is empty and `ITerrainGeneratorService.GenerateNewTerrain` has a
different signature than the concrete `TerrainGeneratorService` method (which does not implement
the interface). `Server` is a hello-world console app; its README notes an intent to use MediatR.
`Interaction`, `Physic`, `Form`, `Character`, and `Staff` are empty abstract placeholders. The
`Tests` project has no reference to `Application`/`Data`, so its two tests are `Assert.Pass()`
stubs — add the `ProjectReference` before writing real tests.
