using AutoMapper;
using MaxiShop.Application.DTO.Product;
using MaxiShop.Application.InputModel;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Application.ViewModel;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPaginationService<ProductDTO,Product> _paginationService;

        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IPaginationService<ProductDTO, Product> paginationService, IMapper mapper)
        {
            _productRepository = productRepository;
            _paginationService = paginationService;
            _mapper = mapper;
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);

            var createdEntity = await _productRepository.CreateAsync(product);


            var entity = _mapper.Map<ProductDTO>(createdEntity);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(x => x.Id == id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var product = await _productRepository.GetAllProductAsync();

            return _mapper.Map<List<ProductDTO>>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllByFilterAsync(int? categoryId, int? brandId)
        {
            var data = await _productRepository.GetAllProductAsync();

            IEnumerable<Product> query = data; 

            if(categoryId > 0)
            {
                query = query.Where(x=> x.CategoryId == categoryId);

                if(brandId > 0)
                {
                    query = query.Where(x => x.BrandId == brandId);

                }
                
            }
            var result = _mapper.Map<List<ProductDTO>>(query);
            return result;
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<PaginationVM<ProductDTO>> GetPagination(PaginationInputModel pagination)
        {
            var source = await _productRepository.GetAllProductAsync();

            var result = _paginationService.GetPagination(source, pagination);

            return result;
        }

        public async Task UpdateAsync(UpdateProductDTO updateproductDTO)
        {
            var product = _mapper.Map<Product>(updateproductDTO);

            await _productRepository.UpdateAsync(product);
        }
    }
}
