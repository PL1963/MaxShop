using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Comman
{
    public class APIWarning
    {
        public string Description { get; set; }
        public APIWarning(string description)
        {
            Description = description;
        }
    }
}
