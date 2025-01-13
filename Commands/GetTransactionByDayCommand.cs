using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class GetTransactionsByDayCommand : Command
    {
        public GetTransactionsByDayCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("5", "description", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
