namespace EgenInlämning
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(Guid user_id, double amount, string type);

        List<Transaction> GetTransactionsByYear(Guid user_Id, int year);
        void CheckBalanceCmd();
        List<Transaction> GetTransactionsByMonth(Guid user_id, int year, int month);

        // public Transaction RemoveTransactionCommand() { }
        List<Transaction> GetTransactionsByDay(Guid user_id, int year, int month, int day);
        List<Transaction> GetTransactionsByWeek(Guid user_id, int year, int week);
    }
}
