using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullsAndCows
{
    public class Game
    {
        private GameLogic logic;

        public Game()
        {
            logic = new GameLogic();
        }

        public void Start()
        {
            Console.WriteLine("Игра \"Быки и коровы\".");
            Attempt lastAttempt; //последняя попытка
            Console.WriteLine(logic.Combination);
            //цикл событий
            while (true)
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
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
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
    }
}
