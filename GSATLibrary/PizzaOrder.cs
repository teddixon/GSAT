using System;
using System.Collections.Generic;
using System.Text;

namespace GSATLibrary
{
    /// <summary>
    /// Pizza Order Class inheriting from Order Class
    /// </summary>
    public class PizzaOrder : Order
    {
        /// <summary>
        /// Toppings on Pizza Order
        /// </summary>
        public System.Collections.Generic.List<string> Toppings { get; set; }


        /// <summary>
        /// Distinct Hash Code for toppings regardless or toppings order in list.
        /// </summary>
        public int ToppingsHash { get; set; }
    }
}