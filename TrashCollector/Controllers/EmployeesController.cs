using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrashCollector.Contracts;

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
            var employee = _repo.Employee.FindByCondition(x => x.UserId == this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(employee is null)
            {
                return View();
            }


            return View();
        }
         

        public IActionResult Create() => View();
    }
}