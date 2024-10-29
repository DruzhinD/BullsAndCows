using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace BullsAndCows
{
    /// <summary>
    /// Класс конфигурации
    /// </summary>
    public class Config
    {
        static Config config;
        private Config()
        {
            using (FileStream file = new FileStream(this.configPath, FileMode.Open))
            {
                data = JsonSerializer.Deserialize<Dictionary<string, string>>(file);
            }
        }

        string configPath = @"data\\config.json";
        public Dictionary<string, string>? data;

        public static Config GetInstance()
        {
            if (config == null)
                config = new Config();
            return config;

        }
    }
}
