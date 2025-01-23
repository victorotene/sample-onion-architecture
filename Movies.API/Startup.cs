using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Movies.Application.Handlers;
using Movies.Core.Repositories;
using Movies.Core.Repositories.Base;
using Movies.Infrastructure.Data;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Repositories.Base;

namespace Movies.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning();

            // DbContext registration
            services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MoviesConnection")),
                ServiceLifetime.Scoped);

            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Movie API Review",
                    Version = "v1"
                });
            });

            // AutoMapper registration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // MediatR registration
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateMovieCommandHandler).Assembly));

            // Repository registrations
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IMovieRepository, MovieRepository>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Enable Swagger middleware
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API Review v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
