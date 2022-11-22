using InventoryManagementBlazor.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementBlazor.ViewModelsValidations
{
    public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var sallViewModel = validationContext.ObjectInstance as SellViewModel;
            if (sallViewModel != null)
            {
                if (sallViewModel.Product != null)
                {
                    if (sallViewModel.Product.Quantity < sallViewModel.QuantityToSell)
                    {
                        return new ValidationResult($"There isn't enough product. There is only {sallViewModel.Product.Quantity} in the warehouse.",
                          new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
