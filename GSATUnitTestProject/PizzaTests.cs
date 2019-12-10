using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace GSATUnitTestProject
{
    /// <summary>
    /// PizzaTests
    /// </summary>
    [TestClass]
    public class PizzaTests
    {
        /// <summary>
        /// TestGetFavoriteToppings
        /// </summary>
        [TestMethod]
        public void TestGetFavoriteToppings()
        {
            // Enhancement, get these from console args
            string uri = "http://files.olo.com/pizzas.json";
            int takeCount = 20;

            // Spin the Get Favorite Toppings task onto it's own thread then wait for it to finish.
            var t = Task.Run(() => GSATLibrary.Pizza.GetFavoriteToppings(uri, takeCount));
            t.Wait();

            // At least verify the count
            Assert.IsTrue(t.Result.Count == 20);
        }
    }
}
