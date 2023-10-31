using Domain.Models;

namespace Service.IService
{
    public interface IGestionUtilisateurService
    {
        Task DeleteUser(string id);
        Task UpdateRoles(UserRolesViewModel model);
        Task<UserRolesViewModel> getUserAndRoles(string userId);
        Task<List<UserViewModel>> GetUsersBasicInfo();
        Task<List<string>> GetRoles(string id);
    }
}
