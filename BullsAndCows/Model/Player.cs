using BullsAndCows.db;
using BullsAndCows.db.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace BullsAndCows.Model
{
    public class Player
    {
        PlayerEntity player;
        public PlayerEntity PlayerInfo => player;
        string dbPath;

        public Player(string dbPath) => this.dbPath = dbPath;

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns>True - авторизация выполнена успешно</returns>
        public bool Authorization(string name, string password)
        {
            PlayerEntity player;
            try
            {
                using GameDbContext dbContext = new GameDbContext(this.dbPath);
                player = dbContext.Player.FirstOrDefault(x => x.Name == name);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return false;
            }

            //нет пользователя с таким именем
            if (player == null)
                return false;

            //пароль не совпадает
            if (player.Password != password)
                return false;

            //сохраняем запись из бд
            this.player = player;
            return true;
        }

        public bool Registration(string name, string password)
        {
            using (GameDbContext dbContext = new GameDbContext(this.dbPath))
            {
                try
                {
                    PlayerEntity player = dbContext.Player.FirstOrDefault(x => x.Name == name);

                    //пользователь с таким именем уже существует
                    if (player != null)
                        return false;

                    this.player = new PlayerEntity()
                    {
                        Name = name,
                        Password = password
                    };
                    //добавляем нового пользователя в бд
                    dbContext.Player.Add(player);
                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                    return false;
                }
            }
            return true;
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
                using (GameDbContext dbContext = new GameDbContext(this.dbPath))
                {
                    dbContext.History.Add(history);
                    dbContext.Player.Attach(this.player);
                    dbContext.SaveChanges();
                }
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
