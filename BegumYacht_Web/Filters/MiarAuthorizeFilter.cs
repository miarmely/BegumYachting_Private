using BegumYatch.Core.DTOs.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;
using System.Security.Claims;


namespace BegumYacht_Web.Filters
{
	public partial class MiarAuthorizeFilter : IAsyncAuthorizationFilter
	{
		private readonly List<string> _roleNamesOnAttribute;
		private readonly string _webProjectName = "BegumYacht_Web";
		
		public MiarAuthorizeFilter(List<string> roleNamesOnAttribute) =>
			_roleNamesOnAttribute = roleNamesOnAttribute;

		public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
			#region get projectName
			var actionDetails = context.ActionDescriptor.DisplayName.Split('.');
			var projectName = actionDetails[0];
			#endregion

			#region get language
			string? language = "TR";  // default error language

			#region for web
			if (projectName.Equals(_webProjectName))
			{
				#region get language on http context
				var languageInContext = context.HttpContext
					.Items
					.FirstOrDefault(i => i.Key.Equals("language"))
					.Value
					as string;

				// change default language
				if (languageInContext != null)
					language = languageInContext;
				#endregion
			}
			#endregion

			#region for mobile
			else
			{
				#region get language from query
				var languageInQuery = context.HttpContext
					.Request
					.Query
					.FirstOrDefault(q => q.Key.Equals("language"))
					.Value
					.ToString();
				#endregion

				#region when language is exists in query
				if (!languageInQuery.Equals(""))
					language = languageInQuery.ToUpper();
				#endregion
			}
            #endregion

            #endregion

            #region when user not sign in (THROW)
            if (!context.HttpContext.User.Identity.IsAuthenticated)
			{
				#region for web (REDIRECT)
				if (projectName.Equals(_webProjectName))
				{
					#region redirect to login page
					context.Result = new RedirectToActionResult(
						"Login",
						"Authentication",
						null);
					#endregion

					return;
				}
				#endregion

				#region for mobile (THROW)
				else  // projectName= "BegumYatch.API"
					throw new MiarException(
						401,
						"AE-U",
						"Authorization Error - Unauthorized",
						ConvertErrorCodeToErrorMessageByLanguage(language, "AE-U"));
				#endregion
			}
            #endregion

			/*
            #region control expires (THROW)

            #region get expires claim
            var expiresClaim = context.HttpContext.User.Claims
				.FirstOrDefault(c => c.Type.Equals("exp"));
            #endregion

            #region when expires time finished (THROW) 
            var expiresInLong = long.Parse(expiresClaim.Value);
			var expiresInDateTime = DateTimeOffset
				.FromUnixTimeSeconds(expiresInLong)
				.DateTime;

			if (expiresInDateTime < DateTime.UtcNow)
			{
				#region for web (REDIRECT)
				if (projectName.Equals(_webProjectName))
				{
					context.Result = new RedirectToActionResult(
						"Login",
						"Authentication",
						null);

					return;
				}
                #endregion

                #region for mobile (THROW)
                else
                    throw new MiarException(
						404,
						"AE-E",
						"Authentication Error - Expires",
						ConvertErrorCodeToErrorMessageByLanguage(language, "AE-E"));
				#endregion
			}
			#endregion

			#endregion
			*/

			#region control roles (THROW)
			// when there is a role restriction  
			if(_roleNamesOnAttribute.Count != 0)
			{
				#region get role claims on token

				#region get roleClaims
				var roleClaims = context.HttpContext.User
					.Claims
					.Where(c => c.Type.Equals(ClaimTypes.Role));
				#endregion

				#region get roleName in roleClaims
				var roleNamesOnToken = new List<string>();

				foreach (var roleClaim in roleClaims)
					roleNamesOnToken.Add(roleClaim.Value);
                #endregion

                #endregion

                #region control role names in role claims (THROW)

                #region compare role names 
                foreach (var roleNameOnToken in roleNamesOnToken)
				{
					// when authority is has
					if (_roleNamesOnAttribute.Contains(roleNameOnToken))
						return;
				}
				#endregion

				#region when role level isn't enough (throw)
				throw new MiarException(
					403,
					"AE-F",
					"Authorization Error - Forbidden",
					ConvertErrorCodeToErrorMessageByLanguage(language, "AE-F"));
				#endregion

				#endregion
			}
			#endregion
		}
	}

	public partial class MiarAuthorizeFilter  // private
	{
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
