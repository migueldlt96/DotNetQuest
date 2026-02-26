using Microsoft.Extensions.Logging;
using DotNetQuest.App.ViewModels;
using DotNetQuest.App.Views;
using DotNetQuest.App.Services;
using DotNetQuest.Core.Services;
using DotNetQuest.CodeEngine;

namespace DotNetQuest.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Add Blazor WebView
        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        // Register services
        builder.Services.AddSingleton<ChallengeService>();
        builder.Services.AddSingleton<GameStateService>();
        builder.Services.AddSingleton<CodeCompiler>();
        builder.Services.AddSingleton<EditorStateService>();

        // Register ViewModels
        builder.Services.AddTransient<TitleViewModel>();
        builder.Services.AddTransient<ChallengeViewModel>();

        // Register Views
        builder.Services.AddTransient<TitleView>();
        builder.Services.AddTransient<ChallengeView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
