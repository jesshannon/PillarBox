using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models
{
    public class PaginatedListModel<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IList<T> Items { get; set; }

    }
}
