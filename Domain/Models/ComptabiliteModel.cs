using Domain.Entities;

namespace Domain.Models
{
    public class ComptabiliteModel
    {
        public Financement Financement { get; set; }
        public Fonctionnement Fonctionnement { get; set; }
        public Chapitre Chapitre { get; set; }
        public int MyProperty { get; set; }
    }
}
