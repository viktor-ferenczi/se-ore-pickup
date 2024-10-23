using ClientPlugin.Settings;
using ClientPlugin.Settings.Layouts;
using HarmonyLib;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using System.Reflection;
using System.Text;
using VRage.Plugins;
using VRageMath;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin
    {
        public const string Name = "OrePickup";
        public static Plugin Instance { get; private set; }
        public static int frameCounter;
        private Generator ConfigGenerator;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;
            Instance.ConfigGenerator = new Generator();
            
            // TODO: Put your one time initialization code here.
            Harmony harmony = new Harmony(Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            MySession.OnLoading += OnSessionLoading;
            MySession.OnUnloading += OnSessionUnloading;
        }

        public void Dispose()
        {
            MySession.OnLoading -= OnSessionLoading;
            
            // TODO: Save state and close resources here, called when the game exits (not guaranteed!)
            // IMPORTANT: Do NOT call harmony.UnpatchAll() here! It may break other plugins.

            Instance = null;
        }

        private void OnSessionLoading()
        {
            OrePickup.OnSessionLoading();
        }

        private void OnSessionUnloading()
        {
            OrePickup.OnSessionUnloading();
        }

        public void Update()
        {
            if (frameCounter++ % 6 == 0)
                OrePickup.CollectOre();
        }

        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {
            if (OrePickup.HasDetectedIncompatibleMod)
            {
                var messageBox = MyGuiSandbox.CreateMessageBox(
                    MyMessageBoxStyleEnum.Info,
                    messageText: new StringBuilder($"The Ore Pickup plugin has been disabled in this world\ndue to the presence of an incompatible mod:\n\n{OrePickup.IncompatibleModName}"),
                    messageCaption: new StringBuilder("Ore Pickup"),
                    size: new Vector2(0.65f, 0.35f));
                MyGuiSandbox.AddScreen(messageBox);
                return;
            }

            Instance.ConfigGenerator.SetLayout<Simple>();
            MyGuiSandbox.AddScreen(Instance.ConfigGenerator.Dialog);
        }
    }
}