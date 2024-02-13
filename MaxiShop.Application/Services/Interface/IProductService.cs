using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModel;
using MaxiShop.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services.Interface
{
    public interface IProductService
    {
        Task<PaginationVM<ProductDTO>> GetPagination(PaginationInputModel paginationInputModel);
        Task<ProductDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<IEnumerable<ProductDTO>> GetAllByFilterAsync(int? categoryId, int? brandId);

        Task<ProductDTO> CreateAsync(CreateProductDTO productDTO);
        Task UpdateAsync(UpdateProductDTO updateproductDTO);
        Task DeleteAsync(int id);
    }
}
