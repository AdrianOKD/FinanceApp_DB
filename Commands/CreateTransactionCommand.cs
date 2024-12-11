// using EgenInlämning.Menus;
// using EgenInlämning.Transactions;

// namespace EgenInlämning.Commands
// {
//     public class CreateTransactionCommand : Command
//     {
//         public CreateTransactionCommand(
//             IUserService userService,
//             IMenuService menuService,
//             ITransactionService transactionService
//         )
//             : base(
//                 "create-post",
//                 "Create and upload a post.",
//                 userService,
//                 menuService,
//                 transactionService
//             ) { }

//         public override void Execute(string[] args)
//         {
//             double amount = double.Parse(args[1]);
//             string type = args[2];
//             DateOnly date = DateOnly.Parse(args[3]);
//             string description = string.Join(" ", args[4..]);

//             Transaction transaction = transactionService.CreateTransaction(
//                 amount,
//                 type,
//                 date,
//                 description,
//                 Guid.NewGuid()
//             );
//             Console.WriteLine("Created transaction!");
//         }
//     }
// }
