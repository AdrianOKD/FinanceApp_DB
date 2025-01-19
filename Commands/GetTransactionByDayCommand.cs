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

            System.Console.WriteLine("Enter day (1-31):");
            int day = Convert.ToInt32(Console.ReadLine());

            try
            {
                var specificDate = new DateTime(year, month, day);
                List<Transaction> transactions = transactionService.GetTransactionsByDay(
                    currentUser.Id,
                    year,
                    month,
                    day
                );

                transactions = transactions.Where(t => t.Type == transactionType).ToList();

                if (!transactions.Any())
                {
                    Console.WriteLine($"No transactions found for {specificDate:yyyy-MM-dd}");
                    return;
                }

                Console.WriteLine($"\nTransactions for Year: {year}");
                Console.WriteLine("Index\tDate\t\tType\t\tAmount");
                Console.WriteLine("-----------------------------------------------");

                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    Console.WriteLine(
                        $"{i + 1, -2}\t{transaction.Date:yyyy-MM-dd}\t{transaction.Type, -14}\t{transaction.Amount, 8:C}"
                    );
                }
                Console.WriteLine("\nWould you like to remove a transaction? (Y/N)");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    Console.Write(
                        "Enter the number of the transaction to remove (or 0 to cancel): "
                    );
                    if (int.TryParse(Console.ReadLine(), out int index))
                    {
                        if (index == 0)
                        {
                            Console.WriteLine("Cancelled removal");
                            return;
                        }

                        index--;

                        if (index >= 0 && index < transactions.Count)
                        {
                            var transactionToRemove = transactions[index];
                            try
                            {
                                bool removed = transactionService.RemoveTransaction(
                                    transactionToRemove.Id,
                                    currentUser.Id
                                );
                                if (removed)
                                {
                                    Console.WriteLine("Transaction removed successfully");
                                }
                                else
                                {
                                    Console.WriteLine("Failed to remove transaction");
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Unable to remove transaction");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Returning to transactions menu...");
                    menuService.SetMenu(
                        new TransactionsMenu(userService, menuService, transactionService)
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
