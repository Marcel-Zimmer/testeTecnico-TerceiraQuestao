namespace questaoTres.Dtos
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string CategoryName { get; set; }
        public decimal Price { get; set; }
    }
}