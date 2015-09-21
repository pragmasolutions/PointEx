using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PointEx.Web.Configuration
{
    public static class AppSettings
    {
        public static string Theme
        {
            get { return ConfigurationManager.AppSettings["Theme"]; }
        }

        public static string ThemeTitle
        {
            get
            {
                return Theme == "Jovenes" ? "TARJETA JÓVEN" : "TEKOVÉ POTÍ";
            }
        }

        public static bool ShowEducationalInstitution
        {
            get { return AppSettings.Theme == ThemeEnum.Jovenes; }
        }
        
        public static string SiteBaseUrl
        {
            get
            {
                string url = string.Empty;
                HttpRequest request = HttpContext.Current.Request;

                if (request.IsSecureConnection)
                    url = "https://";
                else
                    url = "http://";

                url += request["HTTP_HOST"] + "/";

                return url;
            }
        }
    }

    public abstract class ThemeEnum
    {
        public const string Jovenes = "TarjetaVerde";
        public const string TekovePoti = "TekovePoti";
    }
}