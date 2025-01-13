using System.Data.Common;
using System.Runtime;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class WithdrawCommand : Command
    {
        public WithdrawCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("2", "Withdraw from account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make a deposit");
                return;
            }

            string type = args[0];
            double amount = -Convert.ToDouble(args[1]);

            Transaction transaction = transactionService.CreateTransaction(
                user_Id: currentUser.Id,
                amount: amount,
                type: "withdraw"
            );

            System.Console.WriteLine($"You withdrew {amount}");
        }
    }
}
