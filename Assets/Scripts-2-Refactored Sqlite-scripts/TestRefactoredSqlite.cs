using System.Xml.Linq;
using UnityEngine;

public class TestRefactoredSqlite : MonoBehaviour
{
    private HighscoreRepository repository;

    private void Start()
    {
        //string dbPath ="URI=file:" +Application.persistentDataPath +"/Highscores.db";
        string dbPath = "URI=file:" + Application.dataPath + "/Sqlite/Highscores.db";

        DatabaseManager db = new DatabaseManager(dbPath);

        repository = new HighscoreRepository(db);

        repository.CreateTable();

        //repository.AddHighscore("Maria", 1000);
        //repository.AddHighscore("Masa", 750);

        foreach (Highscore score in repository.GetHighscores())
        {
            Debug.Log(score);
        }

        Debug.Log(
            $"Rivejä yhteensä: {repository.GetHighscoreCount()}");
    }
}