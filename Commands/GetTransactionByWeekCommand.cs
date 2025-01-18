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
                "5",
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
            System.Console.WriteLine("Enter year (YYYY):");
            string yearInput = Console.ReadLine();
            System.Console.WriteLine("What week?");
            string weekInput = Console.ReadLine();

            int year = Convert.ToInt32(yearInput);
            int week = Convert.ToInt32(weekInput);

            List<Transaction> transactions = transactionService.GetTransactionsByYear(
                currentUser.Id,
                year
            );

            if (!transactions.Any())
            {
                Console.WriteLine($"No transactions found for {year} {week}");
                return;
            }

            Console.WriteLine($"\nTransactions for {year}:");
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
