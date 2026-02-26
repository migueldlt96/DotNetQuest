namespace DotNetQuest.Core.Models;

public class Tutorial
{
    public string Title { get; set; } = string.Empty;
    public List<TutorialStep> Steps { get; set; } = new();
}

public class TutorialStep
{
    public string Explanation { get; set; } = string.Empty;
    public string? CodeExample { get; set; }
    public string? CodeOutput { get; set; }
    public string? Tip { get; set; }
}
