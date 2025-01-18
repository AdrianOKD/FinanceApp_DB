namespace EgenInlämning;

public class DataBaseService
{
    public static void InitializeDatabase()
    {
        // string connectionString =
        //     "Host=localhost;Username=postgres;Password=password;Database=FinanceDatabase";
        // using var connection = new NpgsqlConnection(connectionString);
        // connection.Open();

        // var createTablesSql =
        //     @"
        //         CREATE TABLE IF NOT EXISTS users (
        //             user_id UUID PRIMARY KEY,
        //             name TEXT,
        //             password TEXT
        //         );

        //         CREATE TABLE IF NOT EXISTS transaction (
        //             transaction_id UUID PRIMARY KEY,
        //             user_id UUID REFERENCES users(user_id),
        //             type TEXT,
        //             description TEXT,
        //             creation_Date Date
        //         );";

        // using var createTableCmd = new NpgsqlCommand(createTablesSql, connection);
        // createTableCmd.ExecuteNonQuery();

        // IUserService userService = new PostgresUserService(connection);
        // ITransactionService transactionService = new PostgresTransactionService(userService, connection);
        // IMenuService menuService = new SimpleMenuService();
        // Menu initialMenu = new LoginMenu(userService, menuService, transactionService);
        // menuService.SetMenu(initialMenu);

        // MainMenu mainMenu = new MainMenu(userService, menuService, transactionService);

        // mainMenu.CreateNewConnection();

        //var userService = new PostgresUserService(connection);
        // User? user = userService.Login("Ironman", "tonystark");
        // if (user != null)
        // {
        //     Console.WriteLine(user.Id);
        // }
        // else
        // {
        //     Console.WriteLine("Wrong username or password");
        // }
    }
}
