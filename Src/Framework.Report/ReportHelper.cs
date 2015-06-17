using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Reporting.WebForms;

namespace Framework.Report
{
    public class ReportHelper
    {
        public static byte[] RenderReport(ReportTypeEnum tipo, Dictionary<string, object> dataSources, string reportFullPath,
                                         string[] columnsToShow, IDictionary<string, string> parameters, IDictionary<string, string> images,
                                         decimal? pageWidth, decimal? pageHeight, decimal? marginTop, decimal? marginLeft,
                                         decimal? marginRight, decimal? marginBottom, out string mimeType)
        {
            var localReport = new LocalReport();

            LoadReportDefinition(localReport, reportFullPath, images);

            localReport.DataSources.Clear();

            foreach (var dataSourcesName in dataSources.Keys)
            {
                var reporteDataSource = new ReportDataSource(dataSourcesName, dataSources[dataSourcesName]);
                localReport.DataSources.Add(reporteDataSource);
            }

            localReport.EnableExternalImages = true;

            if (columnsToShow.Any())
            {
                var sb = new StringBuilder();
                foreach (string column in columnsToShow)
                {
                    sb.AppendFormat("#{0}#", column.Trim());
                }
                parameters.Add("Columns", sb.ToString());
            }

            foreach (var key in parameters.Keys)
            {
                var param = new ReportParameter();
                param.Name = key;
                param.Values.Add(parameters[key]);
                localReport.SetParameters(param);
            }

            string outputFormat = OutputFormat(tipo);
            string reporteType = ReportType(tipo);
            string encoding;
            string fileNameExtension;

            StringBuilder deviceInfo = new StringBuilder();

            deviceInfo.AppendFormat("<DeviceInfo>");
            deviceInfo.AppendFormat("<OutputFormat>{0}</OutputFormat>", outputFormat);

            if (pageWidth.HasValue)
            {
                deviceInfo.AppendFormat("  <PageWidth>{0}cm</PageWidth>", pageWidth);
            }

            if (pageHeight.HasValue)
            {
                deviceInfo.AppendFormat("  <PageHeight>{0}cm</PageHeight>", pageHeight);
            }

            if (marginTop.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginTop>{0}cm</MarginTop>", marginTop);
            }

            if (marginLeft.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginLeft>{0}cm</MarginLeft>", marginLeft);
            }

            if (marginRight.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginRight>{0}cm</MarginRight>", marginRight);
            }

            if (marginBottom.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginBottom>{0}cm</MarginBottom>", marginBottom);
            }

            deviceInfo.AppendLine("<Encoding>UTF-8</Encoding>");
            deviceInfo.AppendFormat("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;

            localReport.Refresh();

            return localReport.Render(
                reporteType,
                deviceInfo.ToString(),
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
        }

        public static byte[] RenderReport(ReportTypeEnum rerportType, Dictionary<string, object> dataSources,
                                          string reportFullPath, string[] columnsToShow, IDictionary<string, string> parameters,
                                          IDictionary<string, string> images, out string mimeType)
        {
            return RenderReport(rerportType, dataSources, reportFullPath, columnsToShow, parameters, images, null, null, null, null, null, null, out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum reporType, string dataSourceName, object dataSource,
                                          string reportePathCompleto, string[] columnasMostrar, IDictionary<string, string> parametros, 
                                          out string mimeType)
        {

            var dataSources = new Dictionary<string, object>();
            dataSources.Add(dataSourceName, dataSource);

            return RenderReport(reporType, dataSources, reportePathCompleto, columnasMostrar, parametros, null, out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum tipo, string dataSourceName, object dataSource, string reportePathCompleto,
                                          string[] columnasMostrar, out string mimeType)
        {
            return RenderReport(tipo, dataSourceName, dataSource, reportePathCompleto, columnasMostrar,
                new Dictionary<string, string>(), out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum tipo, string dataSourceName, object dataSource, string reportePathCompleto,
                                          IDictionary<string, string> parametros, out string mimeType)
        {
            return RenderReport(tipo, dataSourceName, dataSource, reportePathCompleto, new string[0], parametros, out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum tipo, string dataSourceName, object dataSource,
                                          string reportePathCompleto, out string mimeType)
        {
            return RenderReport(tipo, dataSourceName, dataSource, reportePathCompleto, new string[0],
                new Dictionary<string, string>(), out mimeType);
        }

        public static void LoadReportDefinition(LocalReport localReport, string reportePathCompleto, IDictionary<string, string> images)
        {
            string strReport = System.IO.File.ReadAllText(reportePathCompleto, System.Text.Encoding.Default);
            if (strReport.Contains("http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition"))
            {
                strReport =
                    strReport.Replace(
                        "<Report xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\" xmlns:cl=\"http://schemas.microsoft.com/sqlserver/reporting/2010/01/componentdefinition\" xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition\">",
                        "<Report xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\" xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition\">");
                strReport =
                    strReport.Replace("<ReportSections>", "").Replace("<ReportSection>", "").Replace(
                        "</ReportSection>", "").Replace("</ReportSections>", "");
            }

            if (images != null)
            {
                foreach (var imageName in images.Keys)
                {
                    strReport = ChangeEmbeddedImages(strReport, imageName, images[imageName]);
                }
            }

            byte[] bytReport = System.Text.Encoding.UTF8.GetBytes(strReport);
            localReport.LoadReportDefinition(new MemoryStream(bytReport));
        }

        public static string OutputFormat(ReportTypeEnum rerportType)
        {
            string typeString = string.Empty;
            switch (rerportType)
            {
                case ReportTypeEnum.Pdf:
                    typeString = "PDF";
                    break;
                case ReportTypeEnum.Excel:
                    typeString = "Excel";
                    break;
                case ReportTypeEnum.Word:
                    typeString = "Word";
                    break;
                case ReportTypeEnum.Imagen:
                    typeString = "Image";
                    break;
                case ReportTypeEnum.PNG:
                    typeString = "PNG";
                    break;
            }

            return typeString;
        }

        public static string ReportType(ReportTypeEnum rerportType)
        {
            string typeString = string.Empty;
            switch (rerportType)
            {
                case ReportTypeEnum.Pdf:
                    typeString = "PDF";
                    break;
                case ReportTypeEnum.Excel:
                    typeString = "Excel";
                    break;
                case ReportTypeEnum.Word:
                    typeString = "Word";
                    break;
                case ReportTypeEnum.Imagen:
                    typeString = "Image";
                    break;
                case ReportTypeEnum.PNG:
                    typeString = "Image";
                    break;
            }

            return typeString;
        }

        private static string ChangeEmbeddedImages(string xml, string imageName, string imageBase64)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

                var node = doc.SelectSingleNode("/ns:Report/ns:EmbeddedImages", nsMgr);
                if (node != null)
                {
                    foreach (XmlNode image in node.ChildNodes)
                    {
                        if (image.Attributes["Name"].Value.ToLower() == imageName.ToLower())
                        {
                            foreach (XmlNode elem in image.ChildNodes)
                            {
                                if (elem.Name == "ImageData")
                                {
                                    elem.InnerText = imageBase64;
                                }
                            }
                        }

                    }
                }

                StringWriter sw = new StringWriter();
                XmlTextWriter xtw = new XmlTextWriter(sw);
                doc.WriteTo(xtw);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
                /*
                 * Possible Exceptions:
                 *  System.ArgumentException
                 *  System.ArgumentNullException
                 *  System.InvalidOperationException
                 *  System.IO.DirectoryNotFoundException
                 *  System.IO.FileNotFoundException
                 *  System.IO.IOException
                 *  System.IO.PathTooLongException
                 *  System.NotSupportedException
                 *  System.Security.SecurityException
                 *  System.UnauthorizedAccessException
                 *  System.UriFormatException
                 *  System.Xml.XmlException
                 *  System.Xml.XPath.XPathException
                */
            }
        }
    }
}
