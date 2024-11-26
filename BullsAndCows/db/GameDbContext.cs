using BullsAndCows.db.Configurations;
using BullsAndCows.db.models;
using BullsAndCows.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BullsAndCows.db
{
    /// <summary>
    /// Контекст базы данных для игры быки и коровы
    /// </summary>
    public class GameDbContext : DbContext
    {
        /// <summary>
        /// путь к файлу бд sqlite
        /// </summary>
        string _conStr;

        /// <summary>
        /// Инициализация контекста БД 
        /// </summary>
        /// <param name="conStr"></param>
        public GameDbContext(string conStr)
        {
            this._conStr = $"Data Source={conStr}";
            //проверка на существование бд. В данном случае не нужна, ибо гарантируется, что бд существует
            //Database.EnsureCreated(); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_conStr)
                .LogTo(Logger.Log, LogLevel.Information) //логи
                ;
        }

        /// <summary>
        /// Таблица Player
        /// </summary>
        public DbSet<PlayerEntity> Player { get; set; }

        /// <summary>
        /// Таблица истории игр History
        /// </summary>
        public DbSet<HistoryEntity> History { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }
}
