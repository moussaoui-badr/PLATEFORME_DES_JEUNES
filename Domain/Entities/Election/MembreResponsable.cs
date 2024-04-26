using KhalfiElection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Election
{
    public class MembreResponsable
    {
        public int Id { get; set; }
        public int MembreId { get; set; }
        public Personne Membre { get; set; }
        public int ResponsableId { get; set; }
        public Personne Responsable { get; set; }
    }
}
