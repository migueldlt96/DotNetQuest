namespace DotNetQuest.Core.Models;

public class CompilationResult
{
    public bool Success { get; set; }
    public List<CompilationError> Errors { get; set; } = new();
    public List<TestResult> TestResults { get; set; } = new();
    public string? Output { get; set; }
    public string? GameMessage { get; set; }
}

public class CompilationError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Line { get; set; }
    public int Column { get; set; }
    public string GameFriendlyMessage { get; set; } = string.Empty;
}

public class TestResult
{
    public string Description { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? ErrorMessage { get; set; }
}
