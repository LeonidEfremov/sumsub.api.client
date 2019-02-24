using System;
using System.Globalization;

namespace SumSub.Api.Tests
{
    /// <summary>Randomization helper.</summary>
    public static class Randomization
    {
        /// <summary><see cref="Random"/> instance.</summary>
        public static readonly Random Random = new Random();

        /// <summary>Get random enum value.</summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <returns>Random enum value.</returns>
        public static T GetRandom<T>()
            where T : struct, IConvertible
        {
            var array = Enum.GetValues(typeof(T));
            var value = (T)array.GetValue(Random.Next(0, array.Length));

            return value;
        }

        /// <summary>Get random Boolean.</summary>
        /// <returns>Random bool value.</returns>
        public static bool GetBool() => Random.Next(100) <= 50;

        /// <summary>Create random array of strings.</summary>
        /// <param name="mark">Mark for random string.</param>
        /// <param name="length">Length of array.</param>
        /// <returns>Array of string.</returns>
        public static string[] GetStringArray(string mark, int? length = null)
        {
            var count = length ?? Random.Next(100);
            var result = new string[count];

            for (var i = 0; i < count; i++)
            {
                var s = i.ToString(CultureInfo.InvariantCulture);

                result[i] = $"{mark}{s}";
            }

            return result;
        }
    }
}