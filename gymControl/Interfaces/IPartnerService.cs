using gymControl.Models;

namespace gymControl.Interfaces
{
    public interface IPartnerService
    {
        Task<List<Partner>> GetPartners(PartnerQuery query);
        Task<Partner> GetPartner(string partnerId);
        Task<Partner> AuthenticatePartner(string username, string passwd);
        Task<Partner> AddPartner(Partner partner);
        Task<Partner> EditPartner(Partner partner);
        Task<Partner> RemovePartner(int? partnerId);
    }
}
