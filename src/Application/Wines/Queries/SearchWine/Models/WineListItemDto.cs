using System.Diagnostics;
using System.Text.Json.Serialization;
using wineo.Application.Common.Mappings;
using wineo.Domain.Entities;

namespace wineo.Application.Wines.Queries.SearchWine.Models;

public class WineListItemDto : IMapFrom<Wine>
{
    public string Name { get; set; }
    public int Year { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }

    public string Description { get; set; }

    public int Id { get; set; }

    public WineTypeDto Type { get; set; }


    public double? Score { get; set; }

    public double? Aroma { get; set; }

    public double? Taste { get; set; }

    public double? Appearance { get; set; }

    public decimal? Price => CurrentPrice?.Price;
    public DateTime? PriceDate => CurrentPrice?.Date;

    public string CommercialLink => CurrentPrice?.CommercialLink;
    [JsonIgnore]
    public WinePriceDto CurrentPrice{get; set; }

}