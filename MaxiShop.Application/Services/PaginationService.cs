using AutoMapper;
using MaxiShop.Application.InputModel;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class PaginationService<T, S> : IPaginationService<T, S> where T : class
    {
        private readonly IMapper _mapper;
        public PaginationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PaginationVM<T> GetPagination(List<S> source, PaginationInputModel pagination)
        {
            var currentpage = pagination.PageNumber;
            var pagesize = pagination.PageSize;

            var totalnoOfrecode = source.Count;

            var totalPages = (int)Math.Ceiling(totalnoOfrecode / (double)pagesize);

            var result = source
                .Skip((pagination.PageNumber - 1) * (pagination.PageSize))
                .Take(pagination.PageSize)
                .ToList();

            var items = _mapper.Map<List<T>>(result);

            PaginationVM<T> paginationVM = new PaginationVM<T>(currentpage, totalPages, pagesize, totalnoOfrecode, items); 

            return paginationVM;
        }
    }
}
