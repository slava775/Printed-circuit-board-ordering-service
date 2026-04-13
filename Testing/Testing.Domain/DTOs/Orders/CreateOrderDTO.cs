using System.ComponentModel.DataAnnotations;

namespace Testing.Domain.DTOs.Orders;

public class CreateOrderDTO
{
    [Required]
    public int IdProduct { get; set; }

    [Required]
    [Range(1, 1000)]
    public int Quantity { get; set; }

    public string? Comment { get; set; }
}