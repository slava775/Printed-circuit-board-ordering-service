namespace Testing.Domain.Entities;

public partial class PcbSpecification
{
    public int IdSpec { get; set; }

    public int? IdProduct { get; set; }

    public int? Layers { get; set; }

    public decimal? Width { get; set; }

    public decimal? Height { get; set; }

    public decimal? Thickness { get; set; }

    public decimal? CopperThickness { get; set; }

    public string? Material { get; set; }

    public string? SolderMaskColor { get; set; }

    public string? SurfaceFinish { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
