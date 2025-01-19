namespace EgenInlämning
{
    public class ExpenseCommand : Command
    {
        public ExpenseCommand(
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
                Console.WriteLine("You must be logged in to add an expense");
                return;
            }

            System.Console.WriteLine("Type amount you want add as an expense.");
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                System.Console.WriteLine("Please enter an amount");
                return;
            }
            double amount = -Convert.ToDouble(input);

            Transaction transaction = transactionService.CreateTransaction(
                user_id: currentUser.Id,
                amount: amount,
                type: "expense"
            );

            System.Console.WriteLine($"You added an expense of:  {amount}");
        }
    }
}
