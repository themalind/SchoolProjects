using Microsoft.AspNetCore.Mvc;
namespace Web.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult NotFoundPage()
    {
        Response.StatusCode = 404;
        return View("NotFound");
    }

    [Route("Error")]
    public IActionResult Error()
    {
        return View();
    }
}
