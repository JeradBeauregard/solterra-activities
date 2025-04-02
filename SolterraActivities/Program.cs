using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Services;
using Microsoft.OpenApi.Models;
using SolterraActivities.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IHobbyService, HobbyService>();
builder.Services.AddScoped<IMoodService, MoodService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityMoodService, ActivityMoodService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();

// Associate service interfaces with their implementations
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemTypesService, ItemTypesService>();



//swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SolterraActivities API", Version = "v1" });
});

//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    // swagger
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Set Swagger UI at root (/)
    });
    
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
