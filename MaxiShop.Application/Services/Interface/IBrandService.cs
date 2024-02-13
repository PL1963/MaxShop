using MaxiShop.Application.DTO.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface IBrandService
    {
        Task<BrandDTO> GetByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllAsync();
        Task<BrandDTO> CreateAsync(CreateBrandDTO brandDTO);
        Task UpdateAsync(UpdateBrandDTO updatebrandDTO);
        Task DeleteAsync(int id);
    }
}
