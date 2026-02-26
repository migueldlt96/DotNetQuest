using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetQuest.CodeEngine;
using DotNetQuest.Core.Models;
using DotNetQuest.Core.Services;
using DotNetQuest.App.Services;

namespace DotNetQuest.App.ViewModels;

public partial class ChallengeViewModel : ObservableObject
{
    private readonly GameStateService _gameStateService;
    private readonly ChallengeService _challengeService;
    private readonly CodeCompiler _codeCompiler;
    private readonly EditorStateService _editorState;

    private int _currentHintIndex = 0;
    private bool _challengeCompleted = false;
    private string? _pendingNextRealm = null;

    [ObservableProperty]
    private Challenge? _currentChallenge;

    [ObservableProperty]
    private string _userCode = string.Empty;

    [ObservableProperty]
    private string _playerName = string.Empty;

    [ObservableProperty]
    private int _playerLevel = 1;

    [ObservableProperty]
    private string _xpDisplay = "XP: 0/100";

    [ObservableProperty]
    private string _progressDisplay = "0/5 Complete";

    [ObservableProperty]
    private string _hintsDisplay = "Hints: 3";

    [ObservableProperty]
    private bool _showFeedback = false;

    [ObservableProperty]
    private string _feedbackTitle = string.Empty;

    [ObservableProperty]
    private string _feedbackMessage = string.Empty;

    [ObservableProperty]
    private Color _feedbackBackgroundColor = Colors.Transparent;

    [ObservableProperty]
    private Color _feedbackTitleColor = Colors.White;

    [ObservableProperty]
    private bool _showHint = false;

    [ObservableProperty]
    private string _currentHint = string.Empty;

    [ObservableProperty]
    private bool _canGetHint = true;

    [ObservableProperty]
    private string _submitButtonText = "Cast Spell";

    [ObservableProperty]
    private List<TestCaseDisplay> _testCaseDisplays = new();

    [ObservableProperty]
    private bool _hasReadingMaterial = false;

    [ObservableProperty]
    private List<ReadingMaterialDisplay> _readingMaterialItems = new();

    [ObservableProperty]
    private bool _hasTutorial = false;

    [ObservableProperty]
    private bool _showTutorial = false;

    [ObservableProperty]
    private Tutorial? _currentTutorial;

    [ObservableProperty]
    private int _currentTutorialStep = 0;

    [ObservableProperty]
    private TutorialStep? _displayedStep;

    [ObservableProperty]
    private string _tutorialProgress = "Step 1 of 1";

    [ObservableProperty]
    private bool _canGoNextStep = false;

    [ObservableProperty]
    private bool _canGoPrevStep = false;

    [ObservableProperty]
    private bool _showRealmSelect = false;

    [ObservableProperty]
    private List<RealmOption> _realmOptions = new();

    [ObservableProperty]
    private string _saveStatusMessage = string.Empty;

    [ObservableProperty]
    private bool _showSaveStatus = false;

    // Exploration Mode
    [ObservableProperty]
    private bool _isExplorationMode = false;

    [ObservableProperty]
    private bool _hasExploration = false;

    [ObservableProperty]
    private Exploration? _currentExploration;

    [ObservableProperty]
    private int _currentExampleIndex = 0;

    [ObservableProperty]
    private ExplorationExample? _currentExample;

    [ObservableProperty]
    private string _explorationOutput = string.Empty;

    [ObservableProperty]
    private string _exampleProgress = "Example 1 of 1";

    [ObservableProperty]
    private bool _canNextExample = false;

    [ObservableProperty]
    private bool _canPrevExample = false;

    public ChallengeViewModel(
        GameStateService gameStateService,
        ChallengeService challengeService,
        CodeCompiler codeCompiler,
        EditorStateService editorState)
    {
        _gameStateService = gameStateService;
        _challengeService = challengeService;
        _codeCompiler = codeCompiler;
        _editorState = editorState;

        // Subscribe to compilation results from Blazor
        _editorState.CompilationCompleted += OnCompilationCompleted;

        UpdatePlayerDisplay();
    }

