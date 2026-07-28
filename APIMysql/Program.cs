using APIMysql.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace APIMysql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("APIDbContext") ?? throw new InvalidOperationException("Connection string 'APIDbContext' not found.");

            // Add services to the container.
            builder.Services.AddDbContext<APIDbContext>(options =>
            options.UseInMemoryDatabase("BancoInMemory"));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
