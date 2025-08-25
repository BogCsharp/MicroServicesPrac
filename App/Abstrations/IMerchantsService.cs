using App.Models.Merchants;
using Domain.Entities;

namespace App.Abstrations
{
    public interface IMerchantsService
    {
        Task<MerchantDto> Create(MerchantDto merchant);
    }
}
