using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.Application.Features.Product.DTOs
{
    public class ProductCategorySummaryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }

}
