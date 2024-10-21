using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace ClientPlugin
{
    public class OrePickupConfig
    {
        public bool Enabled = true;
        public bool CollectIce = true;
        public bool CollectStone = true;
        
        private const string ConfigFileName = "OrePickup.ini";
        private static string ConfigFilePath => Path.Combine(MyFileSystem.UserDataPath, ConfigFileName);
        private static OrePickupConfig Default => new OrePickupConfig();

        public void Save()
        {
            var path = ConfigFilePath;
            using (var text = File.CreateText(path))
                new XmlSerializer(typeof(OrePickupConfig)).Serialize(text, this);
        }

        public static OrePickupConfig Load()
        {
            var path = ConfigFilePath;
            if (!File.Exists(path))
            {
                return Default;
            }

            var xmlSerializer = new XmlSerializer(typeof(OrePickupConfig));
            try
            {
                using (var streamReader = File.OpenText(path))
                    return (OrePickupConfig)xmlSerializer.Deserialize(streamReader) ?? Default;
            }
            catch (Exception)
            {
                MyLog.Default.Warning($"OrePickup: Failed to read config file: path");
            }
            
            return Default;
        }
    }
}