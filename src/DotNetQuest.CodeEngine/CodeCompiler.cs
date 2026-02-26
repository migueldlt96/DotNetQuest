using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using DotNetQuest.Core.Models;
using System.Reflection;

namespace DotNetQuest.CodeEngine;

public class CodeCompiler
{
    private static readonly ScriptOptions DefaultScriptOptions = ScriptOptions.Default
        .WithReferences(
            typeof(object).Assembly,
            typeof(Console).Assembly,
            typeof(Enumerable).Assembly,
            Assembly.Load("System.Runtime"))
        .WithImports("System", "System.Linq", "System.Collections.Generic", "System.Text");

    public async Task<CompilationResult> CompileAndRunAsync(string code, Challenge challenge)
    {
        var result = new CompilationResult();

        // Sanitize code: replace smart quotes with straight quotes
        code = SanitizeCode(code);

        try
        {
            // First, check for syntax errors
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var diagnostics = syntaxTree.GetDiagnostics().ToList();

            var syntaxErrors = diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
            if (syntaxErrors.Any())
            {
                result.Success = false;
                result.Errors = syntaxErrors.Select(d => new CompilationError
                {
                    Code = d.Id,
                    Message = d.GetMessage(),
                    Line = d.Location.GetLineSpan().StartLinePosition.Line + 1,
                    Column = d.Location.GetLineSpan().StartLinePosition.Character + 1,
                    GameFriendlyMessage = TranslateErrorToGameLore(d)
                }).ToList();
                result.GameMessage = "Your spell fizzled! The ancient runes contain errors.";
                return result;
            }

            // Run each test case
            foreach (var testCase in challenge.TestCases)
            {
                var testResult = await RunTestCaseAsync(code, testCase);
                result.TestResults.Add(testResult);
            }

            result.Success = result.TestResults.All(t => t.Passed);
            result.GameMessage = result.Success
                ? "Your spell blazes with power! The code magic flows perfectly."
                : "Your spell partially worked, but some runes failed to activate.";
        }
        catch (CompilationErrorException ex)
        {
            result.Success = false;
            result.Errors = ex.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => new CompilationError
                {
                    Code = d.Id,
                    Message = d.GetMessage(),
                    Line = d.Location.GetLineSpan().StartLinePosition.Line + 1,
                    Column = d.Location.GetLineSpan().StartLinePosition.Character + 1,
                    GameFriendlyMessage = TranslateErrorToGameLore(d)
                }).ToList();
            result.GameMessage = "Your spell backfired! The compiler spirits reject your incantation.";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Errors.Add(new CompilationError
            {
                Code = "RUNTIME",
                Message = ex.Message,
                GameFriendlyMessage = $"A wild Exception appeared! {TranslateExceptionToLore(ex)}"
            });
            result.GameMessage = "Your spell exploded unexpectedly! A runtime demon escaped.";
        }

        return result;
    }

    private async Task<TestResult> RunTestCaseAsync(string code, TestCase testCase)
    {
        var testResult = new TestResult
        {
            Description = testCase.Description
        };

        try
        {
            // Combine user code with test assertion
            var fullCode = $@"
{code}
return {testCase.Assertion};";

            var scriptResult = await CSharpScript.EvaluateAsync<bool>(fullCode, DefaultScriptOptions);
            testResult.Passed = scriptResult;

            if (!testResult.Passed)
            {
                testResult.ErrorMessage = "The spell target was not affected as expected.";
            }
        }
        catch (Exception ex)
        {
            testResult.Passed = false;
            testResult.ErrorMessage = ex.Message;
        }

        return testResult;
    }

    private static string TranslateErrorToGameLore(Diagnostic diagnostic)
    {
        var message = diagnostic.GetMessage();

        return diagnostic.Id switch
        {
            "CS0103" => $"Unknown rune detected! The mystical symbol is not recognized in this realm.",
            "CS1002" => "Missing semicolon! Every spell must end with the sacred ';' symbol.",
            "CS0029" => "Type mismatch! You're trying to mix incompatible magical essences.",
            "CS1525" => "Unexpected symbol! The spell syntax has been corrupted.",
            "CS0246" => "Unknown type! This magical type hasn't been discovered yet.",
            "CS0128" => "Duplicate declaration! This variable name is already bound to another spell.",
            "CS0019" => "Invalid operation! These magical types cannot be combined this way.",
            "CS0165" => "Uninitialized variable! This magical container is empty - fill it with a value first.",
            _ => $"Arcane error: {message}"
        };
    }

    private static string TranslateExceptionToLore(Exception ex)
    {
        return ex switch
        {
            NullReferenceException => "You tried to use a null reference - like grasping at shadows!",
            IndexOutOfRangeException => "You reached beyond the bounds of the array - there be dragons!",
            DivideByZeroException => "Division by zero! The universe nearly collapsed!",
            InvalidCastException => "Failed transformation! These types refuse to change forms.",
            _ => $"The spell unleashed chaos: {ex.Message}"
        };
    }

    private static string SanitizeCode(string code)
    {
        // Replace smart/curly quotes with straight quotes (macOS auto-converts these)
        return code
            .Replace('\u201C', '"')  // Left double quote "
            .Replace('\u201D', '"')  // Right double quote "
            .Replace('\u2018', '\'') // Left single quote '
            .Replace('\u2019', '\'') // Right single quote '
            .Replace('`', '\'');     // Backtick to single quote
    }

    // Simple execution for exploration mode - just runs code and captures output
    public async Task<ExplorationResult> RunExplorationCodeAsync(string code)
    {
        var result = new ExplorationResult();
        code = SanitizeCode(code);

        try
        {
            // Check for syntax errors first
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var syntaxErrors = syntaxTree.GetDiagnostics()
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .ToList();

            if (syntaxErrors.Any())
            {
                result.Success = false;
                result.CompilationErrors = syntaxErrors.Select(d => d.GetMessage()).ToList();
                return result;
            }

            // Capture Console.WriteLine output
            var outputCapture = new StringWriter();
            var originalOut = Console.Out;

            try
            {
                Console.SetOut(outputCapture);
                await CSharpScript.RunAsync(code, DefaultScriptOptions);
                result.Success = true;
                result.Output = outputCapture.ToString().TrimEnd();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
        catch (CompilationErrorException ex)
        {
            result.Success = false;
            result.CompilationErrors = ex.Diagnostics
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => d.GetMessage())
                .ToList();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.RuntimeError = ex.Message;
        }

        return result;
    }
}

public class ExplorationResult
{
    public bool Success { get; set; }
    public string Output { get; set; } = string.Empty;
    public List<string> CompilationErrors { get; set; } = new();
    public string? RuntimeError { get; set; }
}
