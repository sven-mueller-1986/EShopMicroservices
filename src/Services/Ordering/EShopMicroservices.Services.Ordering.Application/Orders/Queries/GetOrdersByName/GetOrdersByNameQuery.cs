namespace EShopMicroservices.Services.Ordering.Application.Orders.Commands.GetOrdersByName;

public record GetOrdersByNameQuery(string Name)
    : IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

public class GetOrdersByNameValidator : AbstractValidator<GetOrdersByNameQuery>
{
    public GetOrdersByNameValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}
