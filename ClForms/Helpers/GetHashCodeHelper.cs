using System.Linq;

namespace ClForms.Helpers
{
    /// <summary>
    /// Internal helper for calculate object hash code
    /// </summary>
    internal class GetHashCodeHelper
    {
        internal const int PrimeNumber = 19;
        private const int InitializeHashPrimeNumber = 17;
        private const int IterationPrimeNumber = 23;

        /// <summary>
        /// Returns the hash code by object property values
        /// </summary>
        internal static int CalculateHashCode(params int[] propertyValues)
        {
            var hash = InitializeHashPrimeNumber;
            unchecked
            {
                hash = propertyValues.Aggregate(hash, (current, value) => current * IterationPrimeNumber + value);
            }

            return hash;
        }
    }
}
