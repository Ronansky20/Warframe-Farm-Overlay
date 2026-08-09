# Warframe-Farm-Overlay

C# / .NET 10 solution. A Warframe farming overlay: given a target item, it
expands recipes to farmable parts and shows the best drop location for each.

## Projects

- `Core` — class library. Pure logic only: Planner, WarframeDataAdapter,
  ItemSearch. No I/O, no HTTP, no WPF. This purity is deliberate — do not
  break it.
- `Core.Tests` — xUnit, references Core.
- `ConsoleApp` — references Core. Does the HTTP fetching of WFCD JSON.
- `Overlay` — WPF, references Core. MainWindow (hub) and FarmOverlay
  (always-on-top panel).

## Conventions

- Records for models: `Ingredient`, `Recipe`, `DropLocation`.
- TDD: write the failing test first, then the implementation.
- Run tests with `dotnet test` in the terminal.

## Rules for agents

- Never edit the `x:Class` line in any `.xaml` file, and never create a `.cs`
  file that duplicates a XAML code-behind class.
- Never add HTTP, file I/O, or WPF references to `Core`.
- Report findings with file and line references; do not restructure code that
  wasn't part of the request.
- Ignore `bin/` and `obj/` entirely.
