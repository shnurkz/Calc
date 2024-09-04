using System;
using System.Data.SQLite;

namespace Calc
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Accounts (
                                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    Name TEXT NOT NULL,
                                                    Value1 REAL,
                                                    Value2 REAL,
                                                    Balance REAL)";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddAccount(string name, double value1, double value2, double balance)
        {
            if (AccountExists(name))
            {
                throw new Exception("Счет с таким именем уже существует.");
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Accounts (Name, Value1, Value2, Balance) VALUES (@name, @value1, @value2, @balance)";
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    command.Parameters.AddWithValue("@balance", balance);
                    command.ExecuteNonQuery();
                }
            }

            // Проверка, была ли запись успешно добавлена
            if (!AccountExists(name))
            {
                throw new Exception("Ошибка при добавлении счета в базу данных.");
            }
        }

        public void DeleteAccount(string name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Accounts WHERE Name = @name";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateAccount(string oldName, string newName, double value1, double value2, double balance)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string updateQuery = "UPDATE Accounts SET Name = @newName, Value1 = @value1, Value2 = @value2, Balance = @balance WHERE Name = @oldName";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@newName", newName);
                    command.Parameters.AddWithValue("@value1", value1);
                    command.Parameters.AddWithValue("@value2", value2);
                    command.Parameters.AddWithValue("@balance", balance);
                    command.Parameters.AddWithValue("@oldName", oldName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<(string Name, double Value1, double Value2, double Balance)> GetAccounts()
        {
            var accounts = new List<(string Name, double Value1, double Value2, double Balance)>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT Name, Value1, Value2, Balance FROM Accounts";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                            double value1 = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                            double value2 = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                            double balance = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);

                            accounts.Add((name, value1, value2, balance));
                        }
                    }
                }
            }
            return accounts;
        }

        public bool AccountExists(string name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(1) FROM Accounts WHERE Name = @name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
