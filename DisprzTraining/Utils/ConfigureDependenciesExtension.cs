using DisprzTraining.Business;
using DisprzTraining.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
// using DisprzTraining.Business;

namespace DisprzTraining.Utils
{
    public static class ConfigureDependenciesExtension
    {
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAppointmentsBL, AppointmentsBL>();
            services.AddScoped<IAppointmentDAL, AppointmentDAL>();
        }
    }
}
