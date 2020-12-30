using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JWTtest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NameController : Controller
    {
        private readonly IJwtAuthManager _jwtAuthManager;

        public NameController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[]
            {
                "val1",
                "val2"
            };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var token = _jwtAuthManager.Authenticate(userCred.Username, userCred.Password);
            if(token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
