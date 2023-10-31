namespace Domain.Entities
{
    public class InscriptionFormation
    {
        public int InscriptionFormationID { get; set; }
        public int ClientID { get; set; }
        public int FormationID { get; set; }
        public Client Client { get; set; }
        public Formation Formation { get; set; }
    }
}
