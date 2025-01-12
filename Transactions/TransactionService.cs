using System.Data.SqlTypes;
using EgenInlämning;
using Npgsql;

//using EgenInlämning.User;
//När jag vill deleta, ta in transaktionerna i en lista, gå sedan igenom listan och sätt ett index på den kopplat till id, för att genom den deleta transaktionen

namespace EgenInlämning.Transactions
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
                cmd.Parameters.AddWithValue("@id", transaction.Id);
                cmd.Parameters.AddWithValue("@user_id", user.Id);
                cmd.Parameters.AddWithValue("@type", transaction.Type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@creation_date", transaction.Date);

                cmd.ExecuteNonQuery();
            }

            var updateBalanceSql = SqlQueries.UpdateBalanceSql;

            using (var updatecmd = new NpgsqlCommand(updateBalanceSql, connection))
            {
                updatecmd.Parameters.AddWithValue("@user_Id", user.Id);
                updatecmd.Parameters.AddWithValue("@amount", amount);

                Console.WriteLine($"Updating balance for user: {user.Name}");
                Console.WriteLine($"Amount to add: {amount}");

                updatecmd.ExecuteNonQuery();
                var checkBalanceSql = "SELECT balance FROM users WHERE id = @user_Id";
                using (var checkBalanceCmd = new NpgsqlCommand(checkBalanceSql, connection))
                {
                    checkBalanceCmd.Parameters.AddWithValue("@user_Id", user.Id);
                    var newBalance = (decimal)checkBalanceCmd.ExecuteScalar();
                    Console.WriteLine($" Your new balance is: {newBalance}");
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
            var sql = SqlQueries.BalanceSql;
            using (var checkBalanceCmd = new NpgsqlCommand(sql, connection))
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

            var sql = SqlQueries.YearSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
                cmd.Parameters.AddWithValue("@user_Id", user.Id);
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
                                Date = reader.GetDateTime(reader.GetOrdinal("creation_date")),
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
            var sql = SqlQueries.MonthSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
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
                                Date = reader.GetDateTime(reader.GetOrdinal("creation_date")),
                            }
                        );
                    }
                    return transactions;
                }
            }
        }

        public List<Transaction> GetTransactionsByWeek(int year, int month)
        {
            var user = userService.GetLoggedInUser();
            if (user == null)
            {
                throw new ArgumentException("You are not logged in.");
            }
            var sql = SqlQueries.WeekSql;

            using (var cmd = new NpgsqlCommand(sql, this.connection))
            {
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
                                Date = reader.GetDateTime(reader.GetOrdinal("creation_date")),
                            }
                        );
                    }
                    return transactions;
                }
            }
        }
    }
}
