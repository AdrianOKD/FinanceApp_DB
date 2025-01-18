
namespace EgenInlämning
{
    public class DepositCommand : Command
    {
        public DepositCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("1", "Deposit into account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("You must be logged in to make a deposit");
                return;
            }
            //add function for
            //string type = args[0];
            System.Console.WriteLine("Type amount you want to deposite.");
            string input = Console.ReadLine();
            
            double amount = Convert.ToDouble(input);

            Transaction transaction = transactionService.CreateTransaction(
                user_id: currentUser.Id,
                amount: amount,
                type: "deposit"
            );

            Console.WriteLine($"Successfully deposited");


        }
    }
}
