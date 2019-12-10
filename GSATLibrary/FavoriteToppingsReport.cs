using System;

namespace GSATLibrary
{
    /// <summary>
    /// FavoriteToppingsReport
    /// </summary>
    public class FavoriteToppingsReport
    {
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Comma Separated List of Toppings
        /// </summary>
        public string Toppings { get; set; }

        /// <summary>
        /// ToppingsHash
        /// </summary>
        public int ToppingsHash { get; set; }
    }
}
