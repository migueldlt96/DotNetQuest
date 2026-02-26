using System.Text.Json.Serialization;

namespace DotNetQuest.Core.Models;

public class Challenge
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("realm")]
    public string Realm { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("story")]
    public string Story { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public ChallengeType Type { get; set; }

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }

    [JsonPropertyName("xpReward")]
    public int XpReward { get; set; }

    [JsonPropertyName("starterCode")]
    public string StarterCode { get; set; } = string.Empty;

    [JsonPropertyName("solution")]
    public string Solution { get; set; } = string.Empty;

    [JsonPropertyName("testCases")]
    public List<TestCase> TestCases { get; set; } = new();

    [JsonPropertyName("hints")]
    public List<string> Hints { get; set; } = new();

    [JsonPropertyName("concepts")]
    public List<string> Concepts { get; set; } = new();

    [JsonPropertyName("readingMaterial")]
    public List<ReadingMaterial> ReadingMaterial { get; set; } = new();

    [JsonPropertyName("tutorial")]
    public Tutorial? Tutorial { get; set; }

    [JsonPropertyName("exploration")]
    public Exploration? Exploration { get; set; }
}

public class Exploration
{
    [JsonPropertyName("introduction")]
    public string Introduction { get; set; } = string.Empty;

    [JsonPropertyName("examples")]
    public List<ExplorationExample> Examples { get; set; } = new();
}

public class ExplorationExample
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("explanation")]
    public string Explanation { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("expectedOutput")]
    public string? ExpectedOutput { get; set; }
}

public class ReadingMaterial
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public class TestCase
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("assertion")]
    public string Assertion { get; set; } = string.Empty;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChallengeType
{
    [JsonPropertyName("code-completion")]
    CodeCompletion,

    [JsonPropertyName("bug-hunt")]
    BugHunt,

    [JsonPropertyName("spell-craft")]
    SpellCraft,

    [JsonPropertyName("boss-battle")]
    BossBattle
}
