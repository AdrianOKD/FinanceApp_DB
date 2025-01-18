namespace EgenInlämning.Transactions
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(Guid user_Id, double amount, string type);

        List<Transaction> GetTransactionsByYear(Guid user_Id, int year);
        void CheckBalanceCmd();
        List<Transaction> GetTransactionsByMonth(Guid user_id, int year, int month);

       // public Transaction RemoveTransactionCommand() { }

        // Transaction GetTransactionByWeek();
    }
}
