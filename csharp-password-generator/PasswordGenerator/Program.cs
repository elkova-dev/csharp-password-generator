using System.Security.Cryptography;
using System.Text;

namespace PasswordGenerator;

internal static class Program
{
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string Symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";
    private const int MaximumLength = 4096;

    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== Password Generator ===");

        int length = ReadLength();
        bool useLetters = ReadYesNo("Include letters? (y/n): ");
        bool useDigits = ReadYesNo("Include digits? (y/n): ");
        bool useSymbols = ReadYesNo("Include symbols? (y/n): ");

        try
        {
            string password = GeneratePassword(length, useLetters, useDigits, useSymbols);
            Console.WriteLine($"\nGenerated password: {password}");
        }
        catch (ArgumentException ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.ExitCode = 1;
        }
    }

    private static int ReadLength()
    {
        while (true)
        {
            Console.Write($"Password length (1-{MaximumLength}): ");
            string? input = Console.ReadLine();
            if (input is null)
                throw new EndOfStreamException("No password length was provided.");

            if (int.TryParse(input, out int length) && length >= 1 && length <= MaximumLength)
                return length;

            Console.WriteLine("Enter a valid positive whole number within the allowed range.");
        }
    }

    private static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? response = Console.ReadLine()?.Trim().ToLowerInvariant();
            if (response is null)
                throw new EndOfStreamException("The input stream was closed.");
            if (response is "y" or "yes" or "+")
                return true;
            if (response is "n" or "no" or "-")
                return false;

            Console.WriteLine("Enter y or n.");
        }
    }

    private static string GeneratePassword(int length, bool letters, bool digits, bool symbols)
    {
        var selectedSets = new List<string>();
        if (letters) selectedSets.Add(Letters);
        if (digits) selectedSets.Add(Digits);
        if (symbols) selectedSets.Add(Symbols);

        if (selectedSets.Count == 0)
            throw new ArgumentException("Select at least one character category.");
        if (length < selectedSets.Count)
            throw new ArgumentException(
                $"Length must be at least {selectedSets.Count} for the chosen categories.");

        string allCharacters = string.Concat(selectedSets);
        var password = new char[length];
        int position = 0;

        // Guarantee at least one character from each selected category.
        foreach (string set in selectedSets)
            password[position++] = set[RandomNumberGenerator.GetInt32(set.Length)];

        while (position < password.Length)
            password[position++] = allCharacters[RandomNumberGenerator.GetInt32(allCharacters.Length)];

        // Cryptographically secure Fisher-Yates shuffle removes predictable positions.
        for (int index = password.Length - 1; index > 0; index--)
        {
            int other = RandomNumberGenerator.GetInt32(index + 1);
            (password[index], password[other]) = (password[other], password[index]);
        }

        return new string(password);
    }
}
