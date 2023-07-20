﻿using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;

namespace BlazorSessionScopedContainer.RazorComponents
{
    public abstract class NSessionComponentBase : ComponentBase
    {
        [Inject]
        public NSession Session { get; set; }
    }
}
