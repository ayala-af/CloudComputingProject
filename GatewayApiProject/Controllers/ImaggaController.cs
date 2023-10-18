using Microsoft.AspNetCore.Mvc;
using GatewayApiProject.Models;
using Newtonsoft.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GatewayApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImaggaController : ControllerBase
    {
        // GET: api/<ImaggaController>
        [HttpGet]
        public bool Get(string url,string typeOfImage,double threshold)
        {
            
            var check = new ImaggaResponse();
            var result = check.CheckImage("https://cdn.britannica.com/50/80550-050-5D392AC7/Scoops-kinds-ice-cream.jpg");
           if (result != null&&result.status.type=="success" )
            {
                foreach(var item in result.result.tags)
                {
                    if (item.tag.en.Equals(typeOfImage)&&item.confidence>=threshold)
                    {
                       return true;
                    }
                }
            }
           return false;
            
        }

        // GET api/<ImaggaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ImaggaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ImaggaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ImaggaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
