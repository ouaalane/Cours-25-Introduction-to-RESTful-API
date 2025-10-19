using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyFirstApiController : ControllerBase
    {



        [HttpGet("PrintName")]
        public string PrintName(string MyName)
        {
            return MyName;
        }


        [HttpGet("PrintFullName",Name ="heleoo")]
        public string PrintFullName(string FirstName , string LastName)
        {
            return FirstName  + " " + LastName; 
        }
    }
}
