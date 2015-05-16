using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Common.Extentensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Returns the center point of the rectangle
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center point of the rectangle</returns>
        public static Point Center(this Rectangle r)
        {
            return new Point((r.Left + r.Right)/2, (r.Top + r.Bottom)/2);
        }

        /// <summary>
        /// Returns the center right point of the rectangle
        /// i.e. the right hand edge, centered vertically.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center right point of the rectangle</returns>
        public static Point CenterRight(this Rectangle r)
        {
            return new Point(r.Right, (r.Top + r.Bottom)/2);
        }

        /// <summary>
        /// Returns the center left point of the rectangle
        /// i.e. the left hand edge, centered vertically.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center left point of the rectangle</returns>
        public static Point CenterLeft(this Rectangle r)
        {
            return new Point(r.Left, (r.Top + r.Bottom)/2);
        }

        /// <summary>
        /// Returns the center bottom point of the rectangle
        /// i.e. the bottom edge, centered horizontally.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center bottom point of the rectangle</returns>
        public static Point CenterBottom(this Rectangle r)
        {
            return new Point((r.Left + r.Right)/2, r.Bottom);
        }

        /// <summary>
        /// Returns the center top point of the rectangle
        /// i.e. the topedge, centered horizontally.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center top point of the rectangle</returns>
        public static Point CenterTop(this Rectangle r)
        {
            return new Point((r.Left + r.Right)/2, r.Top);
        }
    }
}
