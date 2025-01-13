using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class LoginCommand : Command
    {
        public LoginCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "1",
                "Login with username and password.",
                userService,
                menuService,
                transactionService
            ) { }

        public override void Execute(string[] args)
        {
            while (true)
            {
                System.Console.WriteLine("Enter Username");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    System.Console.WriteLine("username cant be empty");
                    continue;
                }

                string username = input;

                System.Console.WriteLine("Enter Password");
                var input2 = Console.ReadLine();
                if (string.IsNullOrEmpty(input2))
                {
                    System.Console.WriteLine("password cant be empty");
                    continue;
                }

                string password = input2;

                User? user = userService.Login(username, password);
                if (user == null)
                {
                    Console.WriteLine("Wrong username or password.");
                    continue;
                }

                Console.WriteLine("You successfully logged in.");
                Console.ReadKey();
                menuService.SetMenu(new MainMenu(userService, menuService, transactionService));
                break;
            }
        }
    }
}
