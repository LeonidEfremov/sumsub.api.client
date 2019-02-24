using System.Collections.Generic;
using System.Linq;

namespace SumSub.Api.Tests.Extensions
{
    /// <summary>Global Lists extension.</summary>
    public static class ListExtensions
    {
        /// <summary>Return random element from list.</summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="enumerable">Enumeration.</param>
        /// <returns>Enumeration element.</returns>
        public static T RandomElement<T>(this List<T> enumerable)
        {
            var position = Randomization.Random.Next(0, enumerable.Count);

            return enumerable.ElementAt(position);
        }
    }
}
