namespace EgenInlämning
{
    public class DepositCommand : Command
    {
        public DepositCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("1", "Deposit into account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make a deposit");
                return;
            }
            System.Console.WriteLine("Type amount you want to deposite.");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                System.Console.WriteLine("Please enter a number");
                return;
            }
            double amount;
            if (double.TryParse(input, out amount))
            {
                Transaction transaction = transactionService.CreateTransaction(
                    user_id: currentUser.Id,
                    amount: amount,
                    type: "deposit"
                );

                Console.WriteLine($"Successfully deposited");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }
}
