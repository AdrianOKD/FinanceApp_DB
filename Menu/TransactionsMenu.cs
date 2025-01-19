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
                --------------------------------
                [1] Show transactions by year
                [2] Show transactions by month
                [3] Show transactions by week
                [4] Show transactions by day
                [5] Return to main menu

                Choose an option

                """
            );
        }
    }
}
