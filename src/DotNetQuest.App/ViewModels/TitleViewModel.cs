using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetQuest.App.Services;
using DotNetQuest.Core.Services;

namespace DotNetQuest.App.ViewModels;

public partial class TitleViewModel : ObservableObject
{
    private readonly GameStateService _gameStateService;
    private readonly ChallengeLoader _challengeLoader;

    [ObservableProperty]
    private string _playerName = "Code Mage";

    [ObservableProperty]
    private bool _hasSaveFile = false;

    public TitleViewModel(GameStateService gameStateService, ChallengeLoader challengeLoader)
    {
        _gameStateService = gameStateService;
        _challengeLoader = challengeLoader;
        HasSaveFile = _gameStateService.HasSaveFile();
    }

    [RelayCommand]
    private async Task StartGame()
    {
        if (string.IsNullOrWhiteSpace(PlayerName))
        {
            PlayerName = "Code Mage";
        }

        _gameStateService.NewGame(PlayerName);
        _gameStateService.UpdateStreak();

        // Load challenges from JSON files
        await _challengeLoader.LoadChallengesAsync();

        await Shell.Current.GoToAsync("Challenge");
    }

    [RelayCommand]
    private async Task LoadGame()
    {
        var loaded = await _gameStateService.LoadGameAsync();
        if (loaded)
        {
            _gameStateService.UpdateStreak();
            await _challengeLoader.LoadChallengesAsync();
            await Shell.Current.GoToAsync("Challenge");
        }
    }
}
