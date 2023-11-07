/**
 * Methods used for being shared across the application.  
 */

using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Services.Interfaces;
public interface ISharedService
{
    void SetToastNotification(Controller controller, string message, bool isSuccess);

    /**
     *  Copy properties between both user instances.
     *  Sender object sends the values to the receiver.
     */
    void CopyObjectProperties(object sender, object receiver);
}
