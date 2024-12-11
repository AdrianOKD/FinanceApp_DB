using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class DepositCommand : Command
    {
        public DepositCommand( IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("deposte", "Deposit into account", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {
           string type = args[0];
           double amount = Convert.ToDouble(args[1]);

           Transaction transaction = transactionService.CreateTransaction(amount, type);

           System.Console.WriteLine($"You have depositet {amount}");


        }
    }
}