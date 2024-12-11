using EgenInlämning.Commands;
using EgenInlämning.Transactions;
using Npgsql;

namespace EgenInlämning.Menus
{
    public class MainMenu : Menu
    {
        public MainMenu(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
        {
            AddCommand(new DepositCommand(userService, menuService, transactionService));
        }
        public override void Display()
        {
            Console.Write(
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
