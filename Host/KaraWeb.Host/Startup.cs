using KaraWeb.Core.Helpers;
using KaraWeb.Host.Providers.Collections;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using System.Collections.Generic;

namespace KaraWeb.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc(Constants.ProjectName, new OpenApiInfo { Title = Constants.ProjectName, Version = $"{GetType().Assembly.GetName().Version}" });
            });

            RegisterProviders(services);
        }

        private void RegisterProviders(IServiceCollection services)
        {
            services.AddSingleton<ICollectionsProvider, CollectionsProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var swaggerPrefix = Constants.ApiMainRoutePrefix + "swagger";
            const string DocName = "openapi.json";
            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerPrefix + "/{documentName}/" + DocName;
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = swaggerPrefix;
                c.SwaggerEndpoint($"{Constants.ProjectName}/{DocName}", Constants.ProjectName);

            });
        }
    }
}