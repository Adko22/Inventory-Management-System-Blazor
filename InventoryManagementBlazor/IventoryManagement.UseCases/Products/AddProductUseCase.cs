using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using IventoryManagement.UseCases.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Products
{
    public class AddProductUseCase : IAddProductUseCase
    {
        private readonly IProductRepository _productRepository;

        public AddProductUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task ExecuteAsync(Product product)
        {
            if (product == null)
            {
                return;
            }

            await _productRepository.AddProductAsync(product);
        }
    }
}
