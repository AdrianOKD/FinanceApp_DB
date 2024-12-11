namespace EgenInlämning;
using EgenInlämning.Transactions;
using EgenInlämning.Menus;
using EgenInlämning.Commands;
using System;
using Npgsql;

// users:
// - user_id (PRIMARY KEY)
// - name
// - password

// posts: (inlägg eller kommentar)
// - post_id (PRIMARY KEY)
// - user_id (FOREIGN KEY -> users)
// - parent_post_id NULLABLE (FOREIGN KEY -> posts)
// - content
// - creation timestamp

// user_likes:
// - user_id (FOREIGN KEY -> users)
// - post_id (FOREIGN KEY -> posts)



class Program
{
    public static void Main(string[] args)
    {
        string connectionString =
          "Host=localhost;Username=postgres;Password=password;Database=financedatabase";
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var createTablesSql =
            @"
                CREATE TABLE IF NOT EXISTS users (
                    user_id UUID PRIMARY KEY,
                    name TEXT,
                    password TEXT
                );

                CREATE TABLE IF NOT EXISTS transaction (
                    transaction_id UUID PRIMARY KEY,
                    user_id UUID REFERENCES users(user_id),
                    type TEXT,
                    description TEXT,
                    creation_Date Date
                );";

        using var createTableCmd = new NpgsqlCommand(createTablesSql, connection);
        createTableCmd.ExecuteNonQuery();

        IUserService userService = new PostgresUserService(connection);
        ITransactionService transactionService = new PostgresTransactionService(userService, connection);
        IMenuService menuService = new SimpleMenuService();
        Menu initialMenu = new LoginMenu(userService, menuService, transactionService);
        menuService.SetMenu(initialMenu);

        MainMenu mainMenu = new MainMenu(userService, menuService, transactionService);
        while (true)
        {
            string? inputCommand = Console.ReadLine();
            if (inputCommand != null)
            {
                menuService.GetMenu().ExecuteCommand(inputCommand);
            }
            else
            {
                break;
            }
        }
        // mainMenu.CreateNewConnection();
        // DataBaseService.InitializeDatabase();
    }
}
