using Microsoft.Extensions.DependencyInjection;

namespace MultipleChoiceTest.Domain.AutoMappingConfig
{
    public static class AutoMapConfig
    {
        public static IServiceCollection AddDomainMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            return services;
        }
    }
}
