using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dto
{
    public class PlatformCreateDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Publisher { get; set; }

        [Required]
        public required string Cost { get; set; }
    }
}