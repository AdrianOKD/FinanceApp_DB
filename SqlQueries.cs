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
