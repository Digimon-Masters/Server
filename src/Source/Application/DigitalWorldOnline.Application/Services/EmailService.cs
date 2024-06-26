using DigitalWorldOnline.Commons.Interfaces;
using System.Net.Mail;
using System.Net;

namespace DigitalWorldOnline.Application.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() { }

        public void Send(string destination)
        {
            // Configurações do servidor de email
            string smtpServer = "smtp.gmail.com"; // Substitua pelo servidor SMTP real
            int smtpPort = 587; // Porta SMTP padrão 587 /465 for SSL
            string smtpUsername = "officialdigitalshinkaonline@gmail.com"; // Seu endereço de email
            string smtpPassword = "i6PNH63gSYKk5dq"; // Sua senha de email

            // Criando a mensagem de email
            MailMessage message = new MailMessage();
            message.From = new MailAddress(smtpUsername);
            message.To.Add("marioneibfj@hotmail.com"); // Endereço do destinatário
            message.Subject = "Digital Shinka Online - Account created.";
            message.Body = "Account created! Set your password here: http://digitalshinkaonline.com:2052/.";

            // Configurando o cliente SMTP
            SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.EnableSsl = true;

            try
            {
                // Enviando o email
                smtpClient.Send(message);
                Console.WriteLine("Email enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao enviar o email: " + ex.Message);
            }
        }
    }
}
