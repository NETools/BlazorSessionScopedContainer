using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;
namespace BlazorSessionScopedContainer.RazorComponents
{
    public class NSessionMainBase : LayoutComponentBase
    {
        [Inject]
        public NSession Session { get; set; }
    }
}
