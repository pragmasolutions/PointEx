using System;

namespace Framework.Data.Helpers
{
    /// <summary>
    /// Class used to save paged queries information.
    /// </summary>
    public class PagedMetadata
    {
        public PagedMetadata() { }

        public PagedMetadata(int totalItemCount, int pageSize, int pageNumber)
        {
            PageCount = (int)Math.Ceiling(totalItemCount / (float)pageSize);
            PageNumber = pageNumber;
            TotalItemCount = totalItemCount;
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < PageCount;
        }

        /// <summary>
        /// Total number of subsets
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Total number of items in the superset
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// One-based index of this subset
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a first page.
        /// </summary>
        public bool IsFirstPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a last page.
        /// </summary>
        public bool IsLastPage { get; set; }
    }
}
