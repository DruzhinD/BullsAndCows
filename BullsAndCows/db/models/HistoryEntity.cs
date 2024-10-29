using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.db.models
{
    public class HistoryEntity
    {
        public int Id { get; set; }
        [Column("player_id")]
        public int Player_id { get; set; }
        public string Combination { get; set; }
        public int Attempts { get; set; }

        /// <summary>
        /// Сущность игрока, на которую ссылается текущая запись истории игры
        /// </summary>
        public PlayerEntity Player { get; set; }
    }
}
