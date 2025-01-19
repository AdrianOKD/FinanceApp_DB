

namespace EgenInlämning
{
    public class ShowBalanceCommand : Command
    {
        public ShowBalanceCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base(
                "3",
                "Checks users balance",
                userService,
                menuService,
                transactionService
            ) { }

        public override void Execute(string[] args)
        {
            transactionService.CheckBalanceCmd();
        }
    }
}
