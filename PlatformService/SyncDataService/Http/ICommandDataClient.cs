using PlatformService.Dto;

namespace PlatformService.SyncDataService
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}