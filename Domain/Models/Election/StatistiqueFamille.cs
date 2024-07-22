using Domain.Entities.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Election
{
    public class StatistiqueFamille
    {
        public string SecteurName { get; set; }
        public int? SecteurId { get; set; }

        public string PivotName { get; set; }
        public int? PivotId { get; set; }

        public int? ResponsableId { get; set; }

        public int? MembreId { get; set; }

        public int NombrePivot { get; set; }
        public int NombreResponsable { get; set; }
        public int NombreMembre { get; set; }

        public int TotalFamille { get; set; }

        public int TotalGeneral { get; set; }


        public string? SearchNom { get; set; }
        public string? SearchCIN { get; set; }
        public List<PersonnePivot>? PersonnePivot { get; set; }

        public string? Extend { get; set; }
    }
}
