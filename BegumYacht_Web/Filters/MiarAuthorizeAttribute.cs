using Microsoft.AspNetCore.Mvc;


namespace BegumYacht_Web.Filters
{
	public class MiarAuthorizeAttribute : TypeFilterAttribute
	{
		public MiarAuthorizeAttribute(string? roleNames = null)
			: base(typeof(MiarAuthorizeFilter))
		{
            #region set role names
            // string to list
            var roleNamesInList = roleNames == null ?
				new List<string>()  // when all roles is valid (empty list)
				: roleNames.Split(',').ToList();  // for specific roles is valid

			// set constructor parameters of "AuthorizationFilter"
			base.Arguments = new object[] { roleNamesInList };
			#endregion
		}
	}
}
