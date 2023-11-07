using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System.Reflection;
using UserManagement.Services.Interfaces;

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
        if (ModelState.IsValid)
        {
            _userService.CreateUser(user);

            _sharedService.SetToastNotification(this, "New User created.", true);
            return RedirectToAction(nameof(List));
        }

        _sharedService.SetToastNotification(this, "New User was not created.", false);
        return View(user);
    }

    // GET: Users/View/1
    public IActionResult View(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _userService.FindUser((long)id);

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // GET: Users/Edit/1
    public IActionResult Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _userService.FindUser((long)id);

        if (user == null)
        {
            return NotFound();
        }
        return View(user);
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
        User existingUser = _userService.FindUser(id);

        // Copy properties between both user instances.
        _sharedService.CopyObjectProperties(user, existingUser);

        if (ModelState.IsValid)
        {
            _userService.EditUser(existingUser);
            _sharedService.SetToastNotification(this, "User updated.", true);
            return RedirectToAction(nameof(List));
        }

        _sharedService.SetToastNotification(this, "User was not updated.", false);
        return View(user);
    }

    // GET: Users/Delete/1
    public IActionResult Delete(long id)
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
}
