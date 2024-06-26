using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Discord.WebSocket;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace Template.Modules
{
    public enum StatusCode : int
    {
        Success = 0,
        EmailInUse = 1,
        UsernameInUse = 2,
        DiscordIdInUse = 3,

    }

    public enum InvalidCode : int
    {
        Password = 0,
        Username = 1,
        InvalidCharacters = 2,
        Email = 3,
        Success = 4,
    }
    public class InteractiveCommands : InteractiveBase
    {

        public Account AccountInfo = new();
        public string UrlImage = "https://media.discordapp.net/attachments/1144762545777954847/1144783776078119022/Digital_Shinka_Online.png?width=473&height=473";

        List<IMessage> _channel = new List<IMessage>();
        List<DateTime> ChannelsTime = new List<DateTime>();

        [Command("creation", RunMode = RunMode.Async)]
        public async Task CreateAccountAsync()
        {
            try
            {
                var UserDM = Context.Channel is IPrivateChannel;

                if (!UserDM)
                    return;

                var ServerUser = Context.User.MutualGuilds.First();
                var User = ServerUser.Users.FirstOrDefault(x => x.Id == Context.User.Id) as SocketGuildUser;

                if (User == null)
                    return;



                var Roles = User.Roles;

                if (Roles != null)
                {
                    if (Roles.FirstOrDefault(x => x.Id == 951185528089362502) != null)
                    {
                        if (_channel.Count > 0)
                        {
                            DeleteMessagesListAsync();
                        }

                        var time = DateTime.Now;
                        ChannelsTime.Add(time);
                        _channel.Add(await Context.Channel.SendMessageAsync("```Please enter your username!```"));

                        var UserName = await NextMessageAsync(); // Supondo que isso retorne um valor do tipo que você espera
                        var UserNameExpiredTime = DateTime.Now;

                        while (UserName == null)
                        {
                            if (DateTime.Now >= UserNameExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }

                            UserName = await NextMessageAsync(); // Supondo que isso retorne um valor do tipo que você espera
                        }

                        InvalidCode isValidUsername = ValidateUsername(UserName.ToString()); // Verifica se não há espaços no nome de usuário
                        UserNameExpiredTime = DateTime.Now;

                        while (isValidUsername != InvalidCode.Success) // Se a String não for valida While irá repetir todo processo de digitação de string
                        {
                            if (DateTime.Now >= UserNameExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }

                            time = DateTime.Now;
                            ChannelsTime.Add(time);
                            if (isValidUsername == InvalidCode.Username)
                            {

                                _channel.Add(await Context.Channel.SendMessageAsync("```Invalid username (6-12 characters).```"));
                            }
                            else if (isValidUsername == InvalidCode.InvalidCharacters)
                            {
                                _channel.Add(await Context.Channel.SendMessageAsync("```Invalid username (special characters).```"));
                            }

                            UserName = await NextMessageAsync();
                            if (UserName != null)
                            {
                                isValidUsername = ValidateUsername(UserName.ToString()); // Verifica se não há espaços no nome de usuário
                            }
                        }

                        time = DateTime.Now;
                        ChannelsTime.Add(time);
                        _channel.Add(await Context.Channel.SendMessageAsync("```Please enter your password!```"));

                        var Password = await NextMessageAsync(); // Supondo que isso retorne um valor do tipo que você espera
                        var PasswordExpiredTime = DateTime.Now;

                        while (Password == null)
                        {
                            if (DateTime.Now >= UserNameExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }


                            Password = await NextMessageAsync(); // Supondo que isso retorne um valor do tipo que você espera
                        }

                        InvalidCode isValidPassword = ValidatePassword(Password.ToString()); // Verifica se não há espaços no nome de usuário
                        PasswordExpiredTime = DateTime.Now;

                        while (isValidPassword != InvalidCode.Success) // Se a String não for valida While irá repetir todo processo de digitação de string
                        {
                            if (DateTime.Now >= PasswordExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }

                            time = DateTime.Now;
                            ChannelsTime.Add(time);

                            if (isValidPassword == InvalidCode.Password)
                            {

                                _channel.Add(await Context.Channel.SendMessageAsync("```Invalid password (6-12 characters).```"));
                            }

                            Password = await NextMessageAsync();
                            if (Password != null)
                            {
                                isValidPassword = ValidatePassword(Password.ToString()); // Verifica se não há espaços no nome de usuário
                            }
                        }


                        time = DateTime.Now;
                        ChannelsTime.Add(time);

                        _channel.Add(await Context.Channel.SendMessageAsync("```please enter your e-mail.```"));

                        var requestemail = await NextMessageAsync();

                        var requestemailExpiredTime = DateTime.Now;

                        while (requestemail == null)
                        {
                            if (DateTime.Now >= requestemailExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }

                            requestemail = await NextMessageAsync(); // Supondo que isso retorne um valor do tipo que você espera
                        }

                        InvalidCode isValidrequestemail = Validateemail(requestemail.ToString()); // Verifica se não há espaços no nome de usuário

                        requestemailExpiredTime = DateTime.Now;

                        while (isValidrequestemail != InvalidCode.Success) // Se a String não for valida While irá repetir todo processo de digitação de string
                        {
                            if (DateTime.Now >= requestemailExpiredTime.AddMinutes(1))
                            {
                                await ReturnExpiredTime();
                                return;
                            }

                            time = DateTime.Now;
                            ChannelsTime.Add(time);
                            if (isValidrequestemail == InvalidCode.Email)
                            {

                                _channel.Add(await Context.Channel.SendMessageAsync("```Invalid e-mail address.```"));
                            }
                            else if (isValidrequestemail == InvalidCode.InvalidCharacters)
                            {

                                _channel.Add(await Context.Channel.SendMessageAsync("```Invalid e-mail (special characters).```"));

                            }

                            requestemail = await NextMessageAsync();
                            if (requestemail != null)
                            {
                                isValidrequestemail = Validateemail(requestemail.ToString());  // Verifica se não há espaços no nome de usuário
                            }
                        }

                        SetAccountInfo(UserName, Password, requestemail);

                        await GetInfoByAPI(UserName, requestemail);

                        DeleteMessagesListAsync();

                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        #region[Set AccountInfo]
        private void SetAccountInfo(SocketMessage UserName, SocketMessage Password, SocketMessage requestemail)
        {
            AccountInfo = AccountInfo.SetAccountInfo(
                                UserName.ToString(),
                                Password.ToString(),
                                Context.User.Id.ToString(),
                                requestemail.ToString());
        }
        #endregion

        #region [GetInfoByAPI]
        private async Task GetInfoByAPI(SocketMessage UserName, SocketMessage requestemail)
        {
            var encodedData = new
            {
                email = Convert.ToBase64String(Encoding.UTF8.GetBytes(AccountInfo.GetEmail())),
                discordId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Context.User.Id.ToString())),
                username = Convert.ToBase64String(Encoding.UTF8.GetBytes(AccountInfo.GetUsername())),
                password = Convert.ToBase64String(Encoding.UTF8.GetBytes(AccountInfo.GetPassword()))


            };

            var token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var apiUrl = "http://api.digitalshinkaonline.com:7189/v1/account";

            var headers = new Dictionary<string, string>
                {
                    { "Authorization", token }
                };

            using (var httpClient = new HttpClient())
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var json = JsonConvert.SerializeObject(encodedData);
                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl, requestContent);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    StatusCode statusCode = (StatusCode)int.Parse(response.StatusCode.ToString());
                    await ValidateSendMessageAsync(statusCode);
                }
                else
                {
                    await ValidateSendMessageAsync(StatusCode.Success);

                }

            }
        }

        private async Task ValidateSendMessageAsync(StatusCode statusCode)
        {

            var time = DateTime.Now;

            switch (statusCode)
            {
                case StatusCode.Success:
                    await CreateAccountByAPI();
                    break;
                case StatusCode.UsernameInUse:
                    time = DateTime.Now;
                    ChannelsTime.Add(time);
                    _channel.Add(await Context.Channel.SendMessageAsync("```The selected username has already been taken.```"));
                    break;
                case StatusCode.DiscordIdInUse:
                    time = DateTime.Now;
                    ChannelsTime.Add(time);
                    _channel.Add(await Context.Channel.SendMessageAsync("```The current DiscordId has already been used.```"));
                    break;
                case StatusCode.EmailInUse:
                    time = DateTime.Now;
                    ChannelsTime.Add(time);
                    _channel.Add(await Context.Channel.SendMessageAsync("```The selected e-mail address has already been taken.```"));
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region[Create Account]
        private async Task CreateAccountByAPI()
        {
            var embed = new EmbedBuilder()
            .WithTitle($"Welcome to Digital Shinka Online!")
            .WithThumbnailUrl($"{UrlImage}")
            .AddField("Username:", $"```{AccountInfo.GetUsername()}```")
            .AddField("Password:", $"```{AccountInfo.GetPassword()}```")
            .AddField("Email:", $"```{AccountInfo.GetEmail()}```")
            .WithColor(Color.Green)
            .AddField("Create Date:", $"```{DateTime.Now.ToString()}```")
            .Build();

            // Enviar a mensagem com o objeto de embed
            _channel.Add(await Context.Channel.SendMessageAsync(embed: embed));
            var time = DateTime.Now;
            ChannelsTime.Add(time);
        }
        #endregion


        #region[Delete Messages]
        private void DeleteMessagesListAsync()
        {
            #region[Delete Async Messages]
            Thread DellThread = new Thread(async _ =>
            {

                Task.Delay(TimeSpan.FromSeconds(60)).ContinueWith(async _ =>
                {
                    for (int i = 0; i < _channel.Count; i++)
                    { // Verificar se a mensagem ainda existe antes de tentar excluí-la
                        if (DateTime.Now >= ChannelsTime[i].AddSeconds(60))
                        {

                            var retrievedMessage = await Context.Channel.GetMessageAsync(_channel[i].Id);
                            if (retrievedMessage != null)
                            {
                                await retrievedMessage.DeleteAsync();

                                _channel.Remove(_channel[i]);
                                ChannelsTime.Remove(ChannelsTime[i]);
                            }
                        }

                    }

                });

            });
            DellThread.Start();
            #endregion
        }
        #endregion

        #region[Validate]
        public InvalidCode ValidateUsername(string username)
        {
            if (!username.Contains(" ") && username.Length <= 20 && username.Length >= 6)
            {
                string pattern = "^[a-zA-Z0-9_]+$";

                // Use a classe Regex para verificar a correspondência do padrão
                bool isValid = Regex.IsMatch(username, pattern);

                if (!isValid)
                {
                    return InvalidCode.Username;
                }
                else
                    return InvalidCode.Success;
            }
            return
                InvalidCode.Username;
        }
        public InvalidCode Validateemail(string email)
        {
            if (!email.Contains(" "))
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                // Use a classe Regex para verificar a correspondência do padrão
                bool isValid = Regex.IsMatch(email, pattern);
                if (!isValid)
                {
                    return InvalidCode.Email;
                }
                else
                    return InvalidCode.Success;

            }
            else
                return InvalidCode.Email;

        }
        public InvalidCode ValidatePassword(string username)
        {
            if (!username.StartsWith(" ") && !username.EndsWith(" ") && username.Length <= 12 && username.Length >= 6)
            {

                return InvalidCode.Success;
            }
            return
                InvalidCode.Password;
        }
        #endregion

        #region[Util]
        private async Task ReturnExpiredTime()
        {
            await Context.Channel.SendMessageAsync("Expired util time, try again.");
        }
        #endregion
    }


}
