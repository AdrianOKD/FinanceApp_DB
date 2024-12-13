namespace EgenInlämning.Transactions
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(
            double amount,
            string type
        );
       public void UpdateUserBalance (Guid userId, double amount);
    }
}
