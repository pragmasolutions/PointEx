using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace PointEx.Web
{
	public class ValidationConfig
	{
		public static void Config()
		{
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "PointExGlobalResources";
            DefaultModelBinder.ResourceClassKey = "PointExGlobalResources";
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredAttribute),
                typeof(ResourseBaseRequiredAttributeAdapter));
		}
	}

    public class ResourseBaseRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public ResourseBaseRequiredAttributeAdapter(ModelMetadata metadata,
                                          ControllerContext context,
                                          RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(PointExGlobalResources);
            attribute.ErrorMessageResourceName = "Required";
        }
    }
}