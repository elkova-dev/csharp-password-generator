# C# Password Generator

A lightweight console application that creates cryptographically random passwords. Originally a small educational C# exercise, refactored into an English-language .NET project for a developer portfolio.

## Features

- Select a password length (1-4096 characters).
- Include letters, digits, and/or symbols.
- At least one character from every enabled category.
- Input validation and helpful error messages.
- Uses `RandomNumberGenerator.GetInt32`, not the predictable `Random` class.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Run

Open `PasswordGenerator.slnx` in a compatible Visual Studio version, or run:

```bash
dotnet run --project PasswordGenerator/PasswordGenerator.csproj
```

Example interaction:

```text
=== Password Generator ===
Password length (1-4096): 16
Include letters? (y/n): y
Include digits? (y/n): y
Include symbols? (y/n): y

Generated password: (random result shown here)
```

## Design notes

A secure random selection and shuffle are used. This is a learning project, **not** a password manager; generated passwords are printed to the terminal and may remain visible in your terminal history or recording.

## Original coursework and changes

The original `ConsoleApp` used `Random` and `int.Parse`. The refactor retains its interactive console workflow while replacing the RNG, validating input, and giving the project meaningful names.

## License

MIT. See [LICENSE](LICENSE).
