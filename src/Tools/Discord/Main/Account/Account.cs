

using System.Reflection.Metadata.Ecma335;

namespace Template.Modules
{
    public class Account
    {
        private string Username = string.Empty;
        private string Password = string.Empty;
        private string DiscordId = string.Empty;
        private string Email = string.Empty;

      
        public Account SetAccountInfo(string username,string password,string discordId,string email)
        {
            Username = username;
            Password = password;
            DiscordId = discordId;  
            Email = email;  

            return this;
        }

        public string GetUsername()
        {
           
            return Username;
        }

        public string GetPassword()
        {

            return Password;
        }
        public string GetEmail()
        {

            return Email;
        }
    }
}
