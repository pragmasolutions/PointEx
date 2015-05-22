using System;
using System.Web.Mvc;

namespace Framework.Common.Web.Metadata
{
    public class FileAttribute : Attribute, IMetadataAware
    {
        public const string HttpPostedFileBasePropertyKey = "HttpPostedFileBaseProperty";

        public string HttpPostedFileBaseProperty { get; set; }

        public void OnMetadataCreated(System.Web.Mvc.ModelMetadata metadata)
        {
            metadata.AdditionalValues.Add(HttpPostedFileBasePropertyKey, HttpPostedFileBaseProperty);
        }
    }
}