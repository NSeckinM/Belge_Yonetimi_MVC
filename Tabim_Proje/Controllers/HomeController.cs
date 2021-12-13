using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            else if (inf == "pending")
            {
                vm.Unapproved = requests.Where(x => x.ConsiderationStatus == false && x.TimeOfConsideration == DateTime.MinValue).OrderByDescending(x => x.CreationTime).ToList();
                return View(vm.Unapproved);
            }
            else
            {
                vm.AllRequest = requests.OrderByDescending(x => x.CreationTime).ToList();
                return View(vm.AllRequest);
            }

        }

        public IActionResult Report()
        {

            return View();
        }
        public IActionResult ShowDocument(string infpath)
        {
            return File(infpath, "application/pdf");
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
            vm.ApplicationUserId = userRequest.ApplicationUserId;

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EvaluateRequest(RequestEvaluateVM vm, string status)
        {
            var user = _db.Users.Find(vm.ApplicationUserId);
            string mailAddress = user.Email;
            bool RequestStatus;
            string mailstatus;

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

            if (RequestStatus)
                mailstatus = "Approved";
            else
                mailstatus = "Rejected";

            SendMail(vm.ResultOfConsideration, mailstatus, mailAddress);
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
        private void SendMail(string content, string status, string email)
        {
            string sender = "seckinmantar@gmail.com";//kullanıcı adı
            string to = email;
            string subject = "Result of Consideration";
            string body = "Dear applicant, \n" + content + "\n Result:  " + status;

            MailMessage posta = new MailMessage(sender, to, subject, body);
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))//gönderici maili
            {
                smtp.Credentials = new NetworkCredential("seckinmantar@gmail.com", "seckinmntr");
                smtp.EnableSsl = true;
                smtp.Send(posta);
            }

        }
    }
}
