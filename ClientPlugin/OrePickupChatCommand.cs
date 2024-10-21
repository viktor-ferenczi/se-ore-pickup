using Sandbox.Game.GameSystems.Chat;
using Sandbox.ModAPI;
using VRage.Game.ModAPI;

namespace ClientPlugin
{
    // ReSharper disable once UnusedType.Global
    public class OrePickupChatCommand : IMyChatCommand
    {
        public string CommandText => "/pickup";
        public string HelpText => "Valid commands:\n/pickup on     Enables the plugin\n/pickup off    Disables the plugin\n/pickup ice    Toggles picking up ice\n/pickup stone  Toggles picking up stone";
        public string HelpSimpleText => "Ore Pickup plugin";
        public MyPromoteLevel VisibleTo => MyPromoteLevel.None;
        
        public void Handle(string[] args)
        {
            if (args == null)
                return;

            if (OrePickup.HasDetectedIncompatibleMod)
            {
                MyAPIGateway.Utilities.ShowMessage("Ore Pickup", $"The Ore Pickup plugin has been disabled in this world due to the presence of an incompatible mod: {OrePickup.IncompatibleModName}");
                return;
            } 
            
            switch (args.Length == 1 ? args[0] : "help")
            {
                case "h":
                case "help":
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", HelpText);
                    break;
                    
                case "?":
                case "info":
                    ShowStatus();
                    break;
                    
                case "on":
                case "1":
                    OrePickup.Config.Enabled = true;
                    ShowStatus();
                    break;

                case "off":
                case "0":
                    OrePickup.Config.Enabled = false;
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Ore pickup disabled");
                    break;

                case "i":
                case "ice":
                    OrePickup.Config.CollectIce = !OrePickup.Config.CollectIce;
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Ice pickup " + (OrePickup.Config.CollectIce ? "enabled" : "disabled"));
                    break;

                case "s":
                case "stone":
                    OrePickup.Config.CollectStone = !OrePickup.Config.CollectStone;
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Stone pickup " + (OrePickup.Config.CollectStone ? "enabled" : "disabled"));
                    break;

                default:
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Invalid command.");
                    MyAPIGateway.Utilities.ShowMessage("Ore Pickup", HelpText);
                    break;
            }
        }

        private static void ShowStatus()
        {
            MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Ore pickup: " + (OrePickup.Config.Enabled ? "enabled" : "disabled"));
            MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Ice pickup: " + (OrePickup.Config.CollectIce ? "enabled" : "disabled"));
            MyAPIGateway.Utilities.ShowMessage("Ore Pickup", "Stone pickup: " + (OrePickup.Config.CollectStone ? "enabled" : "disabled"));
        }
    }
}