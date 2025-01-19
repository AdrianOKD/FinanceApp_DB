namespace EgenInlämning
{
    public class GetTransactionsByWeekCommand : Command
    {
        public GetTransactionsByWeekCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "3",
                "Shows transactions for a certain week",
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
            Console.WriteLine("View: [1] Deposits or [2] Expenses");
            string choice = Console.ReadLine();

            if (choice != "1" && choice != "2")
            {
                Console.WriteLine("Invalid choice. Please select 1 or 2.");
                return;
            }

            string transactionType = (choice == "1") ? "deposit" : "expense";
            System.Console.WriteLine("Enter year (YYYY):");
            string yearInput = Console.ReadLine();
            System.Console.WriteLine("What week?");
            string weekInput = Console.ReadLine();

            int year = Convert.ToInt32(yearInput);
            int week = Convert.ToInt32(weekInput);

            List<Transaction> transactions = transactionService.GetTransactionsByWeek(
                currentUser.Id,
                year,
                week
            );

             transactions = transactions.Where(t => t.Type == transactionType).ToList();

            if (!transactions.Any())
            {
                Console.WriteLine($"No transactions found for Year: {year} Week: {week}");
                return;
            }

            Console.WriteLine($"\nTransactions for {year} {week}:");
            Console.WriteLine("Date\t\tType\t\tAmount");
            Console.WriteLine("----------------------------------------");

            foreach (var transaction in transactions)
            {
                Console.WriteLine(
                    $"{transaction.Date:yyyy-MM-dd}\t{transaction.Type, -12}\t{transaction.Amount,6:C}"
                );
            }
        }
    }
}
