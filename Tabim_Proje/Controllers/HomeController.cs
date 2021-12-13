using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tabim_Proje.Data;
using Tabim_Proje.Models;

namespace Tabim_Proje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("IndexPublic");
            }

            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (User.Identity.IsAuthenticated && userEmail == "Admin@example.com")
            {
                return RedirectToAction("IndexAdmin", "Home");

                //return View("IndexAdmin");
            }
            else
            {
                return View(_db.UserRequests.Where(x => x.ApplicationUserId == userId).ToList());
            }

        }

        public IActionResult IndexAdmin(string inf)
        {
            IQueryable<UserRequest> requests = _db.UserRequests;

            AdminVM vm = new();

            if (inf == "false")
            {
                vm.Unapproved = requests.Where(x => x.ConsiderationStatus == false && x.TimeOfConsideration != DateTime.MinValue).OrderByDescending(x => x.CreationTime).ToList();
                return View(vm.Unapproved);
            }
            else if (inf == "true")
            {
                vm.Approved = requests.Where(x => x.ConsiderationStatus == true).OrderByDescending(x => x.CreationTime).ToList();
                return View(vm.Approved);
            }
            else
            {
                vm.AllRequest = requests.OrderByDescending(x => x.CreationTime).ToList();
                return View(vm.AllRequest);
            }

        }


        public IActionResult ShowDocument(string infpath)
        {
            //return File("~/documents/seckinMantar-Resume.pdf", "application/pdf");
            return File(infpath, "application/pdf");  //fileResult
        }


        public IActionResult EvaluateRequest(int infId)
        {
            UserRequest userRequest = _db.UserRequests.Find(infId);
            if (userRequest == null)
            {
                return NotFound();
            }
            RequestEvaluateVM vm = new();
            vm.Id = userRequest.Id;
            
            return View(vm);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult EvaluateRequest(RequestEvaluateVM vm, string status)
        {
            bool RequestStatus;

            if (status == "true")
            {
                RequestStatus = true;
            }
            else
            {
                RequestStatus = false;
            }

                UserRequest userRequest = _db.UserRequests.Find(vm.Id);
                userRequest.ConsiderationStatus = RequestStatus;
                userRequest.TimeOfConsideration = DateTime.Now;
                userRequest.ResultOfConsideration = vm.ResultOfConsideration;
                _db.SaveChanges();

                return RedirectToAction("IndexAdmin", "Home");
 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
