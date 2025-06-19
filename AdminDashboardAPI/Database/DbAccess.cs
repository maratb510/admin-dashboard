using Microsoft.Data.Sqlite;
using AdminDashboardAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdminDashboardAPI.Database;

public class DbAccess
{
    private readonly string _connectionString;

    public DbAccess(string dbFile = "dashboard.db")
    {
        _connectionString = $"Data Source={dbFile}";
    }

    public List<Client> GetClients()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var clients = new List<Client>();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, Email, BalanceT FROM Clients";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            clients.Add(new Client
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                BalanceT = reader.GetDecimal(3)
            });
        }

        return clients;
    }

    public List<Payment> GetPayments(int take)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var payments = new List<Payment>();
        var cmd = connection.CreateCommand();
        cmd.CommandText = $"""
            SELECT p.Id, p.Timestamp, p.AmountT, c.Name, c.Email
            FROM Payments p
            JOIN Clients c ON p.ClientId = c.Id
            ORDER BY p.Timestamp DESC
            LIMIT {take};
        """;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            payments.Add(new Payment
            {
                Id = reader.GetInt32(0),
                Timestamp = DateTime.Parse(reader.GetString(1)),
                AmountT = reader.GetDecimal(2),
                Client = new Client
                {
                    Name = reader.GetString(3),
                    Email = reader.GetString(4)
                }
            });
        }

        return payments;
    }

    public Rate? GetRate()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT Id, Value FROM Rate WHERE Id = 1";

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Rate
            {
                Id = reader.GetInt32(0),
                Value = reader.GetDecimal(1)
            };
        }

        return null;
    }

    public void UpdateRate(decimal value)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE Rate SET Value = @val WHERE Id = 1";
        cmd.Parameters.AddWithValue("@val", value);
        cmd.ExecuteNonQuery();
    }
}
