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
            )
        { }

        public override void Execute(string[] args)
        {
            System.Console.WriteLine("Enter Username");
            string username = Console.ReadLine();
            if(string.IsNullOrEmpty(username))
            {System.Console.WriteLine("username cant be empty");
            return;}
            System.Console.WriteLine("Enter Password");
            string password = Console.ReadLine();

            User? user = userService.Login(username, password);
            if (user == null)
            {
                Console.WriteLine("Wrong username or password.");
                return;
            }

            Console.WriteLine("You successfully logged in.");
            menuService.SetMenu(new MainMenu(userService, menuService, transactionService));
        }
    }
}
