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
                @"INSERT INTO posts (transaction_id, user_id,  type, date, description ) VALUES (
            @id,
            @user_id,
            @type,
            @created_date
            @description
        )";
            using var cmd = new NpgsqlCommand(sql, this.connection);
            cmd.Parameters.AddWithValue("@id", transaction.Id);
            //cmd.Parameters.AddWithValue("@user_id", User.User.Id);
            cmd.Parameters.AddWithValue("@content", transaction.Type);
            cmd.Parameters.AddWithValue("@created_date", transaction.Date);
            //cmd.Parameters.AddWithValue("@created_date", transaction.Description);

            cmd.ExecuteNonQuery();

            return transaction;
        }
    }
}
