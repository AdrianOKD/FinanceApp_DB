using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class GetTransactionsByMonthCommand : Command
    {
        public GetTransactionsByMonthCommand(IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("sort-month", "Sort transactions by year and month", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to view transactions.");
                return;
            }

            int year = Convert.ToInt32(args[1]);
            int month = Convert.ToInt32(args[2]);

            List<Transaction> transactions = transactionService.GetTransactionsByMonth(currentUser.Id, year, month);

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
                Console.WriteLine($"{transaction.Date:yyyy-MM-dd}\t{transaction.Type,-12}\t{transaction.Amount:C}");
            }
        }
    }
}