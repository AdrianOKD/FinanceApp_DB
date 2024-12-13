using System.Data.Common;
using System.Transactions;
using Npgsql;
//using EgenInlämning.User;

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
                //cmd.Parameters.AddWithValue("@created_date", transaction.Description);

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
                    Console.WriteLine($"New balance for user ID {user.Id}: {newBalance}");
                }

                return transaction;

            }


        }






    }
}
