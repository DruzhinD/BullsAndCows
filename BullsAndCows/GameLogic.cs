using System;
//настроить логи
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GameLogic : IEquatable<GameLogic>, IComparable<GameLogic>
{
    [JsonInclude]
    private string? _combination;
    [JsonInclude]
    private uint _attempts = 0;
    private string logPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\..\..\log.json");
    
    public GameLogic(){
        
    }

    public void Start(){
        _combination = this.GenCombination();
        Console.WriteLine(_combination);

        Console.WriteLine("Игра \"Быки и коровы\".");
        //цикл событий
        while (true){
            Console.Write("Введите комбинацию: ");
            string? userInput = Console.ReadLine();
            string result = "";
            if (userInput != null)
                result = this.IsCorrectInput(userInput);
            
            //если ввод некорректный, то повторяем попытку
            if (result != "0"){
                System.Console.WriteLine(result);
                continue;
            }

            //если все корректно, то считаем кол-во быков и коров
            int cows = this.CalculateCowsCount(userInput);
            int bulls = this.CalculateBullsCount(userInput);
            _attempts += 1;
            string output = $"{_attempts} попытка\t Введенная комбинация: {userInput}\t Угадано: {cows+bulls}\t Угадано с позицией: {bulls}";
            System.Console.WriteLine(output);
            if (bulls == 4){
                System.Console.WriteLine("Комбинация верная!");
                //сериализуем данные
                var data = this.Serialize();
                ShowStatistics(data);
                break;
            }
        }
        Console.WriteLine("Нажмите любую клавишу...");
        Console.ReadKey();
    }

    /// <summary>
    /// генерация числовой комбинации
    /// </summary>
    /// <param name="length">длина комбинации (по ум. = 4)</param>
    /// <returns>комбинация заданной величины в формате строки</returns>
    internal string GenCombination(int length = 4)
    {
        string combination = string.Empty; //генерируемая комбинация
        List<int> numbers = new List<int>()
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9
        };
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            int chosenNum = numbers[rnd.Next(numbers.Count)];
            numbers.Remove(chosenNum);
            combination += chosenNum;
        }

        return combination;
    }

    /// <summary>
    /// подсчет количества быков (точных совпадений) во введенной пользователем комбинации
    /// </summary>
    /// <param name="userCombination">ввод пользователя</param>
    /// <param name="_combination">сгенерированная комбинация</param>
    /// <returns>количество быков</returns>
    internal int CalculateBullsCount(string userCombination)
    {
        int bullsCount = 0;

        for (int i = 0; i < _combination.Length; i++)
        {
            if (_combination[i] == userCombination[i])
            {
                bullsCount++;
            }
        }
        return bullsCount;
    }

    /// <summary>
    /// подсчет количества коров (кол-во угаданных цифр, но не совпавших по месту) во введенной пользователем комбинации
    /// </summary>
    /// <param name="userCombination">ввод пользователя</param>
    /// <param name="_combination">сгенерированная комбинация</param>
    /// <returns>количество коров</returns>
    internal int CalculateCowsCount(string userCombination)
    {
        int cowsCount = 0;
        foreach (char num in userCombination)
        {
            //проверка на содержание цифры (только коровы)
            if (_combination.Contains(num))
            {
                //проверка на быков.
                //причем достоверно известно, что комбинация содержит итерируемый символ,
                //это следует из предыдущего условия
                if (_combination.IndexOf(num) != userCombination.IndexOf(num))
                    cowsCount++;
            }
        }
        return cowsCount;
    }

    /// <summary>
    /// Проверка введенного пользователем числа
    /// </summary>
    /// <param name="userCombination">ввод пользователя</param>
    /// <param name="length">длина комбинации</param>
    /// <returns>0 - в случае успешной проверки всех условий,
    /// иначе - сообщение для вывода пользователю, т.е. ошибки ввода</returns>
    internal string IsCorrectInput(string userCombination, int length = 4)
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

    private List<GameLogic> Serialize()
    {
        List<GameLogic>? gameData = null;
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        //десериализуем список, если файл существует
        if (System.IO.Path.Exists(this.logPath))
            using (FileStream fs = new FileStream(this.logPath, FileMode.Open, FileAccess.Read))
            {
                gameData = JsonSerializer.Deserialize<List<GameLogic>>(fs, options);
            }
        if (gameData == null)
            gameData = new List<GameLogic>();
        gameData.Add(this); //добавляем новые данные

        //сериализуем
        using (FileStream fs = new FileStream(this.logPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            JsonSerializer.Serialize<List<GameLogic>>(fs, gameData, options);
        }

        return gameData;
    }

    void ShowStatistics(List<GameLogic> gameData)
    {
        gameData.Sort();
        int index = gameData.IndexOf(this);
        double percent = ((index + 1) / (double)gameData.Count) * 100;
        Console.WriteLine($"Вы входите в {Math.Round(percent)}% игроков");
    }

    public int CompareTo(GameLogic? other)
    {
        return this._attempts.CompareTo(other._attempts);
    }

    public bool Equals(GameLogic? other)
    {
        return this._attempts == other._attempts;
    }
}