using Microsoft.AspNetCore.Mvc;
using shoppingList20.Models;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace shoppingList20.Controllers
{

    // route zum controller Festlegen
    [Route ("shoppingList/v2/api/")]   // https:// .../shoppinglist/v1/api
    [ApiController] // bestimmtes automatisches Verhalten
    public class ShoppingListController20V2: ControllerBase
    {
        // articles will be stored in a list element
        private static List<Article> shoppingArticles = new List<Article>();

        /// <summary>
        /// ctor
        /// </summary>
        public ShoppingListController20V2()
        {
            Article articleOne = new Article { ID = 1, Name = "Milch", Amount = "2 Liter", Remark = "3,5% Fett", IsBought = false };
            shoppingArticles.Add(articleOne);
            Article articleTwo = new Article { ID = 2, Name = "Wasser", Amount = "4 Kisten", Remark = "Still", IsBought = false };
            shoppingArticles.Add(articleTwo);
            Article articleThree = new Article { ID = 3, Name = "Fanta", Amount = "1 Liter", Remark = "Leckerer als cola", IsBought = false };
            shoppingArticles.Add(articleThree);
            Article articleFour = new Article { ID = 4, Name = "Bananen", Amount = "6 Stückt", Remark = "", IsBought = false };
            shoppingArticles.Add(articleFour);
        }

        #region // http://.../shoppingListController20/api
        /// <summary>
        /// Method to check if webservice is running
        /// </summary>
        /// <returns>Return an information </returns>

        [HttpGet] // Attribut dass es sich um ein Get Wert handelt
        // Responsetype
        [ProducesResponseType(typeof(string), 200)] // Text zu fehler Code Ok
        [ProducesResponseType(typeof(string), 404)] // ...//....//... not found
        [ProducesResponseType(typeof(string), 500)] // ...//....//... server error
        
        public string Get(string name)
        {

                return "*** ShoppingList Rest API is running.***" + name;
        }
        #endregion


       

      

      
    }

}
