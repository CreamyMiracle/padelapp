using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PadelApp.Data;
using PadelApp.Model;
using PadelApp.Models.Mapper;
using PaldeApp;
using Refit;
using SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// SQLite
SQLiteAsyncConnection con = new SQLiteAsyncConnection(@".\padelapp.db");
con.CreateTableAsync<Game>();

// Controllers
builder.Services.AddControllers();

// Repository
builder.Services.AddSingleton<GameRepository>(new GameRepository(con));
builder.Services.AddSingleton<SQLiteAsyncConnection>(con);

IGameAPI gameAPI = RestService.For<IGameAPI>(new HttpClient() { BaseAddress = new Uri("http://localhost:5252/games") }); //"https://padelpoints.azurewebsites.net/games"
builder.Services.AddSingleton<IGameAPI>(gameAPI);

// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new DefaultMappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

app.Run();
