using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DotNetQuest.Core.Services;

namespace DotNetQuest.App.ViewModels;

public partial class TitleViewModel : ObservableObject
{
    private readonly GameStateService _gameStateService;
    private readonly ChallengeService _challengeService;

    [ObservableProperty]
    private string _playerName = "Code Mage";

    [ObservableProperty]
    private bool _hasSaveFile = false;

    public TitleViewModel(GameStateService gameStateService, ChallengeService challengeService)
    {
        _gameStateService = gameStateService;
        _challengeService = challengeService;
        HasSaveFile = _gameStateService.HasSaveFile();
    }

    [RelayCommand]
    private async Task StartGame()
    {
        if (string.IsNullOrWhiteSpace(PlayerName))
        {
            PlayerName = "Code Mage";
        }

        _gameStateService.NewGame(PlayerName);
        _gameStateService.UpdateStreak();

        // Load embedded challenges
        _challengeService.LoadChallengesFromEmbedded(GetEmbeddedChallenges());

        await Shell.Current.GoToAsync("Challenge");
    }

    [RelayCommand]
    private async Task LoadGame()
    {
        var loaded = await _gameStateService.LoadGameAsync();
        if (loaded)
        {
            _gameStateService.UpdateStreak();
            _challengeService.LoadChallengesFromEmbedded(GetEmbeddedChallenges());
            await Shell.Current.GoToAsync("Challenge");
        }
    }

