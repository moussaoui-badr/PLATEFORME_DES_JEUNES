using KhalfiElection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Election
{
    public class PivotRespoMembreModel
    {
        public Personne? Pivot { get; set; }
        public List<Personne>? Responsables { get; set; }
        public List<Personne>? Membres { get; set; }
    }
}
