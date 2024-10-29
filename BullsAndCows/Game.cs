using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullsAndCows.db;
using BullsAndCows.db.models;
using Microsoft.EntityFrameworkCore;

namespace BullsAndCows
{
    public class Game
    {
        private GameLogic logic;
        GameDbContext db; //путь к бд sqlite
        private Config config;

        PlayerEntity currentPlayer; //текущий авторизованный игрок

        public Game()
        {
            logic = new GameLogic();
            config = Config.GetInstance();
            db = new GameDbContext(
                Path.Combine(config.data["db_path"]));
        }

        public void Start()
        {
            Console.WriteLine("Игра \"Быки и коровы\".");
            Attempt lastAttempt = null; //последняя попытка
            //Console.WriteLine(logic.Combination);
            bool StartGame = AuthorizationOrRegistration();

            //цикл событий
            while (StartGame)
            {
                Console.Write("Введите комбинацию: ");
                string? userInput = Console.ReadLine();
                string result = "";
                if (userInput != null)
                    result = IsCorrectInput(userInput);

                //если ввод некорректный, то повторяем попытку
                if (result != "0")
                {
                    System.Console.WriteLine(result);
                    continue;
                }

                lastAttempt = logic.NextStep(userInput);
                Console.WriteLine($"№{logic.Attempts}\t Быки: {lastAttempt.bulls}\t коровы: {lastAttempt.cows}");
                if (lastAttempt.isWin)
                    break;
            }
            if (lastAttempt != null)
            {
                Console.WriteLine("Вы победили!");
                Console.WriteLine($"Комбинацию \"{logic.Combination}\" вы отгадали за {logic.Attempts} попыток.");
                this.SavePlayerHistoryRecord();
            }
            Console.WriteLine("Завершение игры...");
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);

        }

        /// <summary>
        /// Проверка введенного пользователем числа
        /// </summary>
        /// <param name="userCombination">ввод пользователя</param>
        /// <param name="length">длина комбинации</param>
        /// <returns>0 - в случае успешной проверки всех условий,
        /// иначе - сообщение для вывода пользователю, т.е. ошибки ввода</returns>
        static string IsCorrectInput(string userCombination, int length = 4)
        {
            if (userCombination.Length != length)
                return "Число должно состоять из 4 цифр!";

            foreach (char num in userCombination)
            {
                if (!char.IsDigit(num))
                    return "В этой игре можно использовать только цифры!";
            }

            for (int i = 0; i < userCombination.Length; i++)
            {
                for (int j = i + 1; j < userCombination.Length; j++)
                {
                    if (userCombination[i] == userCombination[j])
                        return "Цифры не должны повторяться!";
                }
            }
            return "0";
        }

        #region Регистрация или авторизация
        bool AuthorizationOrRegistration()
        {
            string msg = "Для входа (авторизации) нажмите 1; \n" +
                "Регистрация - 2 \n" + 
                "Выход - 3";
            Console.WriteLine(msg);
            bool loopFlag = true; //для выхода из цикла
            bool result = false;
            while (loopFlag)
            {
                Console.Write("Ввод: ");
                ConsoleKeyInfo input = Console.ReadKey();
                Console.WriteLine();
                switch(input.Key)
                {
                    case ConsoleKey.D1:
                        result = Authorization();
                        if (result) loopFlag = false;
                        break;
                    case ConsoleKey.D2:
                        result = Registration();
                        if (result) loopFlag = false;
                        break;
                    case ConsoleKey.D3:
                        loopFlag = false;
                        break;
                    default:
                        break;
                };

                if (!result)
                    Console.WriteLine("Неверный логин/пароль");
            }
            return result;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <returns>true - авторизация прошла успешно</returns>
        bool Authorization()
        {
            Console.Write("Введите ваш логин: ");
            string login = Console.ReadLine();
            if (login == null)
                return false;
            PlayerEntity player = Identify(login);
            if (player == null)
                return false;

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (password == null)
                return false;
            bool result = Authentification(player, password);
            //в случае успешной авторизации сохраняем сведения об авторизации
            if (result)
                currentPlayer = player;
            return result;
        }

        /// <summary>
        /// регистрация
        /// </summary>
        /// <returns>true - регистрация прошла успешно</returns>
        bool Registration()
        {
            Console.Write("Придумайте логин: ");
            string username = Console.ReadLine();
            if (username == null)
                return false;

            Console.Write("Придумайте пароль: ");
            string password = Console.ReadLine();
            if (password == null)
                return false;
            bool regResult = AddNewUser(username, password);
            return regResult;
        }
        #endregion

        #region Запросы к бд
        /// <summary>
        /// Идентификация (поиск логина в дб)
        /// </summary>
        /// <param name="username">имя пользователя (логин)</param>
        /// <returns>Сущность игрока, если он существует в бд, иначе null</returns>
        PlayerEntity Identify(string username)
        {
            IQueryable<PlayerEntity> user = db.Player
                //.AsNoTracking()
                .Where(p => p.Name == username);
            if (user.Count() == 0)
                return null;
            return user.First();
        }

        /// <summary>
        /// Аутенфикация, т.е. сверка пары логин - пароль
        /// </summary>
        /// <returns>true - пароль введен верно</returns>
        bool Authentification(PlayerEntity player, string password)
        {
            if (player.Password != password)
                return false;
            return true;
        }


        //предполагается, что текущего пользователя нет в бд
        bool AddNewUser(string username, string password)
        {
            try
            {
                currentPlayer = new PlayerEntity()
                {
                    Name = username,
                    Password = password,
                };
                db.Add(currentPlayer);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex){
                return false;
            }
        }

        void SavePlayerHistoryRecord()
        {
            HistoryEntity history = new HistoryEntity()
            {
                Attempts = logic.Attempts, 
                Combination = logic.Combination, 
                Player = currentPlayer};
            db.Add(history);
            db.SaveChanges();
        }
        #endregion
    }
}
