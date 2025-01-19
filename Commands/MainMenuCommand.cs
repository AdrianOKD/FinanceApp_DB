using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgenInlämning
{
    public class MainMenuCommand : Command
    {
        public MainMenuCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("5", "simply calls the main menu to return", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            menuService.SetMenu(new MainMenu(userService, menuService, transactionService));
        }
    }
}
