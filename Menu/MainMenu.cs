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
            AddCommand(new WithdrawCommand(userService, menuService, transactionService ));
            AddCommand(new DepositCommand(userService, menuService, transactionService));
            AddCommand(new RemoveTransactionCommand(userService, menuService, transactionService));
            AddCommand(new ShowBalanceCommand(userService,menuService,transactionService));
           
        }
        public override void Display()
        {
            Console.Write(
                """
                  Welcome to the best finance app in the world of warcraft
                  --------------------------------------------------------
                [1] Deposit... deposit amount
                [2] Withdraw.. withdraw amount
                [3] Exit

                Choose an option
                
                """
            );
        }
    }
}
