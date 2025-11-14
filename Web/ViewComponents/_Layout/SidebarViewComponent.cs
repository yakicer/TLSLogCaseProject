using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents._Layout
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            return View("Default");
        }
    }
}