    private void OnCompilationCompleted(object? sender, CompilationResult result)
    {
        // Update test case displays
        UpdateTestCaseDisplays(result);

        if (result.Success && CurrentChallenge != null)
        {
            _challengeCompleted = true;
            var leveledUp = _gameStateService.AwardXp(CurrentChallenge.XpReward, CurrentChallenge.Id);
            SubmitButtonText = "Next Challenge";
            UpdatePlayerDisplay();
        }
    }

    public void LoadNextChallenge()
    {
        var player = _gameStateService.Player;
        var nextChallenge = _challengeService.GetNextChallenge(
            player.CurrentRealm,
            player.CompletedChallenges);

        if (nextChallenge == null)
        {
            // All challenges completed!
            ShowCompletionMessage();
            return;
        }

        LoadChallenge(nextChallenge);
    }

    private void LoadChallenge(Challenge challenge)
    {
        CurrentChallenge = challenge;
        UserCode = challenge.StarterCode;
        _currentHintIndex = 0;
        _challengeCompleted = false;
        ShowFeedback = false;
        ShowHint = false;
        ShowTutorial = false;
        IsExplorationMode = false;
        ExplorationOutput = "";
        SubmitButtonText = "Cast Spell";

        // Check if challenge has a tutorial
        HasTutorial = challenge.Tutorial != null && challenge.Tutorial.Steps.Count > 0;
        CurrentTutorial = challenge.Tutorial;
        CurrentTutorialStep = 0;

        // Check if challenge has exploration
        HasExploration = challenge.Exploration != null && challenge.Exploration.Examples.Count > 0;
        CurrentExploration = challenge.Exploration;
        CurrentExampleIndex = 0;

        // Notify Blazor component about new challenge
        _editorState.SetChallenge(challenge);

        UpdateTestCaseDisplays(null);
        UpdateReadingMaterial(challenge);
        UpdatePlayerDisplay();
    }

    private void UpdateReadingMaterial(Challenge challenge)
    {
        HasReadingMaterial = challenge.ReadingMaterial.Count > 0;
        ReadingMaterialItems = challenge.ReadingMaterial
            .Select(r => new ReadingMaterialDisplay
            {
                Title = r.Title,
                Description = r.Description,
                Url = r.Url
            })
            .ToList();
    }

    private void UpdatePlayerDisplay()
    {
        var player = _gameStateService.Player;
        PlayerName = player.Name;
        PlayerLevel = player.Level;
        XpDisplay = $"XP: {player.Xp}/{player.XpToNextLevel}";
        HintsDisplay = $"Hints: {player.HintsRemaining}";
        CanGetHint = player.HintsRemaining > 0 && CurrentChallenge != null &&
                     _currentHintIndex < (CurrentChallenge?.Hints.Count ?? 0);

        var totalChallenges = _challengeService.GetChallengesByRealm(player.CurrentRealm).Count;
        var completed = player.CompletedChallenges.Count;
        ProgressDisplay = $"{completed}/{totalChallenges} Complete";
    }

    private void UpdateTestCaseDisplays(CompilationResult? result)
    {
        if (CurrentChallenge == null) return;

        var displays = new List<TestCaseDisplay>();

        for (int i = 0; i < CurrentChallenge.TestCases.Count; i++)
        {
            var testCase = CurrentChallenge.TestCases[i];
            TestResult? testResult = null;

            if (result?.TestResults != null && i < result.TestResults.Count)
            {
                testResult = result.TestResults[i];
            }

            displays.Add(new TestCaseDisplay
            {
                Description = testCase.Description,
                StatusIcon = testResult == null ? "[ ]" : (testResult.Passed ? "[+]" : "[X]"),
                BackgroundColor = testResult == null
                    ? Color.FromArgb("#2a2a4a")
                    : (testResult.Passed ? Color.FromArgb("#1a4a2a") : Color.FromArgb("#4a1a2a"))
            });
        }

        TestCaseDisplays = displays;
    }

