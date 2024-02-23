using BegumYatch.Core.DTOs.Error;
using System.ComponentModel.DataAnnotations;


namespace Entities.Attributes
{
	public class MiarEmailAttribute : ValidationAttribute
	{
		private readonly ErrorDto _errorDto = new ErrorDto
		{
			StatusCode = 400,
			ErrorCode = "FE-U-E",
			ErrorDescription = "Format Error - User - Email",
			ErrorMessage = "\"Email\" geçerli değil"
		};

		protected override ValidationResult? IsValid(
			object? value,
			ValidationContext validationContext)
		{
			#region when value is null (return)
			if (value == null)
				return null;
			#endregion

			#region control '@' (throw)
			var email = value as string;
			var totalAtQuantity = email.Count(c => c.Equals('@'));
			var atIndex = email.IndexOf('@');

			if (totalAtQuantity == 0  // when '@' not found 
				|| totalAtQuantity > 1  // when total '@' quantity more than 1
				|| atIndex == email.Length - 1) // when '@' in last index.
				throw new MiarException(_errorDto);
			#endregion

			#region control email extension (throw)

			#region length control (throw)
			var emailExtension = email.Substring(atIndex + 1); // ex => gmail.com

			if (emailExtension.Length < 3)  // min valid extension: @a.b
				throw new MiarException(_errorDto);
			#endregion

			#region control '.' (throw)
			var totalDotQuantity = email.Count(c => c.Equals('.'));
			var dotIndex = emailExtension.IndexOf('.');

			if (totalDotQuantity == 0 // when '.' not found
				|| dotIndex == 0  // when no char between '@' and '.'
				|| dotIndex == emailExtension.Length - 1)  // when '.' last Index
				throw new MiarException(_errorDto);
			#endregion

			#endregion

			return ValidationResult.Success;
		}
	}
}
