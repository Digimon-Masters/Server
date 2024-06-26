using DataImport.Importers;
using DataImport.Importers.CashShop;
using DigitalWorldOnline.Commons.Utils;
using Spectre.Console;

namespace DataImport;
using Console = AnsiConsole;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        var connectionString = Environment.GetEnvironmentVariable($"{Constants.Configuration.EnvironmentPrefix}{Constants.Configuration.DatabaseKey}", EnvironmentVariableTarget.Machine);
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = Console.Prompt(new TextPrompt<string>("Please insert your connection string:"));
        } 
        else
        {
            var multiSelection = new SelectionPrompt<string>().Title(
                    "A connection string was found in your environment. Would you like to use it or insert a new one?")
                .AddChoices(["Use the environment connection string", "Insert a new one"]);
            var useAnotherConnectionString = Console.Prompt(multiSelection);
            if (useAnotherConnectionString.Contains("Insert"))
            {
                connectionString = Console.Prompt(new TextPrompt<string>("Please insert your connection string:"));
            }
        }


        var importData = true;

        while (importData)
        {
            var importSelection = new SelectionPrompt<string>().Title("What type of data would you like to import?")
                .AddChoices(["Cash Shop", "Exit"]);
            var importSelectionPrompt = Console.Prompt(importSelection);

            switch (importSelectionPrompt)
            {
                case "Cash Shop":
                    new CashShopImporter(connectionString).Import();
                    break;
                case "Exit":
                    default:
                        importData = false;
                        break;
            }
        }
    }
}
