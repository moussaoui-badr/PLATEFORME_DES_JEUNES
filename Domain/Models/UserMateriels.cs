namespace Domain.Models
{
    public class UserMateriels
    {
        public string Nom { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public List<ListeMaterielsUserModel> ListeMaterielsUserModel { get; set; }
    }
}
