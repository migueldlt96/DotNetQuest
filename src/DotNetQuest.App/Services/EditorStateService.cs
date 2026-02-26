using DotNetQuest.Core.Models;

namespace DotNetQuest.App.Services;

public class EditorStateService
{
    public Challenge? CurrentChallenge { get; private set; }
    public string CurrentCode { get; private set; } = "";

    public event EventHandler? ChallengeChanged;
    public event EventHandler<string>? CodeChanged;
    public event EventHandler<CompilationResult>? CompilationCompleted;
    public event Func<Task>? CompileRequested;

    public void SetChallenge(Challenge challenge)
    {
        CurrentChallenge = challenge;
        CurrentCode = challenge.StarterCode;
        ChallengeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateCode(string code)
    {
        CurrentCode = code;
        CodeChanged?.Invoke(this, code);
    }

    public void ResetCode()
    {
        if (CurrentChallenge != null)
        {
            CurrentCode = CurrentChallenge.StarterCode;
            ChallengeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void NotifyCompilationComplete(CompilationResult result)
    {
        CompilationCompleted?.Invoke(this, result);
    }

    public async Task RequestCompilation()
    {
        if (CompileRequested != null)
        {
            await CompileRequested.Invoke();
        }
    }
}
