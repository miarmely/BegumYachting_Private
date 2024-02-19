var builder = WebApplication.CreateBuilder(args);

#region add services
builder.Services.AddControllersWithViews();
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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=Login}/{id?}");
app.Run();
#endregion