using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToValueAttribute : ValidationAttribute
    {
        public object ValueToCompare { get; private set; }
        public Type ValueType { get; private set; }

        public NotEqualToValueAttribute(object value, Type type)
            : base()
        {
            ValueToCompare = value;
            ValueType = type;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value.ToString().Equals(ValueToCompare.ToString()))
                {
                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName), new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
