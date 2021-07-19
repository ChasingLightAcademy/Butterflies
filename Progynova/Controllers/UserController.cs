using System;
using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Progynova.Controllers.Base;
using Progynova.DbModels;
using Progynova.Models.Request;

namespace Progynova.Controllers
{
    [Route("User")]
    public class UserController : ApiControllerBase
    {
        private ProgynovaContext Db { get; }
        private IReCaptchaService ReCaptcha { get; }
        
        public UserController(ProgynovaContext context, IServiceProvider services)
        {
            Db = context;
            ReCaptcha = services.GetService<IReCaptchaService>();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserRegisterModel model)
        {
            throw new NotImplementedException();
        }
    }
}