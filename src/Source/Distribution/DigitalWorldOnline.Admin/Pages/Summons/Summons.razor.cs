using AutoMapper;
using DigitalWorldOnline.Admin.Shared;
using DigitalWorldOnline.Commons.Enums.Admin;
using DigitalWorldOnline.Commons.ViewModel.Summons;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalWorldOnline.Admin.Pages.Summons
{
    public partial class Summons
    {
        [Inject]
        public ISender Sender { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [Inject]
        public ISnackbar Toast { get; set; }

        [Inject]
        public NavigationManager Nav { get; set; }

        [Inject]
        IDialogService DialogService { get; set; }

        private MudTextField<string> _filterParameter;
        private MudTable<SummonViewModel> _table;

        private void Return()
        {
            Nav.NavigateTo("/");
        }

        private async Task<TableData<SummonViewModel>> ServerReload(TableState state)
        {
            //var summons = await Sender.Send(
            //    new GetSummonsQuery(
            //        state.Page,
            //        state.PageSize,
            //        state.SortLabel,
            //        (SortDirectionEnum)state.SortDirection.GetHashCode(),
            //        _filterParameter?.Value
            //    )
            //);

            //var pageData = Mapper.Map<IEnumerable<SummonViewModel>>(summons.Registers);

            return new TableData<SummonViewModel>(); // { TotalItems = summons.TotalRegisters, Items = pageData };
        }

        private void Create()
        {
            Nav.NavigateTo($"/summons/create");
        }

        private async Task Duplicate(long id)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Do you want to create a copy of the selected summon?" },
                { "Color", Color.Primary }
            };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = DialogService.Show<ConfirmDialog>("Duplicate ", parameters, options);

            var result = await dialog.Result;

            if (!result.Cancelled)
                Nav.NavigateTo($"/summons/duplicate/{id}");
            else
                await Refresh();
        }

        private void Update(long id)
        {
            Nav.NavigateTo($"/summons/update/{id}");
        }

        private async Task Delete(long id)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Chrysalimon gonna use Data Crusher and wipe this data. Do you want to proceed? This process cannot be undone." }
            };

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var dialog = DialogService.Show<ConfirmDialog>("Delete", parameters, options);

            var result = await dialog.Result;

            if (!result.Cancelled)
                Nav.NavigateTo($"/summons/delete/{id}");
            else
                await Refresh();
        }

        private async Task Filter()
        {
            await Refresh();
        }

        private async Task Clear()
        {
            _filterParameter.Clear();

            await Refresh();
        }

        private async Task Refresh()
        {
            await _table.ReloadServerData();
            await new Task(() => { _table.Loading = false; });
        }
    }
}