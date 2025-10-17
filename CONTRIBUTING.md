# Contributing to KleeneStar

Thank you for your interest in contributing to KleeneStar! This document provides guidelines for contributing to the project.

## Getting Started

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/kleenestar.git
   cd kleenestar
   ```
3. Create a branch for your changes:
   ```bash
   git checkout -b feature/your-feature-name
   ```

## Development Setup

### Prerequisites
- .NET 9.0 SDK or later
- A code editor (Visual Studio, VS Code, or Rider recommended)

### Building the Project
```bash
dotnet restore
dotnet build
```

### Running Tests
```bash
dotnet test
```

### Running the Application
```bash
cd src/KleeneStar.Core
dotnet run
```

## Code Guidelines

### General Principles
- Write clean, maintainable, and testable code
- Follow SOLID principles
- Keep modules independent and loosely coupled
- Write meaningful commit messages

### Code Style
- Follow standard C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments to public APIs
- Keep methods short and focused
- Use async/await for asynchronous operations

### Module Development
When creating new modules:
- Implement the `IModule` interface from `KleeneStar.Abstractions`
- Provide clear name, version, and description
- Handle initialization and shutdown gracefully
- Use dependency injection where appropriate
- Log important events and errors

### Documentation
- Update README.md if adding major features
- Add XML documentation to public APIs
- Include code examples for complex features
- Update API documentation for endpoint changes

## Submitting Changes

### Pull Request Process
1. Ensure your code builds without errors or warnings
2. Test your changes thoroughly
3. Update documentation as needed
4. Commit your changes with clear commit messages
5. Push to your fork
6. Create a Pull Request against the main repository

### Commit Messages
Follow these guidelines for commit messages:
- Use present tense ("Add feature" not "Added feature")
- Use imperative mood ("Move cursor to..." not "Moves cursor to...")
- Start with a capital letter
- Keep the first line under 50 characters
- Add detailed description in the body if needed

Example:
```
Add module hot reload support

- Implement file system watcher for module directories
- Add reload functionality to ModuleLoader
- Update documentation with hot reload instructions
```

### Pull Request Guidelines
- Provide a clear description of the changes
- Reference any related issues
- Ensure all checks pass
- Be responsive to feedback and questions
- Keep pull requests focused and atomic

## Reporting Issues

When reporting issues, please include:
- A clear, descriptive title
- Steps to reproduce the issue
- Expected behavior
- Actual behavior
- Environment details (OS, .NET version)
- Any relevant logs or error messages

## Module Contribution Guidelines

If you're contributing a new module:
1. Place it in the `src/` directory with naming convention `KleeneStar.Modules.YourModule`
2. Implement the `IModule` interface
3. Add comprehensive documentation
4. Include usage examples
5. Add appropriate tests
6. Update the main README.md with module information

## Code of Conduct

- Be respectful and inclusive
- Welcome newcomers and help them get started
- Focus on constructive feedback
- Respect differing viewpoints and experiences
- Accept responsibility for mistakes and learn from them

## Questions?

If you have questions about contributing, feel free to:
- Open an issue for discussion
- Reach out to the maintainers
- Check existing documentation and issues

Thank you for contributing to KleeneStar!
