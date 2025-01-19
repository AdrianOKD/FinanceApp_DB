namespace EgenInlämning
{
    public class TransactionsMenu : Menu
    {
        public TransactionsMenu(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
        {
            // csharpier-ignore-start
            AddCommand(new GetTransactionsByYearCommand(userService, menuService, transactionService));
            AddCommand(new GetTransactionsByMonthCommand(userService, menuService, transactionService));
            AddCommand(new GetTransactionsByWeekCommand(userService, menuService, transactionService));
            AddCommand(new GetTransactionsByDayCommand(userService, menuService, transactionService));
            AddCommand(new MainMenuCommand(userService, menuService, transactionService));
            //csharp-ignore-end
        }

        public override void Display()
        {
            Console.Write(
                """
                              Transactions Menu
                  --------------------------------------------------------
                [1] Sort by year
                [2] Sort by month
                [3] Sort by week
                [4] Sort by day
                [5] Return to main menu

                Choose an option

                """
            );
        }
    }
}
