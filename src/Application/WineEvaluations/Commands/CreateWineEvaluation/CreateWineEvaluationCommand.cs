using System.Diagnostics;
using MediatR;
using wineo.Application.Common.Interfaces;
using wineo.Domain.Entities;

namespace Microsoft.Extensions.DependencyInjection.WineEvaluations.Commands.CreateWineEvaluation;

public record CreateWineEvaluationCommand : IRequest<int>
{
    public double Appearance { get; set; } // Criteria 1: Appearance
    public double Aroma { get; set; }     // Criteria 2: Aroma
    public double Taste { get; set; }     // Criteria 3: Taste

    public string Evaluation { get; set; }

    public int WineId { get; set; }
}

public class CreateWineEvaluationCommandHandler : IRequestHandler<CreateWineEvaluationCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWineEvaluationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateWineEvaluationCommand request, CancellationToken cancellationToken)
    {
        Debug.Assert(_currentUserService.UserId != null, "_currentUserService.UserId != null");
        var entity = new WineEvaluation
        {
            Appearance = request.Appearance,
            Aroma = request.Aroma,
            Taste = request.Taste,
            Evaluation = request.Evaluation,
            WineId = request.WineId,
            AuthorId = _currentUserService.UserId    
        };

        _context.WineEvaluation.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}