using Npgsql;

namespace EgenInlämning
{
    public class TransactionService : ITransactionService
    {
        private IUserService userService;
        private NpgsqlConnection connection;

        public TransactionService(IUserService userService, NpgsqlConnection connection)
        {
            this.userService = userService;
            this.connection = connection;
        }

        public Transaction CreateTransaction(Guid userId, double amount, string type)
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
            var sql = SqlQueries.CreateTransactionSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@transaction_id", transaction.Id);
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@type", transaction.Type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@created_at", transaction.Date);

                cmd.ExecuteNonQuery();
            }

            var updateBalanceSql = SqlQueries.UpdateBalanceSql;

            using (var updatecmd = new NpgsqlCommand(updateBalanceSql, connection))
            {
                updatecmd.Parameters.AddWithValue("@user_id", user.Id);
                updatecmd.Parameters.AddWithValue("@amount", amount);

                Console.WriteLine($"Updating balance for user: {user.Username}");
                Console.WriteLine($"Amount to add: {amount}");

                updatecmd.ExecuteNonQuery();
                var checkBalanceSql = SqlQueries.GetBalanceSql;
                using (var checkBalanceCmd = new NpgsqlCommand(checkBalanceSql, connection))
                {
                    checkBalanceCmd.Parameters.AddWithValue("@user_id", user.Id);
                    var newBalance = (decimal)checkBalanceCmd.ExecuteScalar();
                    Console.WriteLine($" Your new balance is: {newBalance}");
                }
                return transaction;
            }
        }

        //  public Transaction RemoveTransactionCommand() { return }

        public void CheckBalanceCmd()
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var sql = SqlQueries.GetBalanceSql;
            using (var checkBalanceCmd = new NpgsqlCommand(sql, connection))
            {
                checkBalanceCmd.Parameters.AddWithValue("@user_id", user.Id);
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

            var sql = SqlQueries.GetYearlyTransactionsSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(
                            new Transaction
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                                Amount = reader.IsDBNull(reader.GetOrdinal("amount"))
                                    ? 0.00
                                    : reader.GetDouble(reader.GetOrdinal("amount")),
                                Type = reader.GetString(reader.GetOrdinal("type")),
                                Date = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            }
                        );
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
            var sql = SqlQueries.GetMonthlyTransactionsSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(
                            new Transaction
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                                Amount = reader.IsDBNull(reader.GetOrdinal("amount"))
                                    ? 0.00
                                    : reader.GetDouble(reader.GetOrdinal("amount")),
                                Type = reader.GetString(reader.GetOrdinal("type")),
                                Date = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            }
                        );
                    }
                    return transactions;
                }
            }
        }

        public List<Transaction> GetTransactionsByWeek(Guid user_id, int year, int week)
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var sql = SqlQueries.GetWeeklyTransactionsSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@week", week);
                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(
                            new Transaction
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                                Amount = reader.IsDBNull(reader.GetOrdinal("amount"))
                                    ? 0.00
                                    : reader.GetDouble(reader.GetOrdinal("amount")),
                                Type = reader.GetString(reader.GetOrdinal("type")),
                                Date = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            }
                        );
                    }
                    return transactions;
                }
            }
        }

        public List<Transaction> GetTransactionsByDay(Guid user_id, int year, int month, int day)
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }

            var sql = SqlQueries.GetDailyTransactionsSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@month", month);

                using (var reader = cmd.ExecuteReader())
                {
                    var transactions = new List<Transaction>();
                    while (reader.Read())
                    {
                        transactions.Add(
                            new Transaction
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("transaction_id")),
                                Amount = reader.IsDBNull(reader.GetOrdinal("amount"))
                                    ? 0.00
                                    : reader.GetDouble(reader.GetOrdinal("amount")),
                                Type = reader.GetString(reader.GetOrdinal("type")),
                                Date = reader.GetDateTime(reader.GetOrdinal("created_at")),
                            }
                        );
                    }
                    return transactions;
                }
            }
        }
    }
}
