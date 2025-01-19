namespace EgenInlämning
{
    public class GetTransactionsByDayCommand : Command
    {
        public GetTransactionsByDayCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("4", "description", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to view transactions.");
                return;
            }

            System.Console.WriteLine("Enter year (YYYY):");
            int year = Convert.ToInt32(Console.ReadLine);

            System.Console.WriteLine("Enter month (1-12):");
            int month = Convert.ToInt32(Console.ReadLine);

            System.Console.WriteLine("Enter day (1-31):");
            int day = Convert.ToInt32(Console.ReadLine);

            try
            {
                var specificDate = new DateTime(year, month, day);
                List<Transaction> transactions = transactionService.GetTransactionsByDay(
                    currentUser.Id,
                    year,
                    month,
                    day
                );

                if (!transactions.Any())
                {
                    Console.WriteLine($"No transactions found for {specificDate:yyyy-MM-dd}");
                    return;
                }

                Console.WriteLine($"\nTransactions for {specificDate:yyyy-MM-dd}:");
                Console.WriteLine("Time\t\tType\t\tAmount");
                Console.WriteLine("----------------------------------------");

                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        $"{transaction.Date:HH:mm}\t{transaction.Type, -12}\t{transaction.Amount:C}"
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
