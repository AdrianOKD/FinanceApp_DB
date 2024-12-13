namespace EgenInlämning.Transactions
{
    public interface ITransactionService
    {
        Transaction CreateTransaction(
            Guid user_Id,
            double amount,
            string type
        );
    }
}
