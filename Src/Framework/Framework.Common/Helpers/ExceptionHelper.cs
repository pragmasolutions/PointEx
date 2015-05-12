using System;

namespace Framework.Common.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            return GetInnerException(ex.InnerException);
        }
    }
}
