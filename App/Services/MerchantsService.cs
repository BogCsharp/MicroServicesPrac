using App.Abstrations;
using App.Models.Merchants;
using Domain.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class MerchantsService(OrdersDbContext context) : IMerchantsService
    {
        public async Task<MerchantDto> Create(MerchantDto merchant)
        {
            var entity =new MerchantEntity
            {
                Name= merchant.Name,
                Phone=merchant.Phone,
                WebSite=merchant.WebSite
            };
            var result =await context.Merchants.AddAsync(entity);
            var resultEntity = result.Entity;
            await context.SaveChangesAsync();
            return new MerchantDto
            {
                Name = resultEntity.Name,
                Phone = resultEntity.Phone,
                WebSite = resultEntity.WebSite,
                Id=resultEntity.Id
            };
        }
    }
}
