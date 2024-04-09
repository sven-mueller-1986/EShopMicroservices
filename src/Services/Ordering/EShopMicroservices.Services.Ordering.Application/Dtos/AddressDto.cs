namespace EShopMicroservices.Services.Ordering.Application.Dtos;

public record AddressDto
(
    string FirstName,
    string LastName,
    string Email,
    string Line,
    string Country,
    string State,
    string ZipCode
);
