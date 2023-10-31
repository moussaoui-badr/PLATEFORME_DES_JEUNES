namespace Domain.Models.Authentication
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SocieteId { get; set; }
        public string Password { get; set; }
        public int? TableauBordID { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
