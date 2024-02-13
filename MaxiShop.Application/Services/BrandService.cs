using AutoMapper;
using MaxiShop.Application.DTO.Brand;
using MaxiShop.Application.DTO.Category;
using MaxiShop.Application.Exceptions;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Domain.Contracts;
using MaxiShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BrandDTO> CreateAsync(CreateBrandDTO brandDTO)
        {
            var validator = new CreateBrandDtoValidator();

            var validationResult = await validator.ValidateAsync(brandDTO);

            if (validationResult.Errors.Any()) 
            {
             throw new BadRequestException("Invalid Brand Input", validationResult);
            }

            var brands = _mapper.Map<Brand>(brandDTO);

            var createdEntity = await _brandRepository.CreateAsync(brands);


            var entity = _mapper.Map<BrandDTO>(createdEntity);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(x => x.Id == id);
            await _brandRepository.DeleteAsync(brand);
        }

        public async Task<IEnumerable<BrandDTO>> GetAllAsync()
        {
            var brand = await _brandRepository.GetAllAsync();

            return _mapper.Map<List<BrandDTO>>(brand);
        }

        public async Task<BrandDTO> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<BrandDTO>(brand);
        }   

        public async Task UpdateAsync(UpdateBrandDTO updatebrandDTO)
        {
            var brand = _mapper.Map<Brand>(updatebrandDTO);

            await _brandRepository.UpdateAsync(brand);
        }

        
    }
}
