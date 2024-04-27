using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos
{
    public class CommandCraeteDto
    {
        [Required]
        required public string HowTo { get; set; }

        [Required]
        required public string CommandLine { get; set; }
    }
}