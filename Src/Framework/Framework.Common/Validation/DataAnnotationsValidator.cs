using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Validation
{
    public class DataAnnotationsValidator
    {
        /// <summary>
        /// Determines whether the specified object is valid
        /// </summary>
        /// <param name="object">The object to validate</param>
        /// <param name="results">A collection to hold each failed validation.</param>
        /// <returns>true if the object validates; otherwise, false.</returns>
        public static bool TryValidate(object @object, out ICollection<ValidationResult> results)
        {
            var context = new ValidationContext(@object, serviceProvider: null, items: null);
            results = new List<ValidationResult>();

            Validator.TryValidateObject(
                @object, context, results,
                validateAllProperties: true
            );

            foreach (var validationResult in results.ToArray())
            {
                var compositeValidationResult = validationResult as CompositeValidationResult;
                if (compositeValidationResult != null)
                {
                    foreach (var result in compositeValidationResult.Results)
                    {
                        var prefix = compositeValidationResult.MemberNames.FirstOrDefault();
                        var tempMembers =  result.MemberNames.ToArray();
                        results.Add(result);
                    }
                }
            }

            return !results.Any();
        }
    }
}
