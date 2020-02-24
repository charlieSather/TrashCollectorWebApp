using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrashCollector.Contracts;
using TrashCollector.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace TrashCollector.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly string Api_Key = "AIzaSyAyRUdbGYsbALxmqXoNgPyOLzcqJX1PQos";

        private readonly IRepositoryWrapper _repo;
        private readonly IConfiguration _config;
        public EmployeesController(IRepositoryWrapper repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
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

                var customers = _repo.Customer.GetCustomersByZipCodeAndDate(employee.ZipCode, DateTime.Today).Except(_repo.Transaction.GetTransactionsToday(DateTime.Now).Select(t => t.Customer)).ToList();

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

                var dayAsDate = DateTime.Today.AddDays(DayOfWeekOffset(cvm.Day));

                var customers = _repo.Customer.GetCustomersByZipCodeAndDate(employee.ZipCode, dayAsDate).ToList();

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
        public async Task<IActionResult> GoogleMap(int id)
        {
            var customer = _repo.Customer.GetCustomer(id);
            if (customer is null) return RedirectToAction("Index");

            HttpClient client = new HttpClient();

            var key = _config.GetValue<string>("Keys:MapQuestKey");
            var url = $"http://www.mapquestapi.com/geocoding/v1/address?key={key}&location={customer.Address.ToString()}";

            HttpResponseMessage response = await client.GetAsync(url);
            string jsonResult = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                GeoLocation geoLocation = JsonConvert.DeserializeObject<GeoLocation>(jsonResult);
                if(geoLocation.results.Length > 0)
                {
                    return View(new MapViewModel { Latitude = geoLocation.results[0].locations[0].latLng.lat.ToString(), Longitude = geoLocation.results[0].locations[0].latLng.lng.ToString() });
                }           
            }
            return RedirectToAction("Index");
        }

        public bool UserIsVerifiedEmployee() => User.IsInRole("Employee") && User.Identity.IsAuthenticated;
        public bool SelectedDayIsToday(string day) => day == DateTime.Today.DayOfWeek.ToString();
        public double DayOfWeekOffset(string dayOfWeek) 
        {
            var today = DateTime.Today.DayOfWeek;
            double code = dayOfWeek switch
            {
                ("Sunday") => 0,
                ("Monday") => 1,
                ("Tuesday") => 2,
                ("Wednesday") => 3,
                ("Thursday") => 4,
                ("Friday") => 5,
                ("Saturday") => 6,
                _ => throw new InvalidOperationException()
            };

            return code - (int) today;
        }

        //public bool IsSup()
        //{
        //    var pickups = _repo.Pickup.GetPickups();
        //}
    }
}