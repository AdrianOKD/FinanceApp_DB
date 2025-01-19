namespace EgenInlämning
{
    public abstract class Command
    {
        public string Name { get; init; }
        public string Description { get; init; }

        protected IUserService userService;
        protected IMenuService menuService;
        protected ITransactionService transactionService;

        public Command(
            string name,
            string description,
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
        {
            this.Name = name;
            this.Description = description;
            this.userService = userService;
            this.menuService = menuService;
            this.transactionService = transactionService;
        }

        public abstract void Execute(string[] args);
    }
}
