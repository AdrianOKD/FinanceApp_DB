
namespace EgenInlämning
{
    public class WithdrawCommand : Command
    {
        public WithdrawCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("2", "Withdraw from account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make a deposit");
                return;
            }

            System.Console.WriteLine("Type amount you want to deposite.");
            string input = Console.ReadLine();
            double amount = -Convert.ToDouble(input);

            Transaction transaction = transactionService.CreateTransaction(
                user_id: currentUser.Id,
                amount: amount,
                type: "withdraw"
            );

            System.Console.WriteLine($"You withdrew {amount}");
        }
    }
}
