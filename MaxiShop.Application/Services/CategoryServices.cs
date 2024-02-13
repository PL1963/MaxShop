using AutoMapper;
using MaxiShop.Application.DTO.Category;
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
    public class CategoryServices : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);

            var createdEntity = await _categoryRepository.CreateAsync(category);


            var entity = _mapper.Map<CategoryDTO>(createdEntity);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(x => x.Id == id);
            await _categoryRepository.DeleteAsync(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(x => x.Id == id);

            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateAsync(UpdateCategoryDTO updatecategoryDTO)
        {
            var category = _mapper.Map<Category>(updatecategoryDTO);

            await _categoryRepository.UpdateAsync(category);
        }
    }
}
