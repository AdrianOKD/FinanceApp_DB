
using Npgsql;
//using EgenInlämning.User;
//När jag vill deleta, ta in transaktionerna i en lista, gå sedan igenom listan och sätt ett index på den kopplat till id, för att genom den deleta transaktionen

namespace EgenInlämning.Transactions
{
    public class PostgresTransactionService : ITransactionService
    {
        private IUserService userService;
        private NpgsqlConnection connection;

        public PostgresTransactionService(IUserService userService, NpgsqlConnection connection)
        {
            this.userService = userService;
            this.connection = connection;
        }
        public Transaction CreateTransaction(
            Guid userId,
            double amount,
            string type
        )
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Amount = amount,
                Type = type,
                Date = DateTime.Now,
            };
            var sql =
                @"INSERT INTO transaction (id, user_id, type, amount, creation_date) VALUES (
            @id,
            @user_id,
            @type,
            @amount,
            @creation_date
        )";
            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@id", transaction.Id);
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@type", transaction.Type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@creation_date", transaction.Date);

                cmd.ExecuteNonQuery();
            }

            var updateBalanceSql =
           @"UPDATE users 
              SET balance = balance + @amount 
              WHERE id = @user_Id";

            using (var updatecmd = new NpgsqlCommand(updateBalanceSql, connection))
            {
                updatecmd.Parameters.AddWithValue("@user_Id", user.Id);
                updatecmd.Parameters.AddWithValue("@amount", amount);

                Console.WriteLine($"Updating balance for user ID: {user.Id}");
                Console.WriteLine($"Amount to add: {amount}");

                updatecmd.ExecuteNonQuery();
                var checkBalanceSql = "SELECT balance FROM users WHERE id = @user_Id";
                using (var checkBalanceCmd = new NpgsqlCommand(checkBalanceSql, connection))
                {
                    checkBalanceCmd.Parameters.AddWithValue("@user_Id", user.Id);
                    var newBalance = (decimal)checkBalanceCmd.ExecuteScalar();
                    Console.WriteLine($" {newBalance}");
                }
                return transaction;
            }
        }
        public void CheckBalanceCmd()
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var checkBalanceSql = "SELECT balance FROM users WHERE id = @user_Id";
            using (var checkBalanceCmd = new NpgsqlCommand(checkBalanceSql, connection))
            {
                checkBalanceCmd.Parameters.AddWithValue("@user_Id", user.Id);
                var newBalance = (decimal)checkBalanceCmd.ExecuteScalar();
                Console.WriteLine($"Balance for user ID {user.Id}: {newBalance}");
            }
        }

        public List<Transaction> GetTransactionsByYear(Guid user_Id, int year)
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }

            var sql = @"
        SELECT t.id AS transaction_id, t.amount, t.type, t.creation_date
        FROM transaction t
        INNER JOIN users u ON user_id = u.id
        WHERE u.id = @user_Id
        AND EXTRACT(YEAR FROM creation_date) = @year
        ORDER BY t.creation_date DESC";

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_Id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                            Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0.00 : reader.GetDouble(reader.GetOrdinal("amount")),
                            Type = reader.GetString(reader.GetOrdinal("type")),
                            Date = reader.GetDateTime(reader.GetOrdinal("creation_date"))
                        });
                    }
                    return transactions;
                }
            }
        }
        public List<Transaction> GetTransactionsByMonth(Guid user_Id, int year, int month)
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var sql = @"
        SELECT t.id AS transaction_id, t.amount, t.type, t.creation_date
        FROM transaction t
        INNER JOIN users u ON user_id = u.id
        WHERE u.id = @user_Id
        AND EXTRACT(YEAR FROM creation_date) = @year
        AND EXTRACT(MONTH FROM creation_date) = @month
        ORDER BY t.creation_date DESC";

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_Id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(new Transaction
                        {
                            Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                            Amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0.00 : reader.GetDouble(reader.GetOrdinal("amount")),
                            Type = reader.GetString(reader.GetOrdinal("type")),
                            Date = reader.GetDateTime(reader.GetOrdinal("creation_date"))
                        });
                    }
                    return transactions;
                }
            }
        }
    }
}

