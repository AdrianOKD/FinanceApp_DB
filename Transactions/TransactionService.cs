namespace EgenInlämning.Transactions
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(
            Guid user_Id,
            double amount,
            string type
        );

        List<Transaction> GetTransactionsByYear(Guid user_Id, double year);
        // Transaction SortByMonth();
        // Transaction SortByWeek();
    }
}
