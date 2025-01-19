namespace EgenInlämning
{
    public class TransactionsMenuCommand : Command
    {
        public TransactionsMenuCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("4", "Simply changes menu to transactions Menu", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            menuService.SetMenu(new TransactionsMenu(userService, menuService, transactionService));
        }
    }
}
