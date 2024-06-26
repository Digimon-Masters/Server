using DigitalWorldOnline.Application.Admin.Commands;
using DigitalWorldOnline.Application.Admin.Queries;
using DigitalWorldOnline.Commons.ViewModel.Account;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Serilog;
using System;
using System.Threading.Tasks;

namespace DigitalWorldOnline.Admin.Pages.Accounts
{
    public partial class AccountUpdate
    {
        AccountUpdateViewModel _account = new();
        bool _loading = true;
        long _id;

        [Parameter]
        public string AccountId { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        public ISender Sender { get; set; }

        [Inject]
        public ISnackbar Toast { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (long.TryParse(AccountId, out _id))
            {
                Logger.Information("Searching account by id {id}", _id);

                var target = await Sender.Send(
                    new GetAccountByIdQuery(_id)
                );

                _account = target.Register != null ?
                    new AccountUpdateViewModel(
                        target.Register.Id,
                        target.Register.Username,
                        target.Register.Email,
                        target.Register.AccessLevel,
                        target.Register.Premium,
                        target.Register.Silk)
                    : null;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_id == 0 || _account == null)
            {
                Logger.Information("Invalid account id parameter: {parameter}", AccountId);
                Toast.Add("Account not found, try again later.", Severity.Warning);

                Return();
            }

            if (firstRender)
            {
                _loading = false;
                StateHasChanged();
            }
        }

        private async Task Update()
        {
            if (_account.Empty)
                return;

            try
            {
                _loading = true;

                StateHasChanged();

                Logger.Information("Updating account {id}", _account.Id);

                await Sender.Send(
                    new UpdateAccountCommand(
                        _account.Id,
                        _account.Username,
                        _account.Email,
                        _account.AccessLevel,
                        _account.Premium,
                        _account.Silk)
                );

                Logger.Information("Account {id} updated.", _account.Id);

                Toast.Add("Account updated.", Severity.Success);

                Nav.NavigateTo("/accounts");
            }
            catch (Exception ex)
            {
                Logger.Error("Error updating account id {id}: {ex}", _account.Id, ex.Message);
                Toast.Add("Unable to update account, try again later.", Severity.Error);
            }
            finally
            {
                _loading = false;

                StateHasChanged();
            }
        }

        private void Return()
        {
            Nav.NavigateTo("/accounts");
        }
    }
}