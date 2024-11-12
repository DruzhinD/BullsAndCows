using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.Model
{
    /// <summary>
    /// Класс логгирования
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Записать лог с сообщением message
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            //добавляем время лога
            string time = DateTime.Now.ToString("dd.MM.y HH:mm:ss");
            if (message[message.Length-1] != '\n')
                message = time + message + '\n';
            else
                message = time + message;
            string logPath = Config.GetInstance().LogPath;
            File.AppendAllText(logPath, message);
        }
    }
}
