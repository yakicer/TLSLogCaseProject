using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents._Layout
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("Default");
        }
    }
}
