using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Weapons;
using VRage.Game;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using ClientPlugin.Settings;

namespace ClientPlugin
{
    public static class OrePickup
    {
        private const float CollectionRange = 2.5f;

        public static bool HasDetectedIncompatibleMod;
        public static string IncompatibleModName;

        public static void OnSessionLoading()
        {
            CheckIncompatibleMods();
        }

        public static void OnSessionUnloading()
        {
            HasDetectedIncompatibleMod = false;
        }

        private static readonly Dictionary<ulong, string> IncompatibleMods = new Dictionary<ulong, string>
        {
            // Automatic Ore Pickup
            // https://steamcommunity.com/sharedfiles/filedetails/?id=657749341
            { 657749341UL, "Automatic Ore Pickup" },
        };

        private static void CheckIncompatibleMods()
        {
            foreach (var mod in MySession.Static.Mods)
            {
                var workshopId = mod.GetWorkshopId().Id;
                if (IncompatibleMods.TryGetValue(workshopId, out var modName))
                {
                    HasDetectedIncompatibleMod = true;
                    IncompatibleModName = modName;
                    return;
                }
            }

            HasDetectedIncompatibleMod = false;
            IncompatibleModName = null;
        }

        public static void CollectOre()
        {
            if (!Config.Current.Enabled ||
                HasDetectedIncompatibleMod ||
                MySession.Static == null ||
                MySession.Static.IsUnloading)
                return;

            var character = MySession.Static.LocalCharacter;
            if (character == null || character.Closed ||
                !(character.CurrentWeapon is MyHandDrill drill) ||
                drill.Closed || drill.HasInventory ||
                !drill.IsShooting || !drill.CollectingOre)
                return;

            IMyHandDrill iDrill = drill;
            var sphere = iDrill.WorldVolume;
            sphere.Radius = CollectionRange;

            var oresInRange = new HashSet<IMyEntity>(16);
            MyAPIGateway.Entities.GetEntities(oresInRange, (x) => x is IMyFloatingObject && sphere.Contains(x.GetPosition()) != ContainmentType.Disjoint);

            var oresToCollect = new List<MyFloatingObject>(oresInRange.Count);
            foreach (var ore in oresInRange)
            {
                var floatingObject = (MyFloatingObject)ore;
                if (floatingObject.Item.Content.TypeId != typeof(MyObjectBuilder_Ore))
                    continue;

                var subtypeName = floatingObject.Item.Content.SubtypeName;
                switch (subtypeName)
                {
                    case "Ice":
                        if (!Config.Current.CollectIce)
                            continue;
                        break;

                    case "Stone":
                        if (!Config.Current.CollectStone)
                            continue;
                        break;
                }

                oresToCollect.Add(floatingObject);
            }

            foreach (var floatingObject in oresToCollect)
            {
                // PickUp(character, floatingObject);
                IMyUseObject useObject = floatingObject;
                useObject.Use(UseActionEnum.PickUp, character);
            }
        }
    }
}