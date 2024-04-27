using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Platform
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required]
        required public string Name { get; set; }

        [Required]
        public ICollection<Command> Commands { get; set; } = [];

    }
}