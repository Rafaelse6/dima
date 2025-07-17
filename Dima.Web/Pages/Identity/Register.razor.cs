using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity
{
    public partial class RegisterPage : ComponentBase
    {

        public MudForm MudForm
        {
            get; set;
        } = new MudForm();
    }
}
