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
           System.Console.WriteLine($"Thanks for using this finance app.\nHave a good day.");
           Console.ReadKey();
           Environment.Exit(0);
        }
    }
}
