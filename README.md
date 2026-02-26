# DotNetQuest

A gamified C# learning application built with .NET MAUI. Learn programming through interactive coding challenges set in an RPG-style adventure.

## Features

- **Interactive Code Editor** - Monaco Editor (same as VS Code) with C# syntax highlighting
- **Real-time Compilation** - Powered by Roslyn for instant feedback
- **Gamified Learning** - XP, levels, and progression through themed "realms"
- **Step-by-step Tutorials** - Each challenge includes guided learning
- **Exploration Mode** - Interactive code examples to experiment with

## Realms

| Realm | Concepts | Challenges |
|-------|----------|------------|
| Valley of Variables | Variables, types, assignment | 5 |
| Forest of Flow | Conditionals, loops, operators | 6 |
| Mountains of Methods | Functions, parameters, recursion | 5 |
| Castle of Classes | OOP, inheritance, encapsulation | 5 |
| The Async Abyss | Async/await, Task, parallel ops | 5 |

**Total: 26 challenges** across 5 realms

## Tech Stack

- **.NET 10.0** / **MAUI** - Cross-platform UI
- **Blazor WebView** - Embedded web components
- **Monaco Editor** - Code editing
- **Roslyn** - C# compilation
- **MVVM Toolkit** - Architecture

## Getting Started

### Prerequisites

- .NET 10.0 SDK
- macOS (currently targets Mac Catalyst)

### Build & Run

```bash
dotnet build
dotnet run --project src/DotNetQuest.App
```

### Run Tests

```bash
dotnet test
```

## Project Structure

```
DotNetQuest/
├── src/
│   ├── DotNetQuest.App/        # MAUI application
│   ├── DotNetQuest.Core/       # Domain models & services
│   └── DotNetQuest.CodeEngine/ # Roslyn code compilation
├── tests/
│   ├── DotNetQuest.Core.Tests/
│   └── DotNetQuest.CodeEngine.Tests/
└── content/
    └── challenges/             # Challenge definitions (JSON)
```

## Adding New Challenges

Challenges are defined as JSON files in `content/challenges/{realm}/`. See existing challenges for the schema.

Example:
```json
{
  "id": "var-001",
  "realm": "valley-of-variables",
  "title": "The Naming Ritual",
  "story": "Your quest begins...",
  "type": "CodeCompletion",
  "difficulty": 1,
  "xpReward": 50,
  "starterCode": "// Your code here",
  "solution": "string answer = \"correct\";",
  "testCases": [
    { "description": "Test description", "assertion": "answer == \"correct\"" }
  ],
  "hints": ["Hint 1", "Hint 2"]
}
```

## License

MIT
