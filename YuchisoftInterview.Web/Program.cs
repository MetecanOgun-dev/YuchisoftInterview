using Microsoft.EntityFrameworkCore;
using YuchisoftTest.Application.Interface.Repository;
using YuchisoftTest.Application.Interface.Service;
using YuchisoftTest.Application.Interfaces.Repositories;
using YuchisoftTest.Application.Interfaces.Services;
using YuchisoftTest.Application.Services;
using YuchisoftTest.Infrastructure.Persistence;
using YuchisoftTest.Infrastructure.Repositories;
using YuchisoftTest.Infrastructure.Seeding;
using YuchisoftTest.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStockTypeRepository, StockTypeRepository>();
builder.Services.AddScoped<IStockUnitRepository, StockUnitRepository>();
builder.Services.AddScoped<IStockListRepository, StockListRepository>();
builder.Services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
builder.Services.AddScoped<IStockUnitService, StockUnitService>();
builder.Services.AddScoped<IStockTypeService, StockTypeService>();
builder.Services.AddScoped<IStockListService, StockListService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    await using var context = await factory.CreateDbContextAsync();

    await DbSeeder.MigrateAndSeedAsync(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseMiddleware<ExceptionLoggingMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();