    [RelayCommand]
    private async Task SubmitCode()
    {
        if (CurrentChallenge == null) return;

        // Handle realm transition
        if (_pendingNextRealm != null)
        {
            _gameStateService.Player.CurrentRealm = _pendingNextRealm;
            _pendingNextRealm = null;
            LoadNextChallenge();
            return;
        }

        if (_challengeCompleted)
        {
            // Move to next challenge
            LoadNextChallenge();
            return;
        }

        // Request compilation from Blazor component
        await _editorState.RequestCompilation();
    }

    [RelayCommand]
    private void GetHint()
    {
        if (CurrentChallenge == null) return;
        if (_currentHintIndex >= CurrentChallenge.Hints.Count) return;
        if (_gameStateService.Player.HintsRemaining <= 0) return;

        _gameStateService.UseHint();
        CurrentHint = CurrentChallenge.Hints[_currentHintIndex];
        _currentHintIndex++;
        ShowHint = true;

        UpdatePlayerDisplay();
    }

    [RelayCommand]
    private void ResetCode()
    {
        if (CurrentChallenge == null) return;

        UserCode = CurrentChallenge.StarterCode;
        ShowFeedback = false;
        _challengeCompleted = false;
        SubmitButtonText = "Cast Spell";
        UpdateTestCaseDisplays(null);

        // Notify Blazor component to reset
        _editorState.ResetCode();
    }

    [RelayCommand]
    private async Task OpenUrl(string url)
    {
        try
        {
            await Launcher.OpenAsync(new Uri(url));
        }
        catch
        {
            // Silently fail if URL can't be opened
        }
    }

    [RelayCommand]
    private void OpenTutorial()
    {
        if (CurrentChallenge?.Tutorial == null) return;

        CurrentTutorial = CurrentChallenge.Tutorial;
        CurrentTutorialStep = 0;
        ShowTutorial = true;
        UpdateTutorialDisplay();
    }

    [RelayCommand]
    private void CloseTutorial()
    {
        ShowTutorial = false;
    }

    [RelayCommand]
    private void NextTutorialStep()
    {
        if (CurrentTutorial == null) return;
        if (CurrentTutorialStep < CurrentTutorial.Steps.Count - 1)
        {
            CurrentTutorialStep++;
            UpdateTutorialDisplay();
        }
    }

    [RelayCommand]
    private void PrevTutorialStep()
    {
        if (CurrentTutorialStep > 0)
        {
            CurrentTutorialStep--;
            UpdateTutorialDisplay();
        }
    }

    private void UpdateTutorialDisplay()
    {
        if (CurrentTutorial == null || CurrentTutorial.Steps.Count == 0) return;

        DisplayedStep = CurrentTutorial.Steps[CurrentTutorialStep];
        TutorialProgress = $"Step {CurrentTutorialStep + 1} of {CurrentTutorial.Steps.Count}";
        CanGoNextStep = CurrentTutorialStep < CurrentTutorial.Steps.Count - 1;
        CanGoPrevStep = CurrentTutorialStep > 0;
    }

    [RelayCommand]
    private async Task SaveGame()
    {
        var success = await _gameStateService.SaveGameAsync();
        SaveStatusMessage = success ? "Game Saved!" : "Save Failed";
        ShowSaveStatus = true;

        // Hide message after 2 seconds
        await Task.Delay(2000);
        ShowSaveStatus = false;
    }

    [RelayCommand]
    private void OpenRealmSelect()
    {
        RealmOptions = GameStateService.AvailableRealms
            .Select(r => new RealmOption
            {
                RealmId = r,
                RealmName = GameStateService.RealmDisplayNames.GetValueOrDefault(r, r),
                IsCurrent = r == _gameStateService.Player.CurrentRealm
            })
            .ToList();
        ShowRealmSelect = true;
    }

