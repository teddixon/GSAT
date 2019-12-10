using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace GSATLibrary
{
    /// <summary>
    /// Pizza Class
    /// </summary>
    public class Pizza
    {
        // Single instance of HttpClient
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Method to Get Favorite Toppings (Top 20 by default)
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public static async Task<List<FavoriteToppingsReport>> GetFavoriteToppings(string uri, int topCount = 20)
        {
            var rpt = new List<FavoriteToppingsReport>();

            // Get Pizza Orders from web service
            var pizzaOrders = await GetPizzaOrders(uri);

            // Assign a distinct Toppings Hash to each Pizza order so we don't have to mess with the order of toppings.
            pizzaOrders.ToList().ForEach(po => po.ToppingsHash = GetToppingListHashCode(po.Toppings));

            // Use LINQ to group by Distinct Hash and Count and order by Count descending. 
            // Include the orders as a sublist that meet that distinct hash.
            foreach (var pizzaOrder in pizzaOrders.GroupBy(info => info.ToppingsHash)
                        .Select(toppingsGroup => new
                        {
                            Count = toppingsGroup.Count(),
                            toppingsGroup,
                            ToppingsHash = toppingsGroup.Key,
                        })
                        .OrderByDescending(x => x.Count)
                        .Take(topCount))
            {
                // Pivot the toppings list into a comma seperated value list. 
                // Pick the toppings order from the first pizza order in the grouping.
                string toppingsCsv = String.Join(", ", pizzaOrder.toppingsGroup.FirstOrDefault().Toppings.Select(x => x.ToString()).ToArray());

                rpt.Add(new FavoriteToppingsReport()
                {
                    Count = pizzaOrder.Count,
                    Toppings = toppingsCsv,
                    ToppingsHash = pizzaOrder.ToppingsHash
                });
            }
            return rpt;
        }

        /// <summary>
        /// Get Pizza Orders from Web Service
        /// </summary>
        /// <returns></returns>
        public static async Task<List<PizzaOrder>> GetPizzaOrders(string uri)
        {
            // Make call over Internet to JSON WebService
            var json = await _httpClient.GetStringAsync(uri);

            // Deseriaize and ORM map the JSON content into c# class object.
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            var pizzaOrders = System.Text.Json.JsonSerializer.Deserialize<List<PizzaOrder>>(json, options);

            return pizzaOrders;
        }

        /// <summary>
        /// Returns a distinct hash for a distinct set of toppings regardless of the topping list order (e.g. pepperoni, mushrooms == mushrooms, pepperoni). 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequence"></param>
        /// <returns></returns>
        public static int GetToppingListHashCode(List<string> sequence)
        {
            return sequence
                .Select(item => item.GetHashCode())
                .Aggregate((total, nextCode) => total ^ nextCode);
        }
    }
}
