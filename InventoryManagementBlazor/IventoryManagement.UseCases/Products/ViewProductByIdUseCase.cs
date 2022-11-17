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
    public class ViewProductByIdUseCase : IViewProductByIdUseCase
    {
        private readonly IProductRepository _productRepository;
        public ViewProductByIdUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> ExecuteAsync(int porductId)
        {
            return await _productRepository.GetProductByIdAsync(porductId);
        }
    }
}
