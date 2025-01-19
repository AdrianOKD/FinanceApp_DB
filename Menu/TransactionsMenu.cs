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
            AddCommand(
                new GetTransactionsByYearCommand(userService, menuService, transactionService)
            );
            AddCommand(
                new GetTransactionsByMonthCommand(userService, menuService, transactionService)
            );
            AddCommand(
                new GetTransactionsByWeekCommand(userService, menuService, transactionService)
            );
            AddCommand(
                new GetTransactionsByDayCommand(userService, menuService, transactionService)
            );
        }

        public override void Display()
        {
            Console.Write(
                """
                  Welcome to the best finance app in the world of warcraft
                  --------------------------------------------------------
                [3] Sort by year
                [4] Sort by month
                [5] Sort by week
                [6] Sort by year
                [6] Sort by year
                [3] Exit

                Choose an option

                """
            );
        }
    }
}
