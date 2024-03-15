using BegumYatch.Core.DTOs.Error;
using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;


namespace BegumYatch.API.Filters.AdminPanel
{
    public abstract class MiarAuthorize
    {
        private readonly string _claimTypeForRole = MiarClaimTypes.Role;
        protected readonly List<string> RoleNamesOnAttribute;
        protected string Language = "TR";  // default
        

        protected MiarAuthorize(List<string> roleNamesOnAttribute)
        {
            RoleNamesOnAttribute = roleNamesOnAttribute;
        }


        protected string ConvertErrorCodeToErrorMessageByLanguage(
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

        protected async Task CheckUserRolesAsync(
            AuthorizationFilterContext context)
        {
            #region control roles (THROW)
            // when there is a role restriction  
            if (RoleNamesOnAttribute.Count != 0)
            {
                #region get role names in token

                #region get roleClaims
                var roleClaims = context.HttpContext
                    .User
                    .Claims
                    .Where(c => c.Type.Equals(_claimTypeForRole));
                #endregion

                #region get role names in role claims
                var roleNamesOnToken = new List<string>();

                foreach (var roleClaim in roleClaims)
                    roleNamesOnToken.Add(roleClaim.Value);
                #endregion

                #endregion

                #region control role names (THROW)

                #region compare role names 
                foreach (var roleName in roleNamesOnToken)
                {
                    #region when user role is valid
                    if (RoleNamesOnAttribute.Contains(roleName))
                        return;
                    #endregion
                }
                #endregion

                #region when role is invalid (throw)
                throw new MiarException(
                    403,
                    "AE-F",
                    "Authorization Error - Forbidden",
                    ConvertErrorCodeToErrorMessageByLanguage(Language, "AE-F"));
                #endregion

                #endregion
            }
            #endregion
        }


        /*  CONTROL EXPIRES
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
    }

}