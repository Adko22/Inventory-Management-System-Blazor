using InventoryManagement.EFCoreSqlServer;
using InventoryManagement.Plugins.EFCoreSqlServer;
using InventoryManagement.Plugins.InMemory;
using InventoryManagementBlazor.Data;
using IventoryManagement.UseCases.Activities;
using IventoryManagement.UseCases.Activities.Interfaces;
using IventoryManagement.UseCases.Inventories;
using IventoryManagement.UseCases.Inventories.Interfaces;
using IventoryManagement.UseCases.PluginInterfaces;
using IventoryManagement.UseCases.Products;
using IventoryManagement.UseCases.Products.Interfaces;
using IventoryManagement.UseCases.Reports;
using IventoryManagement.UseCases.Reports.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var constr = builder.Configuration.GetConnectionString("InventoryManagement");

//configure authorizations
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Department", "Administration"));
    options.AddPolicy("Inventory", policy => policy.RequireClaim("Department", "InventoryManagement"));
    options.AddPolicy("Sales", policy => policy.RequireClaim("Department", "Sales"));
    options.AddPolicy("Purchasers", policy => policy.RequireClaim("Department", "Purchasing"));
    options.AddPolicy("Productions", policy => policy.RequireClaim("Department", "ProductionManagement"));
});


//Configure EF core for Identity
builder.Services.AddDbContext<AccountDbContext>(options =>
{
    options.UseSqlServer(constr);
});

//Configure Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<AccountDbContext>();


builder.Services.AddDbContextFactory<IMSContex>(options =>
{
    options.UseSqlServer(constr);
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//Repositories
if (builder.Environment.IsEnvironment("TESTING"))
{
    StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

    builder.Services.AddSingleton<IInventoryRepositoy, InventoryRepositoy>();
    builder.Services.AddSingleton<IProductRepository, ProductRepository>();
    builder.Services.AddSingleton<IInventoryTransactionRepositoy, InventoryTransactionRepository>();
    builder.Services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
}

else
{
    builder.Services.AddTransient<IInventoryRepositoy, InventoryEFCoreRepositoy>();
    builder.Services.AddTransient<IProductRepository, ProductEFCoreRepository>();
    builder.Services.AddTransient<IInventoryTransactionRepositoy, InventoryTransactionEFCoreRepository>();
    builder.Services.AddTransient<IProductTransactionRepository, ProductTransactionEFCoreRepository>();
}

//Inventories
builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IViewInvenotryByIdUseCase, ViewInvenotryByIdUseCase>();

//Products
builder.Services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
builder.Services.AddTransient<IAddProductUseCase, AddProductUseCase>();
builder.Services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();
builder.Services.AddTransient<IEditProductUseCase, EditProductUseCase>();

//Activities
builder.Services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
builder.Services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
builder.Services.AddTransient<ISellProductUseCase, SellProductUseCase>();

//Reports
builder.Services.AddTransient<ISearchInventoryTransactionsUseCase, SearchInventoryTransactionsUseCase>();
builder.Services.AddTransient<ISearchProductTransactionsUseCase, SearchProductTransactionsUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
