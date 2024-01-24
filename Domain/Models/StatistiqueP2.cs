namespace Domain.Models
{
    public class StatistiqueP2
    {
        public double MontantApportPersonnel { get; set; }
        public double MontantINDH { get; set; }
        public double MontantProjet { get; set; }
        public double MontantMaterielAcquis { get; set; }
        public double MontantMaterielECA { get; set; }
        public double NombreMaterielAcquis { get; set; }
        public double NombreMaterielECA { get; set; }
        public double NombreMaterielAmenagement { get; set; }
        public string NombreCandidat { get; set; }
        public double MontantMaterielAmenagement { get; set; }

        public double MontantAcquisition { get; set; }

        public double Autre { get; set; }

        public double PartINDH { get; set; }
        public double ApportEnDHS { get; set; }
        public double ApportEnAmenagement { get; set; }


        public int? Cloture_EnCours { get; set; }

    }
}