    [RelayCommand]
    private void CloseRealmSelect()
    {
        ShowRealmSelect = false;
    }

    [RelayCommand]
    private void SelectRealm(string realmId)
    {
        _gameStateService.SkipToRealm(realmId);
        ShowRealmSelect = false;
        LoadNextChallenge();
    }

    // Exploration Mode Commands
    [RelayCommand]
    private void StartExploration()
    {
        if (CurrentChallenge?.Exploration == null) return;

        CurrentExploration = CurrentChallenge.Exploration;
        CurrentExampleIndex = 0;
        IsExplorationMode = true;
        ExplorationOutput = "";
        UpdateExplorationDisplay();

        // Load the first example's code into the editor
        if (CurrentExample != null)
        {
            _editorState.UpdateCode(CurrentExample.Code);
        }
    }

    [RelayCommand]
    private void ExitExploration()
    {
        IsExplorationMode = false;
        ExplorationOutput = "";
        // Reset to challenge starter code
        if (CurrentChallenge != null)
        {
            _editorState.UpdateCode(CurrentChallenge.StarterCode);
        }
    }

    [RelayCommand]
    private void NextExample()
    {
        if (CurrentExploration == null) return;
        if (CurrentExampleIndex < CurrentExploration.Examples.Count - 1)
        {
            CurrentExampleIndex++;
            UpdateExplorationDisplay();
            if (CurrentExample != null)
            {
                _editorState.UpdateCode(CurrentExample.Code);
                ExplorationOutput = "";
            }
        }
    }

    [RelayCommand]
    private void PrevExample()
    {
        if (CurrentExampleIndex > 0)
        {
            CurrentExampleIndex--;
            UpdateExplorationDisplay();
            if (CurrentExample != null)
            {
                _editorState.UpdateCode(CurrentExample.Code);
                ExplorationOutput = "";
            }
        }
    }

    [RelayCommand]
    private async Task RunExplorationCode()
    {
        // Get current code from editor and run it
        var code = _editorState.CurrentCode;
        var result = await _codeCompiler.RunExplorationCodeAsync(code);

        if (result.CompilationErrors.Any())
        {
            ExplorationOutput = "Errors:\n" + string.Join("\n", result.CompilationErrors);
        }
        else if (!string.IsNullOrEmpty(result.RuntimeError))
        {
            ExplorationOutput = "Runtime Error:\n" + result.RuntimeError;
        }
        else if (!string.IsNullOrEmpty(result.Output))
        {
            ExplorationOutput = result.Output;
        }
        else
        {
            ExplorationOutput = "Code ran successfully! (no output)";
        }
    }

    [RelayCommand]
    private void LoadExampleCode()
    {
        if (CurrentExample != null)
        {
            _editorState.UpdateCode(CurrentExample.Code);
            ExplorationOutput = "";
        }
    }

    private void UpdateExplorationDisplay()
    {
        if (CurrentExploration == null || CurrentExploration.Examples.Count == 0) return;

        CurrentExample = CurrentExploration.Examples[CurrentExampleIndex];
        ExampleProgress = $"Example {CurrentExampleIndex + 1} of {CurrentExploration.Examples.Count}";
        CanNextExample = CurrentExampleIndex < CurrentExploration.Examples.Count - 1;
        CanPrevExample = CurrentExampleIndex > 0;
    }

    private static readonly string[] RealmOrder =
    {
        "valley-of-variables",
        "forest-of-flow",
        "mountains-of-methods",
        "castle-of-classes",
        "the-async-abyss"
    };

