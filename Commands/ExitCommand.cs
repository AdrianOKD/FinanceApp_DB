using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgenInlämning
{
    public class ExitCommand : Command
    {
        public ExitCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("6", "Exit application", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
