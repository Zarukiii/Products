namespace Application.Dto.Product
{
    public record ProductDto
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}