    private static readonly Dictionary<string, string> RealmNames = new()
    {
        { "valley-of-variables", "Valley of Variables" },
        { "forest-of-flow", "Forest of Flow" },
        { "mountains-of-methods", "Mountains of Methods" },
        { "castle-of-classes", "Castle of Classes" },
        { "the-async-abyss", "The Async Abyss" }
    };

    private void ShowCompletionMessage()
    {
        var currentRealm = _gameStateService.Player.CurrentRealm;
        var currentRealmIndex = Array.IndexOf(RealmOrder, currentRealm);
        var realmName = RealmNames.GetValueOrDefault(currentRealm, currentRealm);

        // Check if there's a next realm
        if (currentRealmIndex < RealmOrder.Length - 1)
        {
            var nextRealm = RealmOrder[currentRealmIndex + 1];
            var nextRealmName = RealmNames.GetValueOrDefault(nextRealm, nextRealm);

            CurrentChallenge = new Challenge
            {
                Title = $"{realmName} Complete!",
                Story = $"Congratulations, Code Mage! You have mastered the {realmName}. Your skills grow stronger with each challenge conquered. The path to the {nextRealmName} now opens before you!",
                XpReward = 0,
                TestCases = new List<TestCase>(),
                Hints = new List<string>(),
                Concepts = new List<string> { "mastery", "realm-complete" },
                ReadingMaterial = new List<ReadingMaterial>()
            };

            UserCode = $"// {realmName} Complete!\n// Total XP: {_gameStateService.Player.TotalXp}\n// Next: {nextRealmName}";
            ShowFeedback = true;
            FeedbackTitle = "REALM COMPLETE!";
            FeedbackMessage = $"You've conquered the {realmName}!\n\nTotal XP: {_gameStateService.Player.TotalXp}\nLevel: {_gameStateService.Player.Level}\n\nClick below to enter the {nextRealmName}!";
            FeedbackBackgroundColor = Color.FromArgb("#3a2a5a");
            FeedbackTitleColor = Color.FromArgb("#ff88ff");
            SubmitButtonText = $"Enter {nextRealmName}";
            CanGetHint = false;
            HasReadingMaterial = false;

            // Store the next realm for when button is clicked
            _pendingNextRealm = nextRealm;
        }
        else
        {
            // All realms complete!
            CurrentChallenge = new Challenge
            {
                Title = "Journey Complete!",
                Story = "Incredible, Code Mage! You have mastered all the realms of .NET! From the Valley of Variables to The Async Abyss, you have proven yourself a true master of C#. The kingdom is saved!",
                XpReward = 0,
                TestCases = new List<TestCase>(),
                Hints = new List<string>(),
                Concepts = new List<string> { "master", "complete" },
                ReadingMaterial = new List<ReadingMaterial>()
            };

            UserCode = "// CONGRATULATIONS!\n// You have completed DotNet Quest!\n// Total XP: " + _gameStateService.Player.TotalXp;
            ShowFeedback = true;
            FeedbackTitle = "VICTORY!";
            FeedbackMessage = $"You've completed ALL challenges!\n\nFinal XP: {_gameStateService.Player.TotalXp}\nFinal Level: {_gameStateService.Player.Level}\n\nYou are a true Code Mage!";
            FeedbackBackgroundColor = Color.FromArgb("#4a3a1a");
            FeedbackTitleColor = Color.FromArgb("#ffd700");
            SubmitButtonText = "The End";
            CanGetHint = false;
            HasReadingMaterial = false;
            _pendingNextRealm = null;
        }
    }
}

public partial class TestCaseDisplay : ObservableObject
{
    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _statusIcon = "[ ]";

    [ObservableProperty]
    private Color _backgroundColor = Colors.Transparent;
}

public partial class ReadingMaterialDisplay : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _url = string.Empty;
}

public partial class RealmOption : ObservableObject
{
    [ObservableProperty]
    private string _realmId = string.Empty;

    [ObservableProperty]
    private string _realmName = string.Empty;

    [ObservableProperty]
    private bool _isCurrent = false;
}
