using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Threading.Tasks;
using System;

namespace DigitalWorldOnline.Admin.Pages.Downloads
{
    public partial class Downloads
    {
        [Inject]
        public NavigationManager Nav { get; set; }
        
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public ISnackbar Toast { get; set; }

        private async Task DownloadX64()
        {
            string zipFilePath = "Downloads/x64/DSO_Installer_x64.zip";

            byte[] zipFileBytes = System.IO.File.ReadAllBytes(zipFilePath);

            string fileName = "DSO_Installer_x64.zip";

            string mimeType = "application/zip";

            await DownloadFile(zipFileBytes, fileName, mimeType);

            Toast.Add("Thanks for downloading Digital Shinka Online!", Severity.Success);
        }
        
        private void DownloadX86()
        {
            Nav.NavigateTo("/downloads/x86/DSO_Installer_x86.zip");
        }

        private async Task DownloadFile(byte[] fileBytes, string fileName, string mimeType)
        {
            await Js.InvokeAsync<object>("downloadFile", fileName, mimeType, Convert.ToBase64String(fileBytes));
        }
    }
}