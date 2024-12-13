using EgenInlämning.Menus;
using EgenInlämning.Transactions;
using EgenInlämning.Menus;
using EgenInlämning;

namespace EgenInlämning.Commands
{
    public class DepositCommand : Command
    {
        public DepositCommand( IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("deposit", "Deposit into account", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {
           string type = args[0];
           double amount = Convert.ToDouble(args[1]);

           Transaction transaction = transactionService.CreateTransaction(amount, type);

             var user = userService.GetLoggedInUser();
            Console.WriteLine($"Successfully deposited {amount:C}");
            Console.WriteLine($"New balance: {user.Balance:C}");


        }
    }
}