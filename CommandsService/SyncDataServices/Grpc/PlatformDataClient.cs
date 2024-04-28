using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            string? grpcLink = _configuration["GrpcPlatform"];

            if (grpcLink is null)
            {
                Console.WriteLine($"->> couldn't find GRPC platform from the configuration");
                return [];
            }

            Console.WriteLine($"->> Calling GRPC Service {grpcLink}");

            var channel = GrpcChannel.ForAddress(grpcLink);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"->> Couldn't call GRPC server {ex.Message}");
                return [];
            }
        }
    }
}