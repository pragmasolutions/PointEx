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
    }

    public abstract class ThemeEnum
    {
        public const string Jovenes = "Jovenes";
        public const string TekovePoti = "TekovePoti";
    }
}