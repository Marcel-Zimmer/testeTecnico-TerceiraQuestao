using System.ComponentModel.DataAnnotations;

namespace questaoTres.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
    }
}