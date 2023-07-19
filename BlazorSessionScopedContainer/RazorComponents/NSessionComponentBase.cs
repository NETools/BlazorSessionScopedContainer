using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;

namespace BlazorSessionScopedContainer.RazorComponents
{
    public class NSessionComponentBase : ComponentBase
    {
        [Inject]
        public NSession Session { get; set; }
    }
}
