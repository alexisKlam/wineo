using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using wineo.Application.Common.Interfaces;
using wineo.Application.Wines.Queries.SearchWine.Models;
using wineo.Domain.Entities;
using wineo.Domain.Enums;

namespace wineo.Application.Wines.Queries.SearchWine;

public record SearchWineQuery : IRequest<WinesVM>
{
    public ISet<int>? Ids { get; set; }
    public ISet<int>? Years { get; set; }
    public ISet<string>? Countries { get; set; }
    public ISet<string>? Regions { get; set; }
    public WineTypeDto? Type { get; set; }

    public decimal? minPrice { get; set; }
    public decimal? maxPrice { get; set; }
}


public class SearchWineCommandHandler : IRequestHandler<SearchWineQuery, WinesVM>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchWineCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WinesVM> Handle(SearchWineQuery request, CancellationToken cancellationToken)
    {
       
        WineType wineType = (WineType)(request.Type.HasValue ? (int)request.Type.Value : 0);
        return new WinesVM
        {

            Lists = _mapper.Map<IReadOnlyCollection<WineListItemDto>>((await _context.Wines
                .AsNoTracking()
                .Include(w => w.Prices)
                .Include(w => w.Evaluations)
                .WhereIf(request.Ids?.Count > 0, wine => request.Ids!.Contains(wine.Id))
                .WhereIf(request.Countries?.Count > 0, wine => request.Countries!.Contains(wine.Country))
                .WhereIf(request.Regions?.Count > 0, wine => request.Regions!.Contains(wine.Region))
                .WhereIf(request.Years?.Count > 0, wine => request.Years!.Contains(wine.Year))
                .WhereIf(request.Type.HasValue, wine => wine.Type == wineType)
                .ToListAsync(cancellationToken)))
                .WhereIf(request.minPrice.HasValue, wine => wine.CurrentPrice.Price>= request.minPrice.Value)
                .WhereIf(request.maxPrice.HasValue, wine => wine.CurrentPrice.Price<= request.maxPrice.Value)
                .OrderBy(wine => wine.Score)
                .ToList()
        };
    }
}

public class SearchWineCommandValidator : AbstractValidator<SearchWineQuery>
{
    public SearchWineCommandValidator()
    {
        RuleFor(s => s).Custom((s, context) =>
        {
            if (s.minPrice.HasValue && s.maxPrice.HasValue && s.minPrice.Value > s.maxPrice.Value)
            {
                context.AddFailure("minPrice", "minPrice should be less than maxPrice");
            }
        });
    }
}
