using System.Text.Json;
using DotNetQuest.Core.Models;
using DotNetQuest.Core.Services;

namespace DotNetQuest.App.Services;

/// <summary>
/// Loads challenge JSON files from the MAUI app bundle.
/// </summary>
public class ChallengeLoader
{
    private readonly ChallengeService _challengeService;
    private readonly JsonSerializerOptions _jsonOptions;

    // Challenge files organized by realm
    private static readonly string[] ChallengeFiles = new[]
    {
        // Valley of Variables
        "challenges/valley-of-variables/var-001.json",
        "challenges/valley-of-variables/var-002.json",
        "challenges/valley-of-variables/var-003.json",
        "challenges/valley-of-variables/var-004.json",
        "challenges/valley-of-variables/var-005.json",
        // Forest of Flow
        "challenges/forest-of-flow/flow-001.json",
        "challenges/forest-of-flow/flow-002.json",
        "challenges/forest-of-flow/flow-003.json",
        "challenges/forest-of-flow/flow-004.json",
        "challenges/forest-of-flow/flow-005.json",
        "challenges/forest-of-flow/flow-006.json",
        // Mountains of Methods
        "challenges/mountains-of-methods/method-001.json",
        "challenges/mountains-of-methods/method-002.json",
        "challenges/mountains-of-methods/method-003.json",
        "challenges/mountains-of-methods/method-004.json",
        "challenges/mountains-of-methods/method-005.json",
        // Castle of Classes
        "challenges/castle-of-classes/class-001.json",
        "challenges/castle-of-classes/class-002.json",
        "challenges/castle-of-classes/class-003.json",
        "challenges/castle-of-classes/class-004.json",
        "challenges/castle-of-classes/class-005.json",
    };

    public ChallengeLoader(ChallengeService challengeService)
    {
        _challengeService = challengeService;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
    }

    public async Task LoadChallengesAsync()
    {
        var challenges = new List<Challenge>();

        foreach (var file in ChallengeFiles)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(file);
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                var challenge = JsonSerializer.Deserialize<Challenge>(json, _jsonOptions);
                if (challenge != null)
                {
                    challenges.Add(challenge);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load challenge from {file}: {ex.Message}");
            }
        }

        _challengeService.LoadChallengesFromEmbedded(challenges);
    }
}
