using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrashCollector.Contracts;
using TrashCollector.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Stripe;
using TrashCollector.ActionFilter;

namespace TrashCollector.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomersController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IConfiguration _config;


        public CustomersController(IRepositoryWrapper repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
        [HttpGet]
        public IActionResult Index()
        {

            var customer = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (customer is null) return RedirectToAction("Create");

            var key = _config.GetValue<string>("Keys:StripePublishableKey");
            ViewBag.StripeKey = key;

            return View(new CustomerViewModel() { Customer = customer, Address = customer.Address, Pickup = customer.Pickup });

        }

        [HttpGet]
        public IActionResult Create() => View(new CustomerViewModel { Pickup = new Pickup() });

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SuspensionDates))]
        public IActionResult Create(CustomerViewModel customerViewModel)
        {

            if (ModelState.IsValid)
            {
                var customer = customerViewModel.Customer;
                customer.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!_repo.Address.AddressExists(customerViewModel.Address))
                {
                    _repo.Address.CreateAddress(customerViewModel.Address);
                    _repo.Save();
                    customer.AddressId = customerViewModel.Address.Id;
                }
                else
                {
                    customer.AddressId = _repo.Address.GetAddress(customerViewModel.Address).Id;
                }

                customerViewModel.Pickup.IsSuspended = IsSuspendedDates(DateTime.Now.Date, customerViewModel.Pickup.StartDate.Date, customerViewModel.Pickup.EndDate.Date);

                _repo.Pickup.CreatePickup(customerViewModel.Pickup);
                _repo.Save();


                customer.PickupId = customerViewModel.Pickup.Id;

                _repo.Customer.CreateCustomer(customer);
                _repo.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(customerViewModel);
            }

        }

        //4242 4242 4242 4242
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Charge(string stripeEmail, string stripeToken)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var customerFromDb = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var pickup = _repo.Pickup.GetPickup(customerFromDb.PickupId);

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = (long) pickup.Balance * 100,
                Description = $"{pickup.Balance} Charge",
                Currency = "usd",
                Customer = customer.Id
            });

            pickup.Balance = 0;
            _repo.Pickup.UpdatePickup(pickup);
            _repo.Save();


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePickupDay(CustomerViewModel customerViewModel)
        {

            var pickupFromDb = _repo.Pickup.GetPickup(customerViewModel.Pickup.Id);
            if (pickupFromDb != null)
            {
                pickupFromDb.PickupDay = customerViewModel.Pickup.PickupDay;
                _repo.Pickup.UpdatePickup(pickupFromDb);
                _repo.Save();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOneTimePickup(CustomerViewModel customerViewModel)
        {

            if (customerViewModel.Pickup.OneTimePickup.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Pickup.OneTimePickup", "How does one pickup trash from the past?");
                var customer = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                return View("Index", new CustomerViewModel { Customer = customer, Address = customer.Address, Pickup = customer.Pickup });

            }
            var pickupFromDb = _repo.Pickup.GetPickup(customerViewModel.Pickup.Id);
            if (pickupFromDb != null)
            {
                pickupFromDb.OneTimePickup = customerViewModel.Pickup.OneTimePickup;
                _repo.Pickup.UpdatePickup(pickupFromDb);
                _repo.Save();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult UpdateSuspensionDates()
        {

            var customer = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (customer is null)
            {
                return RedirectToAction("Create");
            }

            return View("Suspension", new SuspensionViewModel { StartDate = customer.Pickup.StartDate, EndDate = customer.Pickup.EndDate });


        }
        public IActionResult Transactions(int id)
        {

            var model = _repo.Transaction.GetCustomersTransactionsThisMonth(id).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(SuspensionDates))]
        public IActionResult UpdateSuspensionDates(SuspensionViewModel svm)
        {

            var customer = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (customer is null) return RedirectToAction("Create");

            if (ModelState.IsValid)
            {
                //if (IsValidStartEndDate(svm.StartDate.Date, svm.EndDate.Date))
                //{
                //    ModelState.AddModelError("StartDate", "Start date can not be the same or past the end date");
                //    return View("Suspension", svm);
                //}


                customer.Pickup.StartDate = svm.StartDate;
                customer.Pickup.EndDate = svm.EndDate;

                customer.Pickup.IsSuspended = IsSuspendedDates(DateTime.Now.Date, svm.StartDate.Date, svm.EndDate.Date);


                _repo.Pickup.UpdatePickup(customer.Pickup);
                _repo.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Suspension", svm);
            }

        }
        public bool UserIsVerifiedCustomer() => User.IsInRole("Customer") && User.Identity.IsAuthenticated;
        public bool IsValidStartEndDate(DateTime start, DateTime end) => start.Date >= end.Date;
        public bool IsSuspendedDates(DateTime today, DateTime start, DateTime end) => today >= start ? (today < end ? true : false) : false;
    }
}