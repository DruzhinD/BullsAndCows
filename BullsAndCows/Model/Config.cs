using System.Text.Json;
#nullable disable

namespace BullsAndCows.Model
{
    /// <summary>
    /// Класс конфигурации
    /// </summary>
    public class Config
    {
        static Config config;
        public string DbPath { get => Path.Combine(Properties.Resources.db_path); }
        public string LogPath => Path.Combine(Properties.Resources.log_path);
        private Config()
        {
            using (FileStream file = new FileStream(configPath, FileMode.Open))
            {
                data = JsonSerializer.Deserialize<Dictionary<string, string>>(file);
            }
        }

        string configPath = @"data\\config.json";
        public Dictionary<string, string> data;

        public static Config GetInstance()
        {
            if (config == null)
                config = new Config();
            return config;

        }
    }
}
