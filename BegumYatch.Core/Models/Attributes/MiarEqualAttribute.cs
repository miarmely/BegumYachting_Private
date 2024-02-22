using BegumYatch.Core.DTOs.Error;
using System.ComponentModel.DataAnnotations;


namespace Entities.Attributes
{
    public class MiarEqualAttribute : ValidationAttribute
    {
        private readonly object[] _values;
        private readonly string _displayNameInTR;
        private readonly string _displayNameInEN;

        public MiarEqualAttribute(
            object[] values, 
            string displayNameInTR,
            string displayNameInEN)
        {
            _values = values;
            _displayNameInTR = displayNameInTR;
            _displayNameInEN = displayNameInEN;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            #region when value is null (return)
            if (value == null)
                return null;
            #endregion

            #region when value is not exists in valid values (throw)			
            if (!_values.Contains(value))
            { 
                #region set error message
                var validValuesInString = string.Join(',', _values);

                var errorMessage = $"\"{_displayNameInTR}\" geçerli değil." +
                    $" Geçerli değerler: [ {validValuesInString} ]";
                #endregion

                #region throw error
                var first2CharOfDisplayNameInEN = _displayNameInEN.Substring(0, 2);

                throw new MiarException(new
                {
                    StatusCode = 400,
                    ErrorCode = $"FE-NV-{first2CharOfDisplayNameInEN}",
                    ErrorDescriptions = 
                        $"Format Error - Not Valid - {_displayNameInEN}",
                    ErrorMessage = errorMessage
                });
                #endregion
            }
            #endregion

            return ValidationResult.Success;
        }
    }
}
