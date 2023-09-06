using wineo.Application.Common.Mappings;
using wineo.Application.Wines.Queries.SearchWine.Models;
using wineo.Domain.Entities;

namespace wineo.Application.Wines.Queries.GetWinePrices;

public class WineDto :  IMapFrom<Wine>
{
    public string Name { get; set; }
    public int Year { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }

    public string Description { get; set; }

    public int Id { get; set; }

    public WineTypeDto Type { get; set; }


    public IList<WineEvaluationDto> Evaluations { get; private set; } = new List<WineEvaluationDto>();

    public IList<WinePriceDto> Prices { get; private set; } = new List<WinePriceDto>();
    
}