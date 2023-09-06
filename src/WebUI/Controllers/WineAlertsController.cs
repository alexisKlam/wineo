using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.WineAlerts.Commands.CreateWineAlert;

namespace wineo.WebUI.Controllers;

[Authorize]
public class WineAlertsController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateWineAlertCommand command)
    {
        return await Mediator.Send(command);
    }
}