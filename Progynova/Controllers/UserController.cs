using System;
using Microsoft.AspNetCore.Mvc;
using Progynova.Controllers.Base;
using Progynova.Models.Request;

namespace Progynova.Controllers
{
    public class UserController : ApiControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserAuthModel model)
        {
            throw new NotImplementedException();
        }
    }
}