namespace EShopMicroservices.WebApps.Web.Models.Ordering;

public record AddressModel
(
    string FirstName,
    string LastName,
    string Email,
    string Line,
    string Country,
    string State,
    string ZipCode
);
