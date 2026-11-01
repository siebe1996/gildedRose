
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationHandlers()
        {
            var assembly = typeof(ServiceCollectionExtensions).Assembly;

            // Register Validators
            services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

            // Register Command and Query Handlers
            var handlerTypes = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false });

            foreach (var type in handlerTypes)
            {
                var serviceTypes = type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                                (i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                                 i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                                 i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

                foreach (var serviceType in serviceTypes)
                {
                    services.AddScoped(serviceType, type);
                }
            }

            return services;
        }
    }
}