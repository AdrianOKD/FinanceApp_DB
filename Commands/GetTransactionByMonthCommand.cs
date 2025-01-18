namespace EgenInlämning
{
    public class GetTransactionsByMonthCommand : Command
    {
        public GetTransactionsByMonthCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "4",
                "Sort transactions for certain year and month",
                userService,
                menuService,
                transactionService
            ) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to view transactions.");
                return;
            }
            System.Console.WriteLine("Enter year (YYYY):");
            string yearInput = Console.ReadLine();
            int year = Convert.ToInt32(yearInput);
            System.Console.WriteLine("Enter month (1-12):");
            string input2 = Console.ReadLine();
            int month = Convert.ToInt32(input2);

            List<Transaction> transactions = transactionService.GetTransactionsByMonth(
                currentUser.Id,
                year,
                month
            );

            if (!transactions.Any())
            {
                Console.WriteLine($"No transactions found for {year}, {month}");
                return;
            }

            Console.WriteLine($"\nTransactions for {year},{month}:");
            Console.WriteLine("Date\t\tType\t\tAmount");
            Console.WriteLine("----------------------------------------");

            foreach (var transaction in transactions)
            {
                Console.WriteLine(
                    $"{transaction.Date:yyyy-MM-dd}\t{transaction.Type, -12}\t{transaction.Amount:C}"
                );
            }
        }
    }
}
