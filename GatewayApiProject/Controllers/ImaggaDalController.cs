using Microsoft.AspNetCore.Mvc;
using GatewayApiProject.Models;
using Newtonsoft.Json;


namespace GatewayApiProject.Controllers
{

    /// <summary>
    /// Controller responsible for interacting with the Imagga API to analyze images and check if they match specific categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaDalController : ControllerBase
    {
        // GET: api/<ImaggaController>
        /// <summary>
        /// This endpoint takes an image URL and an optional category as input and checks if the image matches the specified category. 
        /// If no category is provided, it checks for the presence of ice cream-related tags in the image.
        /// </summary>
        /// <param name="url">The URL of the image to send to Imagga for analysis.</param>
        /// <param name="category">The category to check the image against (optional).</param>
        /// <returns>True if the image matches the category or contains ice cream-related tags; otherwise, false.</returns>
        [HttpGet]
        public string Get(string url,string? category)
        {
            var check = new ImaggaResult();
            var result = check.CheckImage(url);
            try
            {


                if (result != null&&result.status.type=="success")
                {
                    if (category != null)
                    {
                        foreach (var item in result.result.tags)
                        {
                            if (item.tag.en.Equals(category))
                            {
                                return "true";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in result.result.tags)
                        {
                            if (item.tag.en.Equals("ice")||item.tag.en.Equals("cream")||item.tag.en.Equals("ice cream")||item.tag.en.Equals("ice")||item.tag.en.Equals("yogurt"))
                            {
                                return "true";
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
           return "false";
            
        }
    }
}
