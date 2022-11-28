using Microsoft.Build.Framework;

namespace InventoryManagementBlazor.ViewModels
{
    public class ManageUserViewModel
    {
        public string? Email { get; set; }

        [Required]
        public string? Department { get; set; }
    }
}
