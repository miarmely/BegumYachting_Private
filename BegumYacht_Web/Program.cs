using BegumYacht_Web.Controllers;
using BegumYacht_Web.Extensions;
using Microsoft.AspNetCore.Components;

#region add services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.ConfigureConfigModels(builder.Configuration);
builder.Services.ConfigureAuthentication();

var app = builder.Build();
#endregion

#region when mode is not development
if (!app.Environment.IsDevelopment())
{
	app.UseHsts();
}
#endregion

#region run middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.ConfigureMapControllerRoute();
app.Run();
#endregion