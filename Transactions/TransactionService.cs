namespace EgenInlämning.Transactions
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(
            double amount,
            string type
        );
    }
}
