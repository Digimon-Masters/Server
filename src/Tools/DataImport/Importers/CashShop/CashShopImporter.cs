using System.Text.Json;
using DataImport.Models.CashShop;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using Console = Spectre.Console.AnsiConsole;
namespace DataImport.Importers.CashShop;

public class CashShopImporter
{
    private readonly SqlConnection _sqlConnection;
    
    public CashShopImporter(string connectionString)
    {
        _sqlConnection = new SqlConnection(connectionString);
    }

    public void Import()
    {
        var file = $"{Directory.GetCurrentDirectory()}\\Data\\CashShop\\CashShop.json";
        if (!File.Exists(file))
        {
            Console.WriteLine($"File was not found at {file}");
            Thread.Sleep(2000);
        }
        
        Console.WriteLine("Loading file...");

        var fileContent = File.ReadAllText(file);
        var cashShopItems = JsonSerializer.Deserialize<List<CashShopItem>>(fileContent);

        var prompt =
            Console.Prompt(
                new ConfirmationPrompt(
                    $"Found a total of {cashShopItems.Count} to import. Are you sure you want to import?"));

        if (!prompt)
        {
            return;
        }

        var query = @"
        INSERT INTO Shop.CashShop(Id, IsActive, Name, Description, StartDate, EndDate, CategoryId, UniqueId, IconId, SalesPercent, PurchaseCashType, SellingPrice, ItemsJson)
        VALUES(@id, @isActive, @name, @description, @startDate, @endDate, @categoryId, @uniqueId, @iconId, @salesPercent, @purchaseCashType, @sellingPrice, @itemsJson)
        ";

        var itemsQuery = @"
        INSERT INTO Shop.CashShopItems(UniqueId, ItemId, Quantity)
        VALUES(@uniqueId, @itemId, @quantity)
        ";

        _sqlConnection.Open();
        foreach (var cashShopItem in cashShopItems)
        {
            var command = new SqlCommand(query, _sqlConnection);
            command.Parameters.AddWithValue("id", Guid.NewGuid());
            command.Parameters.AddWithValue("isActive", cashShopItem.IsActive);
            command.Parameters.AddWithValue("name", cashShopItem.Name);
            command.Parameters.AddWithValue("description", cashShopItem.Description);
            command.Parameters.AddWithValue("startDate", cashShopItem.Date1);
            command.Parameters.AddWithValue("endDate", cashShopItem.Date2);
            command.Parameters.AddWithValue("categoryId", cashShopItem.CategoryId);
            command.Parameters.AddWithValue("uniqueId", cashShopItem.UniqueId);
            command.Parameters.AddWithValue("iconId", cashShopItem.IconId);
            command.Parameters.AddWithValue("salesPercent", cashShopItem.SalesPercent);
            command.Parameters.AddWithValue("purchaseCashType", cashShopItem.PurchaseCashType);
            command.Parameters.AddWithValue("sellingPrice", cashShopItem.SellingPrice);
            command.Parameters.AddWithValue("itemsJson", JsonSerializer.Serialize(cashShopItem.Items));
            command.Notification = new SqlNotificationRequest();
            command.ExecuteNonQuery();
            foreach (var item in cashShopItem.Items.DistinctBy(x => x.ItemId))
            {
                var itemCommand = new SqlCommand(itemsQuery, _sqlConnection);
                itemCommand.Parameters.AddWithValue("uniqueId", cashShopItem.UniqueId);
                itemCommand.Parameters.AddWithValue("itemId", item.ItemId);
                itemCommand.Parameters.AddWithValue("quantity", item.Quantity);
                itemCommand.ExecuteNonQuery();
            }
        }
        _sqlConnection.Close();
        Console.WriteLine("Finished importing data.");
        Thread.Sleep(2000);
    }
}