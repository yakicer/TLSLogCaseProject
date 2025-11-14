using Microsoft.AspNetCore.Components;
using Radzen;

namespace Web.Blazor.Pages.Components
{
    public partial class BaseComponent : ComponentBase
    {
        [Inject]
        public DialogService Dialog { get; set; }
        [Inject]
        IConfiguration Configuration { get; set; }

    }
}
