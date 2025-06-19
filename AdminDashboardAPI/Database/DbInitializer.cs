using Microsoft.Data.Sqlite;

namespace AdminDashboardAPI.Database;

public static class DbInitializer
{
    public static void Initialize(string fileName)
    {
        var connectionString = $"Data Source={fileName}";
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();

        // Создаём таблицы
        cmd.CommandText = """
            DROP TABLE IF EXISTS Payments;
            DROP TABLE IF EXISTS Clients;
            DROP TABLE IF EXISTS Rate;

            CREATE TABLE Clients (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL,
                BalanceT REAL NOT NULL
            );

            CREATE TABLE Payments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Timestamp TEXT NOT NULL,
                AmountT REAL NOT NULL,
                ClientId INTEGER NOT NULL,
                FOREIGN KEY(ClientId) REFERENCES Clients(Id)
            );

            CREATE TABLE Rate (
                Id INTEGER PRIMARY KEY,
                Value REAL NOT NULL
            );
        """;
        cmd.ExecuteNonQuery();

        // Добавляем данные
        var insert = connection.CreateCommand();
        insert.CommandText = """
            INSERT INTO Clients (Name, Email, BalanceT) VALUES
                ('Alice', 'alice@example.com', 150),
                ('Bob', 'bob@example.com', 300),
                ('Charlie', 'charlie@example.com', 75);

            INSERT INTO Payments (Timestamp, AmountT, ClientId) VALUES
                (DATETIME('now'), 50, 1),
                (DATETIME('now'), 100, 2),
                (DATETIME('now'), 25, 3),
                (DATETIME('now'), 20, 1),
                (DATETIME('now'), 75, 2);

            INSERT INTO Rate (Id, Value) VALUES (1, 10);
        """;
        insert.ExecuteNonQuery();
    }
}
