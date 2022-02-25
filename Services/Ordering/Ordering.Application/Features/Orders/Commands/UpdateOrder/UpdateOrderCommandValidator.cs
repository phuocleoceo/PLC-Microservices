using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("{UserName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} has maximum length 50");

        RuleFor(c => c.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required");

        RuleFor(c => c.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThan(0).WithMessage("{TotalPrice} must greater than 0");
    }
}
