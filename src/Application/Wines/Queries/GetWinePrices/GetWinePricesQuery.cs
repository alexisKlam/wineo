using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using wineo.Application.Common.Interfaces;
using wineo.Application.Common.Mappings;
using wineo.Application.Common.Models;
using wineo.Application.Wines.Queries.SearchWine.Models;

namespace wineo.Application.Wines.Queries.GetWinePrices;

public class GetWinePricesQuery : IRequest<PaginatedList<WinePriceDto>>
{
    public int WineId { get; set; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


public class GetWinePricesQueryHandler : IRequestHandler<GetWinePricesQuery, PaginatedList<WinePriceDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetWinePricesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<WinePriceDto>> Handle(GetWinePricesQuery request, CancellationToken cancellationToken)
    {
        return await _context.WinePrices
            .Where(x => x.WineId == request.WineId)
            .OrderBy(x => x.Date)
            .ProjectTo<WinePriceDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
