using System;
using System.Threading.Tasks;

namespace GSATConsole
{
    /// <summary>
    /// Program Class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main Method of App
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string uri = "http://teddixon.com/pizzas.json";
            int takeCount = 20;

            // Spin the Get Favorite Toppings task onto it's own thread then wait for it to finish.
            var t = Task.Run(() => GSATLibrary.Pizza.GetFavoriteToppings(uri, takeCount));
            t.Wait();

            foreach (GSATLibrary.FavoriteToppingsReport pizzaOrder in t.Result)
            {
                // Write out the results to the console for the purposes of this exercise (don't need to check if IsInvoke required here)
                Console.WriteLine("Order Count: {0}, Toppings: {1}", pizzaOrder.Count, pizzaOrder.Toppings);

                // Techy version that lets me see the hash too.
                //Console.WriteLine("Order Count: {0}, Toppings: {1}   (Toppings Hash: {2})", pizzaOrder.Count, pizzaOrder.Toppings, pizzaOrder.ToppingsHash);
            }
        }
    }
}
