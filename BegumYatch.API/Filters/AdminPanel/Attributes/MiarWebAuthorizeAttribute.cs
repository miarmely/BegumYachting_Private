using Microsoft.AspNetCore.Mvc;


namespace BegumYatch.API.Filters.AdminPanel.Attributes
{
    public class MiarWebAuthorizeAttribute : TypeFilterAttribute
    {
        public MiarWebAuthorizeAttribute(string? roleNames = null)
            : base(typeof(MiarWebAuthorizeFilter))
        {
            #region set role names
            // string to list
            var roleNamesInList = roleNames == null ?
                new List<string>()  // when all roles is valid (empty list)
                : roleNames.Split(',').ToList();  // for specific roles is valid

            // set constructor parameters of "AuthorizationFilter"
            Arguments = new object[] { roleNamesInList };
            #endregion
        }
    }
}
