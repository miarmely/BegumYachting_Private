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

			#region when user roles is invalid (THROW)
			if (!await base.IsUserRolesValidAsync(context))
				throw new MiarException(
					403,
					"AE-F",
					"Authorization Error - Forbidden",
					ConvertErrorCodeToErrorMessageByLanguage(Language, "AE-F"));
			#endregion
        }

		private string ConvertErrorCodeToErrorMessageByLanguage(
		  string language,
		  string errorCode)
		{
			return language switch
			{
				"TR" => errorCode switch
				{
					"AE-U" => "oturum açmadınız",
					"AE-F" => "yetkiniz yok",
					"AE-E" => "oturum süreniz doldu"
				},
				"EN" => errorCode switch
				{
					"AE-U" => "you are not logged in",
					"AE-F" => "you don't have permission",
					"AE-E" => "your session time expired"
				}
			};
		}
	}
}