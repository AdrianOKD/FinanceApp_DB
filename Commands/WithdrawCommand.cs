
namespace EgenInlämning
{
    public class WithdrawCommand : Command
    {
        public WithdrawCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("2", "Add expense", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make add an expense");
                return;
            }

            System.Console.WriteLine("Type amount you want add as an expense.");
            string input = Console.ReadLine();
            double amount = -Convert.ToDouble(input);

            Transaction transaction = transactionService.CreateTransaction(
                user_id: currentUser.Id,
                amount: amount,
                type: "expense"
            );

            System.Console.WriteLine($"You added expense of:  {amount}");
        }
    }
}
