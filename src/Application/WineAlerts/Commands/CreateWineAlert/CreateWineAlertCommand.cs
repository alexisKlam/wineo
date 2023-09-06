using System.Diagnostics;
using MediatR;
using wineo.Application.Common.Interfaces;
using wineo.Domain.Entities;

namespace Microsoft.Extensions.DependencyInjection.WineAlerts.Commands.CreateWineAlert;

public record CreateWineAlertCommand : IRequest<int>
{
    public int? Year { get; set; }

    public string? Country { get; set; }

    public WineType? WineType { get; set; }
}

public class CreateWineAlertCommandHandler : IRequestHandler<CreateWineAlertCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWineAlertCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateWineAlertCommand request, CancellationToken cancellationToken)
    {
        Debug.Assert(_currentUserService.UserId != null, "_currentUserService.UserId != null");
        var entity = new WineAlert()
        {
          
            Year = request.Year,
            Country = request.Country,
            WineType = request.WineType,
            UserId = _currentUserService.UserId
        };

        _context.WineAlerts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}