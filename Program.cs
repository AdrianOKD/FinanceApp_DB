namespace EgenInlämning;
using EgenInlämning.Transactions;
using EgenInlämning.Menus;
using EgenInlämning.Commands;
using System;
using Npgsql;


// user_account
// savings account : user_account
// expendature : user_account 
// potentiellt att användendaren kan skapa olika konton alltså namnge konton, dom innehåller samma

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
                    id UUID PRIMARY KEY,
                    balance DECIMAL(10,2) NOT NULL DEFAULT 0.00,
                    name TEXT,
                    password TEXT
                );

                CREATE TABLE IF NOT EXISTS transaction (
                    id UUID PRIMARY KEY,
                    user_id UUID REFERENCES users(id),
                    amount DECIMAL ,
                    type TEXT,
                    creation_date TIMESTAMP WITH TIME ZONE
                );";

        using var createTableCmd = new NpgsqlCommand(createTablesSql, connection);
        createTableCmd.ExecuteNonQuery();

        IUserService userService = new PostgresUserService(connection);
        IMenuService menuService = new SimpleMenuService();
        ITransactionService transactionService = new PostgresTransactionService(userService, connection);
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
