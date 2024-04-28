using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : Controller
    {
        readonly private ICommandRepo _repository;
        readonly private IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands(int platformId)
        {
            Console.WriteLine($"->> Getting All Commands in PlatformId {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));

        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCraeteDto inputCommand)
        {
            Console.WriteLine($"->> Createing a command in PlatformId {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            Command createdCommand = _mapper.Map<Command>(inputCommand);

            _repository.CreateCommand(platformId, createdCommand);
            _repository.SaveChanges();

            var readCommand = _mapper.Map<CommandReadDto>(createdCommand);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new
            {
                platformId,
                commandId = readCommand.Id
            }, readCommand);



        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"->> Getting a command with id {commandId} in PlatformId {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            Command? command = _repository.GetCommand(platformId, commandId);

            if (command is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

    }
}