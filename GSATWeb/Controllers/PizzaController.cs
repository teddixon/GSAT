using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GSATWeb.Controllers
{
    /// <summary>
    /// PizzaController inherits from ControllerBase
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly ILogger<PizzaController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Base HttpGet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<GSATLibrary.FavoriteToppingsReport>> Get()
        {
            // This simulates a list of the most recent 10000 pizza orders with toppings
            // from which we want to get the current most popular toppings (Pepperoni, Pineapple etc.)
            string uri = "http://teddixon.com/pizzas.json";
            int takeCount = 20;

            // The the get the current 20 trending favorite topping from the orders.
            var x = await GSATLibrary.Pizza.GetFavoriteToppings(uri, takeCount);
            return x;
        }
    }
}
