using Microsoft.AspNetCore.Mvc;

namespace Web.ViewComponents._Layout
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var data = "sdasdasd";
            return View("Default", data);
        }
    }
}
