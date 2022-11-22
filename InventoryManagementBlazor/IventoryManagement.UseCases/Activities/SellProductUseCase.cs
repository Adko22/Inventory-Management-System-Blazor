using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.Activities.Interfaces;
using IventoryManagement.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Activities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTransactionRepository _productTransactionRepository;

        public SellProductUseCase(IProductRepository productRepository, IProductTransactionRepository productTransactionRepository)
        {
            _productRepository = productRepository;
            _productTransactionRepository = productTransactionRepository;
        }

        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, decimal unitPrice, string doneBy)
        {
            await _productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, unitPrice, doneBy);

            product.Quantity -= quantity;
            await _productRepository.UpdateProductAsync(product);
        }

    }
}
