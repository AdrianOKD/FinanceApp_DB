namespace EgenInlämning
{
    public class GetTransactionsByYearCommand : Command
    {
        public GetTransactionsByYearCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "1",
                "Show transaction for certain year",
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
            int year = Convert.ToInt32(Console.ReadLine());
            try
            {
                List<Transaction> transactions = transactionService.GetTransactionsByYear(
                    currentUser.Id,
                    year
                );

                transactions = transactions.Where(t => t.Type == transactionType).ToList();

                if (!transactions.Any())
                {
                    Console.WriteLine($"No transactions found for Year: {year}");
                    return;
                }

                Console.WriteLine($"\nTransactions for Year: {year}");
                Console.WriteLine("Date\t\tType\t\tAmount");
                Console.WriteLine("----------------------------------------");

                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        $"{transaction.Date:yyyy-MM-dd}\t{transaction.Type, -12}\t{transaction.Amount, 6:C}"
                    );
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid date. Please enter a valid date.");
            }
        }
    }
}
