using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;

namespace BlazorSessionScopedContainer.RazorComponents
{
    public abstract class NSessionMainBase : LayoutComponentBase
    {
        [Inject]
        public NSession Session { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
