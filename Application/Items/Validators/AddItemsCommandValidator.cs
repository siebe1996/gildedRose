using Application.Items.Commands;
using FluentValidation;

namespace Application.Items.Validators;

internal class AddItemsCommandValidator : AbstractValidator<AddItemsCommand>
{
    public AddItemsCommandValidator()
    {
        RuleFor(command => command.Items)
            .NotNull()
            .NotEmpty();

        RuleForEach(command => command.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.Name)
                    .NotEmpty()
                    .WithMessage("Name cannot be only whitespace.");

                item.RuleFor(i => i.Quality)
                    .GreaterThanOrEqualTo(0);
            });
    }
}