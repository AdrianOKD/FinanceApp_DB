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
            // csharpier-ignore-start
            AddCommand(new WithdrawCommand(userService, menuService, transactionService ));
            AddCommand(new DepositCommand(userService, menuService, transactionService));
            AddCommand(new RemoveTransactionCommand(userService, menuService, transactionService));
            AddCommand(new ShowBalanceCommand(userService,menuService,transactionService));
            AddCommand(new GetTransactionsByYearCommand(userService, menuService, transactionService));
            AddCommand(new GetTransactionsByMonthCommand(userService, menuService, transactionService));
            //csharp-ignore-end
        }

        public override void Display()
        {
            Console.Write(
                """
                  Welcome to the best finance app in the world of warcraft
                  --------------------------------------------------------
                [1] Deposit amount
                [2] Withdraw amount
                [3] Sort by year
                [4] Sort by month
                [5] Sort by week
                [6] Sort by year
                [3] Exit

                Choose an option

                """
            );
        }
    }
}
