using DigitalWorldOnline.Commons.Extensions;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.ViewModel;

using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Serilog;
using System;
using System.Threading.Tasks;
using DigitalWorldOnline.Commons.ViewModel.Login;
using Microsoft.AspNetCore.WebUtilities;
using DigitalWorldOnline.Application.Separar.Queries;

namespace DigitalWorldOnline.Admin.Pages.Logins
{
    public partial class Login
    {
        private LoginViewModel _login = new();
        private bool _loading = false;
        private bool _disabledButton = false;

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public ISnackbar Toast { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public ISender Sender { get; set; }

        [Inject]
        public LoginModelRepository LoginRepository { get; set; }

        private void Disable()
        {
            _disabledButton = true;
        }

        private async Task DoLogin()
        {
            if (_login.Empty)
            {
                _disabledButton = false;
                return;
            }

            _loading = true;

            StateHasChanged();

            try
            {
                var loginResult = await Sender.Send(new CheckPortalAccessQuery(_login.Username, _login.Password.HashPassword()));

                if (loginResult == UserAccessLevelEnum.Unauthorized)
                {
                    Logger.Information("Log-in failed for username {username}", _login.Username);
                    Toast.Add("Incorrect password or username not found.", Severity.Warning);
                }
                else
                {
                    LoginRepository.Logins.Add(
                    new LoginViewModel()
                    {
                        Username = _login.Username,
                        AccessLevel = loginResult
                    });

                    Logger.Information("Log-in successfull for username {username}", _login.Username);

                    if (QueryHelpers.ParseQuery(Nav.ToAbsoluteUri(Nav.Uri).Query).TryGetValue("returnUrl", out var returnUrl))
                        Nav.NavigateTo($"/do-login?username={_login.Username}&returnUrl={returnUrl}", true);
                    else
                        Nav.NavigateTo($"/do-login?username={_login.Username}", true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error on log-in with username {username}. Ex.: {ex}, Stack.:{stack}", _login.Username, ex.Message, ex.StackTrace);
                Toast.Add("Unable to log-in, try again later.", Severity.Error);
            }
            finally
            {
                _loading = false;
                _disabledButton = false;

                StateHasChanged();
            }
        }
    }
}