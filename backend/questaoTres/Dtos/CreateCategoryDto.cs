using System.ComponentModel.DataAnnotations;

namespace questaoTres.Dtos
{
    public class CreateCategoryDto
    {
        [Required]
        public required string Name { get; set; }
    }
}