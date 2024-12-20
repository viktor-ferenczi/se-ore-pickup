using ClientPlugin.Settings.Elements;

namespace ClientPlugin.Settings
{
    public class Config
    {
        public static readonly Config Default = new Config();
        public static readonly Config Current = Storage.Load();

        // Build your UI
        public readonly string Title = "Config";

        [Checkbox("Enable plugin")]
        public bool Enabled { get; set; } = true;

        [Checkbox("Collect ice")]
        public bool CollectIce { get; set; } = true;

        [Checkbox("Collect stone")]
        public bool CollectStone { get; set; } = true;
    }
}