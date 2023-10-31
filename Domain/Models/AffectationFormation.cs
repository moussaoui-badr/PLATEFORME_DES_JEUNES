using Domain.Entities;

namespace Domain.Models
{
    public class AffectationFormation
    {
        public List<AffectationClient> AffectationClients { get; set; }

        public Formation Formation { get; set; }
    }
}
