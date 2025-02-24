namespace EgenInlämning
{
    public class RegisterUserCommand : Command
    {
        public RegisterUserCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("2", "Create a new user account.", userService, menuService, transactionService)
        { }

        public override void Execute(string[] args)
        {
            Console.WriteLine("Starting registration process...");
            System.Console.WriteLine("Enter Username");
            string? username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                System.Console.WriteLine("username cant be empty");
                return;
            }
            System.Console.WriteLine("Enter Password");
            string? password = Console.ReadLine();

            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Password can't be empty");
                return;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            User? user = userService.RegisterUser(username, hashedPassword);
            if (user == null)
            {
                return;
            }
            Console.WriteLine($"Created user '{user.Username}'");
            userService.Login(username, password);
            menuService.SetMenu(new MainMenu(userService, menuService, transactionService));
        }
    }
}
