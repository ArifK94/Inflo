using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class SharedService : ISharedService
{
    /**
     * The destination object will have the properties with values copied from the source object.
     */
    public void CopyObjectProperties(object sender, object receiver)
    {
        var sourceProperties = sender.GetType().GetProperties();
        var destinationProperties = receiver.GetType().GetProperties();

        foreach (var sourceProperty in sourceProperties)
        {
            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

            if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
            {
                var value = sourceProperty.GetValue(sender);
                destinationProperty.SetValue(receiver, value);
            }
        }
    }

    public void SetToastNotification(Controller controller, string message, bool isSuccess)
    {
        string type = isSuccess ? "success" : "error";
        string title = isSuccess ? "Success" : "Error";

        controller.TempData["toastrMessage"] = JsonConvert.SerializeObject(new { type = type, message = message, title = title });
    }

    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
