using FHT.Infra.Data.Core.Interfaces;
using FHT.Infra.Data.Repository.Base.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace FHT.Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
