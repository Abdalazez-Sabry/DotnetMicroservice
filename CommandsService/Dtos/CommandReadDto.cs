namespace CommandsService.Dtos
{
    public class CommandReadDto
    {
        public int Id { get; set; }

        public int PlatformId { get; set; }

        required public string HowTo { get; set; }

        required public string CommandLine { get; set; }
    }
}