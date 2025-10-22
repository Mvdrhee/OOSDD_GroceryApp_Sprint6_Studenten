using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        public ObservableCollection<Product> Products { get; set; }
        private readonly IProductService _productsService;
        [ObservableProperty]
        Client client;
        public NewProductViewModel(IProductService productService, GlobalViewModel global)
        {
            Title = "Producten toevoegen";
            _productsService = productService;
            Products = new(productService.GetAll());
            client = global.Client;
        }

        [RelayCommand]
        private void AddProduct(string name, int stock, DateOnly date, Decimal price)
        {
            if (Client.Role != Role.Admin) {return; }
            Product new_product = new(0, name, stock, date, price);
            _productsService.Add(new_product);
            Products.Add(new_product);

        }
    }
}