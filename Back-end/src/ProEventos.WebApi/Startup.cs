using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProEventos.Application.Interfaces;
using ProEventos.Application.Services;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Implementations;
using ProEventos.Persistence.Interfaces;
using AutoMapper;
using ProEventos.WebApi.Helpers;

namespace ProEventos.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            #region Controllers
            // Adicionando o serviço de controllers e 
            services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); //Configurando para ignorar as referencias circulares(isso porque estamos expondo as entidades e não objetos modelados para ir para a controoler)
            #endregion

            #region AutoMapper
            //Dentro do dominio da minha aplicação - No dominio corrente (web api) - Procura qual assemblie está herdando de profiel.
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var configMapper = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProEventosProfile());
            });

            IMapper mapper = configMapper.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region DI
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<IEventoPersist, EventoPersist>();
            #endregion

            #region DbContext
            services.AddDbContext<ProEventosContext>(context =>
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
