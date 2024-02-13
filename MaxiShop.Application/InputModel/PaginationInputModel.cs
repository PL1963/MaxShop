using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.InputModel
{
    public class PaginationInputModel
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; } = 10;

    }
}
