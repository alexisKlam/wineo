namespace wineo.Domain.Entities;

public class WinePrice : BaseAuditableEntity
{
    public decimal Price { get; set; }
    public DateTime Date { get; set; }

    public string CommercialLink { get; set; }

    public int WineId { get; set; }

    public Wine Wine { get; set; }

}