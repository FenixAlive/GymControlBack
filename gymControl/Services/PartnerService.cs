using gymControl.Interfaces;
using gymControl.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace gymControl.Services
{
    public class PartnerService: IPartnerService
    {
        private readonly LkyqirhzContext _context;
        private readonly IConfiguration _config;

        public PartnerService(LkyqirhzContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<PartnerAuth> LoginPartner(LoginUser loginPartner)
        {
            var partner = await AuthenticatePartner(loginPartner.UserName, loginPartner.Password);
            if (partner == null)
            {
                return null;
            }
            return new PartnerAuth() { token = GenerateToken(partner), data = partner };
        }

        public async Task<List<Partner>> GetPartners(PartnerQuery query)
        {
            var partners = _context.Partners.Where(v => query.Id != null ? query.Id == v.Id : true);
            if (query.Username != null && query.Username != "") partners = partners.Where(v => v.Username.ToLower().Contains(query.Username.ToLower()));
            if (query.Email != null && query.Email != "") partners = partners.Where(v => v.Email != null && v.Email.ToLower().Contains(query.Email.ToLower()));
            if (query.Phone != null && query.Phone != "") partners = partners.Where(v => v.Phone != null && v.Phone.ToLower().Contains(query.Phone.ToLower()));
            if (query.Active != null) partners = partners.Where(v => query.Active == v.Active);

            return await partners.Select(v => new Partner() { Id = v.Id, Username = v.Username, Email = v.Email, Phone = v.Phone, Created = v.Created, Updated = v.Updated, Active = v.Active }).ToListAsync(); ;
        }

        public async Task<Partner> GetPartner(int partnerId)
        {
            var partner = await _context.Partners.FindAsync(partnerId);
            if(partner != null)
            {
                partner.Passwd = null;
            }
            return partner;
        }

        public async Task<Partner> AuthenticatePartner(string username, string passwd)
        {
            var encodedPasswd = PasswdSecure.EncodePasswordToBase64(passwd);
            var lowerUsername = username.ToLower();
            var response = await _context.Partners.FirstOrDefaultAsync(v => lowerUsername == v.Username.ToLower() && encodedPasswd == v.Passwd);
            if(response != null)
            {
                response.Passwd = null;
            }
            return response;
        }

        public async Task<Partner> AddPartner(Partner partner)
        {
            partner.Id = null;
            partner.Created = DateTime.UtcNow;
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task<Partner> EditPartner(Partner partner)
        {
            var result = _context.Partners.SingleOrDefault(v => v.Id == partner.Id);
            if (result != null)
            {
                if(partner.Passwd == null)
                {
                    partner.Passwd = result.Passwd;
                }
                else
                {
                    partner.Passwd = PasswdSecure.EncodePasswordToBase64(partner.Passwd);
                }
                if(partner.Active == null)
                {
                    partner.Active = result.Active;
                }
                partner.Created = result.Created;
                partner.Updated = DateTime.UtcNow;
                result  = partner;
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<Partner> RemovePartner(int? partnerId)
        {
            var partner = new Partner() { Id = partnerId};
            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public string GenerateToken(Partner partner)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // create claims
            var claims = new[]
            {
                new Claim("id", partner.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, partner.Username),
                new Claim(ClaimTypes.Email, partner.Email)
            };
            // create token
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(16),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
