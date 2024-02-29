﻿using BegumYatch.Core.DTOs.Error;
using System.ComponentModel.DataAnnotations;


namespace BegumYatch.Core.Models.Attributes
{
    public class MiarEnglishCharsAttribute : ValidationAttribute
    {
        private readonly char[]? _validSpecialChars;
        private readonly string _displayNameInTR;
        private readonly string _displayNameInEN;

        public MiarEnglishCharsAttribute(
            char[]? validSpecialChars,
            string displayNameInTR,
            string displayNameInEN)
        {
            _validSpecialChars = validSpecialChars;
            _displayNameInTR = displayNameInTR;
            _displayNameInEN = displayNameInEN;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            #region when value is null (return)
            if (value == null)
                return ValidationResult.Success;
            #endregion

            #region scan the chars of sent (throw)
            var sent = (string)value;

            foreach (var chr in sent)
            {
                #region when char isn't valid (throw)
                if (!(chr >= 'a' && chr <= 'z')
                    && !(chr >= 'A' && chr <= 'Z')
                    && !(chr >= '0' && chr <= '9')
                    && (_validSpecialChars != null
                        && !_validSpecialChars.Any(c => c.Equals(chr))))
                {
                    var first2Char = _displayNameInEN.Substring(0, 2);
                    var validSpecialCharsInStr = string.Join(' ', _validSpecialChars);

                    throw new MiarException(
                        400,
                        "FE-V-" + first2Char,
                        "Format Error - Validation - " + _displayNameInEN,
                        $"\"{_displayNameInTR}\", ingilizce karakterlerden oluşmalıdır ve [{validSpecialCharsInStr}] özel karakterlerini içerebilir.");
                }
                #endregion
            }
            #endregion

            return ValidationResult.Success;
        }
    }
}
