using BegumYatch.Core.Enums.AdminPanel;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data;


namespace BegumYatch.API.Filters.AdminPanel
{
    public abstract class MiarAuthorize
    {
        private readonly string _claimTypeForRole = MiarClaimTypes.RoleName;
        protected readonly List<string> RoleNamesOnAttribute;
        protected string Language = "TR";  // default

        protected MiarAuthorize(List<string> roleNamesOnAttribute)
        {
            RoleNamesOnAttribute = roleNamesOnAttribute;
        }


        protected async Task<bool> IsUserRolesValidAsync(
            AuthorizationFilterContext context)
        {
            #region control roles
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
                var roleNamesOnClaims= new List<string>();

                foreach (var roleClaim in roleClaims)
					roleNamesOnClaims.Add(roleClaim.Value);
                #endregion

                #endregion

                #region compare role names 
                foreach (var roleName in roleNamesOnClaims)
                {
                    #region when user role is valid
                    if (RoleNamesOnAttribute.Contains(roleName))
                        return true;
                    #endregion
                }
                #endregion

                return false;
            }
            #endregion

            return true;
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