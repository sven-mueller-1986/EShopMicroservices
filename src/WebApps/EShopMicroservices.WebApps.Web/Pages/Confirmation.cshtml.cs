namespace EShopMicroservices.WebApps.Web.Pages;

public class ConfirmationModel(ILogger<ConfirmationModel> logger)
    : PageModel
{
    public string Message { get; set; } = default!;

    public void OnGetContact()
    {
        logger.LogInformation("Confirmation page visited.");

        Message = "Your email was sent.";
    }

    public void OnGetOrderSubmitted()
    {
        logger.LogInformation("Confirmation page visited.");

        Message = "Your order submitted successfully.";
    }
}
