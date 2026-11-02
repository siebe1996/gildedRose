using Dal.Items.Repositories;
using Domain.Items;
using Microsoft.Extensions.DependencyInjection;
using Models.Items;

namespace Dal.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDal()
        {
            services.AddSingleton<ItemRepository>(); // this is the right choice since im using an in memory list
            services.AddSingleton<IItemRepository>(sp => sp.GetRequiredService<ItemRepository>());
            services.AddSingleton<IItemsModelRepository>(sp => sp.GetRequiredService<ItemRepository>());

            return services;
        }
    }
}