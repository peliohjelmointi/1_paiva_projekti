public class Highscore
{
    public int Id;
    public string Name;
    public int Score;

    public override string ToString()
    {
        return $"{Name}: {Score}";
    }
}