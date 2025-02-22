using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using NLog.Web;
using shoppingList20.Models;
using System.Runtime.CompilerServices;
using System.Xml.Schema;


namespace shoppingList20.Controllers
{

    // route zum controller Festlegen
    [Route ("shoppingListController20/v1/api/")]   // https:// .../shoppinglist/v1/api
    [ApiController] // bestimmtes automatisches Verhalten
    public class ShoppingListController20: ControllerBase
    {
        // articles will be stored in a list element
        private static List<Article> shoppingArticles = new List<Article>();

        Logger mylogger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        /// <summary>
        /// ctor
        /// </summary>
        public ShoppingListController20()
        {
            Article articleOne = new Article { ID = 1, Name = "Milch", Amount = "2 Liter", Remark = "3,5% Fett", IsBought = false };
            shoppingArticles.Add(articleOne);
            Article articleTwo = new Article { ID = 2, Name = "Wasser", Amount = "4 Kisten", Remark = "Still", IsBought = false };
            shoppingArticles.Add(articleTwo);
            Article articleThree = new Article { ID = 3, Name = "Fanta", Amount = "1 Liter", Remark = "Leckerer als cola", IsBought = false };
            shoppingArticles.Add(articleThree);
            Article articleFour = new Article { ID = 4, Name = "Bananen", Amount = "6 Stückt", Remark = "", IsBought = false };
            shoppingArticles.Add(articleFour);
            mylogger.Info("ShoppinglistController initialized");
        }

        #region
        /// <summary>
        /// Method to check if webservice is running
        /// </summary>
        /// <returns>Return an information </returns>

        [HttpGet] // Attribut dass es sich um ein Get Wert handelt
        // Responsetype
        [ProducesResponseType(typeof(string), 200)] // Text zu fehler Code Ok
        [ProducesResponseType(typeof(string), 404)] // ...//....//... not found
        [ProducesResponseType(typeof(string), 500)] // ...//....//... server error
        [Obsolete("Methode is decrepeted please do not use")]
        public string Get()
        {

                
            mylogger.Info("ShoppingListController 'GET' passed");
            return "*** ShoppingList Rest API is running.***";
        }
        #endregion


        #region // https:// ..../ShoppingList/api/additem
        /// <summary>
        /// Methode to add an article
        /// </summary>
        /// <param name="article">Specifies an object of types 'article'. </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        
        [HttpPost("additem",Name = "PostAddItem")] // [HttpPost("NamenStyle der URL ergänzen "m Name = "Interne Name"))]
        public IActionResult PostAddItem([FromBody]Article article) // [FromBody] wegen Parameterübergabe per object
        {

            try
            {
                if (article == null)
                {
                    return BadRequest(ModelState); // Status code 400
                }
                if (shoppingArticles.Exists(x=>x.Name == article.Name))
                {
                    //    ModelState.AddModelError("", "The specified article still exists");
                    //    return StatusCode(409, ModelState); // Status code for client conflict
                    return Conflict("The specified article still exists");

                }
                shoppingArticles.Add(article); // article der Liste Hinzufügen
                                               //    return Ok();             // Status code 200
                return StatusCode(201, article); //201 = Created
            //    return CreatedAtAction();
            }
            catch(Exception exception)
            {
                return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
            }
        }


        #endregion

        #region // https:// ..../ShoppingList/api/additem/{name}/{amount}/{remark}
        /// <summary>
        /// Methode to add an article
        /// </summary>
        /// <param name="name"> Specifies the name of an article</param>
        /// <param name="amount">Specifies the amount of an article</param>
        /// <param name="remark">Specifies the remark of an article</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPost("additem/{name}/{amount}/{remark}", Name = "PostAddItemUrl")] // [HttpPost("NamenStyle der URL ergänzen "m Name = "Interne Name"))]
        public IActionResult PostAddItemUrl(string name, string amount, string remark) // [FromBody] wegen Parameterübergabe per object
        {


            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState); // Status code 400
                }
                if (shoppingArticles.Exists(x => x.Name == name))
                {
                    //    ModelState.AddModelError("", "The specified article still exists");
                    //    return StatusCode(409, ModelState); // Status code for client conflict
                    return Conflict("The specified article still exists");

                }

                if (string.IsNullOrEmpty(amount))
                {
                    amount = "-";
                }

                if (string.IsNullOrEmpty(remark))
                {
                    remark = "-";
                }

                // create article object
                Article myArticle = new Article();
                myArticle.Name = name;
                myArticle.Amount = amount;
                myArticle.Remark = remark;
                myArticle.IsBought = false;
                myArticle.ID = shoppingArticles.Count + 1;
                
