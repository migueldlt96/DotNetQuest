using System.Text.Json;
using DotNetQuest.Core.Models;

namespace DotNetQuest.Core.Services;

public class GameStateService
{
    private Player _player = new();
    private static readonly string SaveFileName = "dotnetquest_save.json";

    public Player Player => _player;

    public event EventHandler<int>? XpGained;
    public event EventHandler<int>? LeveledUp;
    public event EventHandler<string>? ChallengeCompleted;

    public static string SaveFilePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        SaveFileName);

    public void NewGame(string playerName = "Code Mage")
    {
        _player = new Player { Name = playerName };
    }

    public void LoadPlayer(Player player)
    {
        _player = player;
    }

    public bool AwardXp(int amount, string challengeId)
    {
        if (_player.HasCompletedChallenge(challengeId))
        {
            // Already completed, award reduced XP
            amount /= 4;
        }

        var leveledUp = _player.AddXp(amount);
        _player.MarkChallengeComplete(challengeId);

        XpGained?.Invoke(this, amount);
        ChallengeCompleted?.Invoke(this, challengeId);

        if (leveledUp)
        {
            LeveledUp?.Invoke(this, _player.Level);
        }

        return leveledUp;
    }

    public void UseHint()
    {
        if (_player.HintsRemaining > 0)
        {
            _player.HintsRemaining--;
        }
    }

    public void UpdateStreak()
    {
        var today = DateTime.Today;

        if (_player.LastPlayedDate == null)
        {
            _player.Streak = 1;
        }
        else if (_player.LastPlayedDate.Value.Date == today.AddDays(-1))
        {
            _player.Streak++;
        }
        else if (_player.LastPlayedDate.Value.Date != today)
        {
            _player.Streak = 1;
        }

        _player.LastPlayedDate = DateTime.Now;
    }

    public async Task<bool> SaveGameAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_player, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync(SaveFilePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LoadGameAsync()
    {
        try
        {
            if (!File.Exists(SaveFilePath))
                return false;

            var json = await File.ReadAllTextAsync(SaveFilePath);
            var player = JsonSerializer.Deserialize<Player>(json);
            if (player != null)
            {
                _player = player;
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public bool HasSaveFile()
    {
        return File.Exists(SaveFilePath);
    }

    public void SkipToRealm(string realmId)
    {
        _player.CurrentRealm = realmId;
    }

    public static readonly string[] AvailableRealms =
    {
        "valley-of-variables",
        "forest-of-flow",
        "mountains-of-methods",
        "castle-of-classes",
        "the-async-abyss"
    };

    public static readonly Dictionary<string, string> RealmDisplayNames = new()
    {
        { "valley-of-variables", "Valley of Variables" },
        { "forest-of-flow", "Forest of Flow" },
        { "mountains-of-methods", "Mountains of Methods" },
        { "castle-of-classes", "Castle of Classes" },
        { "the-async-abyss", "The Async Abyss" }
    };
}
