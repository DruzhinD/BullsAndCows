namespace BullsAndCows.db.models
{
    /// <summary>
    /// запись из таблицы Player
    /// </summary>
    public class PlayerEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public List<HistoryEntity> History { get; set; } = new List<HistoryEntity>();

    }
}
