using System.Text.Json;
#nullable disable

namespace BullsAndCows.Model
{
    /// <summary>
    /// Класс конфигурации. Хранит глобальные константы
    /// </summary>
    public class Config
    {
        #region Реализация Синглтона
        private Config() { }
        static Config config;

        /// <summary>
        /// Получить уникальный объект конфигурации
        /// </summary>
        /// <returns>Объект, содержащий конфигурацию проекта</returns>
        public static Config GetInstance()
        {
            if (config == null)
                config = new Config();
            return config;
        }
        #endregion

        /// <summary>
        /// путь к базе данных sqlite
        /// </summary>
        public static string DbPath => Path.Combine(Properties.Resources.db_path);

        /// <summary>
        /// путь к файлу логов
        /// </summary>
        public static string LogPath => Path.Combine(Properties.Resources.log_path);
    }
}
