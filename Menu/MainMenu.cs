namespace EgenInlämning
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
            AddCommand(new ExpenseCommand(userService, menuService, transactionService ));
            AddCommand(new DepositCommand(userService, menuService, transactionService));
            AddCommand(new RemoveTransactionCommand(userService, menuService, transactionService));
            AddCommand(new ShowBalanceCommand(userService,menuService,transactionService));
            AddCommand(new LogoutCommand(userService, menuService, transactionService) );
            AddCommand(new TransactionsMenuCommand(userService, menuService, transactionService) );

            //csharp-ignore-end
        }

        public override void Display()
        {
            Console.Clear();
            Console.Write(
                """
                  Main Menu
                  --------------------------------------------------------
                [1] Deposit amount
                [2] Withdraw amount
                [3] Check balance
                [4] Show deposits and expenses
                [5] Logout
                [6] Exit

                Choose an option

                """
            );
        }
    }
}
