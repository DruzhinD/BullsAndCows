/// <summary>
/// Содержит логику игры Быки и коровы
/// </summary>
public class GameLogic
{
    private string? _combination;
    public string Combination { get => _combination; }
    private int _attempts = 0;
    public int Attempts { get => _attempts; }
    private string logPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"..\..\..\log.json");
    
    public GameLogic(){
        this._combination = this.GenCombination();
    }

    /// <summary>
    /// Проверка на совпадения введенной комбинации пользователем с искомой комбинацией
    /// </summary>
    /// <returns>Результат попытки пользователя: количество быков и коров</returns>
    public Attempt NextStep(string userCombination)
    {
        this._attempts++;
        int bulls = this.CalculateBullsCount(userCombination);
        int cows = this.CalculateCowsCount(userCombination);
        bool isWin = false;
        if (bulls == Combination.Length)
            isWin = true;
        return new Attempt(bulls, cows, isWin);
    }

    /// <summary>
    /// генерация числовой комбинации
    /// </summary>
    /// <param name="length">длина комбинации (по ум. = 4)</param>
    /// <returns>комбинация заданной величины в формате строки</returns>
    protected string GenCombination(int length = 4)
    {
        string combination = string.Empty; //генерируемая комбинация
        List<int> numbers = new List<int>(10);
        for (int i = 0; i < 10; i++)
            numbers.Add(i);
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            int chosenNum = numbers[rnd.Next(numbers.Count)];
            numbers.Remove(chosenNum);
            combination += chosenNum;
        }
        return combination;
    }

    //точные совпадения
    /// <summary>
    /// подсчет количества быков (точных совпадений) во введенной пользователем комбинации
    /// </summary>
    /// <param name="userCombination">ввод пользователя</param>
    /// <param name="_combination">сгенерированная комбинация</param>
    /// <returns>количество быков</returns>
    protected int CalculateBullsCount(string userCombination)
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

    //совпадения по символам
    /// <summary>
    /// подсчет количества коров (кол-во угаданных цифр, но не совпавших по месту) во введенной пользователем комбинации
    /// </summary>
    /// <param name="userCombination">ввод пользователя</param>
    /// <param name="_combination">сгенерированная комбинация</param>
    /// <returns>количество коров</returns>
    protected int CalculateCowsCount(string userCombination)
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
}


/// <summary>
/// По сути это очередная запись (строка) попытки
/// </summary>
public class Attempt
{
    public int bulls;
    public int cows;
    /// <summary>
    /// true - если комбинация угадана
    /// </summary>
    public bool isWin;

    public Attempt(int bulls, int cows, bool win = false)
    {
        this.bulls = bulls;
        this.cows = cows;
        this.isWin = win;
    }
}