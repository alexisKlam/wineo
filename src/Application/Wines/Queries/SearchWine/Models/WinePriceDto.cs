using wineo.Application.Common.Mappings;
using wineo.Domain.Entities;

namespace wineo.Application.Wines.Queries.SearchWine.Models;

public class WinePriceDto : IMapFrom<WinePrice>
{
    public decimal Price { get; set; }
    public DateTime Date { get; set; }

    public string CommercialLink { get; set; }
}