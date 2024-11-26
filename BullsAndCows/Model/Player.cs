using BullsAndCows.db;
using BullsAndCows.db.models;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace BullsAndCows.Model
{
    /// <summary>
    /// Сведения об игроке в контексте игры быки и коровы
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Сведения об игроке
        /// </summary>
        PlayerEntity player;

        /// <summary>
        /// Сведения об игроке
        /// </summary>
        public PlayerEntity PlayerInfo => player;

        /// <summary>
        /// Инициализация пользователя игры быки и коровы 
        /// </summary>
        /// <param name="player">запись об игроке из бд</param>
        protected Player(PlayerEntity player)
        {
            this.player = player;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns>Player в случае успешной авторизации, иначе null</returns>
        public static Player Authorization(string name, string password)
        {
            PlayerEntity playerDbRecord;
            try
            {
                using GameDbContext dbContext = new GameDbContext(Config.DbPath);
                playerDbRecord = dbContext.Player.FirstOrDefault(x => x.Name == name);
            }
            catch (ArgumentNullException ex)
            {
                Logger.Log(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return null;
            }

            //нет пользователя с таким именем
            if (playerDbRecord == null)
                return null;

            //пароль не совпадает
            if (playerDbRecord.Password != password)
                return null;

            //инициализируем игрока для текущих данных авторизации и возвращаем его
            Player playerGame = new(playerDbRecord);
            return playerGame;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <returns>Player в случае успешной регистрации, иначе null</returns>
        public static Player Registration(string name, string password)
        {
            //пользователь в контексте бд
            Player playerGame = null;
            using (GameDbContext dbContext = new GameDbContext(Config.DbPath))
            {
                try
                {
                    //сведения об игроке из БД
                    PlayerEntity playerDbRecord = dbContext.Player.FirstOrDefault(x => x.Name == name);

                    //пользователь с таким именем уже существует
                    if (playerDbRecord != null)
                        return null;

                    playerDbRecord = new PlayerEntity()
                    {
                        Name = name,
                        Password = password
                    };

                    //добавляем нового пользователя в бд
                    dbContext.Player.Add(playerDbRecord);
                    dbContext.SaveChanges();

                    //созданием нового пользователя в контексте игры
                    playerGame = new Player(playerDbRecord);
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.Message);
                    return null;
                }
                catch (ArgumentNullException ex)
                {
                    Logger.Log(ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    return null;
                }
            }
            return playerGame;
        }

        /// <summary>
        /// Добавить запись об очередной игре/сохранение результатов игры
        /// </summary>
        /// <returns>True - запись успешно добавлена в бд</returns>
        public bool AddNewHistoryRecord(int attempts, string combination)
        {
            if (string.IsNullOrEmpty(combination))
                return false;

            HistoryEntity history = new HistoryEntity()
            {
                Attempts = attempts,
                Combination = combination,
                Player = this.PlayerInfo
            };

            try
            {
                using (GameDbContext dbContext = new GameDbContext(Config.DbPath))
                {
                    dbContext.History.Add(history);
                    dbContext.Player.Attach(this.player);
                    dbContext.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }
            return true;
        }
    }
}
