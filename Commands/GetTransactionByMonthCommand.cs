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
                "2",
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
            System.Console.WriteLine("Enter month (1-12):");
            int month = Convert.ToInt32(Console.ReadLine());
            try
            {
                List<Transaction> transactions = transactionService.GetTransactionsByMonth(
                    currentUser.Id,
                    year,
                    month
                );

                transactions = transactions.Where(t => t.Type == transactionType).ToList();

                if (!transactions.Any())
                {
                    Console.WriteLine($"No transactions found for Year: {year} Month: {month}");
                    return;
                }

                Console.WriteLine($"\nTransactions for Year: {year} Month: {month}:");
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
