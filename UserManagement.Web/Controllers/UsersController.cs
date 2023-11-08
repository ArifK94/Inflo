using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System.Reflection;
using UserManagement.Services.Interfaces;
using System;

namespace UserManagement.WebMS.Controllers;

// Uncomment whe solution is found, since this is stopping asp-action from working in the view. 
//[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ISharedService _sharedService;

    public UsersController(IUserService userService, ISharedService sharedService)
    {
        _userService = userService;
        _sharedService = sharedService;
    }

    [HttpGet]
    public ViewResult List()
    {
        var items = _userService.GetAll().Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };



        return View(model);
    }

    [HttpPost]
    public ActionResult FilterByActive(bool isActive)
    {
        var items = _userService.FilterByActive(isActive).Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return PartialView("_UsersListPartial", model);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _sharedService.SetToastNotification(this, "Error occurred", false);
                return View();
            }

            _userService.CreateUser(user);

            _sharedService.SetToastNotification(this, "New User created.", true);
            return RedirectToAction(nameof(List));
        }
        catch (Exception)
        {
            _sharedService.SetToastNotification(this, "New User was not created.", false);
            return View();
        }

    }

    // GET: Users/View/1
    public IActionResult View(long? id)
    {
        string errorMessage = "User was not found.";
        try
        {
            if (id == null)
            {
                _sharedService.SetToastNotification(this, errorMessage, false);
                return RedirectToAction(nameof(List));
            }

            var user = _userService.FindUser((long)id);

            if (user == null)
            {
                _sharedService.SetToastNotification(this, errorMessage, false);
                return RedirectToAction(nameof(List));
            }

            return View(user);
        }
        catch(Exception)
        {
            _sharedService.SetToastNotification(this, errorMessage, false);
            return RedirectToAction(nameof(List));
        }

    }

    // GET: Users/Edit/1
    public IActionResult Edit(long? id)
    {
        string errorMessage = "User was not found.";
        try
        {
            if (id == null)
            {
                _sharedService.SetToastNotification(this, errorMessage, false);
                return RedirectToAction(nameof(List));
            }

            var user = _userService.FindUser((long)id);

            if (user == null)
            {
                _sharedService.SetToastNotification(this, errorMessage, false);
                return RedirectToAction(nameof(List));
            }
            return View(user);

        }
        catch (Exception)
        {
            _sharedService.SetToastNotification(this, errorMessage, false);
            return RedirectToAction(nameof(List));
        }


    }

    // POST: Users/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        try
        {
            if (!ModelState.IsValid)
            {
                _sharedService.SetToastNotification(this, "Error occurred", false);
                return View();
            }

            if (!_sharedService.IsValidEmail(user.Email))
            {
                ModelState.AddModelError("email", "Invalid email address");
                return View();
            }

            User existingUser = _userService.FindUser(id);

            // Copy properties between both user instances.
            _sharedService.CopyObjectProperties(user, existingUser);

            _userService.EditUser(existingUser);
            _sharedService.SetToastNotification(this, "User updated.", true);
            return RedirectToAction(nameof(List));
        }
        catch (Exception)
        {
            _sharedService.SetToastNotification(this, "User was not updated.", false);
            return View(user);
        }
    }

    // GET: Users/Delete/1
    public IActionResult Delete(long id)
    {
        try
        {
            var user = _userService.FindUser(id);

            if (user == null)
            {
                _sharedService.SetToastNotification(this, "User was not found.", false);
                return NotFound();
            }

            _userService.DeleteUser(user);

            _sharedService.SetToastNotification(this, "User was deleted.", true);
            return RedirectToAction(nameof(List));
        }
        catch(Exception)
        {
            _sharedService.SetToastNotification(this, "User was not deleted.", false);
            return RedirectToAction(nameof(List));
        }

    }
}
