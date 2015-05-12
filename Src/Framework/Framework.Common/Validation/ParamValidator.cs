using System;

namespace Framework.Common.Validation
{
    public class ParamValidator
    {
        public static void AssertIsNotNull(object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void AssertIsNotEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException(parameterName);
        }

        public static void AssertIsNotEmpty(Guid parameter, string parameterName)
        {
            if (Guid.Empty.Equals(parameter))
                throw new ArgumentException(parameterName);
        }
    }
}
