using DotNetQuest.CodeEngine;
using DotNetQuest.Core.Models;
using Xunit;

namespace DotNetQuest.CodeEngine.Tests;

public class CodeCompilerTests
{
    private readonly CodeCompiler _compiler = new();

    [Fact]
    public async Task CompileAndRun_ValidCode_ReturnsSuccess()
    {
        var challenge = new Challenge
        {
            Id = "test-001",
            TestCases = new List<TestCase>
            {
                new() { Description = "x should equal 42", Assertion = "x == 42" }
            }
        };

        var code = "int x = 42;";

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        Assert.Single(result.TestResults);
        Assert.True(result.TestResults[0].Passed);
    }

    [Fact]
    public async Task CompileAndRun_SyntaxError_ReturnsErrors()
    {
        var challenge = new Challenge
        {
            Id = "test-002",
            TestCases = new List<TestCase>
            {
                new() { Description = "x should equal 42", Assertion = "x == 42" }
            }
        };

        var code = "int x = 42"; // Missing semicolon

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.False(result.Success);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public async Task CompileAndRun_FailingTest_ReturnsFailure()
    {
        var challenge = new Challenge
        {
            Id = "test-003",
            TestCases = new List<TestCase>
            {
                new() { Description = "x should equal 42", Assertion = "x == 42" }
            }
        };

        var code = "int x = 100;"; // Wrong value

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.False(result.Success);
        Assert.Single(result.TestResults);
        Assert.False(result.TestResults[0].Passed);
    }

    [Fact]
    public async Task CompileAndRun_StringVariable_Works()
    {
        var challenge = new Challenge
        {
            Id = "test-004",
            TestCases = new List<TestCase>
            {
                new() { Description = "heroName should equal Aria", Assertion = "heroName == \"Aria\"" }
            }
        };

        var code = "string heroName = \"Aria\";";

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.True(result.Success);
        Assert.True(result.TestResults[0].Passed);
    }

    [Fact]
    public async Task CompileAndRun_MultipleTestCases_AllPass()
    {
        var challenge = new Challenge
        {
            Id = "test-005",
            TestCases = new List<TestCase>
            {
                new() { Description = "a should equal 10", Assertion = "a == 10" },
                new() { Description = "b should equal 20", Assertion = "b == 20" },
                new() { Description = "sum should equal 30", Assertion = "a + b == 30" }
            }
        };

        var code = "int a = 10; int b = 20;";

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.True(result.Success);
        Assert.Equal(3, result.TestResults.Count);
        Assert.All(result.TestResults, t => Assert.True(t.Passed));
    }

    [Fact]
    public async Task CompileAndRun_UndefinedVariable_ReturnsError()
    {
        var challenge = new Challenge
        {
            Id = "test-006",
            TestCases = new List<TestCase>
            {
                new() { Description = "x should equal 42", Assertion = "x == 42" }
            }
        };

        var code = "int y = 42;"; // Wrong variable name

        var result = await _compiler.CompileAndRunAsync(code, challenge);

        Assert.False(result.Success);
    }
}
