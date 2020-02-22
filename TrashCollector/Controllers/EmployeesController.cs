using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

                //var customers =
                //    from customer in _repo.Customer.GetCustomersByZipCodeAndPickupDay(employee.ZipCode, DateTime.Today.DayOfWeek.ToString())
                //    join transaction in _repo.Transaction.GetTransactionsToday(DateTime.Now) on customer.Id equals transaction.Id into joinGroup
                //    from transaction in joinGroup.DefaultIfEmpty()
                //    where transaction == null
                //    select customer;

                var customers = _repo.Customer.GetCustomersByZipCodeAndPickupDay(employee.ZipCode, DateTime.Today.DayOfWeek.ToString()).Except(_repo.Transaction.GetTransactionsToday(DateTime.Now).Select(t => t.Customer)).ToList();


                return View(new EmployeeViewModel { Customers = customers, Employee = employee });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult FilterByDay(EmployeeViewModel cvm)
        {
            if (UserIsVerifiedEmployee())
            {
                 if(SelectedDayIsToday(cvm.Day)) return RedirectToAction("Index");

                var employee = _repo.Employee.GetEmployee(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (employee is null) return RedirectToAction("Create");

                var customers = _repo.Customer.GetCustomersByZipCodeAndPickupDay(employee.ZipCode, cvm.Day).ToList();

                return View("Index", new EmployeeViewModel { Customers = customers, Employee = employee, HidePickupTrash = true });
            }
         
            return RedirectToAction("Index", "Home");
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
                var customerFromDb = _repo.Customer.GetCustomer(id);

                if (customerFromDb != null)
                {
                    var employee = _repo.Employee.GetEmployee(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    customerFromDb.Pickup.Balance += 5;
                    _repo.Transaction.CreateTransaction(new Transaction { ChargeDate = DateTime.Today, ChargeAmount = 5, CustomerId = customerFromDb.Id, EmployeeId = employee.Id });
                    _repo.Pickup.UpdatePickup(customerFromDb.Pickup);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public bool UserIsVerifiedEmployee() => User.IsInRole("Employee") && User.Identity.IsAuthenticated;
        public bool SelectedDayIsToday(string day) => day == DateTime.Today.DayOfWeek.ToString();
        //public bool IsSup()
        //{
        //    var pickups = _repo.Pickup.GetPickups();
        //}
    }
}