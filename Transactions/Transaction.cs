namespace EgenInlämning.Transactions
{
    public class Transaction
    {
        public double Amount { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        // public string Description { get; set; }
        public Guid Id { get; set; }

        public int? Reference_id {get; set; } = null; // ska användas för att refera till när jag vill ta bort en transaktion, behåller ett tillfälligt id skapad med for loop från lista.

    }
}
