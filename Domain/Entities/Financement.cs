namespace Domain.Entities
{
#nullable disable
    public class Financement
    {
        public int FinancementID { get; set; }
        public string? Candidat { get; set; }
        public string? NumeroCheque { get; set; }
        public string? Beneficiaire { get; set; }
        public DateTime? Date { get; set; }
        public double? Montant { get; set; }
        public DateTime? DateCreation { get; set; } = DateTime.Now;
        public int? TypeFinancement { get; set; }

    }
}
