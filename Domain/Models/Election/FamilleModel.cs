using Domain.Entities.V2;
using KhalfiElection.Models.Entities;

namespace KhalfiElection.Models.Models
{
    public class FamilleModel
    {
        public Personne Personne { get; set; }
        public int? PivotId { get; set; }
        public List<PersonnePivot>? Pivots { get; set; }
        public List<PersonneResponsable>? Responsables { get; set; }
        public List<PersonneMembre>? Membres { get; set; }
    }
}
