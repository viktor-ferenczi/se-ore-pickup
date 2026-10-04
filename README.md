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

You can open the plugin settings by pressing `Ctrl-Alt-/` in-game.
(It does not work in menus.) 

You may also use chat commands, should that be quicker to type for you:
```
/pickup help    Prints this help on usage
/pickup info    Prints the current settings
/pickup on      Enables the plugin
/pickup off     Disables the plugin
/pickup ice     Toggles picking up ice
/pickup stone   Toggles picking up stone
```

Shortcuts for the above parameters:
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

## Development

Load the working copy through a Pulsar development folder: start Pulsar with `-sources`,
then add this repository with the Sources button. Building `OrePickup.sln` deploys the
plugin into Pulsar's `Local` folder only if `Pulsar` is set in `Directory.Build.props.user`
or passed as `-p:Pulsar=...`. If the build cannot find the game, run `setup.py` to write its
folder into `Directory.Build.props.user`.
