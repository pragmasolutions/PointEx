using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace Framework.Common.Validation
{
    public class NumericGreaterThanAttribute : ValidationAttribute
    {
        private const string greaterThanErrorMessage = "{0} must be greater than {1}.";
        private const string greaterThanOrEqualToErrorMessage = "{0} must be greater than or equal to {1}.";        

        public string OtherProperty { get; private set; }

        private bool allowEquality;

        public bool AllowEquality   
        {
            get { return this.allowEquality; }
            set
            {
                this.allowEquality = value;
                this.ErrorMessage = (value ? greaterThanOrEqualToErrorMessage : greaterThanErrorMessage);
            }
        }        

        public NumericGreaterThanAttribute(string otherProperty)
            : base(greaterThanErrorMessage)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }

            this.OtherProperty = otherProperty;            
        }        

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.OtherProperty);
        }

        public string FormatErrorMessage(decimal otherPropertyValue)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, otherPropertyValue);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);   
            
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty), new[] { validationContext.MemberName });
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            decimal decValue;
            decimal decOtherPropertyValue;

            //Do not check validation if the values have not been set.
            if (value == null || otherPropertyValue == null)
            {
                return null;
            }

            // Check to ensure the validating property is numeric
            if (!decimal.TryParse(value.ToString(), out decValue))
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName), new[] { validationContext.MemberName });
            }

            // Check to ensure the other property is numeric
            if (!decimal.TryParse(otherPropertyValue.ToString(), out decOtherPropertyValue))
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty), new[] { validationContext.MemberName });
            }

            // Check for equality
            if (AllowEquality && decValue == decOtherPropertyValue)
            {
                return null;
            }
            // Check to see if the value is less than the other property value
            else if (decValue < decOtherPropertyValue)
            {
                return new ValidationResult(FormatErrorMessage(decOtherPropertyValue), new[] { validationContext.MemberName });
            }                     

            return null;
        }

        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("Value cannot be null or empty.", "property");
            }
            return "*." + property;
        }
    }
}