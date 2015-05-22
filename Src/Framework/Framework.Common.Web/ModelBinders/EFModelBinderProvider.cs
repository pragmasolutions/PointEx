using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Validation;

namespace Framework.Common.Web.ModelBinders
{
    public class DbGeographyModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
            {
                return null;
            }
            
            if(!Regex.IsMatch(valueProviderResult.AttemptedValue, RegexPatterns.Location))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,"The Location is not valid");
                return null;
            }

            string[] latLongStr = valueProviderResult.AttemptedValue.Split(',');
            string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            DbGeography result = valueProviderResult == null ? null :
                DbGeography.FromText(point, 4326);
            return result;
        }
    }

    public class EFModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(DbGeography))
            {
                return new DbGeographyModelBinder();
            }
            return null;
        }
    }
}
