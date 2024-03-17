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

            await base.CheckUserRolesAsync(context);
        }
    }
}
