using BlazorSessionScopedContainer.Contracts.Sessions.Auth;
using BlazorSessionScopedContainer.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSessionScopedContainer.RazorComponents.View.Auth
{
	public class NAuthenticationView<UserType> : ComponentBase where UserType : class, IUser
	{
		private INAuthenticationService<UserType>? _authService;
		private bool _authorized, _predicated;

		[Inject] internal NSession Session { get; private set; }

		[Parameter] public RenderFragment<INAuthenticationService<UserType>>? Authorized { get; set; }
		[Parameter] public RenderFragment<INAuthenticationService<UserType>>? NotAuthorized { get; set; }
		[Parameter] public RenderFragment<INAuthenticationService<UserType>>? PredicateSatisfied { get; set; }
		[Parameter] public RenderFragment<INAuthenticationService<UserType>>? PredicateNotSatisfied { get; set; }

		[Parameter] public string? Roles { get; set; }
		[Parameter] public Predicate<ICredential> Predicate { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{			
			if(!_authorized)
			{
				builder.AddContent(0, NotAuthorized?.Invoke(_authService!));
				return;
			}

			builder.AddContent(0, Authorized?.Invoke(_authService!));

			if (_predicated)
			{
				builder.AddContent(0, PredicateSatisfied?.Invoke(_authService!));
			}
			else
			{
				builder.AddContent(0, PredicateNotSatisfied?.Invoke(_authService!));
			}

		}

		protected override void OnParametersSet()
		{
			_authService = Session.GetGlobalService<INAuthenticationService<UserType>>();
			if (_authService == null)
			{
				_authorized = false;
				return;
			}

			var currentUser = _authService.CurrentUser(Session.GetSession());

			_authorized = currentUser != null && Roles.Replace(" ", "").Split(',').ToList().Contains(currentUser.Role);
			if (Predicate != null)
				_predicated = Predicate(currentUser.UserCredential);
		}

	}
}
