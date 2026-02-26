using DotNetQuest.Core.Models;
using Xunit;

namespace DotNetQuest.Core.Tests;

public class PlayerTests
{
    [Fact]
    public void NewPlayer_HasDefaultValues()
    {
        var player = new Player();

        Assert.Equal("Code Mage", player.Name);
        Assert.Equal(1, player.Level);
        Assert.Equal(0, player.Xp);
        Assert.Equal(0, player.TotalXp);
        Assert.Empty(player.CompletedChallenges);
    }

    [Fact]
    public void XpToNextLevel_IncreasesByLevel()
    {
        var player = new Player { Level = 1 };
        Assert.Equal(100, player.XpToNextLevel);

        player.Level = 5;
        Assert.Equal(500, player.XpToNextLevel);
    }

    [Fact]
    public void AddXp_AccumulatesXp()
    {
        var player = new Player();

        player.AddXp(50);

        Assert.Equal(50, player.Xp);
        Assert.Equal(50, player.TotalXp);
    }

    [Fact]
    public void AddXp_LevelsUp_WhenXpExceedsThreshold()
    {
        var player = new Player();

        var leveledUp = player.AddXp(150);

        Assert.True(leveledUp);
        Assert.Equal(2, player.Level);
        Assert.Equal(50, player.Xp); // 150 - 100 = 50
        Assert.Equal(150, player.TotalXp);
    }

    [Fact]
    public void MarkChallengeComplete_AddsChallengeToList()
    {
        var player = new Player();

        player.MarkChallengeComplete("var-001");

        Assert.Contains("var-001", player.CompletedChallenges);
    }

    [Fact]
    public void MarkChallengeComplete_DoesNotDuplicate()
    {
        var player = new Player();

        player.MarkChallengeComplete("var-001");
        player.MarkChallengeComplete("var-001");

        Assert.Single(player.CompletedChallenges);
    }

    [Fact]
    public void HasCompletedChallenge_ReturnsTrue_WhenCompleted()
    {
        var player = new Player();
        player.MarkChallengeComplete("var-001");

        Assert.True(player.HasCompletedChallenge("var-001"));
        Assert.False(player.HasCompletedChallenge("var-002"));
    }
}
