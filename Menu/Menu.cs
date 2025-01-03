using EgenInlämning.Commands;

namespace EgenInlämning.Menus
{
    public abstract class Menu
    {
        private List<Command> commands = new List<Command>();

        public void AddCommand(Command command)
        {
            this.commands.Add(command);
        }

        public void ExecuteCommand(string inputCommand)
        {
            string[] commandParts = inputCommand.Split(" ");

            foreach (Command command in commands)
            {
                if (command.Name.Equals(commandParts[0]))
                {
                    command.Execute(commandParts);
                    return;
                }
            }

            throw new ArgumentException("Command not found.");
        }

        public abstract void Display();
    }
}
