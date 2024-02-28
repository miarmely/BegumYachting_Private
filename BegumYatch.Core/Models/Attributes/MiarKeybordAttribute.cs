using BegumYatch.Core.DTOs.Error;
using System.ComponentModel.DataAnnotations;


namespace BegumYatch.Core.Models.Attributes
{
    public class MiarEnglishCharsAttribute : ValidationAttribute
    {
        private readonly char[] _validSpecialChars;

        public MiarEnglishCharsAttribute(
            char[] validSpecialChars,
            string _displayName)
        {
            _validSpecialChars = validSpecialChars;
        }

        protected override ValidationResult? IsValid(
            object? value, 
            ValidationContext validationContext)
        {
            #region when value is null (return)
            if (value == null)
                return ValidationResult.Success;
            #endregion

            var sent = (string)value;

            foreach(var chr in sent)
            {
                if(!(chr >= 'a' && chr <= 'z')
                    || !(chr >= 'A' && chr <= 'Z')
                    || !_validSpecialChars.Any(c => c.Equals(chr)))
                    throw new MiarException(
                        400,
                        "FE-V-En",
                        "Format Error")

            }
        }
    }
}
