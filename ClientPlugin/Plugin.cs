using System;
using System.Reflection;
using HarmonyLib;
using Sandbox.Game.World;
using VRage.Plugins;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin
    {
        public const string Name = "OrePickup";
        public static Plugin Instance { get; private set; }
        public static int frameCounter;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            // TODO: Put your one time initialization code here.
            Harmony harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            MySession.OnLoading += OnSessionLoading;
        }

        public void Dispose()
        {
            MySession.OnLoading -= OnSessionLoading;
            
            // TODO: Save state and close resources here, called when the game exits (not guaranteed!)
            // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.

            Instance = null;
        }

        private static readonly ulong[] IncompatibleMods = {
            // Automatic Ore Pickup
            // https://steamcommunity.com/sharedfiles/filedetails/?id=657749341
            657749341UL,
        };

        private void OnSessionLoading()
        {
            RegisterChatCommand();
            EnableIfThereAreNoIncompatibleMods();
        }

        private static void RegisterChatCommand()
        {
            var chatCommands = MySession.Static.ChatSystem.CommandSystem.ChatCommands;
            var handler = new OrePickupChatCommand();
            if (!chatCommands.ContainsKey(handler.CommandText))
            {
                chatCommands.Add(handler.CommandText, handler);
            }
        }

        private static void EnableIfThereAreNoIncompatibleMods()
        {
            foreach (var mod in MySession.Static.Mods)
            {
                var workshopId = mod.GetWorkshopId().Id;
                if (IncompatibleMods.Contains(workshopId))
                {
                    OrePickup.Enabled = false;
                    return;
                }
            }

            OrePickup.Enabled = true;
        }

        public void Update()
        {
            if (frameCounter++ % 6 == 0)
                OrePickup.CollectOre();
        }

        // TODO: Uncomment and use this method to create a plugin configuration dialog
        // ReSharper disable once UnusedMember.Global
        /*public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyPluginConfigDialog());
        }*/

        //TODO: Uncomment and use this method to load asset files
        /*public void LoadAssets(string folder)
        {

        }*/
    }
}