using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class GetTransactionsByWeekCommand : Command
    {
        public GetTransactionsByWeekCommand(
            string name,
            string description,
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(name, description, userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
