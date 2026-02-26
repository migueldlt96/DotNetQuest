using DotNetQuest.App.ViewModels;

namespace DotNetQuest.App.Views;

public partial class ChallengeView : ContentPage
{
    public ChallengeView(ChallengeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ChallengeViewModel vm)
        {
            vm.LoadNextChallenge();
        }
    }
}
