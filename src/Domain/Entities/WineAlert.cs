namespace wineo.Domain.Entities;

public class WineAlert : BaseAuditableEntity
{
    public int? Year{ get; set; }

    public string? Country{ get; set; }

    public WineType? WineType { get; set; }

    public string UserId { get; set; }
}