using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection.WineAlerts.Commands.CreateWineAlert;

public class CreateWineAlertCommandValidator : AbstractValidator<CreateWineAlertCommand>
{
    public CreateWineAlertCommandValidator()
    {
        // Check country is not empty
        RuleFor(e => e.Country).NotEmpty().When(e => e.Country != null);

        // Add a rule to check year is positive
        RuleFor(e => e.Year).GreaterThan(0).When(e => e.Year != null);

        // Check Wine type is a valid value 
        RuleFor(e => e.WineType).IsInEnum().When(e => e.WineType != null);

        // Add a rule to check at least one parameter is set
        RuleFor(e => e)
            .Must(e => e.Year != null || e.Country != null || e.WineType != null)
            .WithMessage("At least one parameter must be set");
    }
}