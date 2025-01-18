

namespace EgenInlämning
{
    public class RemoveUserCommand : Command
    {
        public RemoveUserCommand(
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
