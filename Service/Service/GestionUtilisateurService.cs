using Domain.Models;
using Repository.IRepositories;
using Service.IService;

namespace Service.Service
{
    public class GestionUtilisateurService : IGestionUtilisateurService
    {
        private readonly IGestionUtilisateurRepository _gestionUtilisateurRepository;

        public GestionUtilisateurService(IGestionUtilisateurRepository gestionUtilisateurRepository)
        {
            _gestionUtilisateurRepository = gestionUtilisateurRepository;
        }

        public async Task DeleteUser(string id)
        {
            await _gestionUtilisateurRepository.DeleteUser(id);
        }

        public async Task<List<string>> GetRoles(string id)
        {
            return await _gestionUtilisateurRepository.GetRoles(id);
        }

        public async Task<UserRolesViewModel> getUserAndRoles(string userId)
        {
            return await _gestionUtilisateurRepository.getUserAndRoles(userId);
        }

        public async Task<List<UserViewModel>> GetUsersBasicInfo()
        {
            return await _gestionUtilisateurRepository.GetUsersBasicInfo();
        }

        public async Task UpdateRoles(UserRolesViewModel model)
        {
            await _gestionUtilisateurRepository.UpdateRoles(model);
        }
    }
}
