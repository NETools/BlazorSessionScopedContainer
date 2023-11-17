using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;

namespace BlazorSessionScopedContainer.RazorComponents
{
    public abstract class NSessionComponentBase : ComponentBase
    {
        [Inject]
        public NSession Session { get; set; }
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
            await base.OnAfterRenderAsync(firstRender);
			if (firstRender)
            {
                await OnServicesLoading();
                StateHasChanged();
            }
		}

        protected abstract Task OnServicesLoading();
    }
}
