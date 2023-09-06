using FluentValidation;

namespace Microsoft.Extensions.DependencyInjection.WineEvaluations.Commands.CreateWineEvaluation;

public class CreateWineEvaluationCommandValidator : AbstractValidator<CreateWineEvaluationCommand>
{
    public CreateWineEvaluationCommandValidator()
    {
        RuleFor(e => e.Evaluation).NotEmpty().MaximumLength(500);
        RuleFor(e => e.Taste).InclusiveBetween(0, 5);
        RuleFor(e => e.Appearance).InclusiveBetween(0, 5);
        RuleFor(e => e.Aroma).InclusiveBetween(0, 5);
        RuleFor(e => e.Aroma).InclusiveBetween(0, 5);
    }
}