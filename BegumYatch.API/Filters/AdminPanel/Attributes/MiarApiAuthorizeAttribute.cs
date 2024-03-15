using Microsoft.AspNetCore.Mvc;


namespace BegumYatch.API.Filters.AdminPanel.Attributes
{
    public class MiarApiAuthorizeAttribute : TypeFilterAttribute
    {
        public MiarApiAuthorizeAttribute(string? roleNames = null)
            : base(typeof(MiarApiAuthorizeFilter))
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
