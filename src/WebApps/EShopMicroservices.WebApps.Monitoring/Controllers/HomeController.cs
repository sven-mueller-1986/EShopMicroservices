using EShopMicroservices.WebApps.Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EShopMicroservices.WebApps.Monitoring.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("/healthchecks-ui");
        }
    }
}
