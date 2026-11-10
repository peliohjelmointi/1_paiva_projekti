using System;
using System.Collections.Generic;

public class HighscoreRepository
{
    private readonly DatabaseManager db;

    public HighscoreRepository(DatabaseManager db)
    {
        this.db = db;
    }

    public void CreateTable()
    {
        db.ExecuteNonQuerySQL(
            @"CREATE TABLE IF NOT EXISTS highscores
            (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                score INTEGER
            )");
    }

    public void AddHighscore(string playerName, int score)
    {
        db.ExecuteNonQuerySQL(
            @"INSERT INTO highscores
            (name, score)
            VALUES
            (@name, @score)",

            ("@name", playerName),
            ("@score", score));
    }

    public List<Highscore> GetHighscores()
    {
        List<Highscore> highscores = new List<Highscore>();

        List<Dictionary<string, object>> rows = db.GetRows(
                @"SELECT id, name, score
                  FROM highscores
                  ORDER BY score DESC");

        foreach (Dictionary<string, object> row in rows)
        {
            Highscore highscore = new Highscore
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString(),
                Score = Convert.ToInt32(row["score"])
            };
            highscores.Add(highscore);
        }

        return highscores;
    }

    public int GetHighscoreCount()
    {
        //var rows =  db.GetRows("SELECT COUNT(*) AS count FROM highscores");
        //return Convert.ToInt32(rows[0]["count"]);
        return db.GetCount("SELECT COUNT(*) FROM highscores");
    }

    public void DeleteHighscore(int id)
    {
        db.ExecuteNonQuerySQL(
            @"DELETE FROM highscores
              WHERE id = @id",

            ("@id", id));
    }

    public void ClearTable()
    {
        db.ExecuteNonQuerySQL(
            "DELETE FROM highscores");
    }
}