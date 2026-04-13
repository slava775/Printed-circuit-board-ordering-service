namespace Testing.Domain.DTOs.Products;

public class ProductDTO
{
    public int IdProduct { get; set; }
    public int IdCategory { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public PCBSpecificationDTO? Specification { get; set; }
}