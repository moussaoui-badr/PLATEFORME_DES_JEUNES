
using Domain.Entities;

namespace Domain.Models
{
    public class Notification
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Statut Statut { get; set; }
        public DateTime Date { get; set; }
    }
}
