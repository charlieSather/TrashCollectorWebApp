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
    public class CustomersController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public CustomersController(IRepositoryWrapper repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            if (UserIsVerifiedCustomer())
            {
                var customer = _repo.Customer.GetCustomer(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                if(customer is null)
                {
                    return RedirectToAction("Create");
                }

                var model = new CustomerViewModel();
                model.Customer = customer;
                model.Address = _repo.Address.GetAddress(customer.AddressId);
                model.Pickup = _repo.Pickup.GetPickup(customer.PickupId);

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //public IActionResult Create() => View();

        public IActionResult Create()
        {
            if (UserIsVerifiedCustomer())
            {
                return View(new CustomerViewModel { Customer = new Customer(), Address = new Address(), Pickup = new Pickup() });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel customerViewModel)
        {
            if (UserIsVerifiedCustomer())
            {
                if (ModelState.IsValid)
                {
                    var customer = customerViewModel.Customer;
                    customer.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    if(!_repo.Address.AddressExists(customerViewModel.Address))
                    {
                        _repo.Address.CreateAddress(customerViewModel.Address);
                        _repo.Save();
                        customer.AddressId = customerViewModel.Address.Id;
                    }
                    else
                    {
                        customer.AddressId = _repo.Address.GetAddress(customerViewModel.Address).Id;
                    }

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
            else
            {
                return RedirectToAction("Index","Home");

            }
        }

        //public IActionResult CreateCustomer() => View();
        //public IActionResult CreateAddress() => View();
        //public IActionResult CreatePickup() => View();
        public bool UserIsVerifiedCustomer() => User.IsInRole("Customer") && User.Identity.IsAuthenticated;
    }
}