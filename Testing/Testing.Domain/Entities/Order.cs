namespace Testing.Domain.Entities;

public partial class Order
{
    public int IdOrder { get; set; }

    public int? IdUser { get; set; }

    public int? IdProduct { get; set; }

    public string? OrderNumber { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? IdProductNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
