using wineo.Application.Common.Mappings;
using wineo.Domain.Entities;

namespace wineo.Application.Wines.Queries.SearchWine.Models;

public class WineEvaluationDto : IMapFrom<WineEvaluation>
{
    public double Appearance { get; set; } // Criteria 1: Appearance
    public double Aroma { get; set; }     // Criteria 2: Aroma
    public double Taste { get; set; }     // Criteria 3: Taste


    public string Evaluation { get; set; }

    public string AuthorId { get; set; }
}