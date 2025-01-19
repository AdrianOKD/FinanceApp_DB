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
            System.Console.WriteLine("Enter year (YYYY):");
            string yearInput = Console.ReadLine();

            int year = Convert.ToInt32(yearInput);

            List<Transaction> transactions = transactionService.GetTransactionsByYear(
                currentUser.Id,
                year
            );

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
                    $"{transaction.Date:yyyy-MM-dd}\t{transaction.Type, -12}\t{transaction.Amount:C}"
                );
            }
        }
    }
}
