namespace EgenInlämning
{
    public class RemoveTransactionCommand : Command
    {
        public RemoveTransactionCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "Remove-Transaction",
                "Removes a specific transaction ",
                userService,
                menuService,
                transactionService
            ) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to remove a transaction");
            }
            string type = args[0];
        }
    }
}
