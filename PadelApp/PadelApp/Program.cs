using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using PadelApp.Data;
using PadelApp.Model;
using PaldeApp;
using Refit;
using SQLite;
using System.Reflection;

namespace PadelApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Production
            });

            // Add services to the container.
            builder.Services.AddRazorPages();

            // SQLite
            SQLiteAsyncConnection con = new SQLiteAsyncConnection(@".\padelapp.db");
            con.CreateTableAsync<Game>();

            // Controllers
            builder.Services.AddControllers();

            // Repository
            builder.Services.AddSingleton<GameRepository>(new GameRepository(con));
            builder.Services.AddSingleton<SQLiteAsyncConnection>(con);

            IGameAPI gameAPI = RestService.For<IGameAPI>(new HttpClient() { BaseAddress = new Uri("http://localhost:5252/games") });
            builder.Services.AddSingleton<IGameAPI>(gameAPI);

            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Airshow API",
                    Description = "A simple REST API for Airshows",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Juhana Rajala",
                        Email = "juhana.rajala@hotmail.com",
                        Url = new Uri("https://github.com/CreamyMiracle"),
                    },
                });

                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllers();

            app.Run();
        }
    }
}