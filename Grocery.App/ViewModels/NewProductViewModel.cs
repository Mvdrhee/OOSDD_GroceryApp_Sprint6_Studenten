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
        private Client client;

        [ObservableProperty]
        private string inputName = "";

        [ObservableProperty]
        private string inputStock = "";

        [ObservableProperty]
        private string inputPrice = "";

        [ObservableProperty]
        private string inputDate = "";

        [ObservableProperty]
        private string errorMessage = "";

        public NewProductViewModel(IProductService productService, GlobalViewModel global)
        {
            Title = "Producten toevoegen";
            _productsService = productService;
            Products = new(productService.GetAll());
            client = global.Client;
        }

        [RelayCommand]
        private void AddProduct()
        {
            if (Client.Role != Role.Admin) {return; }
            Product? new_product = null;
            try
                { new_product = new(0, InputName, Int32.Parse(InputStock), DateOnly.Parse(InputDate), Int32.Parse(InputPrice)); }
            catch (Exception)
            {
                ErrorMessage = "Ongeldige invoer";
                return;
            }

            _productsService.Add(new_product);
            Products.Add(new_product);

            }
    }
}