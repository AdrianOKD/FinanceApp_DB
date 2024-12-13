namespace EgenInlämning.Transactions
{
    public class Transaction
    {
        public double Amount { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        // public string Description { get; set; }
        public Guid Id { get; set; }

    }
}
