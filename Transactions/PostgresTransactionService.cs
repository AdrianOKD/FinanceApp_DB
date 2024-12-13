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
                @"INSERT INTO transaction (id, user_id, type, creation_date) VALUES (
            @id,
            @user_id,
            @type,
            @creation_date
        )";
            using var cmd = new NpgsqlCommand(sql, this.connection);
            cmd.Parameters.AddWithValue("@id", transaction.Id);
            cmd.Parameters.AddWithValue("@user_id", user.Id);
            cmd.Parameters.AddWithValue("@type", transaction.Type);
            cmd.Parameters.AddWithValue("@creation_date", transaction.Date);
            //cmd.Parameters.AddWithValue("@created_date", transaction.Description);

            cmd.ExecuteNonQuery();

            return transaction;
        }

        public void UpdateUserBalance(Guid Id, double amount)
        {
            using var cmd = new NpgsqlCommand(
            @"UPDATE users 
              SET balance = balance + @amount 
              WHERE id = @userId",
            connection);

        cmd.Parameters.AddWithValue("@userId", Id);
        cmd.Parameters.AddWithValue("@amount", amount);

        cmd.ExecuteNonQuery();
        }
    }
}
