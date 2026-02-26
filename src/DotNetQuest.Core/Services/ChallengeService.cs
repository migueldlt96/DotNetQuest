using System.Text.Json;
using DotNetQuest.Core.Models;

namespace DotNetQuest.Core.Services;

public class ChallengeService
{
    private readonly List<Challenge> _challenges = new();
    private readonly JsonSerializerOptions _jsonOptions;

    public ChallengeService()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        };
    }

    public async Task LoadChallengesAsync(string challengesPath)
    {
        _challenges.Clear();

        if (!Directory.Exists(challengesPath))
        {
            return;
        }

        var files = Directory.GetFiles(challengesPath, "*.json", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var challenge = JsonSerializer.Deserialize<Challenge>(json, _jsonOptions);
                if (challenge != null)
                {
                    _challenges.Add(challenge);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load challenge from {file}: {ex.Message}");
            }
        }
    }

    public void LoadChallengesFromEmbedded(IEnumerable<Challenge> challenges)
    {
        _challenges.Clear();
        _challenges.AddRange(challenges);
    }

    public IReadOnlyList<Challenge> GetAllChallenges() => _challenges.AsReadOnly();

    public IReadOnlyList<Challenge> GetChallengesByRealm(string realm)
        => _challenges.Where(c => c.Realm == realm).OrderBy(c => c.Difficulty).ToList();

    public Challenge? GetChallenge(string id)
        => _challenges.FirstOrDefault(c => c.Id == id);

    public Challenge? GetNextChallenge(string realm, IReadOnlyList<string> completedIds)
    {
        return _challenges
            .Where(c => c.Realm == realm && !completedIds.Contains(c.Id))
            .OrderBy(c => c.Difficulty)
            .FirstOrDefault();
    }
}
