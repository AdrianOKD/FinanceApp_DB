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
                "login",
                "Login with username and password.",
                userService,
                menuService,
                transactionService
            ) { }

        public override void Execute(string[] args)
        {
            // login [username password]
            string username = args[1];
            string password = args[2];

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
