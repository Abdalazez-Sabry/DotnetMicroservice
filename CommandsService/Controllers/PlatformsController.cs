using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : Controller
    {
        readonly private ICommandRepo _repository;
        readonly private IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("->> Inbound POST @ Coomands Service");
            return Ok("Inbound test from Platforms Controller in Commands Service");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
        {
            Console.WriteLine("->> Getting all platforms");

            var platforms = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms).ToList());
        }
    }
}