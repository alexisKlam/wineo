using CsvHelper.Configuration;
using wineo.Domain.Entities;

namespace wineo.Infrastructure.Persistence.Seed;

public class WineMap : ClassMap<Wine>
{
    public WineMap()
    {
        Map(m => m.Name).Name("Name");
        Map(m => m.Description).Name("Description");
        Map(m => m.Country).Name("Country");
        Map(m => m.Region).Name("Region");
        Map(m => m.Year).Name("Year");
        Map(m => m.Type).Name("Type");
    }
}