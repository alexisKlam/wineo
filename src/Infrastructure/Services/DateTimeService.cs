using wineo.Application.Common.Interfaces;

namespace wineo.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
