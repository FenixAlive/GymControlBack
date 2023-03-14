using gymControl.Models;

namespace gymControl.Interfaces
{
    public interface IUserService
    {
        
        Task<List<User>> GetUsers(string partnerId, UserQuery query);
        Task<User> GetUser(int userId);
        Task<User> AddUser(User user);
        Task<User> EditUser(User user);
        Task<User> RemoveUser(int? userId);
    }
}
