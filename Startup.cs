using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Radzio.Entites;
using Radzio.Middleware;
using Radzio.Services;

namespace Radzio
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
          
            services.AddControllers();
            services.AddDbContext<RestaurantDbContext>(); // rejestracja kontekstu baz danych 
            services.AddScoped<RestaurantSeeder>(); //rejestracja serwisu seeduj¹cego
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>(); // rejestraxcja DishService na podstawie jego interfejsu
            services.AddScoped<ErrorHandlingMiddleware>();  //rejestracja serwisu wy³apuj¹cego wyj¹tki
            services.AddScoped<RequestTimeMiddleware>(); // implementacja metody sprawdzaj¹cej d³ugoœæ czasu trwania zapytania
            services.AddSwaggerGen(); // metoda do korekcji serwisów
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder seeder)
        {
            seeder.Seed();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>(); //dodanie Middleware na pocz¹tku zapytania
            app.UseMiddleware<RequestTimeMiddleware>();
            app.UseHttpsRedirection();

            app.UseSwagger(); // metoda odpowiadaj¹ca za generowanie pliku swagger.json
            app.UseSwaggerUI(c =>   //interfejs
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API");
            });

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
