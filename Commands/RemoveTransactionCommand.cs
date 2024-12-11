using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class RemoveTransactionCommand : Command
    {
        public RemoveTransactionCommand( IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("Remove-Transaction", "Removes a specific transaction ", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {

            
        }
    }
}