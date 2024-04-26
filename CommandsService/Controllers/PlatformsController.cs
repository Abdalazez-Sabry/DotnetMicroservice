using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : Controller
    {
        public PlatformsController()
        {

        }


        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("->> Inbound POST @ Coomands Service");
            return Ok("Inbound test from Platforms Controller in Commands Service");
        }

    }
}