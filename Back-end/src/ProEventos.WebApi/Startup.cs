using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProEventos.WebApi.Data;

namespace ProEventos.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(); // Adicionando o serviço de controllers

            #region DbContext
            services.AddDbContext<DataContext>(context =>
            {
                context.UseMySql(Configuration.GetConnectionString("Default"), ServerVersion.AutoDetect(Configuration.GetConnectionString("Default")));
            });
            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ProEventos.WebApi",
                    Version = "v1",
                    Description = "Projeto de estudo.",
                    Contact = new OpenApiContact
                    {
                        Name = "Gabriel Silva Rodrigues Mota",
                        Email = "gabriel.rodrigues.mota@outlook.com",
                        Url = new Uri("https://github.com/GabrielRodriguesDev/.Net-WebApi-Angular")
                    }
                });
            });
            #endregion
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProEventos.WebApi v1");
                    c.RoutePrefix = string.Empty;
                    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                });
            }

            app.UseStaticFiles(); //permite que os arquivos estáticos sejam atendidos: SwaggerDark.css

            app.UseHttpsRedirection();

            app.UseRouting(); //Configurando o uso de rotas

            #region  Cors
            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod() // permitir qualquer método
                .AllowAnyHeader() // permitir qualquer Header
                .SetIsOriginAllowed(origin => true) // permitir qualquer origem
                .AllowCredentials()); // permite credenciais
            #endregion

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); //Criando o mapeamento de controllers e gerando os endpoints
            });
        }
    }
}
