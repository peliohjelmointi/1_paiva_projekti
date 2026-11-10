using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

public class DatabaseManager
{
    private readonly string connectionString;

    public DatabaseManager(string connectionString)
    {
        this.connectionString = connectionString;
    }

    private SqliteConnection CreateConnection()
    {
        return new SqliteConnection(connectionString);
    }

    // CREATE, INSERT, UPDATE, DELETE
    public void ExecuteNonQuerySQL(string sql, params (string name, object value)[] parameters)
    {
        using var connection = CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = sql;

        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(
                parameter.name,
                parameter.value);
        }

        command.ExecuteNonQuery();
    }

    public int GetCount(string sql)
    {
        using var connection = CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = sql;

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public List<Dictionary<string, object>> GetRows(string sql, params (string name, object value)[] parameters)
    {
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

        using var connection = CreateConnection();
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = sql;

        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue(
                parameter.name,
                parameter.value);
        }

        using IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Dictionary<string, object> row = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.GetName(i), reader.GetValue(i));
            }

            rows.Add(row);
        }

        return rows;
    }
}