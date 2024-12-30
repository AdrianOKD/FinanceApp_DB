namespace EgenInlämning;

using System;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;
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

        var sql = SqlQueries.CreateTablesSql;

        using var createTableCmd = new NpgsqlCommand(sql, connection);
        createTableCmd.ExecuteNonQuery();

        IUserService userService = new PostgresUserService(connection);
        IMenuService menuService = new SimpleMenuService();
        ITransactionService transactionService = new PostgresTransactionService(
            userService,
            connection
        );
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
