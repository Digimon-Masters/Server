using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DigitalWorldOnline.Admin.Pages
{
    public partial class Index
    {
        [Inject]
        public ISender Sender { get; set; }

        private async Task Teste1()
        {
            // Cria um novo IHostBuilder
            //var hostBuilder = CreateHostBuilder(args);
            //
            //// Constrói o IHost
            //var host = hostBuilder.Build();
            //
            //// Adiciona um método de callback para o evento ApplicationStopping
            //host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.Register(OnApplicationStopping);
            //
            //// Inicia a execução assíncrona do IHost
            //await host.RunAsync();
            //
            //ProcessStartInfo startInfo = new ProcessStartInfo
            //{
            //    FileName = "D:\\Projetos\\DWO\\dso-project\\src\\Source\\Distribution\\DigitalWorldOnline.Account.Host\\bin\\Debug\\net6.0\\DigitalWorldOnline.Account.exe",
            //    RedirectStandardOutput = false,
            //    UseShellExecute = false,
            //    CreateNoWindow = true
            //};
            //
            //Process process = new Process
            //{
            //    StartInfo = startInfo
            //};
            //
            //process.EnableRaisingEvents = true;
            //
            //process.Exited += ProcessExited;
            //
            //process.Start();
            //
            //_accountProcessId = process.Id;
        }

        private static void ProcessExited(object sender, EventArgs e)
        {
            Console.WriteLine("Processo encerrado.");
        }

        private void Teste2()
        {
            //var accountProcess = Process.GetProcessById(_accountProcessId);
            //
            //accountProcess?.Kill();
        }
    }
}