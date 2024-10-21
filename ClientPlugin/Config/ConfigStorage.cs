using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace ClientPlugin
{
    public static class ConfigStorage
    {
        private const string ConfigFileName = "OrePickup.ini";
        private static string ConfigFilePath => Path.Combine(MyFileSystem.UserDataPath, ConfigFileName);

        public static void Save(OrePickupConfig config)
        {
            var path = ConfigFilePath;
            using (var text = File.CreateText(path))
                new XmlSerializer(typeof(OrePickupConfig)).Serialize(text, config);
        }

        public static OrePickupConfig Load()
        {
            var path = ConfigFilePath;
            if (!File.Exists(path))
            {
                return OrePickupConfig.Default;
            }

            var xmlSerializer = new XmlSerializer(typeof(OrePickupConfig));
            try
            {
                using (var streamReader = File.OpenText(path))
                    return (OrePickupConfig)xmlSerializer.Deserialize(streamReader) ?? OrePickupConfig.Default;
            }
            catch (Exception)
            {
                MyLog.Default.Warning($"OrePickup: Failed to read config file: path");
            }
            
            return OrePickupConfig.Default;
        }
        
    }
}