using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.DTO.Brand
{
    public class CreateBrandDTO
    {
        public string Name { get; set; }
        public int EstablishedYear { get; set; }
    }
    public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDTO>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x=> x.Name).NotNull().NotEmpty();
            RuleFor(x => x.EstablishedYear).InclusiveBetween(1920, DateTime.UtcNow.Year);
        }
    }
}
