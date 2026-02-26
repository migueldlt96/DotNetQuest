using DotNetQuest.App.Views;

namespace DotNetQuest.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("Challenge", typeof(ChallengeView));
    }
}
