namespace ClientPlugin
{
    public class OrePickupConfig
    {
        public static readonly OrePickupConfig Default = new OrePickupConfig();
        
        [Checkbox("Enable plugin")]
        public bool Enabled { get; set; } = true;
        
        [Checkbox("Collect ice")]
        public bool CollectIce { get; set; } = true;
        
        [Checkbox("Collect stone")]
        public bool CollectStone { get; set; } = true;
    }
}