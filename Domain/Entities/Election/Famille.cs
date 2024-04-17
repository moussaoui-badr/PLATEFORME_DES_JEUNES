using System.ComponentModel.DataAnnotations.Schema;

namespace KhalfiElection.Models.Entities
{
    public class Famille
    {
        public int Id { get; set; }
        public string? Libelle { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
        public List<Personne>? Membres { get; set; }
        public int ResponsableId { get; set; }
    }
}
