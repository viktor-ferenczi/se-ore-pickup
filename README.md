# Ore Pickup

Plugin for the game Space Engineers.

Picks up ore extracted by the hand drill.

- Works both in creative and survival
- Works both in offline and online multiplayer worlds
- Picks up ore only while collecting it (left mouse button)
- Separately configurable ice and stone collection

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Installation

- Install Plugin Loader's [Space Engineers Launcher](https://github.com/sepluginloader/SpaceEngineersLauncher)
- Add the "Ore Pickup" plugin to your list of enabled plugins

## Configuration

Configure the ice and stone pickup via the plugin's settings.

You may also use chat commands:
```
/pickup help    Prints this help on usage
/pickup info    Prints the current settings
/pickup on      Enables the plugin
/pickup off     Disables the plugin
/pickup ice     Toggles picking up ice
/pickup stone   Toggles picking up stone
```

Shortcuts:
```
help    h
info    ?
on      1
off     0
ice     i
stone   s
```

## Troubleshooting

### Conflicting mod

Do not use this plugin together with this mod, because it has similar functionality:

- [Automatic Ore Pickup](https://steamcommunity.com/sharedfiles/filedetails/?id=657749341)

The plugin disables itself if a conflicting mod is detected in the current world.