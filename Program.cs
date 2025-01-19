namespace EgenInlämning;

using System;
using Npgsql;

class Program
{
    public static void Main(string[] args)
    {
        string connectionString =
            "Host=localhost;Username=postgres;Password=password;Database=FinanceDatabase";
        using var connection = new NpgsqlConnection(connectionString);

        connection.Open();

        var sql = SqlQueries.CreateTablesSql;

        using var createTableCmd = new NpgsqlCommand(sql, connection);
        createTableCmd.ExecuteNonQuery();

        IUserService userService = new UserService(connection);
        IMenuService menuService = new SimpleMenuService();
        ITransactionService transactionService = new TransactionService(userService, connection);
        Menu initialMenu = new LoginMenu(userService, menuService, transactionService);
        Console.Clear();
        menuService.SetMenu(initialMenu);

        MainMenu mainMenu = new MainMenu(userService, menuService, transactionService);
        

        while (true)
        {
            string? inputCommand = Console.ReadLine();
            if (inputCommand == null)
                break;
            Console.Clear();
            menuService.GetMenu().ExecuteCommand(inputCommand);
        }
    }
}
