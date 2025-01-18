using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class DepositCommand : Command
    {
        public DepositCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("1", "Deposit into account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make a deposit");
                return;
            }
            //add function for
            //string type = args[0];
            double amount = Convert.ToDouble(args[1]);

            Transaction transaction = transactionService.CreateTransaction(
                user_Id: currentUser.Id,
                amount: amount,
                type: "deposit"
            );

            Console.WriteLine($"Successfully deposited");
        }
    }
}
