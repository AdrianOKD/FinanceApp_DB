public class SqlQueries
{
    public static string CreateTablesSql =>
        @"CREATE TABLE IF NOT EXISTS users (
            user_id UUID PRIMARY KEY,
            username TEXT NOT NULL,
            password TEXT NOT NULL,
            balance DECIMAL(10,2) NOT NULL DEFAULT 0.00
        );

        CREATE TABLE IF NOT EXISTS transactions (
            transaction_id UUID PRIMARY KEY,
            user_id UUID REFERENCES users(user_id),
            amount DECIMAL(10,2) NOT NULL,
            type TEXT NOT NULL,
            created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
        );";

    public static string CreateTransactionSql =>
        @"INSERT INTO transactions (
            transaction_id,
            user_id,
            type,
            amount,
            created_at
        ) VALUES (
            @transaction_id,
            @user_id,
            @type,
            @amount,
            @created_at
        )";

    public static string UpdateBalanceSql =>
        @"UPDATE users 
        SET balance = balance + @amount 
        WHERE user_id = @user_id";

    public static string GetBalanceSql =>
        @"SELECT balance 
        FROM users 
        WHERE user_id = @user_id";

    public static string GetDailyTransactionsSql =>
        @"
        SELECT transaction_id, amount, type, created_at 
        FROM transactions 
        WHERE user_id = @user_id 
        AND EXTRACT(YEAR FROM created_at) = @year
        AND EXTRACT(MONTH FROM created_at) = @month
        AND EXTRACT(DAY FROM created_at) = @day
        ORDER BY created_at";

    public static string GetWeeklyTransactionsSql =>
        @"SELECT 
            t.transaction_id,
            t.amount,
            t.type,
            t.created_at
        FROM transactions t
        INNER JOIN users u ON t.user_id = u.user_id
        WHERE u.user_id = @user_id
            AND EXTRACT(YEAR FROM created_at) = @year
            AND EXTRACT(WEEK FROM created_at) = @week
        ORDER BY t.created_at DESC";

    public static string GetMonthlyTransactionsSql =>
        @"SELECT 
            t.transaction_id,
            t.amount,
            t.type,
            t.created_at
        FROM transactions t
        INNER JOIN users u ON t.user_id = u.user_id
        WHERE u.user_id = @user_id
            AND EXTRACT(YEAR FROM t.created_at) = @year
            AND EXTRACT(MONTH FROM t.created_at) = @month
        ORDER BY t.created_at DESC";

    public static string GetYearlyTransactionsSql =>
        @"SELECT 
            t.transaction_id,
            t.amount,
            t.type,
            t.created_at
        FROM transactions t
        INNER JOIN users u ON t.user_id = u.user_id
        WHERE u.user_id = @user_id
            AND EXTRACT(YEAR FROM created_at) = @year
        ORDER BY t.created_at DESC";

    public static string CreateUserSql =>
        @"INSERT INTO users (
            user_id,
            username,
            password,
            balance
        ) VALUES (
            @user_id,
            @username,
            @password,
            @balance
        )";

    public static string LoginSql =>
        @"SELECT 
            user_id,
            username,
            password,
            balance
        FROM users
        WHERE username = @username";

    public static string GetUserSql =>
        @"SELECT *
        FROM users
        WHERE user_id = @user_id";

    public static string DeleteUserSql =>
        @"DELETE FROM users
        WHERE user_id = @user_id";

    public static string RemoveTransactionSql =>
        @"DELETE FROM transactions 
    WHERE transaction_id = @transaction_id 
    AND user_id = @user_id";

    public static string CheckUserNameSql =>
        @"SELECT COUNT(*) FROM users WHERE username = @username";
}
