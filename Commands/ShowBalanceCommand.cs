using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgenInlämning.Menus;
using EgenInlämning.Transactions;

namespace EgenInlämning.Commands
{
    public class ShowBalanceCommand : Command
    {
        public ShowBalanceCommand(IUserService userService, IMenuService menuService, ITransactionService transactionService) : base("check-balance", "Checks users balance", userService, menuService, transactionService)
        {
        }

        public override void Execute(string[] args)
        {
            transactionService.CheckBalanceCmd();
        }
    }
}