    private static List<Core.Models.Challenge> GetEmbeddedChallenges()
    {
        return new List<Core.Models.Challenge>
        {
            new()
            {
                Id = "var-001",
                Realm = "valley-of-variables",
                Title = "The Naming Ritual",
                Story = "Welcome, young Code Mage! The village elder approaches you with an ancient scroll. \"To begin your journey, you must learn the first spell of our craft - the Variable Declaration. Declare a string variable called 'heroName' and bind it to the name 'Aria'.\"",
                Type = Core.Models.ChallengeType.CodeCompletion,
                Difficulty = 1,
                XpReward = 50,
                StarterCode = "// Declare a string variable called 'heroName' and assign it the value \"Aria\"\n",
                Solution = "string heroName = \"Aria\";",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "heroName should equal \"Aria\"", Assertion = "heroName == \"Aria\"" }
                },
                Hints = new List<string>
                {
                    "In C#, we declare variables with: type name = value;",
                    "Text values are called 'strings' and need double quotes",
                    "The full syntax is: string heroName = \"Aria\";"
                },
                Concepts = new List<string> { "variables", "strings", "assignment" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "C# Variables", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations", Description = "Official docs on declaring variables in C#" },
                    new() { Title = "String Type", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/", Description = "Working with text strings in C#" },
                    new() { Title = "C# Tutorial", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/", Description = "A tour of the C# language" }
                },
                Tutorial = new Core.Models.Tutorial
                {
                    Title = "Variables: Storing Data",
                    Steps = new List<Core.Models.TutorialStep>
                    {
                        new()
                        {
                            Explanation = "In programming, a VARIABLE is like a labeled box where you can store information. Just like you might label a box 'Photos' to store pictures, you give variables names to store data.",
                            Tip = "Think of a variable as a sticky note with a name on it, attached to a value."
                        },
                        new()
                        {
                            Explanation = "In C#, every variable has a TYPE that tells the computer what kind of data it holds. For text (words, sentences, names), we use the type 'string'.",
                            CodeExample = "string",
                            Tip = "The word 'string' refers to a 'string of characters' - letters connected together!"
                        },
                        new()
                        {
                            Explanation = "To create a variable, you write: TYPE NAME = VALUE;\n\nThe equals sign (=) means 'assign this value to this variable'.",
                            CodeExample = "string playerName = \"Alex\";",
                            Tip = "The semicolon (;) at the end tells C# that your statement is complete."
                        },
                        new()
                        {
                            Explanation = "String values MUST be wrapped in double quotes (\"). This tells C# where the text starts and ends.",
                            CodeExample = "string greeting = \"Hello World\";",
                            CodeOutput = "The variable 'greeting' now holds: Hello World",
                            Tip = "Without quotes, C# would think you're referring to other variables named Hello and World!"
                        },
                        new()
                        {
                            Explanation = "Now you're ready! Create a string variable named 'heroName' and give it the value \"Aria\". Remember: type, name, equals, value in quotes, semicolon!",
                            CodeExample = "string heroName = \"Aria\";",
                            Tip = "Copy this pattern exactly - just make sure to use the name 'heroName' and the value \"Aria\"."
                        }
                    }
                },
                Exploration = new Core.Models.Exploration
                {
                    Introduction = "Let's play with string variables! Try running each example, then modify the code to see what happens.",
                    Examples = new List<Core.Models.ExplorationExample>
                    {
                        new()
                        {
                            Title = "Creating a String Variable",
                            Explanation = "This is the basic pattern for creating a string variable. The variable 'message' holds the text \"Hello!\" and we print it out.\n\nTry changing \"Hello!\" to something else and run the code!",
                            Code = "string message = \"Hello!\";\nConsole.WriteLine(message);",
                            ExpectedOutput = "Hello!"
                        },
                        new()
                        {
                            Title = "Variables Can Be Changed",
                            Explanation = "Once you create a variable, you can change its value later. Notice we don't write 'string' again - the variable already exists!\n\nTry adding a third line that changes 'name' to your own name.",
                            Code = "string name = \"Alice\";\nConsole.WriteLine(name);\nname = \"Bob\";\nConsole.WriteLine(name);",
                            ExpectedOutput = "Alice\nBob"
                        },
                        new()
                        {
                            Title = "Combining Strings",
                            Explanation = "You can combine strings using the + operator. This is called 'concatenation'.\n\nTry changing the greeting or the name!",
                            Code = "string greeting = \"Hello, \";\nstring name = \"Adventurer\";\nstring fullMessage = greeting + name + \"!\";\nConsole.WriteLine(fullMessage);",
                            ExpectedOutput = "Hello, Adventurer!"
                        },
                        new()
                        {
                            Title = "Your Turn!",
                            Explanation = "Now create your own string variable called 'heroName' with the value \"Aria\" and print it.\n\nThis is exactly what you'll need for the challenge!",
                            Code = "// Create a string variable called heroName with value \"Aria\"\n// Then print it with Console.WriteLine(heroName);\n\n",
                            ExpectedOutput = "Aria"
                        }
                    }
                }
            },
            new()
            {
                Id = "var-002",
                Realm = "valley-of-variables",
                Title = "The Counting Stone",
                Story = "The blacksmith needs help tracking his inventory. \"I need you to store the number of swords I've forged today. Create an integer variable called 'swordCount' and set it to 42.\"",
                Type = Core.Models.ChallengeType.CodeCompletion,
                Difficulty = 1,
                XpReward = 50,
                StarterCode = "// Declare an integer variable called 'swordCount' with value 42\n",
                Solution = "int swordCount = 42;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "swordCount should equal 42", Assertion = "swordCount == 42" }
                },
                Hints = new List<string>
                {
                    "Whole numbers use the 'int' type",
                    "Numbers don't need quotes like strings do",
                    "The syntax is: int swordCount = 42;"
                },
                Concepts = new List<string> { "variables", "integers", "assignment" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Integral Types", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types", Description = "Understanding int, long, and other number types" },
                    new() { Title = "C# Variables", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/declarations", Description = "How to declare and use variables" }
                }
            },
            new()
            {
                Id = "var-003",
                Realm = "valley-of-variables",
                Title = "The Merchant's Calculation",
                Story = "The traveling merchant scratches her head. \"I have 15 gold coins and just earned 27 more from selling potions. Can you calculate my total wealth? Store the result in a variable called 'totalGold'.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 2,
                XpReward = 75,
                StarterCode = "// Calculate 15 + 27 and store it in 'totalGold'\n",
                Solution = "int totalGold = 15 + 27;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "totalGold should equal 42", Assertion = "totalGold == 42" }
                },
                Hints = new List<string>
                {
                    "You can use + to add numbers together",
                    "The result of addition can be stored directly in a variable",
                    "int totalGold = 15 + 27;"
                },
                Concepts = new List<string> { "variables", "integers", "operators", "addition" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Arithmetic Operators", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/arithmetic-operators", Description = "Addition, subtraction, multiplication and more" },
                    new() { Title = "Operators Overview", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/", Description = "All C# operators explained" }
                }
            },
            new()
            {
                Id = "var-004",
                Realm = "valley-of-variables",
                Title = "The Alchemist's Precision",
                Story = "The village alchemist peers through her spectacles. \"Potions require precise measurements! I need exactly 3.14159 units of moonwater. Create a 'double' variable called 'moonwater' to store this amount.\"",
                Type = Core.Models.ChallengeType.CodeCompletion,
                Difficulty = 2,
                XpReward = 75,
                StarterCode = "// Declare a double variable called 'moonwater' with value 3.14159\n",
                Solution = "double moonwater = 3.14159;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "moonwater should equal 3.14159", Assertion = "Math.Abs(moonwater - 3.14159) < 0.00001" }
                },
                Hints = new List<string>
                {
                    "Decimal numbers use the 'double' type",
                    "double is for numbers with decimal points",
                    "double moonwater = 3.14159;"
                },
                Concepts = new List<string> { "variables", "double", "decimals" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Floating-Point Types", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types", Description = "Understanding float, double, and decimal types" },
                    new() { Title = "Numeric Types", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/numeric-conversions", Description = "Converting between number types" }
                }
            },
            new()
            {
                Id = "var-005",
                Realm = "valley-of-variables",
                Title = "The Guardian's Gate",
                Story = "The gate guardian blocks your path. \"Only those who can wield the Boolean may pass! Create a variable called 'gateOpen' and set it to true to prove your worth.\"",
                Type = Core.Models.ChallengeType.CodeCompletion,
                Difficulty = 2,
                XpReward = 75,
                StarterCode = "// Declare a bool variable called 'gateOpen' and set it to true\n",
                Solution = "bool gateOpen = true;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "gateOpen should be true", Assertion = "gateOpen == true" }
                },
                Hints = new List<string>
                {
                    "Boolean values are either 'true' or 'false'",
                    "The bool type stores true/false values",
                    "bool gateOpen = true;"
                },
                Concepts = new List<string> { "variables", "boolean", "true/false" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Boolean Type", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool", Description = "The bool type for true/false values" },
                    new() { Title = "Boolean Operators", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators", Description = "AND, OR, NOT and other logical operators" }
                }
            },

            // ========== REALM 2: FOREST OF FLOW ==========
            new()
            {
                Id = "flow-001",
                Realm = "forest-of-flow",
                Title = "The Crossroads",
                Story = "You enter the mystical Forest of Flow, where paths branch based on conditions. A wise owl perches above. \"Traveler, you must learn the way of IF. Given a variable 'health' with value 75, create a bool called 'isHealthy' that is true if health is greater than 50.\"",
                Type = Core.Models.ChallengeType.CodeCompletion,
                Difficulty = 3,
                XpReward = 100,
                StarterCode = "int health = 75;\n// Create 'isHealthy' - true if health > 50\n",
                Solution = "bool isHealthy = health > 50;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "isHealthy should be true", Assertion = "isHealthy == true" }
                },
                Hints = new List<string>
                {
                    "Comparison operators like > return a boolean value",
                    "You can assign the result of a comparison directly to a bool",
                    "bool isHealthy = health > 50;"
                },
                Concepts = new List<string> { "comparison", "boolean", "operators" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Comparison Operators", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/comparison-operators", Description = "Greater than, less than, equals and more" },
                    new() { Title = "Boolean Expressions", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators", Description = "Working with true/false logic" }
                }
            },
            new()
            {
                Id = "flow-002",
                Realm = "forest-of-flow",
                Title = "The Branching Path",
                Story = "The path splits before you. A forest spirit speaks: \"Your destiny depends on your level! If playerLevel is 5 or higher, set 'canEnterDungeon' to true, otherwise false. The adventurer's level is 7.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 3,
                XpReward = 100,
                StarterCode = "int playerLevel = 7;\nbool canEnterDungeon;\n// Use an if-else to set canEnterDungeon\n",
                Solution = "if (playerLevel >= 5) { canEnterDungeon = true; } else { canEnterDungeon = false; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "canEnterDungeon should be true", Assertion = "canEnterDungeon == true" }
                },
                Hints = new List<string>
                {
                    "Use if (condition) { } else { }",
                    "The condition checks if playerLevel >= 5",
                    "if (playerLevel >= 5) { canEnterDungeon = true; } else { canEnterDungeon = false; }"
                },
                Concepts = new List<string> { "if-else", "conditionals", "branching" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "If Statement", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-if-statement", Description = "Conditional branching with if-else" },
                    new() { Title = "Selection Statements", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements", Description = "All branching statements in C#" }
                },
                Tutorial = new Core.Models.Tutorial
                {
                    Title = "If-Else: Making Decisions",
                    Steps = new List<Core.Models.TutorialStep>
                    {
                        new()
                        {
                            Explanation = "Programs need to make decisions! An IF statement lets code do different things based on whether something is true or false.\n\nThink of it like a fork in the road: \"IF it's raining, take an umbrella. ELSE, wear sunglasses.\"",
                            Tip = "The computer can only go ONE path - either the 'if' path or the 'else' path, never both!"
                        },
                        new()
                        {
                            Explanation = "The basic structure is:\n\nif (condition)\n{\n    // do this if condition is TRUE\n}\nelse\n{\n    // do this if condition is FALSE\n}",
                            CodeExample = "if (condition)\n{\n    // true path\n}\nelse\n{\n    // false path\n}",
                            Tip = "The condition must be inside parentheses ( ). The code blocks must be inside curly braces { }."
                        },
                        new()
                        {
                            Explanation = "The CONDITION is a question that has a yes/no answer (true/false).\n\nCommon comparison operators:\n• >   greater than\n• <   less than\n• >=  greater than OR equal to\n• <=  less than OR equal to\n• ==  equals (note: TWO equal signs!)\n• !=  not equals",
                            CodeExample = "5 > 3    // true\n10 < 5   // false\n7 >= 7   // true (7 equals 7)\n3 == 3   // true\n3 != 5   // true (3 is not 5)",
                            Tip = "Be careful! One = assigns a value. Two == compares values!"
                        },
                        new()
                        {
                            Explanation = "For this challenge, you need to:\n1. Check if playerLevel >= 5 (is the level 5 or higher?)\n2. If TRUE: set canEnterDungeon = true\n3. If FALSE: set canEnterDungeon = false\n\nSince playerLevel is 7, and 7 >= 5 is true, the dungeon access should be granted!",
                            CodeExample = "if (playerLevel >= 5)\n{\n    canEnterDungeon = true;\n}\nelse\n{\n    canEnterDungeon = false;\n}",
                            Tip = "You can write this all on one line if you prefer: if (playerLevel >= 5) { canEnterDungeon = true; } else { canEnterDungeon = false; }"
                        }
                    }
                }
            },
            new()
            {
                Id = "flow-003",
                Realm = "forest-of-flow",
                Title = "The Counting Trees",
                Story = "You encounter an ancient grove where trees must be counted. The druid says: \"Use a FOR loop to count from 1 to 5, adding each number to 'total'. Start with total at 0.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 4,
                XpReward = 125,
                StarterCode = "int total = 0;\n// Use a for loop to add numbers 1 through 5 to total\n",
                Solution = "for (int i = 1; i <= 5; i++) { total += i; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "total should equal 15 (1+2+3+4+5)", Assertion = "total == 15" }
                },
                Hints = new List<string>
                {
                    "A for loop has three parts: initialization, condition, and increment",
                    "for (int i = 1; i <= 5; i++) loops from 1 to 5",
                    "Use total += i; to add each number to total"
                },
                Concepts = new List<string> { "for-loop", "iteration", "accumulator" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "For Statement", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#the-for-statement", Description = "The classic counting loop" },
                    new() { Title = "Iteration Statements", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements", Description = "All loop types in C#" }
                },
                Tutorial = new Core.Models.Tutorial
                {
                    Title = "For Loops: Counting Made Easy",
                    Steps = new List<Core.Models.TutorialStep>
                    {
                        new()
                        {
                            Explanation = "A FOR loop is perfect when you know EXACTLY how many times you want to repeat something.\n\nThink of it like counting: \"For each number from 1 to 10, do this thing.\"",
                            Tip = "Use 'for' when counting. Use 'while' when you don't know how many times to repeat."
                        },
                        new()
                        {
                            Explanation = "A for loop has THREE parts separated by semicolons:\n\nfor (START; CONDITION; STEP)\n{\n    // code to repeat\n}\n\n• START: Where to begin counting\n• CONDITION: Keep going while this is true\n• STEP: How to count (usually add 1)",
                            CodeExample = "for (int i = 1; i <= 5; i++)\n{\n    // this runs 5 times\n}",
                            Tip = "The variable 'i' is traditional for loop counters. It stands for 'index' or 'iteration'."
                        },
                        new()
                        {
                            Explanation = "Let's break down: for (int i = 1; i <= 5; i++)\n\n• int i = 1  →  Create counter 'i', start at 1\n• i <= 5     →  Keep looping while i is 5 or less\n• i++        →  After each loop, add 1 to i",
                            CodeExample = "for (int i = 1; i <= 5; i++)",
                            CodeOutput = "Loop runs with i = 1\nLoop runs with i = 2\nLoop runs with i = 3\nLoop runs with i = 4\nLoop runs with i = 5\nLoop stops (6 is NOT <= 5)",
                            Tip = "i++ means i = i + 1. It's a shortcut!"
                        },
                        new()
                        {
                            Explanation = "To ADD UP numbers, use an ACCUMULATOR - a variable that collects the total.\n\nThe += operator means 'add this to what's already there':\ntotal += i  is the same as  total = total + i",
                            CodeExample = "int total = 0;\ntotal += 5;  // total is now 5\ntotal += 3;  // total is now 8",
                            Tip = "Always start your accumulator at 0 before the loop!"
                        },
                        new()
                        {
                            Explanation = "Let's trace the complete solution:\n\nStart: total = 0\n\nLoop 1 (i=1): total = 0 + 1 = 1\nLoop 2 (i=2): total = 1 + 2 = 3\nLoop 3 (i=3): total = 3 + 3 = 6\nLoop 4 (i=4): total = 6 + 4 = 10\nLoop 5 (i=5): total = 10 + 5 = 15\n\nFinal total: 15",
                            CodeExample = "int total = 0;\nfor (int i = 1; i <= 5; i++)\n{\n    total += i;\n}",
                            Tip = "Inside the loop, 'i' changes each time. We use its value to add to total!"
                        }
                    }
                },
                Exploration = new Core.Models.Exploration
                {
                    Introduction = "Let's explore for loops! These are great when you know exactly how many times to repeat something.",
                    Examples = new List<Core.Models.ExplorationExample>
                    {
                        new()
                        {
                            Title = "Counting from 1 to 5",
                            Explanation = "The simplest for loop counts from one number to another.\n\nThe three parts:\n• int i = 1 → start at 1\n• i <= 5 → keep going while i is 5 or less\n• i++ → add 1 to i after each loop",
                            Code = "for (int i = 1; i <= 5; i++)\n{\n    Console.WriteLine(\"Count: \" + i);\n}",
                            ExpectedOutput = "Count: 1\nCount: 2\nCount: 3\nCount: 4\nCount: 5"
                        },
                        new()
                        {
                            Title = "Counting by Different Amounts",
                            Explanation = "You can count by any amount! Change i++ to i += 2 to count by twos.\n\nTry changing it to count by 3s or 5s!",
                            Code = "Console.WriteLine(\"Counting by 2s:\");\nfor (int i = 0; i <= 10; i += 2)\n{\n    Console.WriteLine(i);\n}",
                            ExpectedOutput = "Counting by 2s:\n0\n2\n4\n6\n8\n10"
                        },
                        new()
                        {
                            Title = "Adding Numbers Together",
                            Explanation = "Use an ACCUMULATOR variable to build up a total. Start at 0, then add each number.\n\nWatch how 'total' grows: 0+1=1, 1+2=3, 3+3=6...",
                            Code = "int total = 0;\n\nfor (int i = 1; i <= 5; i++)\n{\n    total += i;  // same as: total = total + i\n    Console.WriteLine(\"Added \" + i + \", total is now \" + total);\n}\n\nConsole.WriteLine(\"Final total: \" + total);",
                            ExpectedOutput = "Added 1, total is now 1\nAdded 2, total is now 3\nAdded 3, total is now 6\nAdded 4, total is now 10\nAdded 5, total is now 15\nFinal total: 15"
                        },
                        new()
                        {
                            Title = "Practice: Sum 1 to 5",
                            Explanation = "Now try it yourself! Add up the numbers from 1 to 5.\n\nYou need:\n1. int total = 0; (before the loop)\n2. A for loop from 1 to 5\n3. total += i; (inside the loop)",
                            Code = "int total = 0;\n\n// Write your for loop here to add numbers 1 through 5\n\nConsole.WriteLine(\"Total: \" + total);",
                            ExpectedOutput = "Total: 15"
                        }
                    }
                }
            },
            new()
            {
                Id = "flow-004",
                Realm = "forest-of-flow",
                Title = "The Whispering While",
                Story = "A mystical fountain whispers numbers. \"Keep subtracting 3 from 'water' (starting at 17) WHILE it remains above 5. How many times can you draw?\" Count the draws in 'draws'.",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 4,
                XpReward = 125,
                StarterCode = "int water = 17;\nint draws = 0;\n// Use a while loop: subtract 3 from water while water > 5, count draws\n",
                Solution = "while (water > 5) { water -= 3; draws++; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "draws should equal 4", Assertion = "draws == 4" },
                    new() { Description = "water should equal 5", Assertion = "water == 5" }
                },
                Hints = new List<string>
                {
                    "while (condition) { } repeats as long as condition is true",
                    "Subtract with water -= 3 and increment draws with draws++",
                    "17 -> 14 -> 11 -> 8 -> 5 (stops because 5 is not > 5)"
                },
                Concepts = new List<string> { "while-loop", "iteration", "decrement" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "While Statement", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#the-while-statement", Description = "Loop while a condition is true" },
                    new() { Title = "Increment/Decrement", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/arithmetic-operators#increment-operator-", Description = "The ++ and -- operators" }
                },
                Tutorial = new Core.Models.Tutorial
                {
                    Title = "While Loops: Repeat Until Done",
                    Steps = new List<Core.Models.TutorialStep>
                    {
                        new()
                        {
                            Explanation = "A WHILE loop is like saying: \"Keep doing this action AS LONG AS something is true.\"\n\nImagine eating cookies from a jar: \"While there are cookies, keep eating.\" You stop when the condition becomes false (no more cookies).",
                            Tip = "The loop checks the condition BEFORE each repetition. If it's false from the start, the loop never runs!"
                        },
                        new()
                        {
                            Explanation = "The basic structure of a while loop is:\n\nwhile (condition)\n{\n    // code to repeat\n}",
                            CodeExample = "while (condition)\n{\n    // do something\n}",
                            Tip = "The curly braces { } contain everything that repeats. Don't forget them!"
                        },
                        new()
                        {
                            Explanation = "Let's trace through a simple example. We'll count down from 3:",
                            CodeExample = "int count = 3;\nwhile (count > 0)\n{\n    count = count - 1;\n}",
                            CodeOutput = "count starts at 3\nLoop 1: count becomes 2 (3-1)\nLoop 2: count becomes 1 (2-1)\nLoop 3: count becomes 0 (1-1)\nLoop stops! (0 is NOT > 0)",
                            Tip = "Each time through the loop, we check: Is count > 0? If yes, run the code. If no, stop."
                        },
                        new()
                        {
                            Explanation = "There's a shortcut for subtracting! Instead of writing:\n  water = water - 3\n\nYou can write:\n  water -= 3\n\nThey mean the same thing: 'take the current value of water and subtract 3 from it'.",
                            CodeExample = "water -= 3;  // same as: water = water - 3;",
                            Tip = "The -= operator is called 'subtract and assign'. There's also += for adding!"
                        },
                        new()
                        {
                            Explanation = "To count how many times something happens, use a COUNTER variable. The ++ operator adds 1:\n\ndraws++ is the same as draws = draws + 1",
                            CodeExample = "int draws = 0;\ndraws++;  // draws is now 1\ndraws++;  // draws is now 2",
                            Tip = "Always start your counter at 0, then add 1 each time through the loop."
                        },
                        new()
                        {
                            Explanation = "For this challenge, you need to:\n1. Check if water > 5 (the while condition)\n2. Inside the loop: subtract 3 from water AND add 1 to draws\n\nLet's trace it:\n• Start: water=17, draws=0\n• Loop 1: water=14, draws=1\n• Loop 2: water=11, draws=2\n• Loop 3: water=8, draws=3\n• Loop 4: water=5, draws=4\n• Stop! (5 is NOT greater than 5)",
                            CodeExample = "while (water > 5)\n{\n    water -= 3;\n    draws++;\n}",
                            Tip = "Put both statements (water -= 3 and draws++) inside the curly braces so they both happen each loop."
                        }
                    }
                },
                Exploration = new Core.Models.Exploration
                {
                    Introduction = "Let's explore while loops step by step! Run each example and watch how the loop repeats.",
                    Examples = new List<Core.Models.ExplorationExample>
                    {
                        new()
                        {
                            Title = "A Simple Countdown",
                            Explanation = "This while loop counts down from 5 to 1. Each time through the loop, it prints the number and then subtracts 1.\n\nWatch how the loop stops when count reaches 0 (because 0 > 0 is false).",
                            Code = "int count = 5;\n\nwhile (count > 0)\n{\n    Console.WriteLine(count);\n    count = count - 1;\n}\n\nConsole.WriteLine(\"Blast off!\");",
                            ExpectedOutput = "5\n4\n3\n2\n1\nBlast off!"
                        },
                        new()
                        {
                            Title = "Counting How Many Times",
                            Explanation = "Here we use a COUNTER to track how many times the loop runs. The counter starts at 0 and goes up by 1 each time.\n\nNotice: loops++ is a shortcut for loops = loops + 1",
                            Code = "int number = 10;\nint loops = 0;\n\nwhile (number > 0)\n{\n    number = number - 3;\n    loops++;\n    Console.WriteLine(\"Loop \" + loops + \": number is now \" + number);\n}\n\nConsole.WriteLine(\"Total loops: \" + loops);",
                            ExpectedOutput = "Loop 1: number is now 7\nLoop 2: number is now 4\nLoop 3: number is now 1\nLoop 4: number is now -2\nTotal loops: 4"
                        },
                        new()
                        {
                            Title = "The -= Shortcut",
                            Explanation = "Instead of writing 'water = water - 3', you can use the shortcut 'water -= 3'. They do exactly the same thing!\n\nTry changing -= 3 to -= 5 and see how it affects the result.",
                            Code = "int water = 20;\nint draws = 0;\n\nwhile (water >= 5)\n{\n    water -= 5;  // same as: water = water - 5\n    draws++;     // same as: draws = draws + 1\n    Console.WriteLine(\"Drew water. Remaining: \" + water);\n}\n\nConsole.WriteLine(\"Total draws: \" + draws);",
                            ExpectedOutput = "Drew water. Remaining: 15\nDrew water. Remaining: 10\nDrew water. Remaining: 5\nDrew water. Remaining: 0\nTotal draws: 4"
                        },
                        new()
                        {
                            Title = "Practice: The Challenge Setup",
                            Explanation = "This is very similar to the actual challenge! We start with water=17 and subtract 3 each time WHILE water > 5.\n\nTrace through it: 17→14→11→8→5→STOP (5 is not > 5)\n\nModify this code to match what you'll need!",
                            Code = "int water = 17;\nint draws = 0;\n\n// Your while loop goes here:\n// while water is greater than 5,\n// subtract 3 from water and add 1 to draws\n\nConsole.WriteLine(\"Final water: \" + water);\nConsole.WriteLine(\"Total draws: \" + draws);",
                            ExpectedOutput = "Final water: 5\nTotal draws: 4"
                        }
                    }
                }
            },
            new()
            {
                Id = "flow-005",
                Realm = "forest-of-flow",
                Title = "The Ternary Rune",
                Story = "You find an ancient rune stone. The inscription reads: \"Master the art of the ternary! Given 'score' of 85, set 'grade' to 'Pass' if score >= 60, otherwise 'Fail'. Do this in ONE line using the ternary operator.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 4,
                XpReward = 125,
                StarterCode = "int score = 85;\n// Use the ternary operator: condition ? valueIfTrue : valueIfFalse\nstring grade = ",
                Solution = "score >= 60 ? \"Pass\" : \"Fail\";",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "grade should equal \"Pass\"", Assertion = "grade == \"Pass\"" }
                },
                Hints = new List<string>
                {
                    "The ternary operator is: condition ? trueValue : falseValue",
                    "It's a compact if-else in one expression",
                    "string grade = score >= 60 ? \"Pass\" : \"Fail\";"
                },
                Concepts = new List<string> { "ternary", "conditional-expression", "one-liner" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Conditional Operator", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator", Description = "The ?: ternary operator" },
                    new() { Title = "Expressions", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/", Description = "C# operators and expressions" }
                }
            },
            new()
            {
                Id = "flow-006",
                Realm = "forest-of-flow",
                Title = "The Switch Guardian",
                Story = "A stone golem blocks the forest exit. \"Prove you know the SWITCH! Given 'dayNumber' of 3, set 'dayName' to the day of the week (1=Monday, 2=Tuesday, 3=Wednesday, etc.)\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 5,
                XpReward = 150,
                StarterCode = "int dayNumber = 3;\nstring dayName;\n// Use a switch statement to set dayName\n",
                Solution = "switch (dayNumber) { case 1: dayName = \"Monday\"; break; case 2: dayName = \"Tuesday\"; break; case 3: dayName = \"Wednesday\"; break; default: dayName = \"Unknown\"; break; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "dayName should equal \"Wednesday\"", Assertion = "dayName == \"Wednesday\"" }
                },
                Hints = new List<string>
                {
                    "switch (variable) { case value: ... break; }",
                    "Each case handles one possible value",
                    "Don't forget the break; after each case!"
                },
                Concepts = new List<string> { "switch", "case", "pattern-matching" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Switch Statement", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement", Description = "Multi-way branching with switch" },
                    new() { Title = "Pattern Matching", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching", Description = "Modern pattern matching in C#" }
                }
            },

            // ========== REALM 3: MOUNTAINS OF METHODS ==========
            new()
            {
                Id = "method-001",
                Realm = "mountains-of-methods",
                Title = "The First Incantation",
                Story = "You ascend into the Mountains of Methods. A hermit mage greets you: \"Here we learn to create reusable spells! Write a method called 'GetGreeting' that takes no parameters and returns the string 'Hello, Adventurer!'\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 5,
                XpReward = 150,
                StarterCode = "// Define a method that returns \"Hello, Adventurer!\"\n\nstring result = GetGreeting();",
                Solution = "string GetGreeting() { return \"Hello, Adventurer!\"; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "GetGreeting() should return \"Hello, Adventurer!\"", Assertion = "result == \"Hello, Adventurer!\"" }
                },
                Hints = new List<string>
                {
                    "Methods are defined: returnType MethodName() { }",
                    "Use 'return' to send a value back",
                    "string GetGreeting() { return \"Hello, Adventurer!\"; }"
                },
                Concepts = new List<string> { "methods", "return", "functions" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Methods", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods", Description = "Creating and using methods" },
                    new() { Title = "Local Functions", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions", Description = "Methods inside methods" }
                }
            },
            new()
            {
                Id = "method-002",
                Realm = "mountains-of-methods",
                Title = "The Parameterized Spell",
                Story = "The hermit continues: \"Now create a spell that accepts input! Write a method 'Double' that takes an int parameter 'n' and returns n multiplied by 2.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 5,
                XpReward = 150,
                StarterCode = "// Define a method Double(int n) that returns n * 2\n\nint result = Double(21);",
                Solution = "int Double(int n) { return n * 2; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "Double(21) should return 42", Assertion = "result == 42" }
                },
                Hints = new List<string>
                {
                    "Parameters go inside the parentheses: MethodName(type paramName)",
                    "Return the calculation directly",
                    "int Double(int n) { return n * 2; }"
                },
                Concepts = new List<string> { "methods", "parameters", "return" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Method Parameters", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/passing-parameters", Description = "Passing data to methods" },
                    new() { Title = "Return Values", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods#return-values", Description = "Getting data back from methods" }
                }
            },
            new()
            {
                Id = "method-003",
                Realm = "mountains-of-methods",
                Title = "The Multi-Parameter Portal",
                Story = "\"Excellent progress! Now combine multiple inputs. Create a method 'Add' that takes two integers 'a' and 'b' and returns their sum.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 5,
                XpReward = 150,
                StarterCode = "// Define Add(int a, int b) that returns a + b\n\nint result = Add(17, 25);",
                Solution = "int Add(int a, int b) { return a + b; }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "Add(17, 25) should return 42", Assertion = "result == 42" }
                },
                Hints = new List<string>
                {
                    "Multiple parameters are separated by commas",
                    "int Add(int a, int b) defines a method with two int parameters",
                    "int Add(int a, int b) { return a + b; }"
                },
                Concepts = new List<string> { "methods", "multiple-parameters", "addition" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Method Parameters", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/passing-parameters", Description = "Working with multiple parameters" }
                }
            },
            new()
            {
                Id = "method-004",
                Realm = "mountains-of-methods",
                Title = "The Expression Body",
                Story = "\"There is an ancient shorthand for simple spells! Rewrite your Add method using an expression body (=>) instead of curly braces.\"",
                Type = Core.Models.ChallengeType.SpellCraft,
                Difficulty = 6,
                XpReward = 175,
                StarterCode = "// Define Add using expression body syntax: returnType Name(params) => expression;\n\nint result = Add(17, 25);",
                Solution = "int Add(int a, int b) => a + b;",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "Add(17, 25) should return 42", Assertion = "result == 42" }
                },
                Hints = new List<string>
                {
                    "Expression body uses => instead of { return ... }",
                    "It's a shorthand for simple one-expression methods",
                    "int Add(int a, int b) => a + b;"
                },
                Concepts = new List<string> { "expression-body", "lambda", "shorthand" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Expression Body", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members", Description = "Concise syntax for simple members" },
                    new() { Title = "Lambda Expressions", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions", Description = "Anonymous functions and lambdas" }
                }
            },
            new()
            {
                Id = "method-005",
                Realm = "mountains-of-methods",
                Title = "The Recursive Riddle",
                Story = "At the mountain peak, a sphinx poses a challenge: \"Create a method 'Factorial' that calculates n! recursively. Factorial of 5 is 5 * 4 * 3 * 2 * 1 = 120. Remember: Factorial(0) = 1.\"",
                Type = Core.Models.ChallengeType.BossBattle,
                Difficulty = 7,
                XpReward = 250,
                StarterCode = "// Define Factorial(int n) recursively\n// Base case: if n <= 1, return 1\n// Recursive case: return n * Factorial(n - 1)\n\nint result = Factorial(5);",
                Solution = "int Factorial(int n) { if (n <= 1) return 1; return n * Factorial(n - 1); }",
                TestCases = new List<Core.Models.TestCase>
                {
                    new() { Description = "Factorial(5) should return 120", Assertion = "result == 120" }
                },
                Hints = new List<string>
                {
                    "Recursion means a method calls itself",
                    "You need a base case (n <= 1 returns 1) to stop the recursion",
                    "The recursive case: return n * Factorial(n - 1)"
                },
                Concepts = new List<string> { "recursion", "factorial", "base-case" },
                ReadingMaterial = new List<Core.Models.ReadingMaterial>
                {
                    new() { Title = "Recursion", Url = "https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods#recursive-methods", Description = "Methods that call themselves" },
                    new() { Title = "Recursion Tutorial", Url = "https://learn.microsoft.com/en-us/training/modules/csharp-call-methods/", Description = "Learning about method calls" }
                }
            }
        };
    }
}
