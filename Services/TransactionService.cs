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
            var createTransactionSql = SqlQueries.CreateTransactionSql;
            using (var dbTransaction = connection.BeginTransaction())
            {
                try
                {
                    using (
                        var cmd = new NpgsqlCommand(
                            createTransactionSql,
                            this.connection,
                            dbTransaction
                        )
                    )
                    {
                        cmd.Parameters.AddWithValue("@transaction_id", transaction.Id);
                        cmd.Parameters.AddWithValue("@user_id", user.Id);
                        cmd.Parameters.AddWithValue("@type", transaction.Type);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@created_at", transaction.Date);

                        cmd.ExecuteNonQuery();
                    }

                    var updateBalanceSql = SqlQueries.UpdateBalanceSql;
                    using (
                        var updatecmd = new NpgsqlCommand(
                            updateBalanceSql,
                            connection,
                            dbTransaction
                        )
                    )
                    {
                        updatecmd.Parameters.AddWithValue("@user_id", user.Id);
                        updatecmd.Parameters.AddWithValue("@amount", amount);

                        Console.WriteLine($"Updating balance for user: {user.Username}");
                        Console.WriteLine($"Amount to add: {amount}");

                        updatecmd.ExecuteNonQuery();
                    }
                    var checkBalanceSql = SqlQueries.GetBalanceSql;
                    using (
                        var checkBalanceCmd = new NpgsqlCommand(
                            checkBalanceSql,
                            connection,
                            dbTransaction
                        )
                    )
                    {
                        checkBalanceCmd.Parameters.AddWithValue("@user_id", user.Id);
                        var newBalance = (decimal)checkBalanceCmd.ExecuteScalar();
                        Console.WriteLine($" Your new balance is: {newBalance}");
                    }
                    dbTransaction.Commit();
                    return transaction;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw new Exception($"Failed to process transaction: {ex.Message}");
                }
            }
        }

        public bool RemoveTransaction(Guid transactionId, Guid userId)
        {
            var getTransactionsSql = SqlQueries.GetTransactionsSql;

            using var dbTransaction = connection.BeginTransaction();
            try
            {
                using (
                    var getCmd = new NpgsqlCommand(getTransactionsSql, connection, dbTransaction)
                )
                {
                    getCmd.Parameters.AddWithValue("@transaction_id", transactionId);
                    getCmd.Parameters.AddWithValue("@user_id", userId);

                    using var reader = getCmd.ExecuteReader();
                    if (!reader.Read())
                    {
                        dbTransaction.Rollback();
                        return false;
                    }

                    var amount = reader.GetDouble(0);
                    var type = reader.GetString(1);
                    var adjustmentAmount = type == "deposit" ? -amount : amount;
                    reader.Close();

                    var updateBalanceSql = SqlQueries.UpdateBalanceSql;
                    using (
                        var updateCmd = new NpgsqlCommand(
                            updateBalanceSql,
                            connection,
                            dbTransaction
                        )
                    )
                    {
                        updateCmd.Parameters.AddWithValue("@user_id", userId);
                        updateCmd.Parameters.AddWithValue("@amount", adjustmentAmount);
                        updateCmd.ExecuteNonQuery();
                    }

                    var removeTransactionSql = SqlQueries.RemoveTransactionSql;
                    using (
                        var removeCmd = new NpgsqlCommand(
                            removeTransactionSql,
                            connection,
                            dbTransaction
                        )
                    )
                    {
                        removeCmd.Parameters.AddWithValue("@transaction_id", transactionId);
                        removeCmd.Parameters.AddWithValue("@user_id", userId);

                        int rowsAffected = removeCmd.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                        {
                            dbTransaction.Rollback();
                            return false;
                        }
                    }

                    dbTransaction.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                dbTransaction.Rollback();
                return false;
            }
        }

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
                Console.WriteLine($"Balance for user ID {user.Username}: {newBalance}");
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
            var getMonthlyTransactionsSql = SqlQueries.GetMonthlyTransactionsSql;

            using (var cmd = new NpgsqlCommand(getMonthlyTransactionsSql, this.connection))
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
            var getWeeklyTransactionsSql = SqlQueries.GetWeeklyTransactionsSql;

            using (var cmd = new NpgsqlCommand(getWeeklyTransactionsSql, this.connection))
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

            var getDailyTransactionsSql = SqlQueries.GetDailyTransactionsSql;

            using (var cmd = new NpgsqlCommand(getDailyTransactionsSql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@day", day);

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
