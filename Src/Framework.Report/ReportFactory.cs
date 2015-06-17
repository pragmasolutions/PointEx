using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Framework.Report
{
    public class ReportFactory
    {
        public decimal? PageWidth { get; private set; }

        public decimal? PageHeight { get; private set; }

        public decimal? MarginTop { get; private set; }

        public decimal? MarginLeft { get; private set; }

        public decimal? MarginRight { get; private set; }

        public decimal? MarginBottom { get; private set; }

        public string PathCompleto { get; private set; }

        public Dictionary<string, object> DataSources { get; private set; }

        public Dictionary<string, string> Parametros { get; private set; }

        public Dictionary<string, string> Images { get; private set; }

        public List<string> ColumnsShow { get; private set; }

        public String MimeType { get; private set; }

        public ReportFactory()
        {
            this.DataSources = new Dictionary<string, object>();
            this.Parametros = new Dictionary<string, string>();
            this.Images = new Dictionary<string, string>();
            this.ColumnsShow = new List<string>();
        }

        public ReportFactory SetFullPath(string fullPath)
        {
            this.PathCompleto = fullPath;
            return this;
        }

        public ReportFactory SetDataSource(Dictionary<string, object> dataSources)
        {
            foreach (var clave in dataSources.Keys)
            {
                SetDataSource(clave, dataSources[clave]);
            }

            return this;
        }

        public ReportFactory SetDimensions(decimal pageWidth, decimal pageHeight, decimal marginTop, decimal marginLeft, decimal marginRight, decimal marginBottom)
        {
            this.PageWidth = pageWidth;
            this.PageHeight = pageHeight;
            this.MarginTop = marginTop;
            this.MarginLeft = marginLeft;
            this.MarginRight = marginRight;
            this.MarginBottom = marginBottom;
            return this;
        }

        public ReportFactory SetDataSource(string dataSourceName, object datasource)
        {
            if (String.IsNullOrEmpty(dataSourceName))
            {
                throw new ApplicationException("");
            }

            if (this.DataSources.ContainsKey(dataSourceName))
            {
                this.DataSources[dataSourceName] = datasource;
            }
            else
            {
                this.DataSources.Add(dataSourceName, datasource);
            }

            return this;
        }

        public ReportFactory SetParameter(Dictionary<string, string> param)
        {
            foreach (var clave in param.Keys)
            {
                SetParameter(clave, param[clave]);
            }

            return this;
        }

        public ReportFactory SetParameter(string clave, string valor)
        {

            if (String.IsNullOrEmpty(clave))
            {
                throw new ApplicationException("");
            }

            if (this.Parametros.ContainsKey(clave))
            {
                this.Parametros[clave] = valor;
            }
            else
            {
                this.Parametros.Add(clave, valor);
            }

            return this;
        }

        public ReportFactory SetImagen(Dictionary<string, string> imagenes)
        {
            foreach (var clave in imagenes.Keys)
            {
                SetParameter(clave, imagenes[clave]);
            }

            return this;
        }

        public ReportFactory SetImagen(string clave, string pathImagen)
        {

            if (String.IsNullOrEmpty(pathImagen))
            {
                throw new ApplicationException("");
            }

            if (!File.Exists(pathImagen))
            {
                throw new ApplicationException(String.Format("", pathImagen));
            }

            var bitmap = new Bitmap(pathImagen);
            string imagenEnBase64;
            ImageFormat format = ImageFormat.Png;
            string formatExtension = "png";

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, format);
                imagenEnBase64 = Convert.ToBase64String(ms.ToArray());
            }

            if (this.Images.ContainsKey(clave))
            {
                this.Images[clave] = imagenEnBase64;
            }
            else
            {
                this.Images.Add(clave, imagenEnBase64);
            }

            return this;
        }

        public ReportFactory SetColumToShow(List<string> columnasAMostrar)
        {

            foreach (var columna in columnasAMostrar)
            {
                SetColumToShow(columna);
            }

            return this;
        }

        public ReportFactory SetColumToShow(string[] columnasAMostrar)
        {
            SetColumToShow(columnasAMostrar.ToList());

            return this;
        }

        public ReportFactory SetColumToShow(string columna)
        {
            if (!this.ColumnsShow.Contains(columna))
            {
                this.ColumnsShow.Add(columna);
            }

            return this;
        }

        public ReportFactory SetColumnaAMostrarDesdeCsv(string cadenaSeparaPorComa)
        {
            var columnas = cadenaSeparaPorComa.Split(',');
            return SetColumToShow(columnas);
        }

        #region Render

        public byte[] Render(ReportTypeEnum reportType)
        {
            string mimeTypeOut = string.Empty;
            byte[] file = ReportHelper.RenderReport(
                reportType,
                this.DataSources,
                this.PathCompleto.ToString(),
                this.ColumnsShow.ToArray(),
                this.Parametros,
                this.Images, this.PageWidth, this.PageHeight, this.MarginTop, this.MarginLeft, this.MarginRight,
                this.MarginBottom,
                out mimeTypeOut);

            this.MimeType = mimeTypeOut;
            return file;
        }

        #endregion
    }
}
