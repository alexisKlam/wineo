using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.WineEvaluations.Commands.CreateWineEvaluation;

namespace wineo.WebUI.Controllers;

[Authorize]
public class WineEvaluationController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateWineEvaluationCommand command)
    {
        return await Mediator.Send(command);
    }
}