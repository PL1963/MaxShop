using MaxiShop.Application.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDTO> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();

        Task<CategoryDTO> CreateAsync(CreateCategoryDTO categoryDTO);
        Task UpdateAsync(UpdateCategoryDTO updatecategoryDTO);
        Task DeleteAsync(int id);
    }
}
