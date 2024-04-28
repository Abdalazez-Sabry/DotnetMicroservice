using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DeterminEvent(message);
            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;

                default:
                    break;
            }
        }

        private EventType DeterminEvent(string notificationMessage)
        {
            Console.WriteLine("->> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            if (eventType is null)
            {
                Console.WriteLine("->> Couldn't Deserialize the event type");
                return EventType.Undetermined;
            }

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("->> Platform Published detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("->> Couldn't determin the event");
                    return EventType.Undetermined;

            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                if (repo is null)
                {
                    Console.WriteLine($"->> Couldn't add platform to DB, ICommandRepo is null ");
                    return;
                }

                if (platformPublishedDto is null)
                {
                    Console.WriteLine($"->> Couldn't add platform to DB, Couldn't deserialize the message to PlatformPublishedDto");
                    return;
                }

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalId))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        Console.WriteLine($"->> Created the platform successfully");
                    }
                    else
                    {
                        Console.WriteLine($"->> Platoform already exisits");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"->> Couldn't add platform to DB, {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}