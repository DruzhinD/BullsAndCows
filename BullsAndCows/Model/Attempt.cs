/// <summary>
/// Содержит логику игры Быки и коровы
/// </summary>
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