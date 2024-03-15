using BegumYatch.Core.DTOs.Error;
using Microsoft.AspNetCore.Mvc.Filters;


namespace BegumYatch.API.Filters.AdminPanel
{
    public class MiarApiAuthorizeFilter : MiarAuthorize, IAsyncAuthorizationFilter
    {
        public MiarApiAuthorizeFilter(List<string> roleNamesOnAttribute)
            : base(roleNamesOnAttribute)
        { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            #region get language from query
            var languageInQuery = context.HttpContext
                .Request
                .Query
                .FirstOrDefault(q => q.Key.Equals("language"))
                .Value
                .ToString();

            // change default language
            if (!languageInQuery.Equals(""))
                base.Language = languageInQuery.ToUpper();
            #endregion

            #region when user not signin (THROW)
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                throw new MiarException(
                401,
                "AE-U",
                "Authorization Error - Unauthorized",
                ConvertErrorCodeToErrorMessageByLanguage(base.Language, "AE-U"));
            #endregion

            await base.CheckUserRolesAsync(context);
        }
    }
}
