namespace DotNetQuest.Core.Models;

public class Player
{
    public string Name { get; set; } = "Code Mage";
    public int Level { get; set; } = 1;
    public int Xp { get; set; } = 0;
    public int TotalXp { get; set; } = 0;
    public List<string> CompletedChallenges { get; set; } = new();
    public string CurrentRealm { get; set; } = "valley-of-variables";
    public int HintsRemaining { get; set; } = 3;
    public int Streak { get; set; } = 0;
    public DateTime? LastPlayedDate { get; set; }

    public int XpToNextLevel => Level * 100;

    public bool AddXp(int amount)
    {
        Xp += amount;
        TotalXp += amount;

        if (Xp >= XpToNextLevel)
        {
            Xp -= XpToNextLevel;
            Level++;
            return true; // Leveled up
        }

        return false;
    }

    public void MarkChallengeComplete(string challengeId)
    {
        if (!CompletedChallenges.Contains(challengeId))
        {
            CompletedChallenges.Add(challengeId);
        }
    }

    public bool HasCompletedChallenge(string challengeId)
    {
        return CompletedChallenges.Contains(challengeId);
    }
}
