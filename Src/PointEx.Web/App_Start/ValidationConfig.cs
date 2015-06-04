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

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RangeAttribute),
                typeof(ResourseBaseRangeAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
               typeof(EmailAddressAttribute),
               typeof(ResourseBaseEmailAddressAttributeAdapter));
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

    public class ResourseBaseRangeAttributeAdapter : RangeAttributeAdapter
    {
        public ResourseBaseRangeAttributeAdapter(ModelMetadata metadata,
                                          ControllerContext context,
                                          RangeAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(PointExGlobalResources);
            attribute.ErrorMessageResourceName = "Range";
        }
    }

    public class ResourseBaseEmailAddressAttributeAdapter : DataAnnotationsModelValidator<EmailAddressAttribute>
    {
        public ResourseBaseEmailAddressAttributeAdapter(ModelMetadata metadata,
                                          ControllerContext context,
                                          EmailAddressAttribute attribute)
            : base(metadata, context,attribute)
        {
            attribute.ErrorMessageResourceType = typeof(PointExGlobalResources);
            attribute.ErrorMessageResourceName = "EmailAddress";
        }
    }

    
}