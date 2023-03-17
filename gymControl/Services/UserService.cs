using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gymControl.Services
{
    public class UserService: IUserService
    {
        private readonly LkyqirhzContext _context;
        private readonly IConfiguration _config;

        public UserService(LkyqirhzContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<List<User>> GetUsers(string partnerId, UserQuery query)
        {

            var users = _context.Users.Where(v => partnerId == v.PartnerId.ToString());
            if (query.Id != null && query.Id > 0) users = users.Where(v => v.Id == query.Id);
            if (query.Email != null && query.Email != "") users = users.Where(v => v.Email != null && v.Email.ToLower().Contains(query.Email.ToLower()));
            if (query.Phone != null && query.Phone != "") users = users.Where(v => v.Phone != null && v.Phone.ToLower().Contains(query.Phone.ToLower()));
            if (query.Active != null) users = users.Where(v => query.Active == v.Active);
            var result = await users.ToListAsync();
            return result;
        }

        public async Task<User> GetUser(string partnerId, int userId)
        {
            var response = await _context.Users.FindAsync(userId);
            return response;
        }


        public async Task<User> AddUser( User user)
        {
            user.Id = null;
            user.Created = DateTime.UtcNow;
            user.CaducateDate = user.caducateDateCalc;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> EditUser(User user)
        {
            var result = _context.Users.SingleOrDefault(v => v.Id == user.Id);
            if (result != null)
            {
                if(user.userPass == null)
                {
                    user.userPass = result.userPass;
                }
                if(user.Active == null)
                {
                    user.Active = result.Active;
                }
                if(user.PayDay == null)
                {
                    user.PayDay = result.PayDay;
                }
                if(user.PartnerId == null)
                {
                    user.PartnerId = result.PartnerId;
                }
                user.Created = result.Created;
                user.Updated = DateTime.UtcNow;
                user.CaducateDate = user.caducateDateCalc;
                result  = user;
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<User> RemoveUser(int userId)
        {
            var user = new User() { Id = userId};
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