                shoppingArticles.Add(myArticle); // article der Liste Hinzufügen
                                               //    return Ok();             // Status code 200
                return StatusCode(201, myArticle); //201 = Created
                                                 //    return CreatedAtAction();
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
            }
        }


        #endregion

        #region // https:// ..../ShoppingListController20/api/getarticles
        /// <summary>
        /// Get a list of all articles
        /// </summary>
        /// <returns>Return result code and a list of type 'articles'</returns>

        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("getaricles", Name = "Getarticles")] // ("Namestyle der Url", "interner Name")
        public IActionResult GetArticles()
        {
            return Ok(shoppingArticles);
        }
        #endregion

        #region // https:// ..../ShoppingListController20/api/getarticles/{name}
        /// <summary>
        /// Get a list of an single article by name
        /// </summary>
        /// <param name="name">Specifies the name of the required article</param>
        /// <returns>Return result code and a single 'article'</returns>
        


        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpGet("getaricles/{name}", Name = "Getarticle")] // ("Namestyle der Url", "interner Name")
        public IActionResult GetArticle(string name)
        {
            

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState);
                }
                // search for specific item in collection/database
                var articleResult = shoppingArticles.Find(x=>x.Name == name);
                if (articleResult== null)
                {
                    return NotFound(name);
                }
                return Ok(articleResult);

            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
            }

           
        }

        #endregion


        #region // https:// ..../ShoppingListController20/api/updatearticles/{name}
        /// <summary>
        /// Methode to update an article
        /// </summary>
        /// <param name="name">Specifies the name of the article to be updated. </param>
        /// <param name="article">Specifies an object of types 'article'. </param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [HttpPut("updatearticles/{name}", Name = "PutUpdateItem")] // [HttpPost("NamenStyle der URL ergänzen "m Name = "Interne Name"))]
        
        
        public IActionResult UpdateArticles(string name, [FromBody] Article article) // [FromBody] wegen Parameterübergabe per object
        {

            try
            {
                if (article == null || String.IsNullOrEmpty(name))
                {
                    return BadRequest(ModelState); // Status code 400
                }
                if (name != article.Name)
                {
                    return BadRequest("Submitted parameter 'name' differ from the submitted object 'article.Name' property");
                }
                if (!shoppingArticles.Exists(x => x.Name == article.Name))
                {
                    return NotFound("Article was not found" + name);

                }

                //Update article

                var articleToBeUpdated = shoppingArticles.Find(y => y.Name == name);
                articleToBeUpdated.IsBought = article.IsBought;
                articleToBeUpdated.Amount = article.Amount;
                articleToBeUpdated.Remark = article.Remark;
                // ID nicht ändert DTOs
                // Name ebenfalls nicht ändert da momentant genutzt zum Identifizieren der Articles

                return Ok(articleToBeUpdated);
                
            }
            catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
            }
        }


        #endregion


        #region // https:// ..../ShoppingListController20/api/deletearticles
        /// <summary>
        /// delete all articles
        /// </summary>
        /// <returns>Returns result code.'</returns>

        [ProducesResponseType(typeof(string), 200)]
        [HttpDelete("deletearticles", Name = "DeleteArticles")] // ("Namestyle der Url", "interner Name")
        public IActionResult Deletearticles()
        {
            try
            {
                shoppingArticles.Clear();
                return NoContent();
            }
             catch (Exception exception)
            {
                return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
            }
        }


        #endregion
          

              #region // https:// ..../ShoppingListController20/api/deletearticles/{name}
              /// <summary>
              /// Get a list of an single article by name
              /// </summary>
              /// <param name="name">Specifies the name of the required article</param>
              /// <returns>Return result code and a single 'article'</returns>



              [ProducesResponseType(typeof(string), 200)]
              [ProducesResponseType(typeof(string), 400)]
                [ProducesResponseType(typeof(string), 404)]
                [HttpDelete("deletearicles/{name}", Name = "DeleteArticle")] // ("Namestyle der Url", "interner Name")
              public IActionResult DeleteArticle(string name)
              {


                  try
                  {
                      if (string.IsNullOrEmpty(name))
                      {
                          return BadRequest(ModelState);
                      }
                      // search for specific item in collection/database
                      var articleResult = shoppingArticles.Find(x => x.Name == name);
                      if (articleResult == null)
                      {
                          return NotFound(name);
                      }

                   shoppingArticles.Remove(articleResult);
                   return NoContent();

                  }
                  catch (Exception exception)
                  {
                      return StatusCode(500, "Internal error occured: " + exception.Message + "InnerException:" + exception.InnerException);
                  }


              }

              #endregion


      
    }

}
