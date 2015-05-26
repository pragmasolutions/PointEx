using System;
using System.Globalization;
using System.Web.Mvc;

namespace Framework.Common.Web.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);

            return date;
        }
    }
    public class NullableDateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value != null)
            {
                var date = value.ConvertTo(typeof(DateTime), CultureInfo.CurrentCulture);
                return date;
            }
            return null;
        }
    }
}
