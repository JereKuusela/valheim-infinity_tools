- v1.5
  - Adds a new command `tool_cmd` that's like `tool` but shows normal autocomplete.
  - Fixes the command `tool` being case sensitive.
  - Fixes the command `tool_export` not working.
  - Fixes the command `tool_import` not working.

- v1.4
  - Recompile for some Infnity Hammer changes.

- v1.3
  - Fixes some tools with multiple commands not working.

- v1.2
  - Adds new command parameters `<area>` and `<to>` for custom World Edit Commands tools.
  - Adds frame and ring shapes to hammer area selection.
  - Fixes height offset applying to tools twice.
  - Fixes piece highlightning not working for frame and ring shapes.

- v1.1
  - Fixes tools not initially working in some cases.
  - Fixes tool shape resetting to circle on each selection.
  - Fixes error when using the default remove tool.

- v1.0
  - Split from Infity Hammer mod.
  - Adds mandatory requirement of Server Devcommands and World Edit Commands mods.
  - Adds minimum radius of 0.25 to tools.
  - Adds new settings to disable specific tool shapes.
  - Adds new command `hammer_tool` to select tools.
  - Adds new command `hammer_export` and `hammer_import` to export and import tools.
  - Adds a new field `highlight` to tools (highlights affected pieces).
  - Adds a new field `snapGround` to tools (can be used to disable tool visual snapping to ground).
  - Adds a new field `instant` to tools (can be used to execute command directly from the menu).
  - Adds a new field `terrainGrid` to tools (precise terrain editing).
  - Adds a new field `tabIndex` to tools (to define the build menu tab).
  - Adds a new field `index` to tools (to define the position on build menu).
  - Adds offset support to tools.
  - Adds more space for tool descriptions.
  - Changes the default key binds for tools (scaling with just mouse wheel).
  - Changes the parameter format to be consistent with other mods (sorry!).
  - Removes commands `hammer_command`, `hoe_command`, `hammer_add`, `hoe_add`, `hammer_remove` and `hoe_remove`.
