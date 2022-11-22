using IventoryManagement.CoreBusiness;
using IventoryManagement.UseCases.PluginInterfaces;
using IventoryManagement.UseCases.Reports.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IventoryManagement.UseCases.Reports
{
    public class SearchProductTransactionsUseCase : ISearchProductTransactionsUseCase
    {
        private readonly IProductTransactionRepository _productTransactionRepositoy;

        public SearchProductTransactionsUseCase(IProductTransactionRepository productTransactionRepositoy)
        {
            _productTransactionRepositoy = productTransactionRepositoy;
        }

        public async Task<IEnumerable<ProductTransaction>> ExecuteAsync(
            string productName,
            DateTime? dateFrom,
           DateTime? dateTo,
            ProductTransactionType? transactionType
            )
        {
            if (dateTo.HasValue)
            {
                dateTo = dateTo.Value.AddDays(1);
            }

            return await _productTransactionRepositoy.GetProductTransactionsAsync(
                productName,
                dateFrom,
                dateTo,
                transactionType
                );
        }
    }
}
