using System;
using System.Collections.Generic;

public static class RandomizeHelper
{
    private static readonly Random _random = new Random();
    private static readonly HashSet<int> _usedIds = new HashSet<int>(); // To ensure unique IDs
    private static readonly List<string> _animalNames = new List<string>
    {
        "Lion", "Tiger", "Leopard", "Cheetah", "Wolf", "Fox", "Bear", "Elephant", "Giraffe", "Zebra",
        "Kangaroo", "Panda", "Monkey", "Gorilla", "Chimpanzee", "Dolphin", "Whale", "Shark", "Eagle", "Hawk"
    };
    private static readonly HashSet<string> _usedSpeciesNames = new HashSet<string>(); // To ensure unique species names

    /// <summary>
    /// Generates a unique 10-digit ID.
    /// </summary>
    /// <returns>A unique 10-digit integer.</returns>
    public static int GenerateUniqueId()
    {
        int id;
        do
        {
            id = _random.Next(1000000000, 2000000000); // Generates a 10-digit number
        } while (_usedIds.Contains(id)); // Ensure uniqueness

        _usedIds.Add(id);
        return id;
    }

    /// <summary>
    /// Generates a random unique name for genome species from animal names.
    /// </summary>
    /// <returns>A unique species name.</returns>
    public static string GenerateUniqueSpeciesName()
    {
        string name;
        do
        {
            int index = _random.Next(_animalNames.Count);
            name = _animalNames[index];
        } while (_usedSpeciesNames.Contains(name)); // Ensure uniqueness

        _usedSpeciesNames.Add(name);
        return name;
    }

    /// <summary>
    /// Generates a random double number within the specified range [min, max].
    /// </summary>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>A random double number.</returns>
    public static double GenerateRandomDouble(double min, double max)
    {
        if (min > max)
            throw new ArgumentException("Min value cannot be greater than max value.");

        return _random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Generates a random integer number within the specified range [min, max].
    /// </summary>
    /// <param name="min">The minimum value (inclusive).</param>
    /// <param name="max">The maximum value (inclusive).</param>
    /// <returns>A random integer number.</returns>
    public static int GenerateRandomInt(int min, int max)
    {
        if (min > max)
            throw new ArgumentException("Min value cannot be greater than max value.");

        return _random.Next(min, max + 1); // +1 to include max in the range
    }

    /// <summary>
    /// Generates a random boolean value based on the specified probability.
    /// </summary>
    /// <param name="probability">The probability that the response is true (0.0 to 1.0).</param>
    /// <returns>A random boolean value.</returns>
    public static bool GenerateRandomBool(double probability = 0.5)
    {
        if (probability < 0.0 || probability > 1.0)
            throw new ArgumentException("Probability must be between 0.0 and 1.0.");

        return _random.NextDouble() < probability;
    }

}