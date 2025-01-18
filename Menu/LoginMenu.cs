
namespace EgenInlämning
{
    public class LoginMenu : Menu
    {
        public LoginMenu(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
        {
            AddCommand(new LoginCommand(userService, menuService, transactionService));
            AddCommand(new RegisterUserCommand(userService, menuService, transactionService));
        }

        public override void Display()
        {
            System.Console.Write(
                """
                 Welcome to the best finance app in the world of warcraft
                 --------------------------------------------------------
                 [1] Log in
                 [2] Create Account
                 [3] Exit

                 Choose an option

                """
            );
        }
    }
}
