namespace Domain.Entities
{
#nullable disable
    public class BudgetFinancement
    {
        public int BudgetFinancementID { get; set; }
        public DateTime DateBudget { get; set; }
        public DateTime? DateCreation { get; set; } = DateTime.Now;
        public double MontantBudget { get; set; }
        public string EmetteurBudget { get; set; }
        public string? NuméroCheque { get; set; }
    }
}
