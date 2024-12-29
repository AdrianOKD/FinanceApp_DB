using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgenInlämning
{
    public class SqlQueries
    {
        // public static string dadad =>
        // @"SELECT *  FROM "
        public static string CreateTransactionSql =>
            @"INSERT INTO transaction (id, user_id, type, amount, creation_date) VALUES (
            @id,
            @user_id,
            @type,
            @amount,
            @creation_date";
        public static string UpdateBalanceSql =>
            @"
            UPDATE users 
            SET balance = balance + @amount 
            WHERE id = @user_Id";
        public static string BalanceSql => @"SELECT balance FROM users WHERE id = @user_Id";
        public static string DaySql => @"";
        public static string WeekSql =>
            @"   
            SELECT t.id AS transaction_id, t.amount, t.type, t.creation_date
            FROM transaction t
            INNER JOIN users u ON user_id = u.id
            WHERE u.id = @user_Id
            AND EXTRACT(YEAR FROM creation_date) = @year
            AND EXTRACT(WEEK FROM creation_date) = @week
            ORDER BY t.creation_date DESC";

        public static string MonthSql =>
            @"
            SELECT t.id AS transaction_id, t.amount, t.type, t.creation_date
            FROM transaction t
            INNER JOIN users u ON user_id = u.id
            WHERE u.id = @user_Id
            AND EXTRACT(YEAR FROM creation_date) = @year
            AND EXTRACT(MONTH FROM creation_date) = @month
            ORDER BY t.creation_date DESC";

        public static string YearSql =>
            @"
            SELECT t.id AS transaction_id, t.amount, t.type, t.creation_date
            FROM transaction t
            INNER JOIN users u ON user_id = u.id
            WHERE u.id = @user_Id
            AND EXTRACT(YEAR FROM creation_date) = @year
            ORDER BY t.creation_date DESC";
    }
}
