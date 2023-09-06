using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Wines.Commands.CreateWine;
using Microsoft.Extensions.DependencyInjection.Wines.Queries.GetWine;
using wineo.Application.Common.Models;
using wineo.Application.Wines.Queries.GetWinePrices;
using wineo.Application.Wines.Queries.SearchWine;
using wineo.Application.Wines.Queries.SearchWine.Models;

namespace wineo.WebUI.Controllers;

[Authorize]
public class WinesController : ApiControllerBase
{
    [HttpPost("search")]
    public async Task<ActionResult<WinesVM>> Search(SearchWineQuery command)
    {
        return await Mediator.Send(command);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create(CreateWineCommand command)
    {
        return await Mediator.Send(command);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<WineDto?>> Get(int id)
    {
        return await Mediator.Send(new GetWineQuery{ WineId= id });

    }

    [HttpGet("prices")]
    public async Task<ActionResult<PaginatedList<WinePriceDto>>> GetTodoItemsWithPagination([FromQuery] GetWinePricesQuery query)
    {
        return await Mediator.Send(query);
    }
}