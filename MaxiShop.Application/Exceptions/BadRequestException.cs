﻿using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public IDictionary<string , string[]> ValidationErrors { get; set; }

        public BadRequestException(string message) : base(message)
        {
            
        }
        public BadRequestException(string message, FluentValidation.Results.ValidationResult validationResult) : base(message)
        {
             ValidationErrors = validationResult.ToDictionary();
        }

    }
}
