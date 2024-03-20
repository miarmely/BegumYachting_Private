using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace BegumYatch.API.Filters.AdminPanel
{
    public class MiarWebAuthorizeFilter : MiarAuthorize, IAsyncAuthorizationFilter
    {
        public MiarWebAuthorizeFilter(List<string> roleNamesOnAttribute)
            : base(roleNamesOnAttribute)
        { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            #region get language on http context
            var languageInContext = context.HttpContext
                .Items
                .FirstOrDefault(i => i.Key.Equals("language"))
                .Value
                as string;

            // change default language
            if (languageInContext != null)
                base.Language = languageInContext;
            #endregion

            #region when user not signin (REDIRECT)
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // redirect to login page
                context.Result = new RedirectResult("/login", true);
                return;
            }
            #endregion

            #region when user roles is invalid (LOGOUT + REDIRECT)
            else if (!await base.IsUserRolesValidAsync(context))
            {
				// senario: current user role is admin but user can be change
                // his role from Admin to User.  Thats why i am logging out..
				await context.HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

				// redirect to login page
				context.Result = new RedirectResult("/login", true);
			}
			#endregion
		}
	}
}