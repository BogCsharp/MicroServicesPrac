using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class UserEntity:IdentityUser<long>

    {
        public long? MerchantId { get; set; }

        public MerchantEntity? Merchant {  get; set; }

    }
}
