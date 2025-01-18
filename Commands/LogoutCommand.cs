using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EgenInlämning
{
    public class LogoutCommand : Command
    {
        public LogoutCommand(
            IUserService userService,
            IMenuService menuService,
            ITransactionService transactionService
        )
            : base("9", "Logout of account", userService, menuService, transactionService) { }

        public override void Execute(string[] args)
        {
            var currentUser = userService.GetLoggedInUser();
            if (currentUser == null)
            {
                Console.WriteLine("No user is currently logged in");
                return;
            }

            Console.WriteLine($"Logging out user: {currentUser.Username}");

            try
            {
                userService.Logout();
                Console.WriteLine("Successfully logged out");
                Console.WriteLine("Returning to login menu...");
                Console.ReadKey();
                menuService.SetMenu(new LoginMenu(userService, menuService, transactionService));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during logout: {ex.Message}");
            }
        }
    }
}
