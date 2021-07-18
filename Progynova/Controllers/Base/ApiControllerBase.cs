using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Progynova.Controllers.Base
{
    [ApiController]
    [Route("{controller=Index}/{action=Index}/{id?}")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ApiControllerBase : ControllerBase
    {
    }
}