using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;

namespace wineo.Application.Wines.Queries.SearchWine.Models;

public class WinesVM
{
    public IReadOnlyCollection<WineListItemDto> Lists { get; init; } = Array.Empty<WineListItemDto>();

}