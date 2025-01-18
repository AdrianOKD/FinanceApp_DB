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
            AddCommand(new WithdrawCommand(userService, menuService, transactionService));
            AddCommand(new DepositCommand(userService, menuService, transactionService));
            AddCommand(new RemoveTransactionCommand(userService, menuService, transactionService));
            AddCommand(new ShowBalanceCommand(userService, menuService, transactionService));
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
            throw new NotImplementedException();
        }
    }
}
