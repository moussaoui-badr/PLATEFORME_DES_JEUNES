using Domain.Models;

namespace Repository.IRepositories
{
    public interface IGestionUtilisateurRepository
    {
        Task DeleteUser(string id);
        Task UpdateRoles(UserRolesViewModel model);
        Task<UserRolesViewModel> getUserAndRoles(string userId);
        Task<List<UserViewModel>> GetUsersBasicInfo();
        Task<List<string>> GetRoles(string id);

    }
}
