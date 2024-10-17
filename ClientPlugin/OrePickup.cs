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

namespace ClientPlugin
{
    public static class OrePickup
    {
        private const float CollectionRange = 2.5f;

        public static bool Enabled = true;
        public static bool CollectIce = true;
        public static bool CollectStone = true;

        public static void CollectOre()
        {
            if (!Enabled || MySession.Static == null || MySession.Static.IsUnloading)
                return;

            var character = MySession.Static.LocalCharacter;
            if (character.Closed || !(character.CurrentWeapon is MyHandDrill drill) ||
                drill.Closed || !drill.IsShooting || drill.HasInventory)
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
                        if (!CollectIce)
                            continue;
                        break;

                    case "Stone":
                        if (!CollectStone)
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