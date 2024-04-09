namespace EShopMicroservices.Services.Ordering.Application.Orders.Commands.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId)
    : IQuery<GetOrdersByCustomerResult>;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomerValidator : AbstractValidator<GetOrdersByCustomerQuery>
{
    public GetOrdersByCustomerValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    }
}
