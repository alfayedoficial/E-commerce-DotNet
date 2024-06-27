using System;
using E_Commerce.Models;

namespace E_Commerce.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}

