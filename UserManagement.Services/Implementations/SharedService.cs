using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserManagement.Models;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services.Implementations;
public class SharedService : ISharedService
{
    public void CopyObjectProperties(object sender, object receiver)
    {        
        foreach (PropertyInfo property in sender.GetType().GetProperties().Where(p => p.CanWrite))
        {
            property.SetValue(receiver, property.GetValue(sender, null), null);
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
