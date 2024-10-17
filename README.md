# Ore Pickup

Plugin for the game Space Engineers.

Picks up ore extracted by the hand drill.

- Works both in creative and survival
- Works both in offline and online multiplayer worlds

## Prerequisites

- [Space Engineers](https://store.steampowered.com/app/244850/Space_Engineers/)
- [Plugin Loader](https://github.com/sepluginloader/PluginLoader/)

## Installation

- Install Plugin Loader's [Space Engineers Launcher](https://github.com/sepluginloader/SpaceEngineersLauncher)
- Add the "Simple Ore Pickup" plugin to your list of enabled plugins

## Configuration

Going out to the plugin configuration would be slow for the players,
therefore the runtime configuration is implemented via chat commands:

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

### Mod conflict

Do not use this plugin together with this mod:

- [Automatic Ore Pickup](https://steamcommunity.com/sharedfiles/filedetails/?id=657749341)

On loading or joining a game the plugin disables itself if any
conflicting mod is detected. Otherwise, the plugin enables itself.
