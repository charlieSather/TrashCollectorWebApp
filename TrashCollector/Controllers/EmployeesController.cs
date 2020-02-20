using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrashCollector.Contracts;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public EmployeesController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            if (UserIsVerifiedEmployee())
            {
                var employee = _repo.Employee.GetEmployee(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (employee is null)
                {
                    return RedirectToAction("Create");
                }
                var customers = _repo.Customer.GetCustomersByZipCode(employee.ZipCode).ToList();

                return View(customers);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (UserIsVerifiedEmployee())
            {
                if (ModelState.IsValid)
                {
                    employee.UserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    _repo.Employee.CreateEmployee(employee);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(employee);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult PickUpTrash(int id)
        {
            if (UserIsVerifiedEmployee())
            {
                var pickupFromDb = _repo.Pickup.GetPickup(id);
                if (pickupFromDb != null)
                {
                    pickupFromDb.Balance += 5;
                    _repo.Pickup.UpdatePickup(pickupFromDb);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public bool UserIsVerifiedEmployee() => User.IsInRole("Employee") && User.Identity.IsAuthenticated;
        //public bool IsSup()
        //{
        //    var pickups = _repo.Pickup.GetPickups();
        //}
    }
}