using System.ComponentModel.DataAnnotations.Schema;

namespace wineo.Domain.Entities;

public class Wine : BaseAuditableEntity
{
    public string Name { get; set; }
    public int Year { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }

    public string Description { get; set; }

    public WineType Type { get; set; }

    public IList<WineEvaluation> Evaluations { get; private set; } = new List<WineEvaluation>();

    //Prices
    public IList<WinePrice> Prices { get; private set; } = new List<WinePrice>();

}