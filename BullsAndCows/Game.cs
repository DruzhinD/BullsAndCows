using BullsAndCows.Model;
#nullable disable

namespace BullsAndCows
{
    public class Game
    {
        Player player; //текущий игрок
        private GameLogic logic;
        private Config config;
        public Game()
        {
            logic = new GameLogic();
            config = Config.GetInstance();
            player = new Player(config.DbPath);
        }

        public void Start()
        {
            Console.WriteLine("Игра \"Быки и коровы\".");
            Attempt lastAttempt = null; //последняя попытка
            Console.WriteLine(logic.Combination);
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

        #region Формы регистрации и авторизации
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
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        result = Authorization();
                        if (result) loopFlag = false;
                        break;
                    case ConsoleKey.D2:
                        //todo: не работае регистрация
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
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (password == null)
                return false;

            bool result = this.player.Authorization(login, password);
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
            if (string.IsNullOrEmpty(username))
                return false;

            Console.Write("Придумайте пароль: ");
            string password = Console.ReadLine();
            if (password == null)
                return false;
            bool regResult = this.player.Registration(username, password);
            return regResult;
        }
        #endregion

        /// <summary>
        /// Сохранение результатов игры в бд
        /// </summary>
        void SavePlayerHistoryRecord()
        {
            this.player.AddNewHistoryRecord(this.logic.Attempts, this.logic.Combination);
        }
    }
}
