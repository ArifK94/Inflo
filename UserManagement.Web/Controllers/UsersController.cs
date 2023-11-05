using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using System.Reflection;

namespace UserManagement.WebMS.Controllers;

// Uncomment whe solution is found, since this is stopping asp-action from working in the view. 
//[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

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

        return PartialView("_UserTablePartial", model);
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
            return RedirectToAction(nameof(List));
        }
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

        // TODO: Place this in a separate method in a shared class. 
        // Copy properties between both user instances.
        foreach (PropertyInfo property in typeof(User).GetProperties().Where(p => p.CanWrite))
        {
            property.SetValue(existingUser, property.GetValue(user, null), null);
        }

        if (ModelState.IsValid)
        {
            _userService.EditUser(existingUser);
            return RedirectToAction(nameof(List));
        }
        return View(user);
    }
}
