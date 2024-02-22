using Autofac.Extensions.DependencyInjection;
using Autofac;
using BegumYatch.API.Extensions;
using BegumYatch.API.Filters;
using BegumYatch.API.Modules;
using BegumYatch.Core.DTOs.FuelPurchaseDemand;
using BegumYatch.Core.Models.EmailSettings;
using BegumYatch.Core.Models.Role;
using BegumYatch.Core.Models.User;
using BegumYatch.Core.Permissions;
using BegumYatch.Core.Repositories;
using BegumYatch.Core.Services;
using BegumYatch.Core.UnitOfWorks;
using BegumYatch.Repository;
using BegumYatch.Repository.Repositories;
using BegumYatch.Repository.Seeds;
using BegumYatch.Repository.UnitOfWorks;
using BegumYatch.Service.Mapping;
using BegumYatch.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BegumYatch.Service.Validations.User;
using Autofac.Core;
using System.Text.Json.Serialization;
using BegumYatch.Service.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<AddPersonelInfoValidator>());
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CrewAndPassengerUpdateValidator>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddIdentityExt();
builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

//builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAuthorization(options =>
{
    //basic
    options.AddPolicy("Permissions.AllEntity.Read", policy =>
    {
        policy.RequireClaim("permission", Permissions.Demand.Read); //yani ben tüm entity read policylerini buraya eklicem nerede sadece belirlenen entitylerdeki get kısmında
        policy.RequireClaim("permission", Permissions.Order.Read);
    });
    //advanced
    options.AddPolicy("Permissions.AllEntity.ReadCreateUpdate", policy =>
    {
        policy.RequireClaim("permission", Permissions.Demand.Read);
        policy.RequireClaim("permission", Permissions.Demand.Create);
        policy.RequireClaim("permission", Permissions.Demand.Update);

        policy.RequireClaim("permission", Permissions.Order.Read);
        policy.RequireClaim("permission", Permissions.Order.Create);
        policy.RequireClaim("permission", Permissions.Order.Update);

    });
    //admin
    options.AddPolicy("Permissions.AllEntity.ReadCreateUpdateDelete", policy =>
    {
        policy.RequireClaim("permission", Permissions.Demand.Read);
        policy.RequireClaim("permission", Permissions.Demand.Create);
        policy.RequireClaim("permission", Permissions.Demand.Update);
        policy.RequireClaim("permission", Permissions.Demand.Delete);

        policy.RequireClaim("permission", Permissions.Order.Read);
        policy.RequireClaim("permission", Permissions.Order.Create);
        policy.RequireClaim("permission", Permissions.Order.Update);
        policy.RequireClaim("permission", Permissions.Order.Delete);

    });
});
builder.Services.ConfigureApplicationCookie(opt =>
    {
        var cookieBuilder = new CookieBuilder();
        cookieBuilder.Name = "BegumAppCookie";
        opt.LoginPath = new PathString("/User/UserLogin");
        opt.LogoutPath = new PathString("/User/UserLogOut");
        opt.Cookie = cookieBuilder;
        opt.ExpireTimeSpan = TimeSpan.FromDays(60);
        opt.SlidingExpiration = true;
    });
builder.Services.ConfigureCors();  // by MERT

var app = builder.Build();

app.ConfigureGlobalExceptionHandler();  // by MERT

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await PermissionSeed.Seed(roleManager);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
//app.UseMiddleware<ResponseFormatterMiddleware>();
app.UseCors();  // mert
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
