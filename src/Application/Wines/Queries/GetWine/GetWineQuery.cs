using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using wineo.Application.Common.Interfaces;
using wineo.Application.Wines.Queries.GetWinePrices;

namespace Microsoft.Extensions.DependencyInjection.Wines.Queries.GetWine;

public class GetWineQuery: IRequest<WineDto?>
{
    public int WineId { get; set; }
}

public class GetWineQueryHandler : IRequestHandler<GetWineQuery, WineDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWineQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WineDto?> Handle(GetWineQuery request, CancellationToken cancellationToken)
    {
        return await _context.Wines
            .AsNoTracking()
            .Include(w => w.Prices)
            .Include(w => w.Evaluations)
            .ProjectTo<WineDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(w => w.Id == request.WineId, cancellationToken);

    }
}