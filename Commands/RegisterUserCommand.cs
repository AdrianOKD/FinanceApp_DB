using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
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
            string username = Console.ReadLine();
            if (string.IsNullOrEmpty(username))
            {
                System.Console.WriteLine("username cant be empty");
                return;
            }
            System.Console.WriteLine("Enter Password");
            string password = Console.ReadLine();
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Password can't be empty");
                return;
            }
            User user = userService.RegisterUser(username, password);
            Console.WriteLine($"Created user '{user.Name}' with ID: {user.Id}");
            

            Console.WriteLine($"Created user '{user.Name}'");
             userService.Login(username, password);
            menuService.SetMenu(new MainMenu(userService, menuService, transactionService));
        }
    }
}
