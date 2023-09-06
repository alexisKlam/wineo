using wineo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace wineo.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Wine> Wines { get; }
    DbSet<WinePrice> WinePrices { get; }
    DbSet<WineEvaluation> WineEvaluation { get; }
    DbSet<WineAlert> WineAlerts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
