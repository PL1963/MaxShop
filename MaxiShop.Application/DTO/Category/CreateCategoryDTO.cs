using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Category
{
    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
