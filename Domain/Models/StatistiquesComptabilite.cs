namespace Domain.Models
{
    public class StatistiquesComptabilite
    {
        public double? MontantChapitre { get; set; }
        public double? MonantFonctionnement { get; set; }
        public double? MontantFinancementDebit { get; set; }
        public double? MontantFinancementCredit { get; set; }
        public double? MontantBudgetFonctionnement { get; set; }
        public double? MontantBudgetFinancement { get; set; }

        public double? ResteFonctionnement { get; set; }
        public double? ResteFinancement { get; set; }

        public int NombreFonctionnement { get; set; }
        public int NombreFinancementDebit { get; set; }
        public int NombreFinancementCredit { get; set; }
        public int NombreChapitre { get; set; }

    }
}
