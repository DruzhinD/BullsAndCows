using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows.db.models
{
    public class PlayerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public List<HistoryEntity> History { get; set; } = new List<HistoryEntity>();

    }
}
