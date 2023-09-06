using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.DependencyInjection.WineEvaluations.Commands.CreateWineEvaluation;
using wineo.Application.Common.Interfaces;
using wineo.Domain.Entities;
using wineo.Domain.Events;

namespace Microsoft.Extensions.DependencyInjection.Wines.Commands.CreateWine;

public record CreateWineCommand : IRequest<int>
{
    public string Name { get; set; }
    public int Year { get; set; }
    public string Region { get; set; }
    public string Country { get; set; }

    public string Description { get; set; }

    public WineType Type { get; set; }
}

public class CreateWineCommandHandler : IRequestHandler<CreateWineCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWineCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateWineCommand request, CancellationToken cancellationToken)
    {
        Debug.Assert(_currentUserService.UserId != null, "_currentUserService.UserId != null");
        var entity = new Wine()
        {
            Name = request.Name,
            Year = request.Year,
            Region = request.Region,
            Country = request.Country,
            Description = request.Description,
            Type = request.Type,
        };

        _context.Wines.Add(entity);

        entity.AddDomainEvent(new WineCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}