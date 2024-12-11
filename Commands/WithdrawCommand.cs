using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class WithdrawCommand : Command
    {
        public WithdrawCommand(IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("withdraw", " Withdraw from account", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {
               string type = args[0];
           double amount = Convert.ToDouble(args[1]);

           Transaction transaction = transactionService.CreateTransaction(amount, type);


        }
    }
}