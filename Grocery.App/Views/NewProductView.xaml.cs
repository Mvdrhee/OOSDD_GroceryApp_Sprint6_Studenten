using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class NewProductView : ContentPage
{
    public NewProductView(NewProductView viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}