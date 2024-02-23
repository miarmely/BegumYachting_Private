﻿using BegumYatch.Core.DTOs.Error;
using System.ComponentModel.DataAnnotations;


namespace Entities.Attributes
{
	public class MiarPhoneAttribute : ValidationAttribute
	{
		private readonly ErrorDto _errorDto = new ErrorDto
		{
			StatusCode = 400,
			ErrorCode = "FE-U-P",
			ErrorDescription = "Format Error - User - Phone",
			ErrorMessage = "\"Telefon\" geçerli değil"
		};

		protected override ValidationResult? IsValid(
			object? value,
			ValidationContext validationContext)
		{
			#region when value is null (return)
			if (value == null)
				return null;
			#endregion

			#region control left chunk of string (throw)
			var phoneInStr = value as string;

			var leftChunkOfString = phoneInStr
				.Substring(0, phoneInStr.Length / 2);

			if (!int.TryParse(leftChunkOfString, out int _))
				throw new MiarException(_errorDto);
			#endregion

			#region control right chunk of string (throw)
			var rightChunkOfString = phoneInStr
				.Substring(phoneInStr.Length / 2);

			if (!int.TryParse(rightChunkOfString, out int _))
                throw new MiarException(_errorDto);
            #endregion

            return ValidationResult.Success;
		}
	}
}