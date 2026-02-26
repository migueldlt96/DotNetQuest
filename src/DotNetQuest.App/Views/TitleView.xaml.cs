using DotNetQuest.App.ViewModels;

namespace DotNetQuest.App.Views;

public partial class TitleView : ContentPage
{
    public TitleView(TitleViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
