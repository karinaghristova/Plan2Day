using Plan2Day.Core.Models;
using Plan2Day.Infrastructure.Data.Identity;

namespace Plan2Day.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();
        Task<UserEditViewModel> EditUser(string id);

        Task<bool> UpdateUser(UserEditViewModel model);
        Task<ApplicationUser> GetUserById(string id);

        Task<bool> DeleteUser(string id);

    }
}
