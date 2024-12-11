namespace EgenInlämning.Transactions
{
    public class Transaction
    {
        public double Amount { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }

        // public string Description { get; set; }
        public Guid Id { get; set; }

        // public Transaction(
        //     DateOnly dateOnly,
        //     double amount,
        //     string type,
        //     string description,
        //     Guid id
        // )
        // {
        //     DateOnly = dateOnly;
        //     Amount = amount;
        //     Type = type;
        //     Description = description;
        //     Id = id;
        // }

        // public override string ToString()
        // {
        //     return $"{Type} on {DateOnly:yyyy-MM-dd}: {Amount:C} - {Description} (ID: {Id})";
        // }
    }
}
