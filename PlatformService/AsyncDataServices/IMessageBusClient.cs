using PlatformService.Dto;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        public void PublishNewPlatform(PlatformPublishedDto plat);
    }
}
