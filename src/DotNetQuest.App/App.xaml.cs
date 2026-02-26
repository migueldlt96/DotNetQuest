namespace DotNetQuest.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell())
        {
            Title = "DotNet Quest",
            MinimumWidth = 1024,
            MinimumHeight = 768
        };
    }
}
